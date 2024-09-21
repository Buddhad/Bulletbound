using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    public PlayerHealth pHealth;
    public float _heal;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            AudioManager.Instance.PlaySFX("HealSFX");
            pHealth.health+=_heal;
            gameObject.SetActive(false);
        }
    }
}
