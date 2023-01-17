using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class BouncerTarget : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(1, 0, 0), 0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        var agent = other.gameObject.GetComponent<Agent>();
        if (other.CompareTag("Player"))
        {
            agent.AddReward(1f);
            Respawn();
        }
    }
    public void Respawn()
    {
        gameObject.transform.localPosition =
            new Vector3(
                (1 - 2 * Random.value) * 5f,
                2f + Random.value * 5f,
                (1 - 2 * Random.value) * 5f);
    }
}
