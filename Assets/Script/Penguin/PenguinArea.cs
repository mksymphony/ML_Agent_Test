using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class PenguinArea : MonoBehaviour
{
    [SerializeField] private PenguinAgent _penguinAgent;
    [SerializeField] public GameObject _penguinBaby;
    [SerializeField] private GameObject _fishPrefab;
    [SerializeField] private TextMeshProUGUI _cumulativeRewardText;

    #region FishSpeed
    public float fishSpeed = 0;
    #endregion

    #region FishRadius
    public float feedRadius = 0f;
    #endregion

    #region FishList
    private List<GameObject> _fishList = new List<GameObject>();
    #endregion

    public void ResetArea()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        SpawnFish(4, fishSpeed);
    }
    public void RemoveSpecificFish(GameObject fishObject)
    {
        _fishList.Remove(fishObject);
        Destroy(fishObject);
    }
    public int FishRemaining
    {
        get { return _fishList.Count; }
    }
    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        var radius = minRadius;

        if (maxRadius > minRadius)
        {
            radius = Random.Range(minRadius, maxRadius);
        }
        if (maxAngle > minAngle)
        {
            minAngle = Random.Range(minAngle, maxAngle);
        }
        return center + Quaternion.Euler(0f, Random.Range(minAngle, maxAngle), 0f) * Vector3.forward * radius;
    }
    private void RemoveAllFish()
    {
        if (_fishList != null)
        {
            for (int i = 0; i < _fishList.Count; i++)
            {
                if (_fishList[i] != null)
                {
                    Destroy(_fishList[i]);
                }
            }
        }
        else
            return;

        _fishList = new List<GameObject>();
    }

    private void PlacePenguin()
    {
        Rigidbody rigidbody = _penguinAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        _penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        _penguinAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void PlaceBaby()
    {
        Rigidbody rigidbody = _penguinBaby.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        _penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        _penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void SpawnFish(int count, float fishSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn and place the fish
            GameObject fishObject = Instantiate<GameObject>(_fishPrefab.gameObject);
            fishObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            fishObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            // Set the fish's parent to this area's transform
            fishObject.transform.SetParent(transform);

            // Keep track of the fish
            _fishList.Add(fishObject);

            // Set the fish speed
            fishObject.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }

    private void Start()
    {
        ResetArea();
    }
    private void Update()
    {
        _cumulativeRewardText.text = _penguinAgent.GetCumulativeReward().ToString();
    }
}