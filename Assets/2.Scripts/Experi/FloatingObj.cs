using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObj : MonoBehaviour
{
    [SerializeReference] Transform _anchorTransform;
    [SerializeReference] Vector3 _separationDistance;    //0,0,-1f

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _anchorTransform.localPosition + _separationDistance, 0.01f);
    }
}
