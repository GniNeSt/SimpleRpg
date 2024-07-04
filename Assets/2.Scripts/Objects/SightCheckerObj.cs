using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCheckerObj : MonoBehaviour
{
    MonsterCtrlObj _owner;
    SphereCollider _checkCollider;
    Transform _seePos;

    Vector3 gizmoDirection;
    Vector3 gizmoPos;
    Transform target;

    public void InitSet(MonsterCtrlObj owner, float sightDist, Transform see)
    {
        _owner = owner;
        _seePos = see;
        _checkCollider = GetComponent<SphereCollider>();
        _checkCollider.radius = sightDist;
    }
    public void EnableChecek()
    {
        _checkCollider.enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dir = other.transform.GetChild(0).position - _seePos.position;
            RaycastHit rHit;
            if(Physics.Raycast(_seePos.position, dir, out rHit, _checkCollider.radius))
            {
                if (rHit.transform.CompareTag("Player"))
                {
                    _owner.OnDetect(rHit.transform);
                    //checkcollider.enabled=false;
                }
            }
        }
        
    }

}
