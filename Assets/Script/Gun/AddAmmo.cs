using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    public PlayerShooter ShootingDis;
    private int _ammo=14; // Amount of ammo to add

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            ShootingDis.ammoAmmount+=_ammo;
            gameObject.SetActive(false);
            AudioManager.Instance.PlaySFX("AddReload");
        }
    }
}
