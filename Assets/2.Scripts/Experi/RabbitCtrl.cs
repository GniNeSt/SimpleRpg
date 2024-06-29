using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RabbitCtrl : MonoBehaviour
{
    [SerializeReference] Transform _anchorTransform;
    [SerializeReference] Vector3 _separationDistance;    //1,0,0
    GameObject _modelObj;
    Vector3 _objVelocity;
    [SerializeField]float _curTime;
    bool _moveFlag;
    Vector3 _targetPos
    {
        get { return VectorYKiller(_anchorTransform.position + _separationDistance); }
    }
    private Vector3 VectorYKiller(Vector3 vec)
    {
        return vec - Vector3.up * vec.z;
    }
    private void Awake()
    {
        _modelObj = transform.parent.gameObject;
        _objVelocity = _modelObj.GetComponent<Rigidbody>().velocity;
    }

    private void Update()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, -transform.up, out hit, 6.0f) && !_moveFlag)
        {
            Vector3 hitPos = hit.point;

            gameObject.transform.position = hitPos;
        }
        else
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, _targetPos, 0.01f);
            if(Vector3.Distance(transform.position, _targetPos) < 0.1f)
            {
                _moveFlag = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!_moveFlag)
        {
            _curTime+=Time.deltaTime;
            if(_curTime > 3f)
            {
                _moveFlag = true;
            }
        }
    }
}
