using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineEnums;
using DefineUtilitys;
public class MonsterCtrlObj : CharacterBase
{
    [Header("Parameter")]
    [SerializeField] float _walkSpeed = 0.9f;
    [SerializeField] float _runSpeed = 4f;
    [SerializeField] float _nonCombatRotAngle = 120;
    [SerializeField] float _battleRotAngle = 270f;
    [SerializeField] float _sightRange = 10;
    [SerializeField] float _attackRange = 3;

    int _level;
    Rank _rank;
    PlayerAnimState _nowState;


    NavMeshAgent _navAgent;
    Animator _aniController;
    private void Awake()
    {
        //юс╫ц
        _aniController = GetComponent<Animator>();
        //==
    }
    public void InitMonster(int level, string name,StatInformation info, Rank rank)
    {
        InitSetStat(name, info);
        
        _level = level;
        _rank = rank;

        _navAgent = GetComponent<NavMeshAgent>();
        _aniController = GetComponent<Animator>();
    }
    public override void OnHitting(CharacterBase attacker)
    {

    }

    void animatorChange(PlayerAnimState curAnim, bool isWait = false)
    {

        _aniController.SetBool("Battle", false);
        _aniController.SetBool("Walk", false);
        _aniController.SetBool("Run", false);
        _aniController.SetBool("Attack", false);
        switch (curAnim)
        {
            case PlayerAnimState.DEATH:
                _aniController.SetTrigger("Die");
                break;
            case PlayerAnimState.BATTLEIDLE:
                _aniController.SetBool("Battle", true);
                break;
            case PlayerAnimState.ATTACK:
                _aniController.SetBool("Battle", true);
                _aniController.SetBool("Attack",true);
                break;
            case PlayerAnimState.WALK:
                _aniController.SetBool("Walk", true);
                break;
            case PlayerAnimState.RUN:
                _aniController.SetBool("Run", true);
                break;
            case PlayerAnimState.init:
                _aniController.Play("idle");
                break;
        }
    }
    

    private void OnGUI()
    {

        GUIStyle style = new GUIStyle("button");
        style.fontSize = 60;

        if (GUI.Button(new Rect(0, 0, 570, 190), "Idle", style))
        {
            animatorChange(PlayerAnimState.init);

        }
        if (GUI.Button(new Rect(0, 190, 570, 190), "Run", style))
        {
            animatorChange(PlayerAnimState.RUN);
        }
        if (GUI.Button(new Rect(0, 380, 570, 190), "Battle", style))
        {
            animatorChange(PlayerAnimState.BATTLEIDLE);
        }
        if (GUI.Button(new Rect(0, 570, 570, 190), "Attack", style))
        {
            animatorChange(PlayerAnimState.ATTACK);
        }
        if (GUI.Button(new Rect(0, 760, 570, 190), "Death", style))
        {
            animatorChange(PlayerAnimState.DEATH);
        }
        if (GUI.Button(new Rect(0, 950, 570, 190), "Walk", style))
        {
            animatorChange(PlayerAnimState.WALK);
        }
    }

}
