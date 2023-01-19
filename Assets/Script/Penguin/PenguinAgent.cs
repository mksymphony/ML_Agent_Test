using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class PenguinAgent : Agent
{
    [Tooltip("Agent의 움직임 속도")]
    private float _moveSpeed = 5f;

    [Tooltip("Agent의 회전 속도")]
    private float _turnSpeed = 180f;

    [SerializeField]
    [Tooltip("아기 팽귄의 음식이 나타낼때")]
    private GameObject heartPrefab;

    [SerializeField]
    [Tooltip("")]
    private GameObject regurgitatedFishPrefab;

    [SerializeField] private PenguinArea _penguinArea;
    private Rigidbody _rigid;
    public GameObject _baby;
    private bool _isFull;


    public override void Initialize()
    {
       // _penguinArea = GetComponent<PenguinArea>();
        //_baby = _penguinArea._penguinBaby;
        _rigid = GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var forwardAmount = actions.DiscreteActions[0];

        var turnAmount = 0f;

        if (actions.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actions.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        _rigid.MovePosition(transform.position + transform.forward * forwardAmount * _moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * _turnSpeed * Time.fixedDeltaTime);

        if (MaxStep > 0)
            AddReward(-1f / MaxStep);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var forwardAction = 0;
        var turnAction = 0;

        if (Input.GetKey(KeyCode.W))
        {
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnAction = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnAction = 2;
        }
        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }

    public override void OnEpisodeBegin()
    {
        _isFull = false;
        _penguinArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_isFull);

        sensor.AddObservation(Vector3.Distance(_baby.transform.position, transform.position));

        sensor.AddObservation(transform.forward);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("fish"))
        {
            EatFish(col.gameObject);
        }
        else if (col.transform.CompareTag("baby"))
        {
            RegurgitateFish();
        }
    }

    private void RegurgitateFish()
    {
        if (!_isFull)
            return;
        _isFull = false;

        var regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);

        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = _baby.transform.position;
        Destroy(regurgitatedFish, 4f);

        AddReward(1f);

        if (_penguinArea.FishRemaining <= 0)
            EndEpisode();
    }

    private void EatFish(GameObject ob)
    {
        if (_isFull)
        {
            return;
        }
        _isFull = true;

        _penguinArea.RemoveSpecificFish(ob);

        AddReward(1f);
    }
}
