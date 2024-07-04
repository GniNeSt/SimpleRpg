using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCheckerObj : MonoBehaviour
{
    BoxCollider _checker;
    CharacterBase _owner;
    int _index;
    public void InitSet(int id, CharacterBase charBase)
    {
        _index = id;
        _owner = charBase;
        _checker = GetComponent<BoxCollider>();
    }
    public void EnableChecker(bool isOn)
    {
        _checker.enabled = isOn;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dummy"))
        {
            Debug.LogFormat("{0}�� ���������� [{1}]�浹�Ͽ����ϴ�.", _index, other.transform.name);
        }
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterCtrlObj>().OnHitting(transform.parent.GetComponentInParent<PlayerCtrlObj>());
        }
        if (other.CompareTag("Player"))
        {

        }
    }
}
