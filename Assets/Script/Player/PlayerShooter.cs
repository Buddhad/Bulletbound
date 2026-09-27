using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting")]
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public float fireRate = 0.25f;

    [Header("Animation")]
    public Animator anim;

    [Header("Ammo")]
    public int maxAmmo = 14;
    public int ammoAmmount = 14;

    [Header("UI")]
    public TextMeshProUGUI showAmmo;

    [Header("Reload")]
    public float reloadDuration = 1f;
    public bool isReloading = false;

    private bool isFiring = false;
    private float nextFireTime = 0f;

    private void Awake()
    {
        // Automatically find Animator on Player
        // if it wasn't assigned in Inspector.
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        // Make sure ammo starts within valid range.
        ammoAmmount = Mathf.Clamp(ammoAmmount, 0, maxAmmo);

        ValidateReferences();
        UpdateAmmoUI();
    }

    private void Update()
    {
        // Don't do anything while the game is paused.
        if (Time.timeScale == 0f)
            return;

        // Don't shoot while reloading.
        if (isReloading)
            return;

        Shoot();

        // Reload
        if (Input.GetKeyDown(KeyCode.R) &&
            !isReloading &&
            ammoAmmount < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    // =========================================================
    // SHOOT
    // =========================================================

    private void Shoot()
    {
        // Check required references before shooting.
        if (shootingPoint == null)
        {
            Debug.LogError(
                "PlayerShooter: Shooting Point is not assigned!",
                this
            );

            return;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError(
                "PlayerShooter: Bullet Prefab is not assigned!",
                this
            );

            return;
        }

        if (Input.GetKeyDown(KeyCode.F) &&
            !isFiring &&
            ammoAmmount > 0 &&
            Time.time >= nextFireTime)
        {
            // Create bullet.
            Instantiate(
                bulletPrefab,
                shootingPoint.position,
                transform.rotation
            );

            // Play gun sound if AudioManager exists.
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Gun");
            }
            else
            {
                Debug.LogWarning(
                    "PlayerShooter: AudioManager.Instance is missing."
                );
            }

            isFiring = true;

            ammoAmmount--;

            nextFireTime = Time.time + fireRate;

            // Currently there is no firing animation delay.
            isFiring = false;

            UpdateAmmoUI();
        }
    }

    // =========================================================
    // RELOAD
    // =========================================================

    private IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        if (ammoAmmount >= maxAmmo)
            yield break;

        isReloading = true;

        Debug.Log("Reloading...");

        // Reload animation
        if (anim != null)
        {
            anim.SetBool("isReloading", true);
        }

        yield return new WaitForSeconds(reloadDuration);

        // Reload sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Reload");
        }
        else
        {
            Debug.LogWarning(
                "PlayerShooter: AudioManager.Instance is missing."
            );
        }

        // Refill ammo.
        ammoAmmount = maxAmmo;

        // Stop reload animation.
        if (anim != null)
        {
            anim.SetBool("isReloading", false);
        }

        isReloading = false;
        isFiring = false;

        UpdateAmmoUI();

        Debug.Log("Reload complete.");
    }

    // =========================================================
    // AMMO UI
    // =========================================================

    private void UpdateAmmoUI()
    {
        if (showAmmo == null)
        {
            // Don't generate a NullReferenceException.
            return;
        }

        if (ammoAmmount <= 0)
        {
            showAmmo.text = "Out of Ammo!";
        }
        else
        {
            showAmmo.text =
                "Bullet: " +
                ammoAmmount +
                "/" +
                maxAmmo;
        }
    }

    // =========================================================
    // VALIDATE REFERENCES
    // =========================================================

    private void ValidateReferences()
    {
        if (shootingPoint == null)
        {
            Debug.LogWarning(
                "PlayerShooter: Shooting Point is not assigned.",
                this
            );
        }

        if (bulletPrefab == null)
        {
            Debug.LogWarning(
                "PlayerShooter: Bullet Prefab is not assigned.",
                this
            );
        }

        if (anim == null)
        {
            Debug.LogWarning(
                "PlayerShooter: Animator is not assigned.",
                this
            );
        }

        if (showAmmo == null)
        {
            Debug.LogWarning(
                "PlayerShooter: Ammo UI (Show Ammo) is not assigned.",
                this
            );
        }
    }
}