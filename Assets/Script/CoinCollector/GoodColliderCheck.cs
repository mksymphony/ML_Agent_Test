using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodColliderCheck : MonoBehaviour
{
    [SerializeField] private AdvaancedCollectorAgent agent;
    [SerializeField] private CastleArea castle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            Debug.Log("GoodPosition");
            agent.RewardSet(0.2f / agent.MaxStep);
            castle.SetOnCoin();
        }
    }
}
