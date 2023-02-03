using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    [SerializeField] private GameObject[] _goalObject;
    [SerializeField] private int _goalObjectMaxValue;

    private int _index = 0;
    private void Awake()
    {
        _goalObjectMaxValue = _goalObject.Length;
        for (int i = 0; i < _goalObject.Length; i++)
        {
            _goalObject[i].SetActive(false);
            _goalObject[_index].gameObject.SetActive(true);
        }
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
}
