using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int CurrentScore { get; private set; } = 0;

    // =========================================================
    // ADD SCORE
    // =========================================================

    public static void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        float multiplier = 1f;

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            PlayerAbilityManager abilityManager =
                player.GetComponent<PlayerAbilityManager>();

            if (abilityManager != null)
            {
                multiplier = abilityManager.GetScoreMultiplier();
            }
        }

        int finalScore = Mathf.RoundToInt(amount * multiplier);

        CurrentScore += finalScore;

        Debug.Log(
            "Score +" + finalScore +
            " | Total Score: " + CurrentScore +
            " | Multiplier: " + multiplier
        );
    }

    // =========================================================
    // RESET SCORE
    // =========================================================

    public static void ResetScore()
    {
        CurrentScore = 0;

        Debug.Log("Score reset.");
    }
}