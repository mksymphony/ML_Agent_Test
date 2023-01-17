using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private JumpJump _player;

    private void Awake()
    {
        if (!_player)
            _player = GameObject.Find("Agent(Player)").GetComponent<JumpJump>();

    }
    private void Start()
    {


    }
    private void FixedUpdate()
    {

        Vector2 pos = transform.position;

        pos.x -= _player.velocity.x * Time.fixedDeltaTime;

        if (pos.x < -50)
        {
            Destroy(gameObject);
        }

        transform.position = pos;
    }
}
