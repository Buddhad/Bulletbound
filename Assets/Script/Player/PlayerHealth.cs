using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("UI")]
    public Image healthbar;
    [SerializeField] private GameObject GameOverScreen;

    [Header("Debug")]
    [Tooltip("Player cannot lose health when enabled.")]
    public bool debugInvincible = false;

    private bool isDead = false;

    private void Awake()
    {
        // Make sure max health is valid.
        if (maxHealth <= 0)
        {
            maxHealth = health;
        }

        // Make sure health starts correctly.
        if (health <= 0)
        {
            health = maxHealth;
        }

        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    // =========================================================
    // TAKE DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        // DEBUG MODE
        // Player can still be attacked, but takes no damage.
        if (debugInvincible)
        {
            Debug.Log(
                "DEBUG: Player damage blocked. " +
                "Incoming damage: " + damage
            );

            return;
        }

        // Don't take damage after death.
        if (isDead)
            return;

        // Check shield.
        PlayerAbilityManager abilities =
            GetComponent<PlayerAbilityManager>();

        if (abilities != null &&
            abilities.IsShieldActive())
        {
            Debug.Log("Hit blocked by Shield!");

            return;
        }

        // Play damage sound safely.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Player_Damage");
        }

        // Apply damage.
        health -= damage;

        // Prevent negative health.
        health = Mathf.Max(health, 0f);

        UpdateHealthBar();

        Debug.Log(
            "Player took " +
            damage +
            " damage. Health: " +
            health
        );
    }

    // =========================================================
    // RESTORE HEALTH
    // =========================================================

    public void RestoreHealth(float amount)
    {
        if (isDead)
            return;

        health += amount;

        health = Mathf.Clamp(
            health,
            0f,
            maxHealth
        );

        UpdateHealthBar();
    }

    // =========================================================
    // DEATH
    // =========================================================

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Player died.");

        // Disable player.
        gameObject.SetActive(false);

        ShowGameOverScreen();

        // Play death sound safely.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Player_Die");
        }
    }

    // =========================================================
    // GAME OVER
    // =========================================================

    private void ShowGameOverScreen()
    {
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

        Invoke(nameof(GameOver), 1f);

        Time.timeScale = 0f;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    // =========================================================
    // HEALTH BAR
    // =========================================================

    private void UpdateHealthBar()
    {
        if (healthbar == null)
            return;

        if (maxHealth <= 0)
            return;

        healthbar.fillAmount =
            Mathf.Clamp01(health / maxHealth);
    }
}