using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    [SerializeField] private Transform[] SetPoints;

    [SerializeField] private float _turnSpeed;
    [SerializeField] private GameObject _target;
    [SerializeField] private Transform _turnPoints;
    //[SerializeField] private Rigidbody _rid;

    public float turnInput { get; private set; }
    public float switchPoint { get; private set; }
    public bool shotInput { get; private set; }



    private void Awake()
    {

    }
    private void Update()
    {
        SearchTarget();
    }

    private void SearchTarget()
    {
        if (turnInput != 0)
        {
            var angle = Mathf.Clamp(turnInput, -1f, 1f) * _turnSpeed;
            _turnPoints.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }
        var dir = _target.transform.position - transform.position;
        var lookRotation = Quaternion.LookRotation(dir);
        var rotation = lookRotation.eulerAngles;
        _turnPoints.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
