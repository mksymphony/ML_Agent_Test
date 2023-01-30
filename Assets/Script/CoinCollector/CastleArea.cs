using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.MLAgents;
using UnityEditor.Tilemaps;
using UnityEngine;

public class CastleArea : MonoBehaviour
{
    public List<GameObject> collectibles;
    [SerializeField] private GameObject _block;
    [Range(0, 10)][SerializeField] private float _blockOffset = 10f;

    private Vector3 _originalBlockPosition;
    private Quaternion _originalBlockRotation;

    private void Awake()
    {
        _originalBlockPosition = _block.transform.position;
        _originalBlockRotation = _block.transform.rotation;
        _block.transform.position += Vector3.forward * Academy.Instance.EnvironmentParameters.GetWithDefault("block_offset", _blockOffset);
    }

    public void ResetArea()
    {
        foreach (var col in collectibles)
            col.SetActive(true);

        _block.transform.position = _originalBlockPosition + Vector3.forward * Academy.Instance.EnvironmentParameters.GetWithDefault("block_offset", _blockOffset);
        _block.transform.rotation = _originalBlockRotation;
    }

    public void Collect(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
