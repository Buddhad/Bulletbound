using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;
    public float damage = 40f;

    private Rigidbody2D rb;
    private bool hasHitTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(
                "Bullet: Rigidbody2D is missing!",
                this
            );
        }
    }

    private void Start()
    {
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
    }

    // =========================================================
    // COLLISION
    // =========================================================

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasHitTarget)
            return;

        HandleCollision(other.gameObject);
    }

    // =========================================================
    // TRIGGER
    // =========================================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitTarget)
            return;

        HandleCollision(other.gameObject);
    }

    // =========================================================
    // HANDLE COLLISION
    // =========================================================

    private void HandleCollision(GameObject hitObject)
    {
        if (hitObject == null)
            return;

        Debug.Log("Bullet hit: " + hitObject.name);

        // -----------------------------------------------------
        // Don't damage the player
        // -----------------------------------------------------

        if (hitObject.CompareTag("Player"))
        {
            return;
        }

        // -----------------------------------------------------
        // Find EnemyHealth
        // -----------------------------------------------------

        EnemyHealth enemy =
            hitObject.GetComponent<EnemyHealth>();

        // If collider belongs to a child object,
        // check the parent too.
        if (enemy == null)
        {
            enemy =
                hitObject.GetComponentInParent<EnemyHealth>();
        }

        if (enemy != null)
        {
            hasHitTarget = true;

            enemy.TakeDamage(damage);

            // Play damage sound only if AudioManager exists.
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Enemy_damage");
            }
            else
            {
                Debug.LogWarning(
                    "Bullet: AudioManager.Instance is missing. " +
                    "Enemy damage sound was not played."
                );
            }

            Destroy(gameObject);

            return;
        }

        // -----------------------------------------------------
        // Ability collision
        // -----------------------------------------------------

        if (IsAbility(hitObject))
        {
            // Bullet passes through abilities.
            return;
        }

        // -----------------------------------------------------
        // Other objects
        // -----------------------------------------------------

        hasHitTarget = true;

        Destroy(gameObject);
    }

    // =========================================================
    // ABILITY CHECK
    // =========================================================

    private bool IsAbility(GameObject obj)
    {
        return obj.CompareTag("Ability_DoubleCoins") ||
               obj.CompareTag("Ability_FireRate") ||
               obj.CompareTag("Ability_Speed") ||
               obj.CompareTag("Ability_Shield") ||
               obj.CompareTag("Ability_Health") ||
               obj.CompareTag("Ability_Jump");
    }
}