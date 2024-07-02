using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGizmos : MonoBehaviour
{
    [SerializeField] Color _drawColor = Color.yellow;
    [SerializeField] float _radius = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = _drawColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
