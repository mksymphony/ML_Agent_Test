using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class JumpJump : Agent
{
    [SerializeField] private Vector2 _velocity;

    [SerializeField] private float _maxAcceleration = 10;
    [SerializeField] private float _acceleration = 10;
    [SerializeField] private float _maxXVelocity = 50f;
    [SerializeField] private float _distance = 0;

    [SerializeField] private float _gravity;
    [SerializeField] public float _groundHeight = 10;
    [SerializeField] private float _jumpVelocity = 20;

    [SerializeField] private float _maxHoldJumpTime = 0.4f;
    [SerializeField] private float _holdJumpTimer = 0f;
    [SerializeField] private float _jumpGroundThreshold = 1f;

    [SerializeField] private bool _isGround = false;
    [SerializeField] private bool _isHoldingJump = false;
    private bool _isDead = false;

    private float _maxMaxHoldJumpTime = 0.4f;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private GroundFall _fall;

    public float distance => _distance;
    public Vector2 velocity => _velocity;
    public float jumpVelocity => _jumpVelocity;
    public float maxHoldJumpTime => _maxHoldJumpTime;
    public float gravity => _gravity;
    public bool isDead => _isDead;

    private void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - _groundHeight);
        if (_isGround || groundDistance <= _jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isGround = false;
                _velocity.y = _jumpVelocity;
                _isHoldingJump = true;
                _holdJumpTimer = 0;

                if (_fall != null)
                {
                    _fall.player = null;
                    _fall = null;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isHoldingJump = false;
        }
    }
    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (this.gameObject.transform.position.y <= -12)
        {
            _isDead = true;
            Destroy(gameObject);
        }

        if (!_isGround)
        {
            if (_isHoldingJump)
            {
                _holdJumpTimer += Time.fixedDeltaTime;
                if (_holdJumpTimer >= _maxHoldJumpTime)
                {
                    _isHoldingJump = false;
                }
            }

            pos.y += _velocity.y * Time.fixedDeltaTime;
            if (!_isHoldingJump)
            {
                _velocity.y += _gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = _velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, _groundLayerMask);

            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();

                if (ground != null)
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        _groundHeight = ground.groundHeight;
                        pos.y = _groundHeight;
                        _velocity.y = 0;
                        _isGround = true;
                    }
                    _fall = ground.GetComponent<GroundFall>();
                    if (_fall != null)
                    {
                        _fall.player = this;
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, _velocity.x * Time.fixedDeltaTime, _groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        _velocity.x = 0;
                    }
                }
            }

        }

        _distance += _velocity.x * Time.fixedDeltaTime;

        if (_isGround)
        {
            float velocityRatio = _velocity.x / _maxXVelocity;
            _acceleration = _maxAcceleration * (1 - velocityRatio);
            _maxMaxHoldJumpTime = _maxMaxHoldJumpTime * velocityRatio;

            _velocity.x += _acceleration * Time.fixedDeltaTime;

            if (_velocity.x >= _maxXVelocity)
            {
                _velocity.x = _maxXVelocity;
            }

            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = _velocity.y * Time.fixedDeltaTime;

            if (_fall != null)
            {
                rayDistance = -_fall.fallSpeed * Time.fixedDeltaTime;
            }

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);

            if (hit2D.collider == null)
            {
                _isGround = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }
        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitx = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, _obstacleLayerMask);
        if (obstHitx.collider != null)
        {
            Obstacle obstacle = obstHitx.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                HitObstacle(obstacle);
                AddReward(-1f);
            }
        }

        RaycastHit2D obstHity = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.x * Time.fixedDeltaTime, _obstacleLayerMask);
        if (obstHity.collider != null)
        {
            Obstacle obstacle = obstHity.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                HitObstacle(obstacle);
                AddReward(-1f);
            }
        }

        transform.position = pos;
    }

    private void HitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        _velocity.x *= 0.7f;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (this.gameObject.transform.position.y <= -12)
        {
            EndEpisode();
            AddReward(-1f);
        }
        else
        {
            AddReward(0.1F);
        }
        Vector2 obstOrigin = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        RaycastHit2D obstHitx = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, _obstacleLayerMask);
        if (obstHitx.collider != null)
        {
            Obstacle obstacle = obstHitx.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                HitObstacle(obstacle);
                AddReward(-1f);
            }
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.Space))
        {
            continuousActionsOut[0] = 0;
            continuousActionsOut[1] = 1;
        }

    }
}
