using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCheckerObj : MonoBehaviour
{
    MonsterCtrlObj _owner;
    SphereCollider _checkCollider;
    Transform _seePos;
    public void InitSet(MonsterCtrlObj owner, float sightDist, Transform see)
    {
        _owner = owner;
        _seePos = see;
        _checkCollider = GetComponent<SphereCollider>();
        _checkCollider.radius = sightDist;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dir = other.transform.position - transform.position.normalized;
            Ray r = new Ray(_seePos.position, dir);
            RaycastHit rHit;
            if(Physics.Raycast(r, out rHit, _checkCollider.radius))
            {
                if (rHit.transform.CompareTag("Player"))
                {
                    // ¿¸≈ı
                }
            }
        }
    }
}
