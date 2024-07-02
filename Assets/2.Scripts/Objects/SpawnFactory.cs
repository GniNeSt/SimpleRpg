using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFactory : MonoBehaviour
{
    //юс╫ц
    [SerializeField] MonsterCtrlObj _mon;

    private void Start()
    {
        SettingRoammingPos();
        _mon.setRommingPos(_roamingPositionList);
    }
    //

    List<Vector3> _roamingPositionList;
    void SettingRoammingPos()
    {
        Transform root = transform.GetChild(0);
        _roamingPositionList = new List<Vector3>();

        for(int n = 0; n < root.childCount; n++)
        {
            _roamingPositionList.Add(root.GetChild(n).position);
        }

    }
}
