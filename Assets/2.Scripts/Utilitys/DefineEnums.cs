using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineEnums
{
    public enum TableName
    {
        LevelUpTable,
        StageInfoTable,
        MonsterInfoTable
    }
    public enum PlayerAnimState
    {
        IDLE,
        RUN,
        ATTACK,
        SKILL,

        WALK,
        BATTLEIDLE,
        GOBACK,

        DEATH,
        init
    }

    public enum Rank
    {
        Normal,
        Rare,
        Elite,
        Boss
    }
}
