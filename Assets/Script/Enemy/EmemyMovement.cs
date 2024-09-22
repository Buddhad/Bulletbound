using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmemyMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public bool canJump = true;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true; // Assuming the enemy starts facing right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        // Calculate the direction towards the player
        Vector3 direction = player.position - transform.position;

        // Normalize the direction to ensure constant speed
        direction.Normalize();

        // Check if the enemy needs to flip
        if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // Move the enemy horizontally
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Jump if grounded and the player is above
        if (canJump && isGrounded && direction.y > 0)
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }
}
