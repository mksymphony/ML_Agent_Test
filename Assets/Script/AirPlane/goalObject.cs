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
            if (_checker.index == _checker.goalObject.Length - 1)
            {
                FinalGoalObjectActive();
            }
            _checker.ActiveOtherObject();
            _agent.AddReward(0.5f);
        }
    }

    private void FinalGoalObjectActive()
    {
        for (int i = 0; i < _checker.goalObject.Length; i++)
        {
            _checker.goalObject[i].SetActive(false);
        }
        _agent.AddReward(1f);
        _agent.EndEpisode();
        _checker.FinalGoal.SetActive(true);
    }
}
