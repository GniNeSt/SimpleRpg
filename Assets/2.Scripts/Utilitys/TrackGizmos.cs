using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGizmos : MonoBehaviour
{
    [SerializeField] Color _lineColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Transform[] points = GetComponentsInChildren<Transform>();
        
        Gizmos.color = _lineColor;

        int nextIndex = 1;

        Vector3 currPos = points[nextIndex].position;

        Vector3 nextPos;
        for(int n = 0; n <= points.Length; n++)
        {
            nextPos = (++nextIndex >= points.Length) ? points[1].position : points[nextIndex].position;
            Gizmos.DrawLine(currPos, nextPos);
            currPos = nextPos;
        }
    }

}
