using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePowerEffect : MonoBehaviour
{
    public Powerups powerups;
    private void OnTriggerEnter2D(Collider2D other) {
        powerups.Apply(other.gameObject);
        AudioManager.Instance.PlaySFX("HealSFX");
        Destroy(gameObject);
    }
}
