using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetect : MonoBehaviour
{
    [SerializeField] public PushAgent _agent;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("goal"))
        {
            Debug.Log("Collision Enter");
            _agent.ScoredAGoal();
        }
    }
}
