using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class SimpleCollectorAgent : Agent
{
    [SerializeField] private GameObject _platform;

    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private SimpleCharacterController _characterController;
    private Rigidbody _rigid;

    public override void Initialize()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<SimpleCharacterController>();
        _rigid = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        _rigid.velocity = Vector3.zero;

        _platform.transform.position = _startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * 5f;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        var horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        var jump = Input.GetKey(KeyCode.Space);

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = jump ? 1 : 0;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (Vector3.Distance(_startPosition, transform.position) > 10f)
        {
            AddReward(-1f);
            EndEpisode();
        }

        var vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1f;
        var horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1f;
        var jump = actions.DiscreteActions[2] > 0;

        _characterController.ForwardInput = vertical;
        _characterController.TurnInput = horizontal;
        _characterController.JumpInput = jump;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Collectible")
        {
            AddReward(1f);
            EndEpisode();
        }
    }
}
