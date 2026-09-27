using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    [Header("Healing")]
    public float _heal = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth pHealth = other.GetComponent<PlayerHealth>();

        if (pHealth == null)
        {
            Debug.LogWarning(
                "PlayerHeal: PlayerHealth component not found on Player.",
                other.gameObject
            );

            return;
        }

        // Heal the player through PlayerHealth.
        pHealth.RestoreHealth(_heal);

        // Play sound safely.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("HealSFX");
        }

        Debug.Log("Player healed by " + _heal);

        // Remove pickup.
        Destroy(gameObject);
    }
}