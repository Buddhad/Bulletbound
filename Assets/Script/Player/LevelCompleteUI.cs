using UnityEngine;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        UpdateLevelCompleteUI();
    }

    private void UpdateLevelCompleteUI()
    {
        int currentScore = ScoreManager.CurrentScore;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Check for new high score
        if (currentScore > highScore)
        {
            highScore = currentScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            Debug.Log("New High Score: " + highScore);
        }

        // Update current score
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + currentScore;
        }
        else
        {
            Debug.LogWarning(
                "LevelCompleteUI: Current Score Text is not assigned.",
                this
            );
        }

        // Update high score
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
        else
        {
            Debug.LogWarning(
                "LevelCompleteUI: High Score Text is not assigned.",
                this
            );
        }
    }
}