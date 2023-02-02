using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalObject : MonoBehaviour
{
    [SerializeField] private GoalChecker _checker;

    private void Awake()
    {
        if (_checker == null)
        {
            _checker = GameObject.Find("Goal").GetComponent<GoalChecker>();
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _checker.ActiveOtherObject();
        }
    }
}
