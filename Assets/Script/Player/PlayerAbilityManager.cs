using System.Collections;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private PlayerShooter weaponScript;

    // =========================================================
    // ABILITY STATE
    // =========================================================

    private bool isDoubleCoinActive = false;
    private bool isShieldActive = false;

    private float scoreMultiplier = 1f;

    // =========================================================
    // BOOST VALUES
    // =========================================================

    [Header("Speed Boost")]
    public float speedBoostAmount = 2f;

    [Header("Fire Rate Boost")]
    public float fireRateBoostMultiplier = 0.5f;

    [Header("Health Boost")]
    public float healthBoostAmount = 100f;

    [Header("Shield")]
    public float shieldDuration = 5f;

    // =========================================================
    // ORIGINAL VALUES
    // =========================================================

    private float originalSpeed;
    private float originalFireRate;

    // =========================================================
    // INITIALIZE
    // =========================================================

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        weaponScript = GetComponent<PlayerShooter>();

        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
        }
        else
        {
            Debug.LogWarning(
                "PlayerAbilityManager: PlayerMovement not found.",
                this
            );
        }

        if (weaponScript != null)
        {
            originalFireRate = weaponScript.fireRate;
        }
        else
        {
            Debug.LogWarning(
                "PlayerAbilityManager: PlayerShooter not found.",
                this
            );
        }

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

    public void ActivateSpeedBoost(float duration)
    {
        if (playerMovement == null)
            return;

        StopCoroutine(nameof(ResetSpeed));

        playerMovement.moveSpeed =
            originalSpeed * speedBoostAmount;

        PlaySFX("SpeedBoostSFX");

        StartCoroutine(ResetSpeed(duration));
    }

    private IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed;
        }
    }

    // =========================================================
    // FIRE RATE BOOST
    // =========================================================

    public void ActivateFireRateBoost(
        float newFireRate,
        float duration)
    {
        if (weaponScript == null)
            return;

        StopCoroutine(nameof(ResetFireRate));

        weaponScript.fireRate = newFireRate;

        PlaySFX("SpeedBoostSFX");

        StartCoroutine(ResetFireRate(duration));
    }

    private IEnumerator ResetFireRate(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (weaponScript != null)
        {
            weaponScript.fireRate = originalFireRate;
        }
    }

    // =========================================================
    // SHIELD
    // =========================================================

    public void ActivateShield()
    {
        StopCoroutine(nameof(ResetShield));

        isShieldActive = true;

        PlaySFX("ShieldSFX");

        StartCoroutine(ResetShield(shieldDuration));

        Debug.Log("Shield Activated!");
    }

    private IEnumerator ResetShield(float duration)
    {
        yield return new WaitForSeconds(duration);

        isShieldActive = false;

        Debug.Log("Shield Deactivated!");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    // =========================================================
    // HEALTH BOOST
    // =========================================================

    public void ActivateHealthBoost()
    {
        if (playerHealth == null)
            return;

        playerHealth.RestoreHealth(healthBoostAmount);

        PlaySFX("HealthSFX");

        Debug.Log(
            "Health restored by " + healthBoostAmount
        );
    }

    // =========================================================
    // DOUBLE COINS
    // =========================================================

    public void ActivateDoubleCoins(float duration)
    {
        if (isDoubleCoinActive)
            return;

        isDoubleCoinActive = true;

        scoreMultiplier = 2f;

        PlaySFX("CoinSFX");

        Debug.Log("Double Coins Activated!");

        StartCoroutine(ResetDoubleCoins(duration));
    }

    private IEnumerator ResetDoubleCoins(float duration)
    {
        yield return new WaitForSeconds(duration);

        scoreMultiplier = 1f;
        isDoubleCoinActive = false;

        Debug.Log("Double Coins Deactivated!");
    }

    public float GetScoreMultiplier()
    {
        return scoreMultiplier;
    }

    // =========================================================
    // AUDIO
    // =========================================================

    private void PlaySFX(string soundName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(soundName);
        }
        else
        {
            Debug.LogWarning(
                "PlayerAbilityManager: AudioManager.Instance is missing."
            );
        }
    }
}