using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage = 40;
    EnemyHealth enemy;
    private bool hasHitTarget = false; // Add this flag

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasHitTarget) return; // Prevent double hits
        HandleCollision(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitTarget) return; // Prevent double hits
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject hitObject)
    {
        Debug.Log("Bullet hit: " + hitObject.name);

        if (hitObject.CompareTag("Player"))
        {
            return; // Don't destroy when hitting player
        }

        enemy = hitObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            hasHitTarget = true; // Mark as hit to prevent double damage
            enemy.TakeDamage(damage);
            AudioManager.Instance.PlaySFX("Die");
            Destroy(gameObject);
            return;
        }

        // Don't destroy on ability collision
        if (hitObject.CompareTag("Ability_DoubleCoins") ||
            hitObject.CompareTag("Ability_FireRate") ||
            hitObject.CompareTag("Ability_Speed") ||
            hitObject.CompareTag("Ability_Shield") ||
            hitObject.CompareTag("Ability_Health") ||
            hitObject.CompareTag("Ability_Jump"))
        {
            return;
        }

        hasHitTarget = true; // Mark as hit
        Destroy(gameObject);
    }
}