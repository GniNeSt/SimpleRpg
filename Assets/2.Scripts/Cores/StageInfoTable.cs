using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class StageInfoTable : TableBase
{
    public enum ColumnName
    {
        Index,
        StageName,
        MapName,
        Rank,
        Size,

        max
    }
    public override bool Load(string txtData)
    {
        txtData = txtData.Replace("\r", "");
        string[] field = txtData.Split('\n');
        List<string> columnNameList = GetColumnNames(field[0]);
        //
        int count = (int)ColumnName.max;
        if (count == columnNameList.Count)
        {
            for (int n = 0; n < (int)ColumnName.max; n++)
            {
                string std = ((ColumnName)n).ToString();
                if (std.CompareTo(columnNameList[n]) != 0)
                {
                    return false;
                }
            }
        }
        else
            return false;

        //
        for (int n = 1; n < field.Length; n++)
        {
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            string[] vals = field[n].Split('¡ò');

            for (int m = 0; m < vals.Length; m++)
            {
                fieldData.Add(columnNameList[m], vals[m]);

                Add(vals[0], columnNameList[m], vals[m]);
            }
            //_levelUpTable.Add(fieldData[columnNameList[0]], fieldData);
        }
        return true;
    }

    private List<string> GetColumnNames(string str)
    {
        List<string> columnNames = new List<string>();
        string[] result = str.Split('¡ò');
        foreach (string s in result)
        {
            columnNames.Add(s);
        }
        return columnNames;
    }
}
