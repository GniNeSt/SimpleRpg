using DefineEnums;
using System.Collections.Generic;
using UnityEngine;
using DefineUtilitys;
public class SpawnFactory : MonoBehaviour
{
    [Header("Factory 정보")]
    [SerializeField] int _maxSpawnMonCount = 20;
    [SerializeField] int _limitTurnGenCount = 1;
    [SerializeField] float _generateDelayTime = 10;
    [SerializeField] int _generationOnce = 1;
    [Header("몬스터 정보")]
    [SerializeField, Range(1, 4)] int _stageMonKind = 1;
    //[SerializeField] Rank _minRank = Rank.Normal;
    //[SerializeField] Rank _maxRank = Rank.Normal;
    [SerializeField] RoamingType _roamingType = RoamingType.RandomIndex;
    GameObject _prefabSpawnObj;

    AvatarInformatin monInfo;

    [Header("변수")]
    [SerializeField]float _spawnTime;
    StatInformation _tempInfo;

    List<GameObject> _prefabSpawnList;
    List<Vector3> _roamingPositionList;

    private void Awake()
    {
        ////기본 초기화 설정
        //_prefabSpawnList = new List<GameObject>();
        //SettingRoammingPos();
        ////임시
        //_prefabSpawnObj = Resources.Load("Prefabs/Objects/Catfish_N") as GameObject;

        ////Debug.Log(TableManager._instance.Get(TableName.MonsterInfoTable).ToString(1001, "Name"));
        //_tempInfo = new StatInformation(3,0, 5,30,100);
    }
    private void Start()
    {
        InitSetData();
    }
    public void InitSetData()
    {
        _prefabSpawnList = new List<GameObject>();
        monInfo = new AvatarInformatin();

        string GenMonsterID = "GenMonsterID" + _stageMonKind;
        Debug.Log(GenMonsterID);
        int monID = TableManager._instance.Get(TableName.StageInfoTable).ToInteger(1, GenMonsterID);
        Debug.Log("monID : "+monID);
        TableBase table = TableManager._instance.Get(TableName.MonsterInfoTable);

        string prefabName = table.ToString(monID, "FileName");
        Debug.Log(prefabName);
        _prefabSpawnObj = Resources.Load("Prefabs/Objects/"+prefabName) as GameObject;
        
        monInfo._charName = table.ToString(monID, "Name");
        monInfo._charLevel = table.ToInteger(monID, "Level");
        monInfo._stat._charAtt = table.ToInteger(monID, "ATT");
        monInfo._stat._charHit = table.ToInteger(monID, "HIT");
        monInfo._stat._charHP = table.ToInteger(monID, "HP");
        monInfo._stat._charDef = table.ToInteger(monID, "DEF");
        monInfo._stat._charAvd = table.ToInteger(monID, "AVD");
        
        SettingRoammingPos();
    }
    private void Update()
    {
        if (_maxSpawnMonCount <= 0) return;
        foreach (var prefabSpawn in _prefabSpawnList)
        {
            if (prefabSpawn == null)
            {
                _prefabSpawnList.Remove(prefabSpawn);
                break;
            }
        }

        if (_prefabSpawnList.Count < _limitTurnGenCount)
        {
            _spawnTime +=Time.deltaTime;
            if(_spawnTime > _generateDelayTime)
            {
                spawnMonster();
                _spawnTime = 0;
            }
        }
    }
    void spawnMonster()
    {
        for (int i = 0; i < _generationOnce; i++)
        {
            if (_maxSpawnMonCount <= 0 || _prefabSpawnList.Count >= _limitTurnGenCount) break;
            
            GameObject go = Instantiate(_prefabSpawnObj);
            MonsterCtrlObj mc = go.GetComponent<MonsterCtrlObj>();
            //몬스터 초기화
            mc.InitMonster(1, "CatFish", _tempInfo, Rank.Normal, _roamingPositionList, _roamingType);
            mc.InitMonster(monInfo._charLevel, monInfo._charName,monInfo._stat,/*임시*/ Rank.Normal, _roamingPositionList, _roamingType);
            _prefabSpawnList.Add(go);
            _maxSpawnMonCount--;
        }
    }
    void SettingRoammingPos()
    {
        Transform root = transform.GetChild(0);
        _roamingPositionList = new List<Vector3>();

        for (int n = 0; n < root.childCount; n++)
        {
            _roamingPositionList.Add(root.GetChild(n).position);
        }

    }
    Rank CheckRankFormStr(string rank)
    {
        Rank r = Rank.Normal;
        for(int n = 0; n < (int)Rank.Boss; n++)
        {
            r = ((Rank)n);
            if (rank.CompareTo(r.ToString()) == 0)
                break;
        }
        return r;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle("button");
        style.fontSize = 60;
        if (GUI.Button(new Rect(0, 0, 570, 190), "spawn", style))
        {
            spawnMonster();
        }
    }
}
