using UnityEngine;

public class DoubleCoinPickup : MonoBehaviour
{
    public float duration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAbilityManager ability = other.GetComponent<PlayerAbilityManager>();
            if (ability != null)
            {
                //ScoreManager.AddScore(10);
                ability.ActivateDoubleCoins(duration);
                
            }

            Destroy(gameObject);
        }
    }
}