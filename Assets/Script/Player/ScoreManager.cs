using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // =========================================================
    // SCORE
    // =========================================================

    // Stores the current score for the level.
    //
    // Other scripts can read the score using:
    // ScoreManager.CurrentScore
    //
    // Other scripts cannot directly change it.
    public static int CurrentScore { get; private set; } = 0;


    // =========================================================
    // ADD SCORE
    // =========================================================

    public static void AddScore(int amount)
    {
        // Ignore zero or negative score values.
        if (amount <= 0)
            return;


        // Add the score directly.
        //
        // Double Coins does NOT affect score.
        // It only affects the number of coins dropped.
        CurrentScore += amount;


        // Debug information.
        Debug.Log(
            "Score +" +
            amount +
            " | Total Score: " +
            CurrentScore
        );
    }


    // =========================================================
    // RESET SCORE
    // =========================================================

    public static void ResetScore()
    {
        // Reset score when starting/restarting a level.
        CurrentScore = 0;


        Debug.Log(
            "Score reset."
        );
    }
}