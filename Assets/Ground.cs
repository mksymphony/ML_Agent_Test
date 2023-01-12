using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private JumpJump _player;

    private float _groundHeight;
    private float _groundRight;
    private float _screenRight;

    [SerializeField] private BoxCollider2D _boxCollider;

    private bool didGenerateGround = false;
    public float groundHeight => _groundHeight;
    private void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.Find("Agent(Player)").GetComponent<JumpJump>();
        }

        _boxCollider = GetComponent<BoxCollider2D>();
        _groundHeight = gameObject.transform.position.y + _boxCollider.size.y / 2;
        _screenRight = Camera.main.transform.position.x * 2;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= _player.velocity.x * Time.fixedDeltaTime;

        _groundRight = transform.position.x + (_boxCollider.size.x / 2);

        if (_groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (_groundRight < _screenRight)
            {
                didGenerateGround = true;
                GenerateGround();
            }
        }
    }
    private void GenerateGround()
    {
        var game = Instantiate(gameObject);
        var goCollider = game.GetComponent<BoxCollider2D>();
        Vector2 pos;

        var h1 = _player.jumpVelocity * _player.maxHoldJumpTime;
        var t = _player.jumpVelocity / _player.gravity;
        var h2 = _player.jumpVelocity * t + (0.5f * (_player.gravity * (t * t)));
        var maxJumpHeight = h1 + h2;
        var maxY = _player.transform.position.y + maxJumpHeight;
        maxY *= 0.7f;
        var minY = 1;
        var actualY = Random.Range(minY, maxY) - goCollider.size.y / 2;

        pos.y = actualY;

        pos.x = _screenRight + 30;
        pos.y = transform.position.y;
        game.transform.position = pos;

        Ground goGround = game.GetComponent<Ground>();
        //goGround.groundHeight = goGround.transform.position.y + (goCollider.size.y / 2);
    }
}
