using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=20f;
    public Rigidbody2D rb;
    public float damage=20;
    EnemyHealth enemy;

    private void Start() {
        rb=GetComponent<Rigidbody2D>();
        rb.velocity=transform.right*speed;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        enemy=other.GetComponent<EnemyHealth>();
        if(enemy!=null){
            enemy.TakeDamage(damage);
            AudioManager.Instance.PlaySFX("Die");
        }
        Destroy(gameObject);
        
    }
    
}
