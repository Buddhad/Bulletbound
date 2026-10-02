using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // =========================================================
    // HEALTH
    // =========================================================

    [Header("Health")]

    // Current health of the enemy.
    public float health = 100f;

    // Maximum health of the enemy.
    public float maxHealth = 100f;


    // =========================================================
    // DEATH EFFECT
    // =========================================================

    [Header("Death Effect")]

    // Particle/effect prefab played when the enemy dies.
    [SerializeField] private GameObject DeathEffect;


    // =========================================================
    // PLAYER DAMAGE
    // =========================================================

    [Header("Player Damage")]

    // Damage dealt to the Player when the enemy attacks.
    public float damageAmount = 10f;

    // Cooldown between player damage.
    public float damageCooldown = 1f;


    // =========================================================
    // KILL AREA
    // =========================================================

    [Header("Kill Area")]

    // Minimum X position where the enemy can receive damage.
    [SerializeField] private float minKillX = -21.9f;

    // Maximum X position where the enemy can receive damage.
    [SerializeField] private float maxKillX = 14.9f;


    // =========================================================
    // INTERNAL VARIABLES
    // =========================================================

    // Enemy Rigidbody2D reference.
    private Rigidbody2D rb;

    // Prevents the enemy from dealing collision damage
    // repeatedly while touching the Player.
    private bool hasDealtDamage = false;

    // Prevents the enemy from dying multiple times.
    private bool isDead = false;


    // =========================================================
    // SETUP
    // =========================================================

    private void Awake()
    {
        // Get the Rigidbody2D from the enemy.
        rb = GetComponent<Rigidbody2D>();


        // Make sure health has a valid starting value.
        if (health <= 0f)
        {
            health = maxHealth;
        }


        // Make maxHealth match the starting health.
        maxHealth = health;
    }


    // =========================================================
    // DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        // Do not allow damage after the enemy has died.
        if (isDead)
            return;


        // Ignore zero or negative damage.
        if (damage <= 0f)
            return;


        // -----------------------------------------------------
        // KILL AREA CHECK
        // -----------------------------------------------------

        // Only allow the enemy to take damage while it is
        // inside the playable/killable horizontal area.
        if (transform.position.x < minKillX ||
            transform.position.x > maxKillX)
        {
            return;
        }


        // Apply damage.
        health -= damage;


        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. Health: " +
            health
        );


        // Check whether the enemy has died.
        if (health <= 0f)
        {
            Die();
        }
    }


    // =========================================================
    // DEATH
    // =========================================================

    public void Die()
    {
        // Prevent double death.
        if (isDead)
            return;


        // Mark the enemy as dead immediately.
        isDead = true;


        Debug.Log(
            gameObject.name +
            " died."
        );


        // =====================================================
        // SCORE
        // =====================================================

        // Give the Player score for killing this enemy.
        ScoreManager.AddScore(10);


        // =====================================================
        // DEATH SOUND
        // =====================================================

        // Play enemy death sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Die_Enemy");
        }
        else
        {
            Debug.LogWarning(
                "EnemyHealth: AudioManager.Instance is missing. " +
                "Death sound was not played."
            );
        }


        // =====================================================
        // COIN DROP
        // =====================================================

        // Get the CoinDrop component attached to this enemy.
        CoinDrop coinDrop =
            GetComponent<CoinDrop>();


        // If CoinDrop exists, drop a coin.
        if (coinDrop != null)
        {
            coinDrop.DropCoin();
        }
        else
        {
            Debug.LogWarning(
                "EnemyHealth: CoinDrop component is not attached.",
                this
            );
        }


        // =====================================================
        // DEATH PARTICLE EFFECT
        // =====================================================

        if (DeathEffect != null)
        {
            // Create the death effect at the enemy position.
            GameObject explode =
                Instantiate(
                    DeathEffect,
                    transform.position,
                    Quaternion.identity
                );


            // Automatically destroy the effect after 2 seconds.
            Destroy(explode, 2f);
        }
        else
        {
            Debug.LogWarning(
                "EnemyHealth: DeathEffect is not assigned.",
                this
            );
        }


        // =====================================================
        // DESTROY ENEMY
        // =====================================================

        // Remove the enemy from the scene.
        Destroy(gameObject);
    }


    // =========================================================
    // PLAYER COLLISION DAMAGE
    // =========================================================

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Do nothing if the enemy is already dead.
        if (isDead)
            return;


        // Only react to the Player.
        if (!other.gameObject.CompareTag("Player"))
            return;


        // Prevent repeated damage from the same collision.
        if (hasDealtDamage)
            return;


        // Find PlayerHealth on the Player.
        PlayerHealth playerHealth =
            other.gameObject.GetComponent<PlayerHealth>();


        // If PlayerHealth exists, deal damage.
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);

            // Mark that damage has been dealt.
            hasDealtDamage = true;
        }
    }


    // =========================================================
    // PLAYER COLLISION EXIT
    // =========================================================

    private void OnCollisionExit2D(Collision2D other)
    {
        // When the Player leaves the enemy,
        // allow damage again on the next collision.
        if (other.gameObject.CompareTag("Player"))
        {
            hasDealtDamage = false;
        }
    }
}