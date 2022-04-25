using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Range(1, 10)] public float jumpVelocity;

    public LayerMask Ground;
    public Collider2D GroundCheckCollider;
    public bool _isGrounded;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
    }

   

    void FixedUpdate()
    {
        _isGrounded = GroundCheckCollider.IsTouchingLayers(Ground.value);
    }

    private void Update()
    {
       
            if (Input.GetButtonDown("Jump") && IsGrounded )
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
            }
        
    }
}
