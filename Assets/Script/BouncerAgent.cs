using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BouncerAgent : Agent
{
    public GameObject target;
    public GameObject agentObject;
    public float strength = 350f;

    private Rigidbody _rigid;
    private Vector3 _orientation;
    private float _jumpCoolDown;
    private int totalJumps = 20;
    private int jumpsLeft = 20;

    EnvironmentParameters defaultParams;

    public override void Initialize()
    {
        _rigid = gameObject.GetComponent<Rigidbody>();
        _orientation = Vector3.zero;
        defaultParams = Academy.Instance.EnvironmentParameters;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.transform.position);
        sensor.AddObservation(agentObject.transform.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorActio = actions.DiscreteActions;
        for (var i = 0; i < actions.ContinuousActions.Length; i++)
        {
            vectorActio[i] = (int)Mathf.Clamp(actions.ContinuousActions[i], -1f, 1f);
        }

        float x = vectorActio[0];
        float y = ScaleAction(vectorActio[1], 0, 1);
        float z = vectorActio[2];
        _rigid.AddForce(new Vector3(x, y + 1, z) * strength);

        AddReward(-0.05f * (vectorActio[0] * vectorActio[0] +
            vectorActio[1] * vectorActio[1] +
            vectorActio[2] * vectorActio[2]) / 3f);

        _orientation = new Vector3(x, y, z);

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        discreteActionsOut[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f), 0.51f) && _jumpCoolDown <= 0f)
        {
            RequestDecision();
            _jumpCoolDown -= 1f;
            _rigid.velocity = default(Vector3);
        }

        _jumpCoolDown -= Time.fixedDeltaTime;

        if (gameObject.transform.position.y < -1)
        {
            AddReward(-1f);
            EndEpisode();
            return;
        }
        if (gameObject.transform.localPosition.x < -17 ||
            gameObject.transform.localPosition.x > 17 ||
            gameObject.transform.localPosition.z < -17 ||
            gameObject.transform.localPosition.z > 17)
        {
            AddReward(-1f);
            EndEpisode();
            return;
        }
        if (jumpsLeft == 0)
        {
            EndEpisode();
        }

    }
    private void Update()
    {
        if (_orientation.magnitude > float.Epsilon)
        {
            agentObject.transform.rotation = Quaternion.Lerp(agentObject.transform.rotation,
                Quaternion.LookRotation(_orientation), Time.deltaTime * 10f);
        }
    }
    public override void OnEpisodeBegin()
    {
        gameObject.transform.localPosition = new Vector3((
            1 - 2 * Random.value) * 5, 2, (1 - 2 * Random.value) * 5);
        _rigid.velocity = Vector3.zero;

        //var environment = gameObject.transform.parent.gameObject;
        //var targets = environment.GetComponentInChildren<BouncerTarget>();

        jumpsLeft = totalJumps;

        ResetParamters();

    }
    public void ResetParamters()
    {
        var targetScale = defaultParams.GetWithDefault("target_scale", 1.0f);
        target.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
