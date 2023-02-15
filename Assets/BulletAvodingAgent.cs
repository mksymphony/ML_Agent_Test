using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.AI;

public class BulletAvodingAgent : Agent
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private SimpleCharacterController _characterController;

    private Rigidbody _rid;

    public override void Initialize()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<SimpleCharacterController>();
        _rid = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360));
        _rid.velocity = Vector3.zero;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        var horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (Vector3.Distance(_startPosition, transform.position) > 10f)
        {
            AddReward(-1f);
            EndEpisode();
        }

        var vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        var horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;

        _characterController.ForwardInput = vertical;
        _characterController.TurnInput = horizontal;
        AddReward(-1 / MaxStep);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Name")
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
