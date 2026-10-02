using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // =========================================================
    // COMPONENT REFERENCES
    // =========================================================

    // Player Rigidbody2D.
    private Rigidbody2D rbody;

    // Player Animator.
    [Header("Components")]
    [SerializeField] private Animator anim;

    // Player Transform.
    private Transform selfTransform;

    // Player shooting system.
    private PlayerShooter playerShooter;


    // =========================================================
    // MOVEMENT SETTINGS
    // =========================================================

    [Header("Movement Settings")]

    // Current horizontal input.
    [HideInInspector]
    public float moveX;

    // Player movement speed.
    [SerializeField]
    public float moveSpeed = 5f;

    // Allows other systems to completely disable movement.
    //
    // Example:
    // WaveSpawner can set this to false when the level ends.
    public bool canMove = true;


    // =========================================================
    // PLAYER DIRECTION
    // =========================================================

    [Header("Player Direction")]

    // TRUE  = Player is facing right.
    // FALSE = Player is facing left.
    public bool IsPlayerRight = true;


    // =========================================================
    // POSITION LIMITS
    // =========================================================

    [Header("Position Limits")]

    // Minimum X position the Player can reach.
    [SerializeField] private float minX = -21.9f;

    // Maximum X position the Player can reach.
    [SerializeField] private float maxX = 14.9f;


    // =========================================================
    // ANIMATION STATES
    // =========================================================

    private enum MovementState
    {
        idle,
        running
    }


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // Get Rigidbody2D attached to the Player.
        rbody = GetComponent<Rigidbody2D>();

        // Get Animator if it wasn't assigned manually.
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        // Store Player Transform.
        selfTransform = transform;

        // Get PlayerShooter from the same GameObject.
        playerShooter = GetComponent<PlayerShooter>();

        // Warn if PlayerShooter is missing.
        if (playerShooter == null)
        {
            Debug.LogWarning(
                "PlayerMovement: PlayerShooter script not found!",
                this
            );
        }
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // -----------------------------------------------------
        // PAUSE CHECK
        // -----------------------------------------------------

        // Don't process input while the game is paused.
        if (Time.timeScale == 0f)
        {
            StopHorizontalMovement();
            return;
        }


        // -----------------------------------------------------
        // MOVEMENT ENABLE CHECK
        // -----------------------------------------------------

        // Other systems can disable movement using canMove.
        if (!canMove)
        {
            StopHorizontalMovement();
            UpdateIdleAnimation();
            return;
        }


        // -----------------------------------------------------
        // RELOAD CHECK
        // -----------------------------------------------------

        // Player cannot move while reloading.
        if (playerShooter != null &&
            playerShooter.isReloading)
        {
            StopHorizontalMovement();
            UpdateIdleAnimation();
            return;
        }


        // -----------------------------------------------------
        // READ INPUT
        // -----------------------------------------------------

        // Get horizontal keyboard/controller input.
        //
        // A / Left Arrow  = -1
        // D / Right Arrow = +1
        moveX = Input.GetAxisRaw("Horizontal");


        // -----------------------------------------------------
        // APPLY MOVEMENT
        // -----------------------------------------------------

        if (rbody != null)
        {
            rbody.linearVelocity = new Vector2(
                moveX * moveSpeed,
                rbody.linearVelocity.y
            );
        }


        // -----------------------------------------------------
        // UPDATE ANIMATION AND DIRECTION
        // -----------------------------------------------------

        UpdateAnimation();


        // -----------------------------------------------------
        // LIMIT PLAYER POSITION
        // -----------------------------------------------------

        PositionFixed();
    }


    // =========================================================
    // ANIMATION
    // =========================================================

    public void UpdateAnimation()
    {
        // Don't update animation while paused.
        if (Time.timeScale == 0f)
            return;

        // Don't update if this component is disabled.
        if (!enabled)
            return;


        MovementState state;


        // -----------------------------------------------------
        // MOVING RIGHT
        // -----------------------------------------------------

        if (moveX > 0f)
        {
            state = MovementState.running;

            PlayerMoveRight();

            IsPlayerRight = true;
        }


        // -----------------------------------------------------
        // MOVING LEFT
        // -----------------------------------------------------

        else if (moveX < 0f)
        {
            state = MovementState.running;

            PlayerMoveLeft();

            IsPlayerRight = false;
        }


        // -----------------------------------------------------
        // NOT MOVING
        // -----------------------------------------------------

        else
        {
            state = MovementState.idle;
        }


        // -----------------------------------------------------
        // SEND STATE TO ANIMATOR
        // -----------------------------------------------------

        if (anim != null)
        {
            // 0 = Idle
            // 1 = Running
            anim.SetInteger(
                "state",
                (int)state
            );
        }
    }


    // =========================================================
    // FORCE IDLE ANIMATION
    // =========================================================

    private void UpdateIdleAnimation()
    {
        moveX = 0f;

        if (anim != null)
        {
            anim.SetInteger(
                "state",
                (int)MovementState.idle
            );
        }
    }


    // =========================================================
    // STOP HORIZONTAL MOVEMENT
    // =========================================================

    private void StopHorizontalMovement()
    {
        // Reset input.
        moveX = 0f;

        // Stop horizontal velocity but preserve vertical velocity.
        if (rbody != null)
        {
            rbody.linearVelocity = new Vector2(
                0f,
                rbody.linearVelocity.y
            );
        }
    }


    // =========================================================
    // POSITION LIMIT
    // =========================================================

    private void PositionFixed()
    {
        // Make sure Transform exists.
        if (selfTransform == null)
            return;


        // Clamp the Player's X position.
        float clampedX = Mathf.Clamp(
            selfTransform.position.x,
            minX,
            maxX
        );


        // Keep the current Y position.
        selfTransform.position = new Vector2(
            clampedX,
            selfTransform.position.y
        );
    }


    // =========================================================
    // PLAYER DIRECTION
    // =========================================================

    public void PlayerMoveLeft()
    {
        // Rotate the Player 180 degrees around Y.
        selfTransform.rotation =
            Quaternion.Euler(
                0f,
                180f,
                0f
            );
    }


    public void PlayerMoveRight()
    {
        // Reset rotation so the Player faces right.
        selfTransform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                0f
            );
    }


    // =========================================================
    // FORCE IDLE
    // =========================================================

    public void ForceIdle()
    {
        Debug.Log(
            "PlayerMovement: ForceIdle called."
        );


        // Stop horizontal input.
        moveX = 0f;


        // Stop horizontal movement.
        StopHorizontalMovement();


        // Set Animator to idle.
        UpdateIdleAnimation();
    }


    // =========================================================
    // PUBLIC MOVEMENT CONTROL
    // =========================================================

    public void SetMovementEnabled(bool enabled)
    {
        // Enable or disable Player movement.
        canMove = enabled;


        // If movement has been disabled,
        // immediately stop the Player.
        if (!enabled)
        {
            ForceIdle();
        }
    }


    // =========================================================
    // MOVEMENT SPEED
    // =========================================================

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }


    public void SetMoveSpeed(float newSpeed)
    {
        // Prevent negative movement speed.
        moveSpeed = Mathf.Max(
            0f,
            newSpeed
        );
    }
}