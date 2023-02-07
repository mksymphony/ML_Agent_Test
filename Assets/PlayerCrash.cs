using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    [SerializeField] private AirPlaneMovementAgent _agent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _agent = other.gameObject.GetComponent<AirPlaneMovementAgent>();
            _agent.AddReward(-1f);
            _agent.EndEpisode();
        }
    }
}
