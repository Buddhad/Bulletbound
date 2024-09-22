using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject ParticleEffect;
    //Particle effect for death
    public float health;
    public float maxHealth = 100;
    private Rigidbody2D rb;
    [SerializeField] private EnemyFloatingHealthbar floatingHealthbar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        floatingHealthbar=GetComponentInChildren<EnemyFloatingHealthbar>();
        maxHealth=health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        floatingHealthbar.UpdateHealthBar(health,maxHealth);
        if(health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        AudioManager.Instance.PlaySFX("Hurt");
        Destroy(gameObject);
        GameObject Explode = (GameObject)Instantiate(ParticleEffect, transform.position, Quaternion.identity);

    }
}
