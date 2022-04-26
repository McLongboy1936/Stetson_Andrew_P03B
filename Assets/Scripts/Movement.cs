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


    //repeat code from Jump
    public LayerMask Ground;
    public Collider2D GroundCheckCollider;
    public bool _isGrounded;
    

    //AUDIO CODE
    [SerializeField] AudioClip jumpsfx = null;
    AudioSource audiosource = null;

    void PlayFeedback()
    {
        if (audiosource != null && jumpsfx != null)
        {
            audiosource.clip = jumpsfx;
            audiosource.Play();
            Debug.Log("audio is working");
        }
    }


    void FixedUpdate()
    {
        _isGrounded = GroundCheckCollider.IsTouchingLayers(Ground.value);
    }

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
    }


    public Rigidbody2D rb;

    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 50;

    public int side = 1;


 

    //new
    private Collision collision;
    public WallClimb script;
    public Jump JumpScript;

 

    //me trying to get the isGroundedbool from jump
    //public  jumpScript;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        script = GetComponent<WallClimb>();
        JumpScript = GetComponent<Jump>();

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
            {
                Dash(xRaw, yRaw);
                Debug.Log("2nd dashing condition is true");
            }
                
            

        }

 
        //grab
        if (script.onWall && Input.GetButton("Grab"))
        {
            Debug.Log("Grab is working");
            wallGrabing = true;
            wallSlide = false;
        }
        //slide
        if (script.onWall && Input.GetButton("Grab"))
        {
            if (!wallGrabing)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (script.onWall && Input.GetButtonDown("Jump"))
        {
            WallJump();
        }

        if (script.onWall || !script.onGround)
        {
            Debug.Log("Grab is working");
            wallGrabing = false;
            wallSlide = false;

        }
        if (wallGrabing)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2 (rb.velocity.x,0);
            float speed = y > 0 ? 0.35f : 1;
            rb.velocity = new Vector2(rb.velocity.x, y * speed);
            Debug.Log("Currently grabbing the wall");
        }
        else
        {
            rb.gravityScale = 3;
        }
   

        DashReseter();
        Debug.Log("chungus");
        Move(dir);
    }

    private void WallJump()
    {
        if ((side == 1 && script.onRightWall) || side == -1 && !script.onRightWall)
        {
            side *= -1;
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = script.onRightWall ? Vector2.left : Vector2.right;
        WallJumpPusher((Vector2.up / 1.5f + wallDir / 1.5f), true);

        Debug.Log("WallJumping");


        wallJumped = true;
    }
    private void WallSlide()
    {
        /*
        if (coll.wallSide != side)
            anim.Flip(side * -1);
        */
        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && script.onRightWall) || (rb.velocity.x < 0 && script.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }
    private void Dash(float x, float y)
    {
        PlayFeedback();
        hasDashed = true;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed*2;
        StartCoroutine(DashWait());
    }

    private void WallJumpPusher(Vector2 dir, bool wall)
    {


        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
        

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

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

    }
    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (script.onGround)
            hasDashed = false;
    }

}
