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
    [SerializeField] private Transform player; // assign in Inspector or auto
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
}
