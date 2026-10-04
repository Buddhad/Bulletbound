using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int reserveAmmo = 30;

    public int ReserveAmmo => reserveAmmo;

    [Header("UI")]
    public TextMeshProUGUI showAmmo;

    [Tooltip("Separate UI text for reserve ammunition.")]
    public TextMeshProUGUI reserveAmmoText;

    [Header("Reload")]
    public float reloadDuration = 1f;
    public bool isReloading = false;

    [Header("Camera Shake")]
    private CamShake camShake;

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
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Main Menu = Build Index 0.
        // There is no gameplay UI there.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        FindAmmoUI();
        FindCameraShake();

        UpdateAmmoUI();
    }


    // =========================================================
    // ENABLE
    // =========================================================

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // =========================================================
    // DISABLE
    // =========================================================

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // =========================================================
    // SCENE LOADED
    // =========================================================

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        // Build Index 0 = Main Menu.
        if (scene.buildIndex == 0)
        {
            return;
        }

        Debug.Log(
            "PlayerShooter: Gameplay scene loaded: " +
            scene.name
        );

        // Find the new level's UI.
        FindAmmoUI();

        // Find the new level's camera shake.
        FindCameraShake();

        // Display current persistent ammo.
        UpdateAmmoUI();
    }


    // =========================================================
    // FIND CAMERA SHAKE
    // =========================================================

    private void FindCameraShake()
    {
        camShake = null;

        // Find the CamShake component in the current scene.
        camShake = FindFirstObjectByType<CamShake>();

        if (camShake != null)
        {
            Debug.Log(
                "PlayerShooter: CamShake connected in " +
                SceneManager.GetActiveScene().name
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerShooter: CamShake was not found in " +
                SceneManager.GetActiveScene().name
            );
        }
    }


    // =========================================================
    // FIND AMMO UI
    // =========================================================

    private void FindAmmoUI()
    {
        // Clear references to previous scene UI.
        showAmmo = null;
        reserveAmmoText = null;


        // Find current level AmmoText.
        showAmmo = FindUIObject<TextMeshProUGUI>(
            "AmmoText"
        );


        // Find current level ReserveAmmoText.
        reserveAmmoText = FindUIObject<TextMeshProUGUI>(
            "ReserveAmmoText"
        );


        // Debug information.
        if (showAmmo != null)
        {
            Debug.Log(
                "PlayerShooter: AmmoText connected."
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerShooter: AmmoText was not found in scene: " +
                SceneManager.GetActiveScene().name
            );
        }


        if (reserveAmmoText != null)
        {
            Debug.Log(
                "PlayerShooter: ReserveAmmoText connected."
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerShooter: ReserveAmmoText was not found in scene: " +
                SceneManager.GetActiveScene().name
            );
        }
    }


    // =========================================================
    // FIND UI OBJECT
    // =========================================================

    private T FindUIObject<T>(
        string objectName
    ) where T : Component
    {
        T[] objects =
            Resources.FindObjectsOfTypeAll<T>();

        foreach (T obj in objects)
        {
            if (obj == null)
            {
                continue;
            }

            GameObject uiObject =
                obj.gameObject;

            // Ignore prefab assets.
            if (!uiObject.scene.IsValid())
            {
                continue;
            }

            // Ignore unloaded scenes.
            if (!uiObject.scene.isLoaded)
            {
                continue;
            }

            // Find exact GameObject name.
            if (uiObject.name == objectName)
            {
                return obj;
            }
        }

        return null;
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (isReloading)
        {
            return;
        }

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
            // -------------------------------------------------
            // CREATE BULLET
            // -------------------------------------------------

            Instantiate(
                bulletPrefab,
                shootingPoint.position,
                transform.rotation
            );


            // -------------------------------------------------
            // CAMERA SHAKE
            // -------------------------------------------------

            if (camShake != null)
            {
                camShake.ShakeCamera();
            }
            else
            {
                Debug.LogWarning(
                    "PlayerShooter: CamShake is missing. " +
                    "Camera shake could not be played."
                );
            }


            // -------------------------------------------------
            // GUN SOUND
            // -------------------------------------------------

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Gun");
            }


            // -------------------------------------------------
            // AMMO
            // -------------------------------------------------

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
        {
            return;
        }


        if (ammoAmmount >= maxAmmo)
        {
            Debug.Log(
                "Magazine already full."
            );

            return;
        }


        if (reserveAmmo <= 0)
        {
            Debug.Log(
                "No reserve ammunition."
            );

            return;
        }


        StartCoroutine(
            Reload()
        );
    }


    // =========================================================
    // RELOAD
    // =========================================================

    private IEnumerator Reload()
    {
        if (isReloading)
        {
            yield break;
        }


        if (ammoAmmount >= maxAmmo)
        {
            yield break;
        }


        if (reserveAmmo <= 0)
        {
            yield break;
        }


        isReloading = true;

        Debug.Log(
            "Reloading..."
        );


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


        int bulletsNeeded =
            maxAmmo - ammoAmmount;


        int bulletsToLoad =
            Mathf.Min(
                bulletsNeeded,
                reserveAmmo
            );


        ammoAmmount +=
            bulletsToLoad;


        reserveAmmo -=
            bulletsToLoad;


        // -----------------------------------------------------
        // RELOAD SOUND
        // -----------------------------------------------------

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                "Reload"
            );
        }


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
        {
            return;
        }


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
        if (showAmmo != null)
        {
            showAmmo.text =
                "Bullet: " +
                ammoAmmount +
                "/" +
                maxAmmo;
        }


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


        // UI references are intentionally not validated.
        // The Player is persistent.
        // The Canvas changes with every gameplay scene.
    }
}