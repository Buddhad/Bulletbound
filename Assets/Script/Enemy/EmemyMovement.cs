using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public bool canJump = true;
    public LayerMask groundLayer;

    [Header("Control Settings")]
    public bool canMove = true; // ✅ Add this to toggle enemy movement

    // Private variables - no need to assign in inspector
    private Transform player;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private bool playerFound = false;

    void Start()
    {
        // FIXED: Remove the early return that prevents player finding
        FindPlayer();
        rb = GetComponent<Rigidbody2D>();
    }
    // ADDED: Dedicated method to find player
    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerFound = true;
            Debug.Log("Player found: " + playerObj.name);
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure player has 'Player' tag");
            playerFound = false;
        }
    }
    void Update()
    {
        // FIXED: Check game state and movement permission
        if (!GameStartTimer.GameStarted || !canMove) return;

        // FIXED: Check if player exists before using it
        if (!playerFound || player == null)
        {
            FindPlayer(); // Try to find player again
            return;
        }

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
