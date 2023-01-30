using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AdvaancedCollectorAgent : Agent
{
    private Vector3 _startPosition;
    private SimpleCharacterController _characterController;
    new private Rigidbody _rigidbody;
    [SerializeField] private CastleArea _castleArea;
    [SerializeField] private BlockColliderCheck block;

    public override void Initialize()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<SimpleCharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        _rigidbody.velocity = Vector3.zero;

        _castleArea.ResetArea();
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
        if (Vector3.Distance(_startPosition, transform.position) > 20f)
        {
            block.SetCheck();
            AddReward(-1f);
            EndEpisode();
        }

        var vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        var horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        var jump = actions.DiscreteActions[2] > 0;

        _characterController.ForwardInput = vertical;
        _characterController.TurnInput = horizontal;
        _characterController.JumpInput = jump;

        if (vertical > 0f)
            AddReward(0.2f / MaxStep);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectible")
        {
            AddReward(1f / _castleArea.collectibles.Count);
            _castleArea.Collect(other.gameObject);
        }
        else if (other.tag == "goal")
        {
            block.SetCheck();
            AddReward(1f);
            EndEpisode();
        }
    }
    public void Reward()
    {
        AddReward(0.1f);
    }
}
