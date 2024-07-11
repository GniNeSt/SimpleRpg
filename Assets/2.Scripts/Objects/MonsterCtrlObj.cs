using DefineEnums;
using DefineUtilitys;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterCtrlObj : CharacterBase
{
    [Header("Parameter")]
    [SerializeField] float _walkSpeed = 0.9f;
    [SerializeField] float _runSpeed = 4f;
    [SerializeField] float _nonCombatRotAngle = 120;
    [SerializeField] float _battleRotAngle = 270f;
    [SerializeField] float _sightRange = 10;
    [SerializeField] float _attackRange = 3;
    [Header("Sub Parameter")]
    [SerializeField] float _minWaitTime = 3;
    [SerializeField] float _maxWaitTime = 10;
    [SerializeField] float _waitRate = 30;  //Idle È®·ü
    [SerializeField] float _attackDelay = 1.5f; //°ø°Ý ÈÄ °ø°Ý µô·¹ÀÌ
    [SerializeField] float _followDistance = 15f;   //state goback ¼Óµµ, ¾Ö´Ï¸ÞÀÌ¼Ç 2¹è
    bool FirstAttack;
    Vector3 _AttackStartPos;
    int _level;
    Rank _rank;
    PlayerAnimState _nowState;
    RoamingType _roamType;
    int _nextPosIndex;
    float _remainingTime;   //idle check time   randomtime + Time.time
    bool _isSelectedAct;    //time

    bool _inorder;
    MiniMonInfoWnd _miniMoninfo;
    NavMeshAgent _navAgent;
    Animator _aniController;
    SightCheckerObj _sightChecker;
    Transform _seePos;
    List<Vector3> _goalPosList;
    Transform _targetObj;
    DamageCheckerObj[] _damageZoneObj;
    //private void Awake()
    //{
    //    //ÀÓ½Ã
    //    _navAgent = GetComponent<NavMeshAgent>();
    //    _aniController = GetComponent<Animator>();
    //    _sightChecker = GetComponentInChildren<SightCheckerObj>();
    //    int cnt = transform.GetChild(2).GetChild(2).childCount;
    //    _damageZoneObj = new DamageCheckerObj[cnt];
    //    for (int n = 0; n < cnt; n++)
    //    {
    //        _damageZoneObj[n] = transform.GetChild(2).GetChild(2).GetChild(n).GetComponent<DamageCheckerObj>();
    //        _damageZoneObj[n].InitSet(n, this);
    //        _damageZoneObj[n].EnableChecker(false);
    //    }
    //    _seePos = transform.GetChild(2).GetChild(1);
    //    _sightChecker.InitSet(this, _sightRange, _seePos);

    //    //setRommingPos(RoamingType.RandomIndex);
    //    //==
    //}
    private void Update()
    {
        if (_isDead) return;

        if (_targetObj != null)
        {
            
            if (_isSelectedAct)
            {
                switch (_nowState)
                {
                    case PlayerAnimState.RUN:
                        if(Vector3.Distance(transform.position, _targetObj.position) <= _attackRange)
                        {
                            //Debug.Log(_targetObj.name);
                            _isSelectedAct = false;
                            animatorChange(PlayerAnimState.ATTACK);
                        }
                        else
                        {
                            _navAgent.SetDestination(_targetObj.position);
                        }
                        break;
                    case PlayerAnimState.ATTACK:

                        animatorChange(PlayerAnimState.BATTLEIDLE);
                        
                        break;
                    case PlayerAnimState.BATTLEIDLE:
                        //Debug.Log(Time.time - _remainingTime);
                        transform.LookAt(_targetObj.position);
                        if (_remainingTime <= Time.time)
                        {
                            Debug.Log("AttackCoolEnd");
                            animatorChange(PlayerAnimState.RUN);
                            _isSelectedAct = false;
                        }
                        break;

                }
            }
            else
            {
                switch (_nowState)
                {
                    case PlayerAnimState.RUN:
                        _navAgent.SetDestination(_targetObj.position);
                        _isSelectedAct = true;
                        break;
                    case PlayerAnimState.ATTACK:
                        _remainingTime = Time.time + _attackDelay;
                        _isSelectedAct = true;
                        break;
                }

            }
            if(Vector3.Distance(transform.position, _AttackStartPos) > _followDistance)
            {
                _targetObj = null;
                FirstAttack = false;
                Debug.Log("flollowDistacneEXit");

                animatorChange(PlayerAnimState.GOBACK);
                //sightCheckerobj.enablechecker();
                _navAgent.SetDestination(_goalPosList[_nextPosIndex]);
                _isSelectedAct = true;
            }
        }
        else
        {
            if (_isSelectedAct)
            {
                switch (_nowState)
                {
                    case PlayerAnimState.IDLE:
                        if (_remainingTime - Time.time <= 0)    //µô·¹ÀÌ Å¸ÀÓ
                        {
                            _isSelectedAct = false;
                        }
                        break;
                    case PlayerAnimState.WALK:
                        if (_navAgent.remainingDistance < 0.1f)
                        {
                            _isSelectedAct = false;
                        }
                        break;
                    case PlayerAnimState.GOBACK:
                        if (_navAgent.remainingDistance < 0.1f)
                        {
                            Debug.Log("gobakc");
                            _isSelectedAct = false;

                        }
                        break;
                }
            }
            else if (_navAgent.remainingDistance < 0.1f)
            {
                float idleEvent = Random.Range(0, 100);
                if (idleEvent < _waitRate)  //È®·ü
                {    //µô·¹ÀÌ
                    float idleWaitTime = Random.Range(_minWaitTime, _maxWaitTime);  //µô·¹ÀÌ Å¸ÀÓ ·£´ý
                    Debug.LogFormat("È®·ü ÃßÃ· : {0}, ´ë±â ½Ã°£ : {1}", idleEvent, idleWaitTime);
                    _remainingTime = Time.time + idleWaitTime;
                    //idle anim
                    animatorChange(PlayerAnimState.IDLE);
                }
                else
                {
                    Debug.LogFormat("È®·ü ÃßÃ· : {0}", idleEvent);

                    _nextPosIndex = GetNextIndex(_nextPosIndex);
                    _navAgent.SetDestination(_goalPosList[_nextPosIndex]);
                    animatorChange(PlayerAnimState.WALK);
                }
                _isSelectedAct = true;

            }
        }
    }
    void AllisableZone()
    {
        for (int n = 0; n < _damageZoneObj.Length; n++)
        {
            _damageZoneObj[n].EnableChecker(false);
        }
    }
    void EnableZone(int id)
    {
        _damageZoneObj[id-1].EnableChecker(true);
    }
    void DisableZone(int id)
    {
        _damageZoneObj[id-1].EnableChecker(false);
    }
    void SetAttackType()
    {
        int type = Random.Range(0, _damageZoneObj.Length);
        //_aniController.SetInteger("AttackType", type);
    }
    int GetNextIndex(int nowIndex)
    {
        int index = nowIndex;
        switch (_roamType)
        {
            case RoamingType.RandomIndex:
                index = Random.Range(0, _goalPosList.Count);
                break;
            case RoamingType.Inorder:
                if (++index >= _goalPosList.Count)
                    index = 0;
                break;
            case RoamingType.BackNForth:
                if (_inorder)
                {
                    if (++index >= _goalPosList.Count - 1)
                    {
                        //Debug.Log("Inorder " + _inorder + " End!");
                        _inorder = false;
                    }
                }
                else
                {
                    if (--index <= 0)
                    {
                        //Debug.Log("Inorder " + _inorder + " End!");
                        _inorder = true;
                    }
                }
                break;

        }
        //Debug.Log("index : " + index);
        return index;
    }

    public void InitMonster(int level, string name, StatInformation info, Rank rank, List<Vector3> posList, RoamingType type)
    {
        _miniMoninfo = transform.GetChild(3).GetComponent<MiniMonInfoWnd>();
        _miniMoninfo.InitOpen(name);
        InitSetStat(name, info);

        _level = level;
        _rank = rank;
        _roamType = type;
        _navAgent = GetComponent<NavMeshAgent>();
        _aniController = GetComponent<Animator>();
        _sightChecker = GetComponentInChildren<SightCheckerObj>();
        setRommingPos(posList, type);
        _seePos = transform.GetChild(2).GetChild(1);
        int cnt = transform.GetChild(2).GetChild(2).childCount;
        _damageZoneObj = new DamageCheckerObj[cnt];
        for (int n = 0; n < cnt; n++)
        {
            _damageZoneObj[n] = transform.GetChild(2).GetChild(2).GetChild(n).GetComponent<DamageCheckerObj>();
            _damageZoneObj[n].InitSet(n, this);
            _damageZoneObj[n].EnableChecker(false);
        }
        _sightChecker.InitSet(this, _sightRange, _seePos);
    }
    public void OnDetect(Transform target)
    {
        if (_nowState == PlayerAnimState.GOBACK) return;
        _targetObj = target;
        //Debug.Log(target.name);
        if (FirstAttack) return;
        _AttackStartPos = transform.position;
        if (Vector3.Distance(transform.position, _targetObj.position) > _attackRange)
        {
            animatorChange(PlayerAnimState.RUN);
            _navAgent.SetDestination(_targetObj.position);
        }
        else
        {
            //SetAttackType();
            _navAgent.destination=_targetObj.position;
            animatorChange(PlayerAnimState.ATTACK);
        }
        FirstAttack = true;
    }
    public override void OnHitting(CharacterBase attacker)
    {
        //_nowHp -= attacker.Attack;
        _nowHp -= 1;
        float rate = (float)_nowHp / (float)_baseStat._charHP;
        _miniMoninfo.SetHpRate(rate);

        if(_nowHp <= 0)
        {
            animatorChange(PlayerAnimState.DEATH);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 4f);
        }
    }

    void animatorChange(PlayerAnimState curAnim, bool isWait = false)
    {
        if (_isDead) return;
        AllisableZone();
        _nowState = curAnim;
        _aniController.SetBool("Battle", false);
        _aniController.SetBool("Walk", false);
        _aniController.SetBool("Run", false);
        _aniController.SetBool("Attack", false);
        _aniController.speed = 1;
        switch (curAnim)
        {
            case PlayerAnimState.DEATH:
                _aniController.SetTrigger("Die");
                _isDead = true;
                break;
            case PlayerAnimState.IDLE:
                break;
            case PlayerAnimState.BATTLEIDLE:
                _aniController.SetBool("Battle", true);
                break;
            case PlayerAnimState.ATTACK:
                int rand = Random.Range(1, 3);
                _aniController.Play("attack" + rand);
                _aniController.SetBool("Battle", true);
                _aniController.SetBool("Attack", true);
                break;
            case PlayerAnimState.WALK:
                _aniController.SetBool("Walk", true);
                _navAgent.speed = _walkSpeed;
                _navAgent.angularSpeed = _nonCombatRotAngle;
                _navAgent.stoppingDistance = 0;
                break;
            case PlayerAnimState.RUN:
                _aniController.SetBool("Run", true);
                _navAgent.speed = _runSpeed;
                _navAgent.angularSpeed = _battleRotAngle;
                _navAgent.stoppingDistance = _attackRange + 0.5f;
                break;
            case PlayerAnimState.init:
                _aniController.Play("idle");
                break;
            case PlayerAnimState.GOBACK:
                _aniController.SetBool("Run", true);
                _aniController.speed *= 2;
                _navAgent.speed = _runSpeed * 2;
                _navAgent.angularSpeed = _battleRotAngle;
                _navAgent.stoppingDistance = 0;
                break;

        }
    }

    //public void ExcchangeActionToAni(PlayerAnimState state)
    //{
    //    if (_isDead) return;
    //    AllisableZone();
    //    switch (state)
    //    {
    //        case PlayerAnimState.DEATH:
    //            _aniController.SetTrigger("Dead");
    //            break;
    //        case PlayerAnimState.WALK:
    //            _navAgent.speed = _walkSpeed;
    //            _navAgent.angularSpeed = _nonCombatRotAngle;
    //            _navAgent.stoppingDistance = 0;
    //            break;
    //        case PlayerAnimState.RUN:
    //            _navAgent.speed = _runSpeed;
    //            _navAgent.angularSpeed = _battleRotAngle;
    //            _navAgent.stoppingDistance = _attackRange + 0.5f;
    //            break;
    //    }

    //    _aniController.SetInteger("AniState", (int)state);

    //    _nowState = state;
    //}
    public void setRommingPos(List<Vector3> posList , RoamingType type)
    {
        //ÀÓ½Ã
        _roamType = type;
        //
        _goalPosList = posList;
        if(_roamType == RoamingType.RandomIndex)
        {
            _nextPosIndex = GetNextIndex(_nextPosIndex);
            transform.position = _goalPosList[_nextPosIndex];
        }
        else
            transform.position = _goalPosList[_nextPosIndex++];
        
        _navAgent.SetDestination(_goalPosList[_nextPosIndex]);
    }
    private void OnGUI()
    {

        //GUIStyle style = new GUIStyle("button");
        //style.fontSize = 60;
        //if (GUI.Button(new Rect(0, 0, 570, 190), "RandomIndex", style))
        //{
        //    _roamType = RoamingType.RandomIndex;
        //}
        //if (GUI.Button(new Rect(0, 190, 570, 190), "Inorder", style))
        //{
        //    _roamType = RoamingType.Inorder;
        //}
        //if (GUI.Button(new Rect(0, 380, 570, 190), "BackNForth", style))
        //{
        //    _roamType = RoamingType.BackNForth;
        //}
        //if (GUI.Button(new Rect(0, 0, 570, 190), "Idle", style))
        //{
        //    animatorChange(PlayerAnimState.init);

        //}
        //if (GUI.Button(new Rect(0, 190, 570, 190), "Run", style))
        //{
        //    animatorChange(PlayerAnimState.RUN);
        //}
        //if (GUI.Button(new Rect(0, 380, 570, 190), "Battle", style))
        //{
        //    animatorChange(PlayerAnimState.BATTLEIDLE);
        //}
        //if (GUI.Button(new Rect(0, 570, 570, 190), "Attack", style))
        //{
        //    animatorChange(PlayerAnimState.ATTACK);
        //}
        //if (GUI.Button(new Rect(0, 760, 570, 190), "Death", style))
        //{
        //    animatorChange(PlayerAnimState.DEATH);
        //}
        //if (GUI.Button(new Rect(0, 950, 570, 190), "Walk", style))
        //{
        //    animatorChange(PlayerAnimState.WALK);
        //}
    }

}
