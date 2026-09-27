using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int lastScore = -1;

    private void Start()
    {
        UpdateScoreUI();
    }

    private void Update()
    {
        if (ScoreManager.CurrentScore != lastScore)
        {
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null)
        {
            Debug.LogWarning("ScoreDisplay: Score Text is not assigned.", this);
            return;
        }

        lastScore = ScoreManager.CurrentScore;
        scoreText.text = "Score: " + lastScore;
    }
}