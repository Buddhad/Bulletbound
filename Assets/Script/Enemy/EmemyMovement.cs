using UnityEngine;

public class EmemyMovement : MonoBehaviour
{
    // =========================================================
    // MOVEMENT SETTINGS
    // =========================================================

    [Header("Movement Settings")]

    // Normal movement speed of the zombie.
    public float speed = 3f;

    // Distance from the player where the front zombie stops.
    //
    // Example:
    // Player <---- 1.2 ---- Zombie
    public float stopDistance = 1.2f;


    // =========================================================
    // ZOMBIE QUEUE SETTINGS
    // =========================================================

    [Header("Zombie Queue")]

    // Horizontal distance maintained between zombies.
    //
    // Example:
    // Player <--- 1.2 ---> Zombie <--- 0.8 ---> Zombie
    public float zombieSpacing = 0.8f;

    // Layer used to detect other zombies.
    //
    // IMPORTANT:
    // Set this to your "Enemy" layer in the Inspector.
    public LayerMask zombieLayer;


    // =========================================================
    // CONTROL SETTINGS
    // =========================================================

    [Header("Control Settings")]

    // Allows the zombie to move.
    //
    // Because GameStartTimer has been removed,
    // this should normally remain TRUE.
    public bool canMove = true;


    // =========================================================
    // INTERNAL VARIABLES
    // =========================================================

    // Reference to the Player Transform.
    private Transform player;

    // Reference to the zombie Rigidbody2D.
    private Rigidbody2D rb;

    // Keeps track of the zombie's current facing direction.
    private bool facingRight = true;


    // =========================================================
    // SETUP
    // =========================================================

    private void Awake()
    {
        // Get the Rigidbody2D attached to the zombie.
        rb = GetComponent<Rigidbody2D>();

        // Warn us if the Rigidbody2D is missing.
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
        // Find the Player when the zombie is created.
        FindPlayer();
    }


    // =========================================================
    // FIND PLAYER
    // =========================================================

    private void FindPlayer()
    {
        // Find the GameObject with the "Player" tag.
        GameObject playerObject =
            GameObject.FindWithTag("Player");

        // If the Player exists, save its Transform.
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
        // Stop if the Rigidbody2D is missing.
        if (rb == null)
            return;


        // =====================================================
        // MOVEMENT ENABLE/DISABLE
        // =====================================================

        // If movement has been disabled,
        // stop the zombie immediately.
        //
        // GameStartTimer has been removed, so there is
        // NO GameStarted check here anymore.
        if (!canMove)
        {
            StopHorizontalMovement();
            return;
        }


        // =====================================================
        // FIND PLAYER IF NECESSARY
        // =====================================================

        // If the Player reference is missing,
        // try to find the Player again.
        if (player == null)
        {
            FindPlayer();

            // If the Player still cannot be found,
            // stop the zombie.
            if (player == null)
            {
                StopHorizontalMovement();
                return;
            }
        }


        // =====================================================
        // MOVE TOWARDS PLAYER
        // =====================================================

        MoveTowardsPlayer();
    }


    // =========================================================
    // MAIN MOVEMENT
    // =========================================================

    private void MoveTowardsPlayer()
    {
        // Get the zombie's X position.
        float myX = transform.position.x;

        // Get the Player's X position.
        float playerX = player.position.x;


        // Calculate the horizontal distance between
        // the zombie and the Player.
        float horizontalDistance =
            Mathf.Abs(playerX - myX);


        // =====================================================
        // DETERMINE WHICH SIDE OF PLAYER
        // =====================================================

        // Positive value = zombie is to the right.
        // Negative value = zombie is to the left.
        float mySide =
            Mathf.Sign(myX - playerX);


        // Safety check in case the zombie is exactly
        // on the same X position as the Player.
        if (mySide == 0f)
        {
            mySide = facingRight ? 1f : -1f;
        }


        // =====================================================
        // COUNT ZOMBIES AHEAD
        // =====================================================

        // Find how many zombies are between this zombie
        // and the Player.
        int zombiesAhead =
            CountZombiesAhead(mySide);


        // =====================================================
        // CALCULATE QUEUE POSITION
        // =====================================================

        // The first zombie stops at stopDistance.
        //
        // Every additional zombie needs another
        // zombieSpacing distance.
        //
        // Example:
        //
        // First zombie:
        // Player <---- 1.2 ---- Zombie
        //
        // Second zombie:
        // Player <---- 1.2 ---- Zombie <---- 0.8 ---- Zombie
        //
        // Third zombie:
        // Player <---- 1.2 ---- Zombie
        //                         <---- 0.8 ---- Zombie
        //                                      <---- 0.8 ---- Zombie

        float desiredDistance =
            stopDistance +
            (zombiesAhead * zombieSpacing);


        // =====================================================
        // STOP AT QUEUE POSITION
        // =====================================================

        // If the zombie has reached its position,
        // stop moving.
        if (horizontalDistance <= desiredDistance)
        {
            StopHorizontalMovement();
            return;
        }


        // =====================================================
        // MOVE TOWARDS PLAYER
        // =====================================================

        // Determine whether the zombie needs to move
        // left or right.
        float direction =
            Mathf.Sign(playerX - myX);


        // Calculate normal movement speed.
        float movementX =
            direction * speed;


        // =====================================================
        // HORIZONTAL SEPARATION
        // =====================================================

        // Apply a small separation force so zombies
        // do not occupy exactly the same position.
        float separation =
            GetHorizontalSeparation(mySide);


        // Add separation to the normal movement.
        movementX += separation;


        // Prevent the separation force from making
        // the zombie move faster than its normal speed.
        movementX =
            Mathf.Clamp(
                movementX,
                -speed,
                speed
            );


        // =====================================================
        // FACE MOVEMENT DIRECTION
        // =====================================================

        // If moving right, face right.
        if (movementX > 0.01f)
        {
            FaceRight();
        }

        // If moving left, face left.
        else if (movementX < -0.01f)
        {
            FaceLeft();
        }


        // =====================================================
        // APPLY X MOVEMENT ONLY
        // =====================================================

        // Change only horizontal velocity.
        //
        // Y velocity is preserved so gravity/physics
        // can continue working normally.
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
        // Number of zombies in front of this zombie.
        int count = 0;


        // Find nearby zombie colliders.
        //
        // 100f gives us a large detection area around
        // the Player.
        Collider2D[] zombies =
            Physics2D.OverlapCircleAll(
                player.position,
                100f,
                zombieLayer
            );


        // Distance from this zombie to the Player.
        float myDistance =
            Mathf.Abs(
                transform.position.x -
                player.position.x
            );


        // Check every detected zombie.
        foreach (Collider2D zombie in zombies)
        {
            // Ignore missing objects.
            if (zombie == null)
                continue;


            // Ignore this zombie's own collider.
            if (zombie.gameObject == gameObject)
                continue;


            // Only count objects with the Enemy tag.
            if (!zombie.CompareTag("Enemy"))
                continue;


            // Calculate the other zombie's X offset
            // from the Player.
            float otherOffset =
                zombie.transform.position.x -
                player.position.x;


            // Determine which side of the Player
            // the other zombie is on.
            float otherSide =
                Mathf.Sign(otherOffset);


            // Ignore zombies exactly on the Player's X.
            if (otherSide == 0f)
                continue;


            // Ignore zombies on the opposite side.
            if (otherSide != mySide)
                continue;


            // Calculate the other zombie's distance
            // from the Player.
            float otherDistance =
                Mathf.Abs(otherOffset);


            // If the other zombie is closer to the Player
            // than this zombie, it is ahead.
            if (otherDistance < myDistance)
            {
                count++;
            }
        }


        // Return the number of zombies ahead.
        return count;
    }


    // =========================================================
    // HORIZONTAL SEPARATION
    // =========================================================

    private float GetHorizontalSeparation(float mySide)
    {
        // Total separation force.
        float separation = 0f;


        // Detect nearby zombies around this zombie.
        Collider2D[] nearbyZombies =
            Physics2D.OverlapCircleAll(
                transform.position,
                zombieSpacing,
                zombieLayer
            );


        // Check every nearby zombie.
        foreach (Collider2D zombie in nearbyZombies)
        {
            // Ignore missing objects.
            if (zombie == null)
                continue;


            // Ignore this zombie itself.
            if (zombie.gameObject == gameObject)
                continue;


            // Only separate from objects tagged Enemy.
            if (!zombie.CompareTag("Enemy"))
                continue;


            // Determine which side of the Player
            // the other zombie is on.
            float otherOffset =
                zombie.transform.position.x -
                player.position.x;


            float otherSide =
                Mathf.Sign(otherOffset);


            // Only separate zombies that are on
            // the same side of the Player.
            if (otherSide != mySide)
                continue;


            // Calculate horizontal difference between
            // this zombie and the nearby zombie.
            float difference =
                transform.position.x -
                zombie.transform.position.x;


            // Calculate actual horizontal distance.
            float distance =
                Mathf.Abs(difference);


            // =================================================
            // PREVENT EXACT OVERLAP
            // =================================================

            // If both zombies are at almost exactly
            // the same position, create a small random
            // separation direction.
            if (distance < 0.01f)
            {
                difference =
                    Random.Range(-1f, 1f);


                // Make sure the difference isn't zero.
                if (Mathf.Abs(difference) < 0.01f)
                {
                    difference = 1f;
                }


                // Use a small minimum distance.
                distance = 0.01f;
            }


            // =================================================
            // CALCULATE SEPARATION STRENGTH
            // =================================================

            // Only separate zombies that are closer
            // than the desired spacing.
            if (distance < zombieSpacing)
            {
                // The closer the zombies are,
                // the stronger the separation.
                //
                // distance = 0
                // strength ≈ 1
                //
                // distance = zombieSpacing
                // strength = 0
                float strength =
                    1f -
                    Mathf.Clamp01(
                        distance / zombieSpacing
                    );


                // Push the zombie away from the
                // nearby zombie.
                separation +=
                    Mathf.Sign(difference) *
                    strength *
                    2f;
            }
        }


        // Return the final separation force.
        return separation;
    }


    // =========================================================
    // STOP HORIZONTAL MOVEMENT
    // =========================================================

    private void StopHorizontalMovement()
    {
        // Safety check.
        if (rb == null)
            return;


        // Stop horizontal movement but preserve
        // the current vertical velocity.
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
        // Already facing right.
        if (facingRight)
            return;


        // Update facing state.
        facingRight = true;


        // Get current scale.
        Vector3 scale =
            transform.localScale;


        // Make X scale positive.
        scale.x =
            Mathf.Abs(scale.x);


        // Apply the new scale.
        transform.localScale = scale;
    }


    // =========================================================
    // FACE LEFT
    // =========================================================

    private void FaceLeft()
    {
        // Already facing left.
        if (!facingRight)
            return;


        // Update facing state.
        facingRight = false;


        // Get current scale.
        Vector3 scale =
            transform.localScale;


        // Make X scale negative.
        scale.x =
            -Mathf.Abs(scale.x);


        // Apply the new scale.
        transform.localScale = scale;
    }


    // =========================================================
    // DEBUG GIZMOS
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        // -----------------------------------------------------
        // STOP DISTANCE
        // -----------------------------------------------------

        // Yellow circle shows the distance where the
        // front zombie stops.
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            stopDistance
        );


        // -----------------------------------------------------
        // ZOMBIE SPACING
        // -----------------------------------------------------

        // Red circle shows the spacing detection area
        // used to separate nearby zombies.
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            zombieSpacing
        );
    }
}