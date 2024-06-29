using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;
public class TableManager : TSingleTon<TableManager>
{
    Dictionary<TableName, TableBase> _tableList;
    protected override void Init()
    {
        base.Init();
        _tableList = new Dictionary<TableName, TableBase>();
    }
    TableBase Load<T>(TableName name)where T: TableBase, new()
    {
        if (_tableList.ContainsKey(name))
        {
            TableBase tbase = _tableList[name];
            return tbase;
        }

        string path = "Tables/";
        TextAsset tAsset = Resources.Load(path + name) as TextAsset;
        if (tAsset != null)
        {
            T t = new T();
            t.Load(tAsset.text);
            _tableList.Add(name, t);
        }
        else
            return null;

        return _tableList[name];
    }
    public void LoadAll()
    {
        if(Load<LevelUpTable>(TableName.LevelUpTable) == null)
        {
            Debug.LogError(TableName.LevelUpTable + "로드 실패!!");
        }
        if (Load<StageInfoTable>(TableName.StageInfoTable) == null)
        {
            Debug.LogError(TableName.StageInfoTable + "로드 실패!!");
        }
    }
    public TableBase Get(TableName name)
    {
        if(_tableList.ContainsKey(name))
            return _tableList[name];
        return null;
    }
}
