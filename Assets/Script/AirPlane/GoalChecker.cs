using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    [SerializeField] private GameObject _finalGoal;
    [SerializeField] private GameObject[] _goalObject;
    [SerializeField] private int _goalObjectMaxValue;
    [SerializeField] private float SearchTime;

    [SerializeField] private int _index = 0;

    public GameObject CurrObject;
    public int index => _index;
    public int goalObjectMaxValue => _goalObjectMaxValue;
    public GameObject[] goalObject => _goalObject;
    public GameObject FinalGoal => _finalGoal;
    private void Awake()
    {
        _goalObjectMaxValue = _goalObject.Length;
        for (int i = 0; i < _goalObject.Length; i++)
        {
            _goalObject[i].SetActive(false);
            _goalObject[_index].gameObject.SetActive(true);
        }
        CurrObject = _goalObject[_index];
    }
    private void Update()
    {
        StartCoroutine(SearchTarget());
    }
    public void ActiveOtherObject()
    {

        _goalObject[_index + 1].gameObject.SetActive(true);
        _goalObject[_index].gameObject.SetActive(false);
        _index++;
    }
    public void ResetGoal()
    {
        _index = 0;
        _goalObjectMaxValue = _goalObject.Length;
        for (int i = 0; i < _goalObject.Length; i++)
        {
            _goalObject[i].SetActive(false);
            _goalObject[_index].gameObject.SetActive(true);
        }
    }
    private IEnumerator SearchTarget()
    {
        yield return new WaitForSeconds(SearchTime);
        CurrObject = _goalObject[_index];
    }
}
