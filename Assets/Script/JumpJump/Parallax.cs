using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _depth = 1;
    [SerializeField] private float _maxDistance = 25;

    [SerializeField] private JumpJump _player;

    private void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.Find("Agent(Player)").GetComponent<JumpJump>();
        }
        if (_player.isDead == true)
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (_player.isDead == true)
            Destroy(gameObject);
        var realVelocity = _player.velocity.x / _depth;
        Vector3 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x <= -_maxDistance)
            pos.x = _maxDistance;

        transform.position = pos;
    }
}
