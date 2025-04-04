using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AirPlaneMovement : MonoBehaviour
{
    [Header("Plane State")]

    [Tooltip("How much the throttle ramps up or down.")]
    [SerializeField] private float _throttleIncrement = 0.1f;

    [Tooltip("Maximum engine thrust when at 100% throttles.")]
    [SerializeField] private float _maxThrottle = 200f;

    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    [SerializeField] private float _responsiveness = 10f;

    [SerializeField] private Transform _propella;

    [SerializeField] public float _throttle;

    [SerializeField] private TextMeshProUGUI Throttle;
    [SerializeField] private TextMeshProUGUI AirSpeed;
    [SerializeField] private TextMeshProUGUI Altitude;

    public float rolldInput { get; set; }
    public float pitchInput { get; set; }
    public float yawInput { get; set; }
    public bool EngineStart { get; set; }
    public bool EngineOff { get; set; }


    private float _responseModifier
    {
        get
        {
            return (_rid.mass / 10f) * _responsiveness;
        }
    }

    private Rigidbody _rid;

    private void Awake()
    {
        _rid = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        HandleInputs();
        UpdateHUD();
        Propella();
    }
    private void HandleInputs()
    {
        if (EngineStart)
            _throttle += _throttleIncrement * Time.deltaTime;

        else if (EngineOff)
            _throttle -= _throttleIncrement * Time.deltaTime;
    }
    public void AddForce()
    {
        if (_throttle >= _maxThrottle)
        {
            _throttle = _maxThrottle;
        }
        if (_throttle <= 0)
            _throttle = 0;
        _rid.AddForce(transform.forward * _maxThrottle * _throttle);
        _rid.AddTorque(-transform.up * yawInput * _responseModifier);
        _rid.AddTorque(transform.right * pitchInput * _responseModifier);
        _rid.AddTorque(transform.forward * rolldInput * _responseModifier);

        _rid.AddForce(Vector3.up * _rid.velocity.magnitude * 135);
    }
    private void UpdateHUD()
    {
        if (!Throttle && !AirSpeed && !Altitude)
            return;
        Throttle.text = "Throttle" + _throttle.ToString("F0") + "%";
        AirSpeed.text = "Airspeed :" + (_rid.velocity.magnitude * 3.6f).ToString("F0") + "km/h";
        Altitude.text = "Altitude :" + transform.position.y.ToString("F0") + "m";
    }

    private void Propella()
    {
        _propella.Rotate(Vector3.forward * _throttle);
    }

    private void FixedUpdate()
    {
        AddForce();
    }
}
