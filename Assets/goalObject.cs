using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalObject : MonoBehaviour
{
    [SerializeField] private GoalChecker _checker;
    [SerializeField] private AirPlaneMovementAgent _agent;

    private void Awake()
    {
        if (_checker == null)
            _checker = GameObject.Find("Goal").GetComponent<GoalChecker>();

        if (_agent == null)
            _agent = GameObject.Find("AirPlaneAgent").GetComponent<AirPlaneMovementAgent>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _checker.ActiveOtherObject();
            _agent.AddReward(0.5f);
        }
    }
}
