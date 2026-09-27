using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Death Effect")]
    [SerializeField] private GameObject DeathEffect;

    [Header("Player Damage")]
    public float damageAmount = 10f;
    public float damageCooldown = 1f;

    [Header("Abilities")]
    public GameObject[] abilities;

    [Header("Kill Area")]
    [SerializeField] private float minKillX = -21.9f;
    [SerializeField] private float maxKillX = 14.9f;

    private Rigidbody2D rb;
    private bool hasDealtDamage = false;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Make sure health starts correctly.
        if (health <= 0)
        {
            health = maxHealth;
        }

        maxHealth = health;
    }

    // =========================================================
    // DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        // Don't allow damage after death.
        if (isDead)
            return;

        // Ignore invalid damage.
        if (damage <= 0)
            return;

        // Enemy must be inside killable area.
        if (transform.position.x < minKillX ||
            transform.position.x > maxKillX)
        {
            return;
        }

        health -= damage;

        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. Health: " +
            health
        );

        if (health <= 0)
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

        isDead = true;

        Debug.Log(gameObject.name + " died.");

        // -----------------------------------------------------
        // Score
        // -----------------------------------------------------

        if (ScoreManager.CurrentScore >= 0)
        {
            ScoreManager.AddScore(10);
        }
        else
        {
            Debug.LogWarning(
                "EnemyHealth: ScoreManager is not available."
            );
        }

        // -----------------------------------------------------
        // Death sound
        // -----------------------------------------------------

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

        // -----------------------------------------------------
        // Spawn ability
        // -----------------------------------------------------

        SpawnRandomAbility();

        // -----------------------------------------------------
        // Death particle effect
        // -----------------------------------------------------

        if (DeathEffect != null)
        {
            GameObject explode = Instantiate(
                DeathEffect,
                transform.position,
                Quaternion.identity
            );

            Destroy(explode, 2f);
        }
        else
        {
            Debug.LogWarning(
                "EnemyHealth: DeathEffect is not assigned.",
                this
            );
        }

        // -----------------------------------------------------
        // Destroy enemy
        // -----------------------------------------------------

        Destroy(gameObject);
    }

    // =========================================================
    // RANDOM ABILITY
    // =========================================================

    public void SpawnRandomAbility()
    {
        // No abilities assigned.
        if (abilities == null || abilities.Length == 0)
        {
            Debug.LogWarning(
                "EnemyHealth: No abilities assigned.",
                this
            );

            return;
        }

        // Choose random ability.
        int randomIndex =
            Random.Range(0, abilities.Length);

        GameObject ability =
            abilities[randomIndex];

        // Safety check.
        if (ability == null)
        {
            Debug.LogWarning(
                "EnemyHealth: Ability at index " +
                randomIndex +
                " is empty.",
                this
            );

            return;
        }

        Instantiate(
            ability,
            transform.position,
            Quaternion.identity
        );
    }

    // =========================================================
    // PLAYER COLLISION DAMAGE
    // =========================================================

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead)
            return;

        if (!other.gameObject.CompareTag("Player"))
            return;

        if (hasDealtDamage)
            return;

        PlayerHealth playerHealth =
            other.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);

            hasDealtDamage = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasDealtDamage = false;
        }
    }

    // =========================================================
    // CONTINUOUS DAMAGE
    // =========================================================

    private IEnumerator ContinuousDamageCoroutine(
        PlayerHealth playerHealth)
    {
        yield return new WaitForSeconds(damageCooldown);

        while (true)
        {
            if (playerHealth == null)
                yield break;

            playerHealth.TakeDamage(damageAmount);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Player_Damage");
            }

            yield return new WaitForSeconds(damageCooldown);
        }
    }
}