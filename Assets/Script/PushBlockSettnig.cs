using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockSettnig : MonoBehaviour
{
    [SerializeField] public float _agentRunSpeed;

    [SerializeField] public float _agentRotationSpeed;

    [SerializeField] public float _spawnAreaMarginMultiplier;

    [SerializeField] public Material _goalScoredMaterial;

    [SerializeField] public Material failMaterial;
}
