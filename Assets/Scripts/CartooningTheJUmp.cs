using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartooningTheJUmp : MonoBehaviour
{
    private Rigidbody2D rb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            Debug.Log("y is less than 0");
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            Debug.Log("y is greater than 0");

        }
    }

    
}