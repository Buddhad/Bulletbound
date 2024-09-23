using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject DeathEffect;
    //Particle effect for death
    public float health;
    public float maxHealth = 100;
    private Rigidbody2D rb;
    public GameObject[] abilities;
    //[SerializeField] private EnemyFloatingHealthbar floatingHealthbar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //floatingHealthbar=GetComponentInChildren<EnemyFloatingHealthbar>();
        maxHealth=health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        //floatingHealthbar.UpdateHealthBar(health,maxHealth);
        if(health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        AudioManager.Instance.PlaySFX("Hurt");
        Destroy(gameObject);
        SpawnRandomAbility();
        GameObject Explode = (GameObject)Instantiate(DeathEffect, transform.position, Quaternion.identity);
    }

    public void SpawnRandomAbility()
    {
        int randomIndex = Random.Range(0, abilities.Length);
        GameObject ability = abilities[randomIndex];

        GameObject spawnedAbility = Instantiate(ability, transform.position, Quaternion.identity);
    }
}
