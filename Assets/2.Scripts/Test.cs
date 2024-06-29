using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefineEnums;
public class Test : MonoBehaviour
{
    Dictionary<string, Dictionary<string, string>> _levelUpTable;

    private void Start()
    {
        //_levelUpTable = new Dictionary<string, Dictionary<string, string>>();

        //TextAsset tAsset = Resources.Load("Tables/" + "LevelUpTable") as TextAsset;
        //LoadTable(tAsset.text);


        //string val = GetValueWithKey(4, "ATT");
        //Debug.Log(val);

        TableManager._instance.LoadAll();

        string re = TableManager._instance.Get(TableName.StageInfoTable).ToString(2, StageInfoTable.ColumnName.StageName.ToString());
        Debug.Log(re);
        re = TableManager._instance.Get(TableName.LevelUpTable).ToString(4, LevelUpTable.ColumnName.ATT.ToString());
        Debug.Log(re);
    }

    void LoadTable(string txtData)
    {
        txtData = txtData.Replace("\r", "");
        string[] field = txtData.Split('\n');
        List<string> columnNameList = GetColumnNames(field[0]);
        for(int n = 1; n < field.Length; n++)
        {
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            string[] vals = field[n].Split('∫');
            for(int m = 0; m < vals.Length; m++)
            {
                fieldData.Add(columnNameList[m], vals[m]);
            }
            _levelUpTable.Add(fieldData[columnNameList[0]], fieldData);
        }

    }
    private List<string> GetColumnNames(string str)
    {
        List<string> columnNames = new List<string>();
        string[] result = str.Split('∫');
        foreach(string s in result)
        {
            columnNames.Add(s);
        }
        return columnNames;
    }
    string GetValueWithKey(int key, string subKey)
    {
        string idx = key.ToString();
        if (!_levelUpTable.ContainsKey(idx))
            return idx + "번 인덱스는 없는 인덱스 입니다.";
        if (!_levelUpTable[idx].ContainsKey(subKey))
            return subKey + "컬럼은 이 테이블에 없는 컬럼입니다.";

        return _levelUpTable[idx][subKey];
    }
}
