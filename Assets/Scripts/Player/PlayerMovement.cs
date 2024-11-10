using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  
    public Transform groundCheck;
    public float jumpForce = 5f;    
    public LayerMask groundLayer;    
    private Rigidbody2D rb;         
    private bool isGrounded;        
    private bool canJump = true;     

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        string currentScene = SceneManager.GetActiveScene().name;

       
        if (currentScene == "BaseScene")
        {
            canJump = false;  
        }
        else
        {
            canJump = true;   
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            Jump();
        }
    }

    //fixedupdate

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

   
    
}

