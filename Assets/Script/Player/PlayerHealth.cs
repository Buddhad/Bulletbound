using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // =========================================================
    // HEALTH SETTINGS
    // =========================================================

    [Header("Health")]

    // Current Player health.
    public float health = 100f;

    // Maximum Player health.
    public float maxHealth = 100f;


    // =========================================================
    // UI
    // =========================================================

    [Header("UI")]

    // Actual green health fill Image.
    //
    // This is found automatically from the current level.
    public Image healthbar;

    // Game Over panel from the current level Canvas.
    [SerializeField] private GameObject GameOverScreen;


    // =========================================================
    // DEBUG
    // =========================================================

    [Header("Debug")]

    [Tooltip("Player cannot lose health when enabled.")]
    public bool debugInvincible = false;


    // =========================================================
    // INTERNAL REFERENCES
    // =========================================================

    private PlayerAbilityManager abilityManager;


    // =========================================================
    // INTERNAL STATE
    // =========================================================

    private bool isDead = false;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // Get PlayerAbilityManager from the persistent Player.
        abilityManager = GetComponent<PlayerAbilityManager>();


        // -----------------------------------------------------
        // VALIDATE MAX HEALTH
        // -----------------------------------------------------

        if (maxHealth <= 0f)
        {
            maxHealth = health;
        }


        // -----------------------------------------------------
        // VALIDATE STARTING HEALTH
        // -----------------------------------------------------

        if (health <= 0f)
        {
            health = maxHealth;
        }


        // Make sure health is inside valid range.
        health = Mathf.Clamp(
            health,
            0f,
            maxHealth
        );


        // Do NOT search for UI here.
        //
        // The Player persists between levels,
        // but the Canvas belongs to each level.
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Build Index 0 = Main Menu.
        //
        // Main Menu doesn't need gameplay Health UI.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }


        // Find the current level's UI.
        FindHealthUI();


        // Display current health.
        UpdateHealthBar();
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
        // Ignore Main Menu.
        if (scene.buildIndex == 0)
        {
            return;
        }


        Debug.Log(
            "PlayerHealth: Gameplay scene loaded: " +
            scene.name
        );


        // Find the new scene's HealthBar UI.
        FindHealthUI();


        // Update the new scene's HealthBar
        // using the persistent Player health.
        UpdateHealthBar();


        // Reset death state for the new level.
        isDead = false;
    }


    // =========================================================
    // FIND HEALTH UI
    // =========================================================

    private void FindHealthUI()
    {
        // Clear references from the previous scene.
        healthbar = null;
        GameOverScreen = null;


        // -----------------------------------------------------
        // FIND GREEN HEALTH FILL
        // -----------------------------------------------------

        // Your hierarchy is:
        //
        // HealthBar
        // ├── Border
        // ├── Red
        // └── Green
        //
        // Green contains the Image component with:
        // Image Type = Filled
        // Fill Method = Horizontal

        healthbar = FindUIObject<Image>("Green");


        // -----------------------------------------------------
        // FIND GAME OVER
        // -----------------------------------------------------

        GameOverScreen = FindGameObject("GameOver");


        // -----------------------------------------------------
        // DEBUG
        // -----------------------------------------------------

        if (healthbar != null)
        {
            Debug.Log(
                "PlayerHealth: Green health bar connected."
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerHealth: Green health image was not found in scene: " +
                SceneManager.GetActiveScene().name
            );
        }


        if (GameOverScreen != null)
        {
            Debug.Log(
                "PlayerHealth: GameOver connected."
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerHealth: GameOver was not found in scene: " +
                SceneManager.GetActiveScene().name
            );
        }
    }


    // =========================================================
    // FIND UI COMPONENT
    // =========================================================

    private T FindUIObject<T>(string objectName)
        where T : Component
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
    // FIND GAMEOBJECT
    // =========================================================

    private GameObject FindGameObject(
        string objectName
    )
    {
        GameObject[] objects =
            Resources.FindObjectsOfTypeAll<GameObject>();


        foreach (GameObject obj in objects)
        {
            if (obj == null)
            {
                continue;
            }


            // Ignore prefab assets.
            if (!obj.scene.IsValid())
            {
                continue;
            }


            // Ignore unloaded scenes.
            if (!obj.scene.isLoaded)
            {
                continue;
            }


            // Find exact GameObject name.
            if (obj.name == objectName)
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
        // Keep health bar synchronized.
        UpdateHealthBar();


        // Check for death.
        if (health <= 0f && !isDead)
        {
            Die();
        }
    }


    // =========================================================
    // TAKE DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        // -----------------------------------------------------
        // DEBUG INVINCIBILITY
        // -----------------------------------------------------

        if (debugInvincible)
        {
            Debug.Log(
                "DEBUG: Player damage blocked. " +
                "Incoming damage: " +
                damage
            );

            return;
        }


        // -----------------------------------------------------
        // CHECK DEATH
        // -----------------------------------------------------

        if (isDead)
        {
            return;
        }


        // -----------------------------------------------------
        // VALIDATE DAMAGE
        // -----------------------------------------------------

        if (damage <= 0f)
        {
            return;
        }


        // -----------------------------------------------------
        // SHIELD CHECK
        // -----------------------------------------------------

        if (abilityManager != null &&
            abilityManager.IsShieldActive())
        {
            Debug.Log(
                "Hit blocked by Shield!"
            );

            return;
        }


        // -----------------------------------------------------
        // DAMAGE SOUND
        // -----------------------------------------------------

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                "Player_Damage"
            );
        }


        // -----------------------------------------------------
        // APPLY DAMAGE
        // -----------------------------------------------------

        health -= damage;


        // Prevent negative health.
        health = Mathf.Max(
            health,
            0f
        );


        // Update HealthBar immediately.
        UpdateHealthBar();


        Debug.Log(
            "Player took " +
            damage +
            " damage. Health: " +
            health
        );


        // -----------------------------------------------------
        // CHECK DEATH
        // -----------------------------------------------------

        if (health <= 0f)
        {
            Die();
        }
    }


    // =========================================================
    // RESTORE HEALTH
    // =========================================================

    public void RestoreHealth(float amount)
    {
        // Don't restore health after death.
        if (isDead)
        {
            return;
        }


        // Ignore invalid healing.
        if (amount <= 0f)
        {
            return;
        }


        // Add healing.
        health += amount;


        // Prevent health from exceeding maximum.
        health = Mathf.Clamp(
            health,
            0f,
            maxHealth
        );


        // Update HealthBar.
        UpdateHealthBar();


        Debug.Log(
            "Player healed by " +
            amount +
            ". Health: " +
            health +
            "/" +
            maxHealth
        );
    }


    // =========================================================
    // DEATH
    // =========================================================

    private void Die()
    {
        // Prevent multiple death calls.
        if (isDead)
        {
            return;
        }


        // Mark Player as dead.
        isDead = true;


        Debug.Log(
            "Player died."
        );


        // -----------------------------------------------------
        // DEATH SOUND
        // -----------------------------------------------------

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                "Player_Die"
            );
        }


        // -----------------------------------------------------
        // SHOW GAME OVER
        // -----------------------------------------------------

        ShowGameOverScreen();


        // -----------------------------------------------------
        // DISABLE PLAYER
        // -----------------------------------------------------

        // Keep original behavior.
        gameObject.SetActive(false);
    }


    // =========================================================
    // GAME OVER
    // =========================================================

    private void ShowGameOverScreen()
    {
        // Show Game Over panel.
        if (GameOverScreen != null)
        {
            GameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "PlayerHealth: GameOverScreen is not assigned.",
                this
            );
        }


        // Pause the game.
        Time.timeScale = 0f;


        Debug.Log(
            "Game Over"
        );
    }


    // =========================================================
    // HEALTH BAR
    // =========================================================

    private void UpdateHealthBar()
    {
        // HealthBar not found yet.
        if (healthbar == null)
        {
            return;
        }


        // Prevent division by zero.
        if (maxHealth <= 0f)
        {
            return;
        }


        // Convert health to 0-1.
        //
        // 100 / 100 = 1.00
        // 75  / 100 = 0.75
        // 50  / 100 = 0.50
        // 25  / 100 = 0.25
        // 0   / 100 = 0.00

        healthbar.fillAmount =
            Mathf.Clamp01(
                health / maxHealth
            );
    }
}