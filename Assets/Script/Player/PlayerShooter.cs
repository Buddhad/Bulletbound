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

    [Header("Magazine Ammo")]
    public int maxAmmo = 14;
    public int ammoAmmount = 14;

    [Header("Reserve Ammo")]
    [SerializeField] private int reserveAmmo = 0;

    public int ReserveAmmo => reserveAmmo;

    [Header("UI")]
    public TextMeshProUGUI showAmmo;

    [Tooltip("Separate UI text for reserve ammunition.")]
    public TextMeshProUGUI reserveAmmoText;

    [Header("Reload")]
    public float reloadDuration = 1f;
    public bool isReloading = false;

    private bool isFiring = false;
    private float nextFireTime = 0f;

    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        // Keep magazine ammo within valid range.
        ammoAmmount = Mathf.Clamp(
            ammoAmmount,
            0,
            maxAmmo
        );

        // Make sure reserve ammo isn't negative.
        reserveAmmo = Mathf.Max(
            reserveAmmo,
            0
        );

        ValidateReferences();
        UpdateAmmoUI();
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (isReloading)
            return;

        Shoot();

        // Reload with R.
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    // =========================================================
    // SHOOT
    // =========================================================

    private void Shoot()
    {
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
            Instantiate(
                bulletPrefab,
                shootingPoint.position,
                transform.rotation
            );

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Gun");
            }

            isFiring = true;

            ammoAmmount--;

            nextFireTime =
                Time.time + fireRate;

            isFiring = false;

            UpdateAmmoUI();
        }
    }

    // =========================================================
    // TRY RELOAD
    // =========================================================

    private void TryReload()
    {
        if (isReloading)
            return;

        // Magazine already full.
        if (ammoAmmount >= maxAmmo)
        {
            Debug.Log("Magazine already full.");
            return;
        }

        // No reserve bullets.
        if (reserveAmmo <= 0)
        {
            Debug.Log("No reserve ammunition.");
            return;
        }

        StartCoroutine(Reload());
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

        if (reserveAmmo <= 0)
            yield break;

        isReloading = true;

        Debug.Log("Reloading...");

        // Reload animation.
        if (anim != null)
        {
            anim.SetBool(
                "isReloading",
                true
            );
        }

        yield return new WaitForSeconds(
            reloadDuration
        );

        // Calculate how many bullets are needed.
        int bulletsNeeded =
            maxAmmo - ammoAmmount;

        // Only load bullets that are actually available.
        int bulletsToLoad =
            Mathf.Min(
                bulletsNeeded,
                reserveAmmo
            );

        ammoAmmount += bulletsToLoad;

        reserveAmmo -= bulletsToLoad;

        // Reload sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Reload");
        }

        // Stop reload animation.
        if (anim != null)
        {
            anim.SetBool(
                "isReloading",
                false
            );
        }

        isReloading = false;
        isFiring = false;

        UpdateAmmoUI();

        Debug.Log(
            "Reload complete. Magazine: " +
            ammoAmmount +
            "/" +
            maxAmmo +
            " | Reserve: " +
            reserveAmmo
        );
    }

    // =========================================================
    // ADD RESERVE AMMO
    // =========================================================

    public void AddReserveAmmo(int amount)
    {
        if (amount <= 0)
            return;

        reserveAmmo += amount;

        UpdateAmmoUI();

        Debug.Log(
            "Purchased bullets: +" +
            amount +
            " | Reserve Ammo: " +
            reserveAmmo
        );
    }

    // =========================================================
    // AMMO UI
    // =========================================================

    private void UpdateAmmoUI()
    {
        // Main ammo UI.
        //
        // Example:
        // Bullet: 14/14 

        if (showAmmo != null)
        {
            showAmmo.text =
                "Bullet: " +
                ammoAmmount +
                "/" +
                maxAmmo;
        }

        // Separate reserve ammo UI.
        //
        // Example:
        // Reserve: 30

        if (reserveAmmoText != null)
        {
            reserveAmmoText.text =
                "Reserve: " +
                reserveAmmo;
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
                "PlayerShooter: Main Ammo UI is not assigned.",
                this
            );
        }

        if (reserveAmmoText == null)
        {
            Debug.LogWarning(
                "PlayerShooter: Reserve Ammo Text is not assigned.",
                this
            );
        }
    }
}