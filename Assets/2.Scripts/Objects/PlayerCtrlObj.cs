using DefineEnums;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrlObj : CharacterBase
{
    [Header("Parmeter")]
    [SerializeField] float _moveSpeed = 5f;
    [Header("Reference")]
    [SerializeField] Transform _rootDam;
    //스탯 변수
    int _level;
    float _attackDist = 3f;
    //==

    //

    Animator _animator;
    PlayerAnimState _curAnimState;

    [SerializeField] float _curTime, _nIdleTime;
    bool _animChange, _isDead;


    //
    NavMeshAgent _navAgent;
    Transform _target;
    DamageCheckerObj[] _damCheckers;

    //임시
    int _pickMask;
    //==
    public int _currentXp { get; set; }
    public int _myLevel { get { return _level; } }
    public float _xpRate { get { return (float)_currentXp / UserInfoManager._instance._targetXP; } }
    public int _remainXP
    {
        get { return UserInfoManager._instance._targetXP - _currentXp; }
    }
    public float _hpRate
    {
        get { return _nowHp / _baseStat._charHP; }//////
    }
    public int _currentHP { get { return _nowHp; } }///
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();


        //임시
        _pickMask = 1 << LayerMask.NameToLayer("Road") | 1 << LayerMask.NameToLayer("Monster");
        _pickMask |= 1 << LayerMask.NameToLayer("Architecture");
        _damCheckers = new DamageCheckerObj[_rootDam.childCount];
        for (int i = 0; i < _rootDam.childCount; i++)
        {
            _damCheckers[i] = _rootDam.GetChild(i).GetComponent<DamageCheckerObj>();
            _damCheckers[i].InitSet(i, this);
            //_damCheckers[i].EnableChecker(false);
        }
        AllDisableZone();

        //===
        //
        _animator = GetComponent<Animator>();
        _curTime = 0;
        _nIdleTime = 7.5f;
        _animChange = false;
    }
    private void Update()
    {
        if (_isDead) return;
        if (_animChange)    //함수로 빼야함
        {
            switch (_curAnimState)
            {
                case PlayerAnimState.IDLE:
                    _animator.SetBool("Idle", true);

                    break;
                case PlayerAnimState.RUN:
                    _navAgent.speed = _moveSpeed;
                    if (_target == null)
                        _navAgent.stoppingDistance = 0;
                    else
                    {
                        _navAgent.stoppingDistance = _attackDist;
                    }

                    _animator.SetBool("Run", true);

                    //정지
                    if (_navAgent.remainingDistance < _navAgent.stoppingDistance + 0.1f)
                    {
                        //Debug.Log("stop");
                        if (_target.GetComponent<BoxCollider>() == null)
                            InitAnim(PlayerAnimState.IDLE);
                        else
                            InitAnim(PlayerAnimState.ATTACK);
                    }
                    break;
                case PlayerAnimState.ATTACK:
                    if (_target.GetComponent<BoxCollider>() == null)
                    {
                        InitAnim(PlayerAnimState.IDLE);
                    }
                    else
                    {
                        transform.LookAt(_target);
                        _animator.SetTrigger("Attack");
                    }
                    break;
                case PlayerAnimState.SKILL:
                    _animator.SetTrigger("Skill");
                    break;
                case PlayerAnimState.DEATH:
                    _animator.SetTrigger("Death");
                    _isDead = true;
                    break;
                case PlayerAnimState.init:
                    _animator.Play("BIdle");
                    break;
            }
            //_animChange = false;
        }
    }
    private void LateUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit rHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rHit, Mathf.Infinity, _pickMask))
            {
                //Debug.Log(rHit.point);
                string layName = LayerMask.LayerToName(rHit.transform.gameObject.layer);
                if (layName.CompareTo("Road") == 0)
                {
                    _target = null;
                    _navAgent.SetDestination(rHit.point);
                    InitAnim(PlayerAnimState.RUN);
                }
                else if (layName.CompareTo("Monster") == 0)
                {
                    _target = rHit.transform;
                    if (Vector3.Distance(transform.position, _target.position) > _attackDist)
                    {
                        _navAgent.SetDestination(rHit.point);
                        InitAnim(PlayerAnimState.RUN);
                    }
                    else
                        InitAnim(PlayerAnimState.ATTACK);
                }
                else if (layName.CompareTo("Architecture") == 0)
                {

                }
            }

        }


        //
        if (_curAnimState == PlayerAnimState.IDLE)
        {
            _animator.SetBool("Idle", true);
            _curTime += Time.deltaTime;
            if (_curTime > _nIdleTime)
            {
                _curTime = 0;
                _animator.SetTrigger("nIdle");
            }
        }
        else
        {
            _curTime = 0;
        }
    }
    private void AllDisableZone()//
    {
        for (int i = 0; i < _damCheckers.Length; i++)
        {
            _damCheckers[i].EnableChecker(false);
        }
    }
    void EnableZoneByIndex(int index)
    {
        _damCheckers[index].EnableChecker(true);
    }

    void DisableZoneByIndex(int index)
    {
        _damCheckers[index].EnableChecker(false);
    }
    public void InitPlayer()
    {
        //
        InitSetStat(UserInfoManager._instance._nowAvatarInfo);
        _currentXp = UserInfoManager._instance._nowAvatarInfo._nowXp;

        _pickMask = 1 << LayerMask.NameToLayer("Road") | 1 << LayerMask.NameToLayer("Monster");
        _pickMask |= 1 << LayerMask.NameToLayer("Architecture");

        _damCheckers = new DamageCheckerObj[_rootDam.childCount];
        for (int i = 0; i < _rootDam.childCount; i++)
        {
            _damCheckers[i] = _rootDam.GetChild(i).GetComponent<DamageCheckerObj>();
            _damCheckers[i].InitSet(i, this);
            _damCheckers[i].enabled = false;
        }
    }
    public void SetAcquiSittionXp(int acXP)
    {
        _currentXp += acXP;
        UserInfoManager._instance.SetXP(_currentXp);
    }
    //
    //animatorChane(Enum:animState, bool isWait = false)
    //:
    //switch(State){
    //case AniActionState.Dead:
    //     anicontroller.settrigger("isDead"); break;}
    //anicontroller.setbool("isWait",isWait);
    //anicontroller.setinteger("Anistate", (int)anistate);

    void ResetAnim()
    {
        _isDead = false;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Run", false);
        _animator.ResetTrigger("nIdle");
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Skill");
        _animator.ResetTrigger("Death");
    }
    void InitAnim(PlayerAnimState animState)
    {
        ResetAnim();
        _curAnimState = animState;
        _animChange = true;

    }


    public override void OnHitting(CharacterBase attacker)
    {

    }

    //private void OnGUI()
    //{
    //    GUIStyle style = new GUIStyle("button");
    //    style.fontSize = 60;

    //    if (GUI.Button(new Rect(0, 0, 570, 190), "Idle", style))
    //    {
    //        InitAnim(PlayerAnimState.IDLE);

    //    }
    //    if (GUI.Button(new Rect(0, 190, 570, 190), "Run", style))
    //    {
    //        InitAnim(PlayerAnimState.RUN);
    //    }
    //    if (GUI.Button(new Rect(0, 380, 570, 190), "Attack", style))
    //    {
    //        InitAnim(PlayerAnimState.ATTACK);
    //    }
    //    if (GUI.Button(new Rect(0, 570, 570, 190), "Skill", style))
    //    {
    //        InitAnim(PlayerAnimState.SKILL);
    //    }
    //    if (GUI.Button(new Rect(0, 760, 570, 190), "Death", style))
    //    {
    //        InitAnim(PlayerAnimState.DEATH);
    //    }
    //    if (GUI.Button(new Rect(0, 950, 570, 190), "Init", style))
    //    {
    //        InitAnim(PlayerAnimState.init);
    //    }
    //}
}
