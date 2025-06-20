using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rbody;
    private BoxCollider2D coll;
    public Animator anim;
    Transform selfTransform;
    public bool IsPlayerRight;
    private PlayerShooter playerShooter;
    [SerializeField] private LayerMask jumpableGround;
    //private SpriteRenderer sprite;
    public float moveX;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;

    private enum MovementState { idel, jump, runing, falling }

    //[SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        //sprite = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        playerShooter = GetComponent<PlayerShooter>();
        if (playerShooter == null)
        {
            Debug.LogWarning("PlayerShooter script not found!");
        }
        selfTransform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerShooter != null && playerShooter.isReloading) return; // skip movement update
        if (Time.timeScale == 0f) return; // 🚫 Game is paused, do nothing
        // It's work on unity input Manager 
        moveX = Input.GetAxisRaw("Horizontal"); //if we don't want to slide so then we use raw
        rbody.velocity = new Vector2(moveX * moveSpeed, rbody.velocity.y);

        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            AudioManager.Instance.PlaySFX("Jump");
            rbody.velocity = new Vector2(rbody.velocity.x, jumpForce);
        }
        UpdateAnimation(); // Only runs if not paused
        positionFixed();
    }

    public void UpdateAnimation()
    {
        if (Time.timeScale == 0f) return; // ⛔ Don't update animation if paused
        if (!this.enabled) return; // stop updating animation if movement script is disabled
        MovementState state;
        if (moveX > 0f)
        {
            state = MovementState.runing;
            //sprite.flipX = false;
            PlayerMoveRight();
            IsPlayerRight = true;
        }
        else if (moveX < 0f)
        {
            state = MovementState.runing;
            //sprite.flipX = true; facing left
            PlayerMoveLeft();
            IsPlayerRight = false;
        }
        else
        {
            state = MovementState.idel;
        }
        if (rbody.velocity.y > .1f)
        {
            state = MovementState.jump;
        }
        else if (rbody.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    void positionFixed()
    {
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, -21.9f, 14.9f),
            transform.position.y
        );
    }

    public void PlayerMoveLeft()
    {
        selfTransform.rotation = new Quaternion(0, -180, 0, 0);
    }
    public void PlayerMoveRight()
    {
        selfTransform.rotation = new Quaternion(0, 0, 0, 0);
    }
    public void ForceIdle()
    {
        Debug.Log("ForceIdle called.");
        moveX = 0f;
        rbody.velocity = new Vector2(0f, rbody.velocity.y); // stop movement
        UpdateAnimation();
    }
}
