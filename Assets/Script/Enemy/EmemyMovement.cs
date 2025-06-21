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
    private bool facingRight = true;

    public bool canMove = true; // ✅ Add this to toggle enemy movement

    void Start()
    {
        if (!GameStartTimer.GameStarted || !canMove) return; // ✅ Prevent movement during intro
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (!canMove || player == null) return; // ✅ Prevent movement during intro

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // Flip direction
        if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // Move
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Optional jump
        if (canJump && isGrounded && direction.y > 0)
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }
}
