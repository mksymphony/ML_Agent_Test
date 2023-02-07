using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform[] _povs;
    [SerializeField] private float _speed;

    private int _index = 0;
    private Vector3 _target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            _index = 2;

        _target = _povs[_index].position;
    }
    private void FixedUpdate()
    {
        PositionSet();
    }
    private void PositionSet()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.fixedDeltaTime * _speed);
        transform.forward = _povs[_index].forward;
    }
}
