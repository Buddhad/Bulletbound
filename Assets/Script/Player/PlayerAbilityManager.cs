using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerAbilityManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private PlayerShooter weaponScript;
    private bool isDoubleCoinActive = false;

    [Header("Boost Values")]
    public float speedBoostAmount = 2f;
    public float jumpBoostAmount = 1.5f;
    private float scoreMultiplier = 1f;
    public float fireRateBoostMultiplier = 0.5f;
    public float healthBoostAmount = 100f;

    private float originalSpeed;
    private float originalJumpForce;
    private float originalFireRate;
    private bool isShieldActive = false;
    public float shieldDuration = 5f;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        weaponScript = GetComponent<PlayerShooter>();

        originalSpeed = playerMovement.moveSpeed;
        originalJumpForce = playerMovement.jumpForce;
        originalFireRate = weaponScript.fireRate;
    }

    // Speed Boost
    public void ActivateSpeedBoost(float duration)
    {
        StopCoroutine("ResetSpeed");
        playerMovement.moveSpeed *= speedBoostAmount;
        AudioManager.Instance.PlaySFX("SpeedBoostSFX");
        StartCoroutine(ResetSpeed(duration));
    }

    private IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        playerMovement.moveSpeed = originalSpeed;
    }

    // Jump Boost
    public void ActivateJumpBoost(float duration)
    {
        StopCoroutine("ResetJump");
        playerMovement.jumpForce *= jumpBoostAmount;
        AudioManager.Instance.PlaySFX("SpeedBoostSFX");
        StartCoroutine(ResetJump(duration));
    }

    private IEnumerator ResetJump(float duration)
    {
        yield return new WaitForSeconds(duration);
        playerMovement.jumpForce = originalJumpForce;
    }

    // Shield
    public void ActivateShield()
    {
        if (!isShieldActive)
        {
            isShieldActive = true;
            AudioManager.Instance.PlaySFX("SpeedBoostSFX");
            Debug.Log("🛡️ Shield Activated");
            StartCoroutine(ShieldDuration());
        }
    }
    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(shieldDuration);
        isShieldActive = false;
        Debug.Log("🛡️ Shield Deactivated");
    }
    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    // Fire Rate Boost
    //Normally, the player fires 1 bullet per 0.5 seconds
    //After picking up Ability_FireRate, the player fires 1 bullet per 0.2 seconds (i.e., faster)
    //After 5 seconds, it goes back to normal
    public void ActivateFireRateBoost(float newRate, float duration)
    {
        PlayerShooter shoot = GetComponent<PlayerShooter>();
        if (shoot != null)
        {
            float originalRate = shoot.fireRate;
            shoot.fireRate = newRate;
            StartCoroutine(ResetFireRate(originalRate, duration));
            AudioManager.Instance.PlaySFX("SpeedBoostSFX");
        }
    }

    private IEnumerator ResetFireRate(float originalRate, float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayerShooter shoot = GetComponent<PlayerShooter>();
        if (shoot != null)
        {
            shoot.fireRate = originalRate;
        }
    }
    // Health Boost
    public void ActivateHealthBoost()
    {
        playerHealth.RestoreHealth(healthBoostAmount);
        AudioManager.Instance.PlaySFX("SpeedBoostSFX");
    }

    // Double Coins
    public void ActivateDoubleCoins(float duration)
    {
        if (isDoubleCoinActive) return;
        isDoubleCoinActive = true;
        AudioManager.Instance.PlaySFX("CoinSFX");
        scoreMultiplier = 2f;
        Debug.Log("🪙 Double Coins Activated!");
        StartCoroutine(ResetDoubleCoins(duration));
    }
    private IEnumerator ResetDoubleCoins(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDoubleCoinActive = false;
        scoreMultiplier = 1f;
        Debug.Log("🪙 Double Coins Ended");
    }

    public float GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
