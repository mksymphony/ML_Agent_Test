using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockColliderCheck : MonoBehaviour
{
    [SerializeField] private AdvaancedCollectorAgent agent;
    private bool _isTrue = false;

    public void SetCheck()
    {
        _isTrue = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Block") && !_isTrue)
        {
            Debug.Log("BlockCheck");
            agent.Reward();
            _isTrue = true;
        }
    }
}
