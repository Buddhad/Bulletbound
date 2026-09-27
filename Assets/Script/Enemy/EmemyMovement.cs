using UnityEngine;

public class EmemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public bool canJump = true;
    public LayerMask groundLayer;

    [Header("Separation Settings")]
    public LayerMask zombieLayer;
    public float separationRadius = 0.7f;
    public float separationStrength = 2f;

    [Header("Control Settings")]
    public bool canMove = true;

    private Transform player;
    private Rigidbody2D rb;

    private bool isGrounded;
    private bool facingRight = true;
    private bool playerFound = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerFound = true;
        }
        else
        {
            Debug.LogWarning(
                "Player not found! Make sure player has the 'Player' tag."
            );

            playerFound = false;
        }
    }

    void Update()
    {
        if (!GameStartTimer.GameStarted || !canMove)
            return;

        if (!playerFound || player == null)
        {
            FindPlayer();
            return;
        }

        // Ground check
        isGrounded = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            0.1f,
            groundLayer
        );

        // Direction toward player
        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        // Calculate separation from nearby zombies
        Vector2 separation = GetSeparation();

        // Combine chasing + separation
        float finalX =
            direction.x * speed +
            separation.x * separationStrength;

        // Flip
        if ((finalX > 0 && !facingRight) ||
            (finalX < 0 && facingRight))
        {
            facingRight = !facingRight;

            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }

        // Jump toward player
        if (canJump && isGrounded && direction.y > 0.2f)
        {
            rb.AddForce(
                Vector2.up * 5f,
                ForceMode2D.Impulse
            );
        }

        // Physics movement
        rb.linearVelocity = new Vector2(
            finalX,
            rb.linearVelocity.y
        );
    }

    Vector2 GetSeparation()
    {
        Vector2 separation = Vector2.zero;

        Collider2D[] nearbyZombies =
            Physics2D.OverlapCircleAll(
                transform.position,
                separationRadius,
                zombieLayer
            );

        foreach (Collider2D zombie in nearbyZombies)
        {
            if (zombie.gameObject == gameObject)
                continue;

            Vector2 away =
                (Vector2)transform.position -
                (Vector2)zombie.transform.position;

            float distance = away.magnitude;

            if (distance > 0.01f)
            {
                // Stronger separation when very close
                float strength =
                    1f - Mathf.Clamp01(
                        distance / separationRadius
                    );

                separation +=
                    away.normalized * strength;
            }
        }

        return separation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            separationRadius
        );
    }
}