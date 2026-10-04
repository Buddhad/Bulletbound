using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 40f;

    private Rigidbody2D rb;
    private bool hasHitTarget = false;


    // =========================================================
    // AWAKE
    // =========================================================

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


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (rb != null)
        {
            rb.linearVelocity =
                transform.right * speed;
        }
    }


    // =========================================================
    // COLLISION
    // =========================================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitTarget)
            return;

        HandleCollision(collision.gameObject);
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


        Debug.Log(
            "Bullet hit: " + hitObject.name
        );


        // -----------------------------------------------------
        // PLAYER
        // -----------------------------------------------------

        if (hitObject.CompareTag("Player"))
        {
            // Never damage the player.
            return;
        }


        // -----------------------------------------------------
        // ABILITIES
        // -----------------------------------------------------

        if (IsAbility(hitObject))
        {
            // Bullet passes through abilities.
            return;
        }


        // -----------------------------------------------------
        // ENEMY
        // -----------------------------------------------------

        EnemyHealth enemy =
            hitObject.GetComponent<EnemyHealth>();


        // If the collider belongs to a child object,
        // check the parent.
        if (enemy == null)
        {
            enemy =
                hitObject.GetComponentInParent<EnemyHealth>();
        }


        if (enemy != null)
        {
            hasHitTarget = true;


            // Deal damage.
            enemy.TakeDamage(damage);


            // Play enemy damage sound.
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(
                    "Enemy_damage"
                );
            }
            else
            {
                Debug.LogWarning(
                    "Bullet: AudioManager.Instance is missing."
                );
            }


            // Destroy bullet.
            Destroy(gameObject);

            return;
        }


        // -----------------------------------------------------
        // OTHER OBJECTS
        // -----------------------------------------------------

        hasHitTarget = true;

        Destroy(gameObject);
    }


    // =========================================================
    // ABILITY CHECK
    // =========================================================

    private bool IsAbility(GameObject obj)
    {
        return obj.CompareTag("Ability_Coin") ||
               obj.CompareTag("Ability_DoubleCoins") ||
               obj.CompareTag("Ability_FireRate") ||
               obj.CompareTag("Ability_Speed") ||
               obj.CompareTag("Ability_Shield") ||
               obj.CompareTag("Ability_Health") ||
               obj.CompareTag("Ability_Jump");
    }
}