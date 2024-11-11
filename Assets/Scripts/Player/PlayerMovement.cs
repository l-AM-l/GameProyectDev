using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     
    public float jumpForce = 5f;     
    public Transform groundCheck;    
    public LayerMask groundLayer;    
    private Rigidbody2D rb;          
    private bool isGrounded;         
    private bool canJump = true;     
    private float currentSpeed;      

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
        // Detecta si el personaje está en el suelo.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Procesa el salto
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal con suavizado
        float targetSpeed = Input.GetAxis("Horizontal") * moveSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.1f); // Ajusta el factor de suavizado (0.1f) según tus preferencias
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
