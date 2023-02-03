using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    [SerializeField] private AirPlaneMovementAgent _agent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _agent.AddReward(-1f);
            _agent.EndEpisode();
        }
    }
}
