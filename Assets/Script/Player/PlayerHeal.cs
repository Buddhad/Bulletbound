using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    // =========================================================
    // HEALING SETTINGS
    // =========================================================

    [Header("Healing")]

    // Amount of health restored when the Player
    // collects this healing pickup.
    [SerializeField] private float healAmount = 25f;


    // =========================================================
    // PLAYER DETECTION
    // =========================================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        // -----------------------------------------------------
        // CHECK PLAYER
        // -----------------------------------------------------

        // Only react when the Player touches the pickup.
        if (!other.CompareTag("Player"))
            return;


        // -----------------------------------------------------
        // FIND PLAYER HEALTH
        // -----------------------------------------------------

        // Get the PlayerHealth component from the Player.
        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();


        // Make sure PlayerHealth exists.
        if (playerHealth == null)
        {
            Debug.LogWarning(
                "PlayerHeal: PlayerHealth component not found on Player.",
                other.gameObject
            );

            return;
        }


        // =====================================================
        // HEAL PLAYER
        // =====================================================

        // Restore the configured amount of health.
        //
        // PlayerHealth.RestoreHealth() already makes sure
        // health does not exceed the player's max health.
        playerHealth.RestoreHealth(healAmount);


        // =====================================================
        // PLAY HEALING SOUND
        // =====================================================

        // Play the healing sound if AudioManager exists.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("HealSFX");
        }
        else
        {
            Debug.LogWarning(
                "PlayerHeal: AudioManager.Instance is missing."
            );
        }


        // =====================================================
        // DEBUG
        // =====================================================

        Debug.Log(
            "Player healed by " +
            healAmount
        );


        // =====================================================
        // REMOVE PICKUP
        // =====================================================

        // Destroy the healing pickup after it has been collected.
        Destroy(gameObject);
    }
}