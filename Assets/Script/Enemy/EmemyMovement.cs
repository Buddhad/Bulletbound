using UnityEngine;

public class EmemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;

    [Tooltip("Distance from the player where the front zombie stops.")]
    public float stopDistance = 1.2f;

    [Header("Zombie Queue")]
    [Tooltip("Horizontal distance maintained between zombies.")]
    public float zombieSpacing = 0.8f;

    [Tooltip("Layer used by zombie bodies.")]
    public LayerMask zombieLayer;

    [Header("Control Settings")]
    public bool canMove = true;

    private Transform player;
    private Rigidbody2D rb;

    private bool facingRight = true;

    // =========================================================
    // SETUP
    // =========================================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(
                "EmemyMovement: Rigidbody2D not found.",
                this
            );
        }
    }

    private void Start()
    {
        FindPlayer();
    }

    // =========================================================
    // FIND PLAYER
    // =========================================================

    private void FindPlayer()
    {
        GameObject playerObject =
            GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (rb == null)
            return;

        // Game has not started.
        if (!GameStartTimer.GameStarted)
        {
            StopHorizontalMovement();
            return;
        }

        // Movement disabled.
        if (!canMove)
        {
            StopHorizontalMovement();
            return;
        }

        // Find player if necessary.
        if (player == null)
        {
            FindPlayer();

            if (player == null)
            {
                StopHorizontalMovement();
                return;
            }
        }

        MoveTowardsPlayer();
    }

    // =========================================================
    // MAIN MOVEMENT
    // =========================================================

    private void MoveTowardsPlayer()
    {
        float myX = transform.position.x;
        float playerX = player.position.x;

        float horizontalDistance =
            Mathf.Abs(playerX - myX);

        // =====================================================
        // WHICH SIDE OF PLAYER?
        // =====================================================

        float mySide = Mathf.Sign(myX - playerX);

        // Safety in case zombie is exactly on player X.
        if (mySide == 0f)
        {
            mySide = facingRight ? 1f : -1f;
        }

        // =====================================================
        // COUNT ZOMBIES IN FRONT OF THIS ZOMBIE
        // =====================================================

        int zombiesAhead =
            CountZombiesAhead(mySide);

        // =====================================================
        // QUEUE POSITION
        // =====================================================

        // Front zombie:
        //
        // Player <---- 1.2 ---- Zombie
        //
        // Second zombie:
        //
        // Player <---- 1.2 ---- Zombie <---- 0.8 ---- Zombie
        //
        // Third:
        //
        // Player <---- 1.2 ---- Zombie <---- 0.8 ---- Zombie <---- 0.8 ---- Zombie

        float desiredDistance =
            stopDistance +
            (zombiesAhead * zombieSpacing);

        // =====================================================
        // STOP IF AT QUEUE POSITION
        // =====================================================

        if (horizontalDistance <= desiredDistance)
        {
            StopHorizontalMovement();
            return;
        }

        // =====================================================
        // MOVE TOWARD PLAYER
        // =====================================================

        float direction =
            Mathf.Sign(playerX - myX);

        float movementX =
            direction * speed;

        // =====================================================
        // SMALL HORIZONTAL SEPARATION
        // =====================================================

        float separation =
            GetHorizontalSeparation(mySide);

        movementX += separation;

        // Never allow separation to make zombie
        // move faster than its normal speed.
        movementX =
            Mathf.Clamp(
                movementX,
                -speed,
                speed
            );

        // =====================================================
        // FACE PLAYER
        // =====================================================

        if (movementX > 0.01f)
        {
            FaceRight();
        }
        else if (movementX < -0.01f)
        {
            FaceLeft();
        }

        // =====================================================
        // X MOVEMENT ONLY
        // =====================================================

        rb.linearVelocity =
            new Vector2(
                movementX,
                rb.linearVelocity.y
            );
    }

    // =========================================================
    // COUNT ZOMBIES AHEAD
    // =========================================================

    private int CountZombiesAhead(float mySide)
    {
        int count = 0;

        Collider2D[] zombies =
            Physics2D.OverlapCircleAll(
                player.position,
                100f,
                zombieLayer
            );

        float myDistance =
            Mathf.Abs(
                transform.position.x -
                player.position.x
            );

        foreach (Collider2D zombie in zombies)
        {
            if (zombie == null)
                continue;

            // Ignore own collider.
            if (zombie.gameObject == gameObject)
                continue;

            // Only count Enemy objects.
            if (!zombie.CompareTag("Enemy"))
                continue;

            float otherOffset =
                zombie.transform.position.x -
                player.position.x;

            // Ignore zombies that are on the opposite
            // side of the player.
            float otherSide =
                Mathf.Sign(otherOffset);

            if (otherSide == 0f)
                continue;

            if (otherSide != mySide)
                continue;

            float otherDistance =
                Mathf.Abs(otherOffset);

            // Only zombies closer to the player
            // are ahead of this zombie.
            if (otherDistance < myDistance)
            {
                count++;
            }
        }

        return count;
    }

    // =========================================================
    // HORIZONTAL SEPARATION
    // =========================================================

    private float GetHorizontalSeparation(float mySide)
    {
        float separation = 0f;

        Collider2D[] nearbyZombies =
            Physics2D.OverlapCircleAll(
                transform.position,
                zombieSpacing,
                zombieLayer
            );

        foreach (Collider2D zombie in nearbyZombies)
        {
            if (zombie == null)
                continue;

            if (zombie.gameObject == gameObject)
                continue;

            if (!zombie.CompareTag("Enemy"))
                continue;

            // Only separate from zombies on the
            // same side of the player.
            float otherOffset =
                zombie.transform.position.x -
                player.position.x;

            float otherSide =
                Mathf.Sign(otherOffset);

            if (otherSide != mySide)
                continue;

            // Only horizontal separation.
            float difference =
                transform.position.x -
                zombie.transform.position.x;

            float distance =
                Mathf.Abs(difference);

            if (distance < 0.01f)
            {
                // Prevent exact overlap.
                difference =
                    Random.Range(-1f, 1f);

                if (Mathf.Abs(difference) < 0.01f)
                    difference = 1f;

                distance = 0.01f;
            }

            if (distance < zombieSpacing)
            {
                float strength =
                    1f -
                    Mathf.Clamp01(
                        distance / zombieSpacing
                    );

                separation +=
                    Mathf.Sign(difference) *
                    strength *
                    2f;
            }
        }

        return separation;
    }

    // =========================================================
    // STOP
    // =========================================================

    private void StopHorizontalMovement()
    {
        if (rb == null)
            return;

        rb.linearVelocity =
            new Vector2(
                0f,
                rb.linearVelocity.y
            );
    }

    // =========================================================
    // FACE RIGHT
    // =========================================================

    private void FaceRight()
    {
        if (facingRight)
            return;

        facingRight = true;

        Vector3 scale =
            transform.localScale;

        scale.x =
            Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    // =========================================================
    // FACE LEFT
    // =========================================================

    private void FaceLeft()
    {
        if (!facingRight)
            return;

        facingRight = false;

        Vector3 scale =
            transform.localScale;

        scale.x =
            -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    // =========================================================
    // DEBUG GIZMOS
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        // Yellow = front zombie stopping distance.
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            stopDistance
        );

        // Red = zombie spacing.
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            zombieSpacing
        );
    }
}