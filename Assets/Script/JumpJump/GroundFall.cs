using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    private bool _ShoudFall = false;

    private float _fallSpeed;


    private List<Obstacle> _obstacles = new List<Obstacle>();

    public List<Obstacle> obstacle => _obstacles;
    public float fallSpeed => _fallSpeed;
    public JumpJump player;
    private void Awake()
    {
        _fallSpeed = Random.Range(0.02f, 0.05f);
    }

    private void FixedUpdate()
    {
        if (_ShoudFall)
        {

            Vector2 pos = transform.position;
            var fallAmount = _fallSpeed * Time.fixedDeltaTime;
            pos.y -= _fallSpeed;

            if (player != null)
            {
                player._groundHeight -= fallAmount;
                Vector2 playerPos = player.transform.position;
                playerPos.y -= fallAmount;
                player.transform.position = playerPos;
            }

            foreach (Obstacle o in _obstacles)
            {
                if (o != null)
                {
                    var oPos = o.transform.position;
                    oPos.y -= fallAmount;
                    o.transform.position = oPos;
                }
            }

            transform.position = pos;
        }
        else
        {
            if (player != null)
            {
                _ShoudFall = true;
            }
        }
    }
}
