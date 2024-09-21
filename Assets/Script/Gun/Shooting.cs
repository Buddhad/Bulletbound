using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public Animator anim;
    public int ammoAmmount=7;
    private bool isFiring;
    public TextMeshProUGUI showAmmo;

    private void Update() {
    Shoot();
    }
    private void Shoot(){
        showAmmo.text=ammoAmmount.ToString("Bullet: "+ammoAmmount+"/7");
    if(Input.GetKeyDown(KeyCode.F) && !isFiring && ammoAmmount>0){
        Instantiate(bulletPrefab,shootingPoint.position,transform.rotation);
                isFiring=true;
                ammoAmmount--;
                isFiring=false;
        }if(ammoAmmount==0){
            showAmmo.text=ammoAmmount.ToString("Out of Ammo!");
        }
        if(Input.GetKey(KeyCode.R)){
            ammoAmmount=7;
            anim.SetTrigger("reload");
            
        }
    }
}
