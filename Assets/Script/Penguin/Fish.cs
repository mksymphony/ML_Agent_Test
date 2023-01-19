using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Tooltip("The swim speed")]
    public float fishSpeed;

    private float _randomizedSpeed = 0f;
    private float _nextActionTime = -1f;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _randomizedSpeed = fishSpeed + UnityEngine.Random.Range(0.1f, 0.5f);
    }
    private void Update()
    {
        if (fishSpeed > 0f)
        {
            Swim();
        }
    }

    private void Swim()
    {
        if (Time.deltaTime >= _nextActionTime)
        {


            _targetPosition = PenguinArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);

            transform.rotation = Quaternion.LookRotation(_targetPosition - transform.position, Vector3.up);

            var timeToGetThere = Vector3.Distance(transform.position, _targetPosition) / _randomizedSpeed;
            _nextActionTime = Time.deltaTime + timeToGetThere;
        }
        else
        {
            transform.position = _targetPosition;
            _nextActionTime = Time.deltaTime;
        }

    }
}
