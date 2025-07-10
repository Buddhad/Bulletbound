using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Particle effect for death
    [SerializeField] private GameObject DeathEffect;
    public float health;
    public float maxHealth = 100;
    private Rigidbody2D rb;
    [Header("Player Collision Settings")]
    public float damageAmount = 10f; // Damage to deal to player
    public float damageCooldown = 1f; // Time between damage instances

    // 🔒 Limits where enemy can be killed (same as player's movement limit)
    private float minKillX = -21.9f;
    private float maxKillX = 14.9f;
    public GameObject[] abilities;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;

    }

    public void TakeDamage(float damage)
    {
        // Enemy must be within player's killable X range
        if (transform.position.x < minKillX || transform.position.x > maxKillX)
            return; // Too far — ignore damage
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        ScoreManager.AddScore(10);
        AudioManager.Instance.PlaySFX("Hurt");
        SpawnRandomAbility();
        GameObject explode = Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(explode, 2f); // ⏱️ adjust to match particle duration
        Destroy(gameObject);
    }

    public void SpawnRandomAbility()
    {
        int randomIndex = Random.Range(0, abilities.Length);
        GameObject ability = abilities[randomIndex];
        GameObject spawnedAbility = Instantiate(ability, transform.position, Quaternion.identity);
    }

    // ✅ NEW: Handle continuous collision with player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Deal damage immediately when enemy gets stuck with player
                playerHealth.TakeDamage(damageAmount);
                StartCoroutine(ContinuousDamageCoroutine(playerHealth));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines(); // Stop damage when player leaves
        }
    }

    // ✅ NEW: Continuous damage while stuck with player
    private IEnumerator ContinuousDamageCoroutine(PlayerHealth playerHealth)
    {
        yield return new WaitForSeconds(damageCooldown); // Wait before next damage

        while (true)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                yield return new WaitForSeconds(damageCooldown);
            }
            else
            {
                break; // Stop if player health component is destroyed
            }
        }
    }
}
