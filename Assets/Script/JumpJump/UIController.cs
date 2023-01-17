using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private JumpJump _agentMovement;

    [SerializeField] private TextMeshProUGUI _distanceText;

    [SerializeField] private GameObject _results;
    [SerializeField] private TextMeshProUGUI _finalDistance;

    private void Awake()
    {
        if (_agentMovement == null)
        {
            _agentMovement = GameObject.Find("Player").GetComponent<JumpJump>();
        }
        if (_results == null)
        {
            _results = GameObject.Find("Results");
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
        _distanceText.text = distance + " m";

        if (_agentMovement.isDead)
        {
            _results.SetActive(true);
            _finalDistance.text = distance + " m";
        }
    }
    public void Quit()
    {
        SceneManager.LoadScene("JumpJump");
    }
    public void Retry()
    {
        SceneManager.LoadScene("JumpJump");
    }
}
