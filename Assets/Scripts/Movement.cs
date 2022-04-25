using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Bools and variables
    public bool canMove;
    public bool wallGrabing;
    public bool wallJumped = false;
    public bool wallSlide;
    public bool isDashing;
    private bool hasDashed;


    public Rigidbody2D rb;

    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    //repeat code from Jump
    public LayerMask Ground;
    public Collider2D GroundCheckCollider;
    public bool _isGrounded;

    //new
    private Collision collision;
    public WallClimb script;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
    }

    //me trying to get the isGroundedbool from jump
    //public  jumpScript;

    // Start is called before the first frame update
    void Start()
    {
        script = GetComponent<WallClimb>();

        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        //jumpScript = GetComponent<Jump>;
    }

    // Update is called once per frame
    void Update()
    {
        //creates variables to asign to player positon
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            Debug.Log("first dashing condition is true");
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
            Debug.Log("2nd dashing condition is true");

        }

 

        if (script.onWall && Input.GetButton("Grab"))
        {
            Debug.Log("Grab is working");
        }

        DashReseter();
        Debug.Log("chungus");
        Move(dir);
    }
    private void Dash(float x, float y)
    {

        hasDashed = true;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    private void Move(Vector2 dir)
    {

            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        
    }
    private void DashReseter()
    {
        if (IsGrounded) {
            hasDashed = false;
            Debug.Log("Player Is grounded and Dash Reseter is being called");
        }
            
    }

    IEnumerator DashWait()
    {
        
        rb.gravityScale = 0;
        GetComponent<CartooningTheJUmp>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 3;
        GetComponent<CartooningTheJUmp>().enabled = true;
        wallJumped = false;
        isDashing = false;
        Debug.Log("DashWait Is being called");
    }
}
