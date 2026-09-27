using System.Collections;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private PlayerShooter weaponScript;

    private bool isDoubleCoinActive = false;
    private bool isShieldActive = false;

    [Header("Boost Values")]
    public float speedBoostAmount = 2f;
    public float jumpBoostAmount = 1.5f;
    public float fireRateBoostMultiplier = 0.5f;
    public float healthBoostAmount = 100f;

    private float scoreMultiplier = 1f;

    private float originalSpeed;
    private float originalJumpForce;
    private float originalFireRate;

    [Header("Shield")]
    public float shieldDuration = 5f;

    private void Awake()
    {
        // Get required player components.
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        weaponScript = GetComponent<PlayerShooter>();

        // Check PlayerMovement
        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
            originalJumpForce = playerMovement.jumpForce;
        }
        else
        {
            Debug.LogError(
                "PlayerAbilityManager: PlayerMovement is missing!",
                this
            );
        }

        // Check PlayerShooter
        if (weaponScript != null)
        {
            originalFireRate = weaponScript.fireRate;
        }
        else
        {
            Debug.LogError(
                "PlayerAbilityManager: PlayerShooter is missing!",
                this
            );
        }

        // Check PlayerHealth
        if (playerHealth == null)
        {
            Debug.LogError(
                "PlayerAbilityManager: PlayerHealth is missing!",
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
    // JUMP BOOST
    // =========================================================

    public void ActivateJumpBoost(float duration)
    {
        if (playerMovement == null)
            return;

        StopCoroutine(nameof(ResetJump));

        playerMovement.jumpForce =
            originalJumpForce * jumpBoostAmount;

        PlaySFX("SpeedBoostSFX");

        StartCoroutine(ResetJump(duration));
    }

    private IEnumerator ResetJump(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (playerMovement != null)
        {
            playerMovement.jumpForce = originalJumpForce;
        }
    }

    // =========================================================
    // SHIELD
    // =========================================================

    public void ActivateShield()
    {
        if (isShieldActive)
            return;

        isShieldActive = true;

        PlaySFX("SpeedBoostSFX");

        Debug.Log("Shield Activated");

        StartCoroutine(ShieldDuration());
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(shieldDuration);

        isShieldActive = false;

        Debug.Log("Shield Deactivated");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    // =========================================================
    // FIRE RATE BOOST
    // =========================================================

    public void ActivateFireRateBoost(
        float newRate,
        float duration)
    {
        if (weaponScript == null)
            return;

        StopCoroutine(nameof(ResetFireRate));

        float originalRate = weaponScript.fireRate;

        weaponScript.fireRate = newRate;

        PlaySFX("SpeedBoostSFX");

        StartCoroutine(
            ResetFireRate(originalRate, duration)
        );
    }

    private IEnumerator ResetFireRate(
        float originalRate,
        float duration)
    {
        yield return new WaitForSeconds(duration);

        if (weaponScript != null)
        {
            weaponScript.fireRate = originalRate;
        }
    }

    // =========================================================
    // HEALTH BOOST
    // =========================================================

    public void ActivateHealthBoost()
    {
        if (playerHealth == null)
            return;

        playerHealth.RestoreHealth(healthBoostAmount);

        PlaySFX("SpeedBoostSFX");
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

        StartCoroutine(
            ResetDoubleCoins(duration)
        );
    }

    private IEnumerator ResetDoubleCoins(float duration)
    {
        yield return new WaitForSeconds(duration);

        isDoubleCoinActive = false;

        scoreMultiplier = 1f;

        Debug.Log("Double Coins Ended");
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
                "PlayerAbilityManager: AudioManager.Instance " +
                "is missing. SFX '" +
                soundName +
                "' was not played."
            );
        }
    }
}