using UnityEngine;

public class AbilityCollector : MonoBehaviour
{
    private PlayerAbilityManager abilityManager;

    void Start()
    {
        abilityManager = GetComponent<PlayerAbilityManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Ability_Speed":
                abilityManager.ActivateSpeedBoost(5f);
                break;

            case "Ability_Jump":
                abilityManager.ActivateJumpBoost(5f);
                break;

            case "Ability_FireRate":
                abilityManager.ActivateFireRateBoost(0.1f, 5f); // Fire faster for 5 seconds
                break;

            case "Ability_Shield":
                abilityManager.ActivateShield();
                break;

            case "Ability_Health":
                abilityManager.ActivateHealthBoost();
                break;
            case "Ability_DoubleCoins":
                abilityManager.ActivateDoubleCoins(5f); // Double coins for 5 seconds
                break;
            
            default:
                return;
        }

        Destroy(other.gameObject);
    }
}
