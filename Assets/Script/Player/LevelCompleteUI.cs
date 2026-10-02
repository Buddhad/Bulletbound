using UnityEngine;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    // =========================================================
    // UI REFERENCES
    // =========================================================

    [Header("UI")]

    // Displays the score achieved in the current level.
    [SerializeField] private TextMeshProUGUI currentScoreText;

    // Displays the highest score saved on this device.
    [SerializeField] private TextMeshProUGUI highScoreText;


    // =========================================================
    // UNITY EVENT
    // =========================================================

    private void OnEnable()
    {
        // Update the UI whenever the Level Complete panel
        // becomes active.
        UpdateLevelCompleteUI();
    }


    // =========================================================
    // UPDATE LEVEL COMPLETE UI
    // =========================================================

    private void UpdateLevelCompleteUI()
    {
        // Get the score from ScoreManager.
        int currentScore =
            ScoreManager.CurrentScore;


        // Get the previously saved high score.
        //
        // If no high score exists yet, use 0.
        int highScore =
            PlayerPrefs.GetInt(
                "HighScore",
                0
            );


        // =====================================================
        // CHECK FOR NEW HIGH SCORE
        // =====================================================

        if (currentScore > highScore)
        {
            // The current score becomes the new high score.
            highScore = currentScore;


            // Save the new high score.
            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );


            // Make sure the value is written to disk.
            PlayerPrefs.Save();


            Debug.Log(
                "New High Score: " +
                highScore
            );
        }


        // =====================================================
        // CURRENT SCORE UI
        // =====================================================

        if (currentScoreText != null)
        {
            currentScoreText.text =
                "Score: " +
                currentScore;
        }
        else
        {
            Debug.LogWarning(
                "LevelCompleteUI: " +
                "Current Score Text is not assigned.",
                this
            );
        }


        // =====================================================
        // HIGH SCORE UI
        // =====================================================

        if (highScoreText != null)
        {
            highScoreText.text =
                "High Score: " +
                highScore;
        }
        else
        {
            Debug.LogWarning(
                "LevelCompleteUI: " +
                "High Score Text is not assigned.",
                this
            );
        }
    }
}