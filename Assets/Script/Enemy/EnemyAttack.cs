using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 10f;
    public float attackCooldown = 1f;

    [Tooltip("Horizontal distance at which the zombie attacks.")]
    public float attackRange = 1.5f;

    private Animator animator;
    private PlayerHealth playerHealth;
    private Transform player;

    private float nextAttackTime = 0f;

    private void Awake()
    {
        // Animator is on the Zombie parent.
        animator = GetComponentInParent<Animator>();

        if (animator == null)
        {
            Debug.LogError(
                "EnemyAttack: Animator not found on Zombie parent.",
                gameObject
            );
        }
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();

            if (player == null)
                return;
        }

        // Get horizontal distance only.
        float horizontalDistance =
            Mathf.Abs(
                player.position.x -
                transform.parent.position.x
            );

        // Player is outside attack range.
        if (horizontalDistance > attackRange)
            return;

        // Find PlayerHealth if necessary.
        if (playerHealth == null)
        {
            playerHealth =
                player.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                playerHealth =
                    player.GetComponentInParent<PlayerHealth>();
            }
        }

        TryAttack();
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

            playerHealth =
                playerObject.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                playerHealth =
                    playerObject.GetComponentInParent<PlayerHealth>();
            }
        }
    }

    // =========================================================
    // ATTACK
    // =========================================================

    private void TryAttack()
    {
        if (playerHealth == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime =
            Time.time + attackCooldown;

        // =====================================================
        // ATTACK ANIMATION
        // =====================================================

        if (animator != null)
        {
            Debug.Log(
                "Zombie attacking: " +
                gameObject.transform.parent.name
            );

            animator.SetTrigger("attack");
        }

        // =====================================================
        // DAMAGE PLAYER
        // =====================================================

        playerHealth.TakeDamage(damage);

        Debug.Log(
            "Zombie dealt " +
            damage +
            " damage."
        );
    }

    // =========================================================
    // DEBUG GIZMO
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (transform.parent != null)
        {
            Gizmos.DrawWireSphere(
                transform.parent.position,
                attackRange
            );
        }
    }
}