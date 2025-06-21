using UnityEngine;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        int currentScore = ScoreManager.CurrentScore;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            highScore = currentScore;
        }

        currentScoreText.text = "Score: " + currentScore;
        highScoreText.text = "High Score: " + highScore;
    }
}