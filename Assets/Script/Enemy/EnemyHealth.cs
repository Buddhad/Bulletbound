using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem deathEffect;
    //Particle effect for death
    public float health=100;
    //public int speed;
    private Rigidbody2D rb;

private void Start() {
    rb=GetComponent<Rigidbody2D>();
}

    public void TakeDamage(float damage){
        health-=damage;
        if(health<=0){
            Die();
        deathEffect.Play();
        }
    }
    public void Die(){
        //Instantiate(DeathEffect,transform.position,Quaternion.identity);
        AudioManager.Instance.PlaySFX("Hurt");
        Destroy(gameObject);
        
    }
}
