using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScoreUI(); // Show 0 at start
    }

    private void Update()
    {
        UpdateScoreUI(); // Refresh score every frame
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + ScoreManager.CurrentScore;
    }
}
