using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class PenguinAcademy : MonoBehaviour
{
    private PenguinArea[] _penguinArea;

    private void Awake()
    {
        if (_penguinArea == null)
        {
            _penguinArea = FindObjectsOfType<PenguinArea>();
        }
    }
}
