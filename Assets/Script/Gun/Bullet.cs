using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage = 40;
    EnemyHealth enemy;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            AudioManager.Instance.PlaySFX("Die");
        }
        if (other.CompareTag("Coin"))
        {
            return;
        }
        if (other.CompareTag("Ability_FireRate"))
        {
            return;
        }
        if (other.CompareTag("Ability_Speed"))
        {
            return;
        }
        if (other.CompareTag("Ability_Shield"))
        {
            return;
        }
        if (other.CompareTag("Ability_Health"))
        {
            return;
        }
        if (other.CompareTag("Ability_Jump"))
        {
            return;
        }
        Destroy(gameObject);
    }

}
