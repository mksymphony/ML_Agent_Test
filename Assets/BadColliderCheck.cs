using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadColliderCheck : MonoBehaviour
{
    [SerializeField] private AdvaancedCollectorAgent agent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            Debug.Log("Wannig Platform set badPosition");
            agent.RewardSet(-1f);
            agent.EndEpisode();
        }
    }
}
