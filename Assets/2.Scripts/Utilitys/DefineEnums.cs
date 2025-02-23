using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineEnums
{
    public enum mapName
    {        
        StartsIslandWest            =1,
        StartsIslandSouth
    }
    public enum SceneName
    {
        IngameScene,
    }
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
    public enum RoamingType
    {
        RandomIndex,
        Inorder,
        BackNForth
    }
}
