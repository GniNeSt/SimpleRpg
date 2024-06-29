using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TableBase
{
    Dictionary<string, Dictionary<string, string>> _tableDatas;

    public Dictionary<string, Dictionary<string, string>> _table
    {
        get { return _tableDatas; }
        set { _tableDatas = value; }
    }
    public int _maxCount { get { return _tableDatas.Count; } }

    public TableBase()
    {
        _tableDatas = new Dictionary<string, Dictionary<string, string>>();
    }
    public abstract bool Load(string txtData);

    public void Add(string key, string subKey, string val)
    {
        if(!_tableDatas.ContainsKey(key))
            _tableDatas.Add(key, new Dictionary<string, string>());

        if(!_tableDatas[key].ContainsKey(subKey))
            _tableDatas[key].Add(subKey, val);
    }

    public string ToString(int key, string subKey)
    {
        return ToString(key.ToString(), subKey);    //??
    }
    public string ToString(string key, string subKey)
    {
        string findValue = string.Empty;
        if(_tableDatas.ContainsKey(key))
            _tableDatas[key].TryGetValue(subKey, out findValue);

        return findValue;
    }
    public int ToInteger(int key, string subKey)
    {
        return ToInteger(key.ToString(), subKey);
    }
    public int ToInteger(string key, string subKey)
    {
        int findValue = int.MinValue;
        string result = ToString(key, subKey);
        if(result != string.Empty)
        {
            if (!int.TryParse(result, out findValue))
                findValue = int.MinValue;
        }

        return findValue;
    }
}
