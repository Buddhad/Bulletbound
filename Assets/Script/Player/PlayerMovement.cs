using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rbody;

    public Animator anim;

    private Transform selfTransform;
    private PlayerShooter playerShooter;

    public bool IsPlayerRight;

    public float moveX;
    public float moveSpeed = 5f;

    private enum MovementState
    {
        idle,
        running
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        playerShooter = GetComponent<PlayerShooter>();

        if (playerShooter == null)
        {
            Debug.LogWarning(
                "PlayerShooter script not found!",
                this
            );
        }

        selfTransform = transform;
    }

    private void Update()
    {
        // Don't move while reloading.
        if (playerShooter != null &&
            playerShooter.isReloading)
        {
            return;
        }

        // Don't move while paused.
        if (Time.timeScale == 0f)
            return;

        // Horizontal movement.
        moveX = Input.GetAxisRaw("Horizontal");

        rbody.linearVelocity = new Vector2(
            moveX * moveSpeed,
            rbody.linearVelocity.y
        );

        UpdateAnimation();

        PositionFixed();
    }

    // =========================================================
    // ANIMATION
    // =========================================================

    public void UpdateAnimation()
    {
        if (Time.timeScale == 0f)
            return;

        if (!this.enabled)
            return;

        MovementState state;

        if (moveX > 0f)
        {
            state = MovementState.running;

            PlayerMoveRight();
            IsPlayerRight = true;
        }
        else if (moveX < 0f)
        {
            state = MovementState.running;

            PlayerMoveLeft();
            IsPlayerRight = false;
        }
        else
        {
            state = MovementState.idle;
        }

        if (anim != null)
        {
            anim.SetInteger("state", (int)state);
        }
    }

    // =========================================================
    // POSITION LIMIT
    // =========================================================

    private void PositionFixed()
    {
        transform.position = new Vector2(
            Mathf.Clamp(
                transform.position.x,
                -21.9f,
                14.9f
            ),
            transform.position.y
        );
    }

    // =========================================================
    // PLAYER DIRECTION
    // =========================================================

    public void PlayerMoveLeft()
    {
        selfTransform.rotation =
            new Quaternion(0, -180, 0, 0);
    }

    public void PlayerMoveRight()
    {
        selfTransform.rotation =
            new Quaternion(0, 0, 0, 0);
    }

    // =========================================================
    // FORCE IDLE
    // =========================================================

    public void ForceIdle()
    {
        Debug.Log("ForceIdle called.");

        moveX = 0f;

        if (rbody != null)
        {
            rbody.linearVelocity =
                new Vector2(0f, rbody.linearVelocity.y);
        }

        UpdateAnimation();
    }
}