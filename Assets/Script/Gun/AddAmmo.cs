using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    public Shooting ShootingDis;
    private int _ammo=7;


    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            ShootingDis.ammoAmmount+=_ammo;
            gameObject.SetActive(false);
        }
    }
}
