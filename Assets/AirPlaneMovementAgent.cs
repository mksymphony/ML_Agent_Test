using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AirPlaneMovementAgent : Agent
{
    [SerializeField] private GoalChecker _goalChecker;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private AirPlaneMovement _agent;

    public override void Initialize()
    {
        if (!_goalChecker)
            _goalChecker = GameObject.Find("Goal").GetComponent<GoalChecker>();

        _startPosition = transform.position;
        _startRotation = Quaternion.Euler(0, 0, 0);

        _agent = GetComponent<AirPlaneMovement>();
    }

    public override void OnEpisodeBegin()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _agent._throttle = -60;
        _goalChecker.ResetGoal();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var roll = Mathf.RoundToInt(Input.GetAxis("Roll"));
        var pitch = Mathf.RoundToInt(Input.GetAxis("Pitch"));
        var yaw = Mathf.RoundToInt(Input.GetAxis("Yaw"));

        var EngineOn = Input.GetKey(KeyCode.Space);
        var EngineOff = Input.GetKey(KeyCode.LeftShift);

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = roll >= 0 ? roll : 2;
        actions[1] = pitch >= 0 ? pitch : 2;
        actions[2] = yaw >= 0 ? yaw : 2;
        actions[3] = EngineOn ? 1 : 0;
        actions[4] = EngineOff ? 1 : 0;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        MapLimits();
        SearchTarget();

        var roll = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        var pitch = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        var yaw = actions.DiscreteActions[2] <= 1 ? actions.DiscreteActions[2] : -1;

        var EngineOn = actions.DiscreteActions[3] > 0;
        var EngineOff = actions.DiscreteActions[4] > 0;

        _agent.rolldInput = roll;
        _agent.pitchInput = pitch;
        _agent.yawInput = yaw;
        _agent.EngineStart = EngineOn;
        _agent.EngineOff = EngineOff;

        //AddReward(-1f / MaxStep);
    }

    private void SearchTarget()
    {
        var currobj = _goalChecker.CurrObject;
        var sinceDistance = Vector3.Distance(currobj.transform.position, transform.position);
        if (sinceDistance > 1f)
        {
            AddReward(-1 / MaxStep);
        }

    }

    private void MapLimits()
    {
        if (transform.position.z > 500f)
        {
            AddReward(-1f);
            EndEpisode();
        }
        else if (transform.position.z < -500f)
        {
            AddReward(-1f);
            EndEpisode();
        }
        if (transform.position.x > 500f)
        {
            AddReward(-1f);
            EndEpisode();
        }
        else if (transform.position.x < -500f)
        {
            AddReward(-1f);
            EndEpisode();
        }
        if (transform.position.y > 150f)
        {
            AddReward(-1f);
            EndEpisode();
        }
        else if (transform.position.y < -10f)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
