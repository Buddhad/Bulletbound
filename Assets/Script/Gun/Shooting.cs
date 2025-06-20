using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public Animator anim;
    public int ammoAmmount = 14;
    private bool isFiring;
    public TextMeshProUGUI showAmmo;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    private void Update()
    {
        if (Time.timeScale == 0f) return; // Pause protection
        showAmmo.text = ammoAmmount > 0 ? "Bullet: " + ammoAmmount + "/14" : "Out of Ammo!";
        Shoot();
    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime && ammoAmmount > 0)
        {
            Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            AudioManager.Instance.PlaySFX("Gun");
            ammoAmmount--;
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ammoAmmount = 7;
            anim.SetTrigger("reload");
            AudioManager.Instance.PlaySFX("Reload");
        }
    }
}
