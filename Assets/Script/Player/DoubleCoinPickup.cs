using UnityEngine;

public class DoubleCoinPickup : MonoBehaviour
{
    public float duration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerAbilityManager ability =
            other.GetComponent<PlayerAbilityManager>();

        if (ability != null)
        {
            ability.ActivateDoubleCoins(duration);

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning(
                "DoubleCoinPickup: PlayerAbilityManager " +
                "was not found on Player.",
                other.gameObject
            );
        }
    }
}