using UnityEngine;
using UnityEngine.UI;

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

    // Image used as the health bar.
    //
    // Make sure the Image Type is set to Filled
    // in the Inspector.
    public Image healthbar;

    // Game Over UI displayed when the Player dies.
    [SerializeField] private GameObject GameOverScreen;


    // =========================================================
    // DEBUG
    // =========================================================

    [Header("Debug")]

    [Tooltip("Player cannot lose health when enabled.")]

    // Useful while testing the game.
    //
    // TRUE  = Player cannot take damage.
    // FALSE = Normal health behavior.
    public bool debugInvincible = false;


    // =========================================================
    // INTERNAL REFERENCES
    // =========================================================

    // Reference to PlayerAbilityManager.
    //
    // Used to check whether Shield is active.
    private PlayerAbilityManager abilityManager;


    // =========================================================
    // INTERNAL STATE
    // =========================================================

    // Prevents the Player from dying multiple times.
    private bool isDead = false;


    // =========================================================
    // INITIALIZE
    // =========================================================

    private void Awake()
    {
        // -----------------------------------------------------
        // GET ABILITY MANAGER
        // -----------------------------------------------------

        abilityManager =
            GetComponent<PlayerAbilityManager>();


        // -----------------------------------------------------
        // VALIDATE MAX HEALTH
        // -----------------------------------------------------

        // If maxHealth is invalid, use the current
        // health value as the maximum.
        if (maxHealth <= 0f)
        {
            maxHealth = health;
        }


        // -----------------------------------------------------
        // VALIDATE STARTING HEALTH
        // -----------------------------------------------------

        // If health is invalid, start with full health.
        if (health <= 0f)
        {
            health = maxHealth;
        }


        // Make sure health never starts above max health.
        health =
            Mathf.Clamp(
                health,
                0f,
                maxHealth
            );


        // Update the health bar immediately.
        UpdateHealthBar();
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // Keep the health bar synchronized.
        UpdateHealthBar();


        // Check whether the Player has reached zero health.
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

        // When enabled, attacks are detected but the Player
        // does not actually lose health.
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

        // Don't allow damage after the Player has died.
        if (isDead)
            return;


        // -----------------------------------------------------
        // VALIDATE DAMAGE
        // -----------------------------------------------------

        // Ignore zero or negative damage.
        if (damage <= 0f)
            return;


        // -----------------------------------------------------
        // SHIELD CHECK
        // -----------------------------------------------------

        // If Shield is currently active, block the damage.
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

        // Play the Player damage sound.
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


        // Prevent health from becoming negative.
        health =
            Mathf.Max(
                health,
                0f
            );


        // Update the health bar immediately.
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

        // Die immediately when health reaches zero.
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
            return;


        // Ignore invalid healing values.
        if (amount <= 0f)
            return;


        // Add the healing amount.
        health += amount;


        // Prevent health from exceeding max health.
        health =
            Mathf.Clamp(
                health,
                0f,
                maxHealth
            );


        // Update the health bar.
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
            return;


        // Mark Player as dead.
        isDead = true;


        Debug.Log(
            "Player died."
        );


        // -----------------------------------------------------
        // DEATH SOUND
        // -----------------------------------------------------

        // Play death sound before disabling the Player.
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

        // Disable the Player after the death state
        // has been registered.
        gameObject.SetActive(false);
    }


    // =========================================================
    // GAME OVER
    // =========================================================

    private void ShowGameOverScreen()
    {
        // Show Game Over UI.
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


        // Log the Game Over state.
        Debug.Log(
            "Game Over"
        );
    }


    // =========================================================
    // HEALTH BAR
    // =========================================================

    private void UpdateHealthBar()
    {
        // Make sure the health bar exists.
        if (healthbar == null)
            return;


        // Prevent division by zero.
        if (maxHealth <= 0f)
            return;


        // Convert health into a 0-1 value.
        //
        // 100 / 100 = 1
        // 50 / 100  = 0.5
        // 0 / 100   = 0
        healthbar.fillAmount =
            Mathf.Clamp01(
                health / maxHealth
            );
    }
}