using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private JumpJump _player;
    [SerializeField] private Obstacle _obstacle;

    private float _groundHeight;
    private float _groundRight;
    private float _screenRight;

    [SerializeField] private BoxCollider2D _boxCollider;

    private bool didGenerateGround = false;
    public float groundHeight => _groundHeight;

    [SerializeField] private float _time;
    private float _currTime;

    private bool _timeEnd = false;

    private void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.Find("Agent(Player)").GetComponent<JumpJump>();
        }

        _boxCollider = GetComponent<BoxCollider2D>();
        _groundHeight = gameObject.transform.position.y + _boxCollider.size.y / 2;
        _screenRight = Camera.main.transform.position.x;
    }

    private void Update()
    {
        _currTime += Time.deltaTime;
        if (_currTime > _time)
        {
            _timeEnd = true;
        }

    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= _player.velocity.x * Time.fixedDeltaTime;

        _groundRight = transform.position.x + (_boxCollider.size.x / 2);

        if (_groundRight < -30f)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (_timeEnd)
            {
                didGenerateGround = true;
                GenerateGround();
                _timeEnd = false;
            }
        }
    }
    private void GenerateGround()
    {
        var ob = Instantiate(gameObject);
        var goCollider = ob.GetComponent<BoxCollider2D>();
        Vector2 pos;

        var actualY = Random.Range(-7, 2f);

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2f)
            pos.y = 2f;

        var actualX = Random.Range(31, 35);

        pos.x = actualX + goCollider.size.x;
        ob.transform.position = pos;

        Ground goGround = goCollider.GetComponent<Ground>();
        goGround._groundHeight = ob.transform.position.y + (goCollider.size.y / 2);

        var obstacleNum = Random.Range(0, 3);
        for (int i = 0; i < obstacleNum; i++)
        {
            var box = Instantiate(_obstacle.gameObject);
            var y = goGround._groundHeight;
            var halfWidth = goCollider.size.x / 2 - 1;
            var left = ob.transform.position.x - halfWidth;
            var right = ob.transform.position.x + halfWidth;

            var x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }
    }
}
