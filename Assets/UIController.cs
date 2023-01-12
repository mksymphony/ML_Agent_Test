using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private JumpJump _agentMovement;

    [SerializeField] private TextMeshProUGUI _distanceText;

    private void Awake()
    {
        if (_agentMovement == null)
        {
            _agentMovement = GameObject.Find("Player").GetComponent<JumpJump>();
        }
    }
    private void Update()
    {
        SetScoreDistance();
    }

    private void SetScoreDistance()
    {
        var distance = Mathf.FloorToInt(_agentMovement.distance);
        _distanceText.text = distance + " m";
    }
}
