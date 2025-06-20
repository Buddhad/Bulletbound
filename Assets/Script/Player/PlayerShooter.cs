using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShooter : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public Animator anim;
    public int ammoAmmount = 14;
    private bool isFiring;
    public TextMeshProUGUI showAmmo;
    public float reloadDuration = 1f;
    public bool isReloading = false;
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        Shoot();
    }
    private void Shoot()
    {
        showAmmo.text = "Bullet: " + ammoAmmount + "/14";
        if (Input.GetKeyDown(KeyCode.F) && !isFiring && ammoAmmount > 0)
        {
            Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            AudioManager.Instance.PlaySFX("Gun");
            isFiring = true;
            ammoAmmount--;
            isFiring = false;
        }
        // Display ammo count
        if (ammoAmmount == 0)
        {
            showAmmo.text = "Out of Ammo!";
        }
        else
        {
            showAmmo.text = $"Bullet: {ammoAmmount}/14";
        }
        // Reload logic
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoAmmount < 14)
        {
            StartCoroutine(Reload());
        }
        // Reload coroutine
        IEnumerator Reload()
        {
            isReloading = true;
            anim.SetBool("isReloading", true);
            yield return new WaitForSeconds(reloadDuration); // Adjust the reload time as needed
            AudioManager.Instance.PlaySFX("Reload");
            ammoAmmount = 14; // Reset ammo to full after reload
            anim.SetBool("isReloading", false);
            isReloading = false;
            isFiring = false;
        }
        // Update ammo display
        showAmmo.text = "Bullet: " + ammoAmmount + "/14";
        if (ammoAmmount < 0)
        {
            ammoAmmount = 0; // Prevent negative ammo count
        }
        if (ammoAmmount > 14)
        {
            ammoAmmount = 14; // Prevent exceeding max ammo count
        }


    }
}
