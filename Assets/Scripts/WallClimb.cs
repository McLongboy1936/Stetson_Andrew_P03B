using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    public LayerMask groundLayer;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public float collisionRadius;
    public Vector2 groundOffSet;
    public Vector2 rightOffSet;
    public Vector2 leftOffSet;

    public Color gizmoCOlor = Color.red;
    public int side;

    public void Update()
    {
        //IS GORUNDED DOES NOT WORK, COULD SWAP W OTHER IS GROUNDED
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + groundOffSet, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffSet, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffSet, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffSet, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffSet, collisionRadius, groundLayer);
        side = onRightWall ? 1 : -1;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoCOlor;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundOffSet, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffSet, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffSet, collisionRadius);


    }


}
