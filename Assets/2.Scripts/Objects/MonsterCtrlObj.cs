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

    int _level;
    Rank _rank;
    PlayerAnimState _nowState;
    RoamingType _roamType;
    int _nextPosIndex;
    float _remainingTime;   //idle check time   randomtime + Time.time
    bool _isSelectedAct;    //time

    bool _inorder;

    NavMeshAgent _navAgent;
    Animator _aniController;
    SightCheckerObj _sightChecker;
    Transform _seePos;
    List<Vector3> _goalPosList;
    private void Awake()
    {
        //ÀÓ½Ã
        _navAgent = GetComponent<NavMeshAgent>();
        _aniController = GetComponent<Animator>();
        _sightChecker = GetComponentInChildren<SightCheckerObj>();
        _seePos = transform.GetChild(2).GetChild(1);
        _sightChecker.InitSet(this, _sightRange, _seePos);
        //==
    }
    private void Update()
    {
        if (_isDead) return;

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

    public void InitMonster(int level, string name, StatInformation info, Rank rank, RoamingType type)
    {
        InitSetStat(name, info);

        _level = level;
        _rank = rank;

        _navAgent = GetComponent<NavMeshAgent>();
        _aniController = GetComponent<Animator>();
        _sightChecker = GetComponentInChildren<SightCheckerObj>();
        _seePos = transform.GetChild(2).GetChild(1);
        _sightChecker.InitSet(this, _sightRange, _seePos);
    }
    public override void OnHitting(CharacterBase attacker)
    {

    }

    void animatorChange(PlayerAnimState curAnim, bool isWait = false)
    {
        _nowState = curAnim;
        _aniController.SetBool("Battle", false);
        _aniController.SetBool("Walk", false);
        _aniController.SetBool("Run", false);
        _aniController.SetBool("Attack", false);
        switch (curAnim)
        {
            case PlayerAnimState.DEATH:
                _aniController.SetTrigger("Die");
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
        }
    }

    public void ExcchangeActionToAni(PlayerAnimState state)
    {
        if (_isDead) return;

        switch (state)
        {
            case PlayerAnimState.DEATH:
                _aniController.SetTrigger("Dead");
                break;
            case PlayerAnimState.WALK:
                _navAgent.speed = _walkSpeed;
                _navAgent.angularSpeed = _nonCombatRotAngle;
                _navAgent.stoppingDistance = 0;
                break;
            case PlayerAnimState.RUN:
                _navAgent.speed = _runSpeed;
                _navAgent.angularSpeed = _battleRotAngle;
                _navAgent.stoppingDistance = _attackRange + 0.5f;
                break;
        }

        _aniController.SetInteger("AniState", (int)state);

        _nowState = state;
    }
    public void setRommingPos(List<Vector3> posList)
    {
        _goalPosList = posList;
        transform.position = _goalPosList[_nextPosIndex++];
        _navAgent.SetDestination(_goalPosList[_nextPosIndex]);
    }
    private void OnGUI()
    {

        GUIStyle style = new GUIStyle("button");
        style.fontSize = 60;
        if (GUI.Button(new Rect(0, 0, 570, 190), "RandomIndex", style))
        {
            _roamType = RoamingType.RandomIndex;
        }
        if (GUI.Button(new Rect(0, 190, 570, 190), "Inorder", style))
        {
            _roamType = RoamingType.Inorder;
        }
        if (GUI.Button(new Rect(0, 380, 570, 190), "BackNForth", style))
        {
            _roamType = RoamingType.BackNForth;
        }
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
