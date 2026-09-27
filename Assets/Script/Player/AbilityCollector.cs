using UnityEngine;

public class AbilityCollector : MonoBehaviour
{
    private PlayerAbilityManager abilityManager;

    private void Awake()
    {
        abilityManager = GetComponent<PlayerAbilityManager>();

        if (abilityManager == null)
        {
            Debug.LogError(
                "AbilityCollector: PlayerAbilityManager is missing from the Player.",
                this
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (abilityManager == null)
            return;

        switch (other.tag)
        {
            case "Ability_Speed":
                abilityManager.ActivateSpeedBoost(5f);
                break;

            case "Ability_Jump":
                abilityManager.ActivateJumpBoost(5f);
                break;

            case "Ability_FireRate":
                abilityManager.ActivateFireRateBoost(0.1f, 5f);
                break;

            case "Ability_Shield":
                abilityManager.ActivateShield();
                break;

            case "Ability_Health":
                abilityManager.ActivateHealthBoost();
                break;

            case "Ability_DoubleCoins":
                abilityManager.ActivateDoubleCoins(5f);
                break;

            default:
                return;
        }

        Destroy(other.gameObject);
    }
}