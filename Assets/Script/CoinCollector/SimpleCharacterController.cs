using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    [SerializeField][Range(5f, 60f)] private float _slopeLimit = 35f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 300f;
    [SerializeField] private bool _allowJump = false;
    [SerializeField] private float _jumpSpeed = 4f;
    private Rigidbody _rigid;
    private CapsuleCollider _capsuleCollider;

    public bool IsGrounded { get; private set; }
    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }
    public bool JumpInput { get; set; }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void FixedUpdate()
    {
        CheckGrounded();
        ProcessActions();
    }
    private void CheckGrounded()
    {
        IsGrounded = false;
        var capsuleHeight = Mathf.Max(_capsuleCollider.radius * 2f, _capsuleCollider.height);
        var capsuleBottom = transform.TransformPoint(_capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        var radius = transform.TransformVector(_capsuleCollider.radius, 0f, 0f).magnitude;
        var ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            var normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < _slopeLimit)
            {
                var maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + 0.02f;
                if (hit.distance < maxDist)
                {
                    IsGrounded = true;
                }
            }
        }
    }
    private void ProcessActions()
    {
        if (TurnInput != 0f)
        {
            var angle = Mathf.Clamp(TurnInput, -1f, 1f) * _turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }
        if (IsGrounded)
        {
            _rigid.velocity = Vector3.zero;

            if (JumpInput && _allowJump)
            {
                _rigid.velocity += Vector3.up * _jumpSpeed;
            }

            _rigid.velocity += transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * _moveSpeed;
        }
        else
        {
            if (!Mathf.Approximately(ForwardInput, 0f))
            {
                var verticalVelocity = Vector3.Project(_rigid.velocity, Vector3.up);
                _rigid.velocity = verticalVelocity + transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * _moveSpeed / 2f;
            }
        }
    }
}
