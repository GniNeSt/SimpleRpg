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
    [SerializeField, Range(1, 3)] int _stageMonKind = 1;
    [SerializeField] Rank _minRank = Rank.Normal;
    [SerializeField] Rank _maxRank = Rank.Normal;
    [SerializeField] RoamingType _roamingType = RoamingType.RandomIndex;
    GameObject _prefabSpawnObj;

    [Header("변수")]
    [SerializeField]float _spawnTime;
    StatInformation _tempInfo;

    List<GameObject> _prefabSpawnList;
    List<Vector3> _roamingPositionList;

    private void Awake()
    {
        //기본 초기화 설정
        _prefabSpawnList = new List<GameObject>();
        SettingRoammingPos();
        //임시
        _prefabSpawnObj = Resources.Load("Prefabs/Objects/Catfish_N") as GameObject;

        //Debug.Log(TableManager._instance.Get(TableName.MonsterInfoTable).ToString(1001, "Name"));
        _tempInfo = new StatInformation(3,0, 5,30,100);
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
            mc.InitMonster(1, "CatFish", _tempInfo, _minRank, _roamingPositionList, _roamingType);

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
