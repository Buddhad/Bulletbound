using System.Collections;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    // =========================================================
    // PLAYER REFERENCES
    // =========================================================

    // Reference to the PlayerMovement component.
    private PlayerMovement playerMovement;

    // Reference to the PlayerHealth component.
    private PlayerHealth playerHealth;

    // Reference to the PlayerShooter component.
    private PlayerShooter weaponScript;


    // =========================================================
    // ABILITY STATE
    // =========================================================

    // True while Double Coins is active.
    private bool isDoubleCoinActive = false;

    // True while Shield is active.
    private bool isShieldActive = false;

    // Current score multiplier.
    //
    // Normal = 1x
    // Double Coins = 2x
   
    // =========================================================
    // ABILITY VALUES
    // =========================================================

    [Header("Speed Boost")]

    // Multiplier applied to the original movement speed.
    //
    // Example:
    // Original Speed = 5
    // Speed Boost = 2
    // New Speed = 10
    public float speedBoostAmount = 2f;


    [Header("Fire Rate Boost")]

    // Fire rate value used when the Fire Rate ability
    // is purchased.
    //
    // Smaller value = faster shooting.
    public float fireRateBoostAmount = 0.1f;


    [Header("Health Boost")]

    // Amount of health restored when Health Boost
    // is purchased.
    public float healthBoostAmount = 100f;


    [Header("Shield")]

    // How long the Shield remains active.
    public float shieldDuration = 5f;


    [Header("Double Coins")]

    // How long Double Coins remains active.
    public float doubleCoinsDuration = 5f;


    // =========================================================
    // ORIGINAL PLAYER VALUES
    // =========================================================

    // Player's original movement speed.
    private float originalSpeed;

    // Player's original fire rate.
    private float originalFireRate;


    // =========================================================
    // INITIALIZE
    // =========================================================

    private void Awake()
    {
        // Get all required Player components.
        playerMovement =
            GetComponent<PlayerMovement>();

        playerHealth =
            GetComponent<PlayerHealth>();

        weaponScript =
            GetComponent<PlayerShooter>();


        // -----------------------------------------------------
        // PLAYER MOVEMENT
        // -----------------------------------------------------

        if (playerMovement != null)
        {
            // Store the original movement speed.
            // This allows us to restore it after the
            // Speed Boost expires.
            originalSpeed =
                playerMovement.moveSpeed;
        }
        else
        {
            Debug.LogWarning(
                "PlayerAbilityManager: PlayerMovement not found.",
                this
            );
        }


        // -----------------------------------------------------
        // PLAYER SHOOTER
        // -----------------------------------------------------

        if (weaponScript != null)
        {
            // Store the original fire rate.
            // This allows us to restore it after the
            // Fire Rate Boost expires.
            originalFireRate =
                weaponScript.fireRate;
        }
        else
        {
            Debug.LogWarning(
                "PlayerAbilityManager: PlayerShooter not found.",
                this
            );
        }


        // -----------------------------------------------------
        // PLAYER HEALTH
        // -----------------------------------------------------

        if (playerHealth == null)
        {
            Debug.LogWarning(
                "PlayerAbilityManager: PlayerHealth not found.",
                this
            );
        }
    }


    // =========================================================
    // SPEED BOOST
    // =========================================================

    // Called by the Shop when the Player purchases
    // the Speed Boost ability.
    public void ActivateSpeedBoost(float duration)
    {
        // Make sure PlayerMovement exists.
        if (playerMovement == null)
            return;


        // Stop an existing Speed Boost timer.
        //
        // This prevents multiple reset coroutines from
        // running at the same time.
        StopCoroutine(nameof(ResetSpeed));


        // Apply the Speed Boost.
        playerMovement.moveSpeed =
            originalSpeed * speedBoostAmount;


        // Play Speed Boost sound.
        PlaySFX("SpeedBoostSFX");


        Debug.Log(
            "Speed Boost Activated! " +
            "Speed: " +
            playerMovement.moveSpeed +
            " | Duration: " +
            duration +
            " seconds."
        );


        // Start the timer that restores the original speed.
        StartCoroutine(ResetSpeed(duration));
    }


    private IEnumerator ResetSpeed(float duration)
    {
        // Wait for the ability duration.
        yield return new WaitForSeconds(duration);


        // Restore the original movement speed.
        if (playerMovement != null)
        {
            playerMovement.moveSpeed =
                originalSpeed;
        }


        Debug.Log("Speed Boost Deactivated.");
    }


    // =========================================================
    // FIRE RATE BOOST
    // =========================================================

    // Called by the Shop when the Player purchases
    // the Fire Rate Boost ability.
    public void ActivateFireRateBoost(float duration)
    {
        // Make sure PlayerShooter exists.
        if (weaponScript == null)
            return;


        // Stop an existing Fire Rate timer.
        StopCoroutine(nameof(ResetFireRate));


        // Apply the faster fire rate.
        weaponScript.fireRate =
            fireRateBoostAmount;


        // Play Fire Rate Boost sound.
        PlaySFX("FireRateBoostSFX");


        Debug.Log(
            "Fire Rate Boost Activated! " +
            "Fire Rate: " +
            weaponScript.fireRate +
            " | Duration: " +
            duration +
            " seconds."
        );


        // Start the reset timer.
        StartCoroutine(
            ResetFireRate(duration)
        );
    }


    private IEnumerator ResetFireRate(float duration)
    {
        // Wait for the ability duration.
        yield return new WaitForSeconds(duration);


        // Restore the original fire rate.
        if (weaponScript != null)
        {
            weaponScript.fireRate =
                originalFireRate;
        }


        Debug.Log(
            "Fire Rate Boost Deactivated."
        );
    }


    // =========================================================
    // SHIELD
    // =========================================================

    // Called by the Shop when the Player purchases
    // the Shield ability.
    public void ActivateShield()
    {
        // Stop an existing Shield timer.
        StopCoroutine(nameof(ResetShield));


        // Activate Shield.
        isShieldActive = true;


        // Play Shield sound.
        PlaySFX("ShieldSFX");


        Debug.Log(
            "Shield Activated for " +
            shieldDuration +
            " seconds!"
        );


        // Start Shield duration timer.
        StartCoroutine(
            ResetShield(shieldDuration)
        );
    }


    private IEnumerator ResetShield(float duration)
    {
        // Wait for Shield duration.
        yield return new WaitForSeconds(duration);


        // Disable Shield.
        isShieldActive = false;


        Debug.Log(
            "Shield Deactivated!"
        );
    }


    // Used by PlayerHealth to determine whether
    // incoming damage should be blocked.
    public bool IsShieldActive()
    {
        return isShieldActive;
    }


    // =========================================================
    // HEALTH BOOST
    // =========================================================

    // Called by the Shop when the Player purchases
    // the Health Boost ability.
    public void ActivateHealthBoost()
    {
        // Make sure PlayerHealth exists.
        if (playerHealth == null)
            return;


        // Restore Player health.
        playerHealth.RestoreHealth(
            healthBoostAmount
        );


        // Play Health Boost sound.
        PlaySFX("HealthSFX");


        Debug.Log(
            "Health Boost Activated! " +
            "Health restored: " +
            healthBoostAmount
        );
    }


    /// =========================================================
// DOUBLE COINS
// =========================================================

public void ActivateDoubleCoins(float duration)
{
    // Prevent multiple Double Coins effects
    // from running at the same time.
    if (isDoubleCoinActive)
    {
        Debug.Log("Double Coins is already active.");
        return;
    }

    // Activate Double Coins.
    isDoubleCoinActive = true;

    // Play activation sound.
    PlaySFX("CoinSFX");

    Debug.Log(
        "Double Coins Activated for " +
        duration +
        " seconds!"
    );

    // Start duration timer.
    StartCoroutine(
        ResetDoubleCoins(duration)
    );
}


// =========================================================
// RESET DOUBLE COINS
// =========================================================

private IEnumerator ResetDoubleCoins(float duration)
{
    // Wait for the ability duration.
    yield return new WaitForSeconds(duration);

    // Disable Double Coins.
    isDoubleCoinActive = false;

    Debug.Log(
        "Double Coins Deactivated."
    );
}


// =========================================================
// CHECK DOUBLE COINS
// =========================================================

public bool IsDoubleCoinActive()
{
    return isDoubleCoinActive;
}
    // =========================================================
    // SCORE MULTIPLIER
    // =========================================================

    // ScoreManager uses this value to determine
    // how many points the Player receives.
    //
    // Normal:
    // 1 × 10 = 10
    //
    // Double Coins:
    // 2 × 10 = 20
    


    // =========================================================
    // AUDIO
    // =========================================================

    // Plays an ability-related sound through AudioManager.
    private void PlaySFX(string soundName)
    {
        // Check whether AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                soundName
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerAbilityManager: " +
                "AudioManager.Instance is missing."
            );
        }
    }
}