using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    // =========================================================
    // UI
    // =========================================================

    [Header("UI")]

    // Text component used to display the current score.
    [SerializeField] private TextMeshProUGUI scoreText;


    // =========================================================
    // INTERNAL DATA
    // =========================================================

    // Stores the last score displayed on screen.
    //
    // -1 ensures the UI updates the first time.
    private int lastScore = -1;


    // =========================================================
    // ENABLE
    // =========================================================

    private void OnEnable()
    {
        // Update the score immediately when this object
        // becomes enabled.
        UpdateScoreUI();
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // Only update the UI when the score has actually changed.
        //
        // This avoids repeatedly assigning the same text every
        // frame.
        if (ScoreManager.CurrentScore != lastScore)
        {
            UpdateScoreUI();
        }
    }


    // =========================================================
    // UPDATE SCORE UI
    // =========================================================

    private void UpdateScoreUI()
    {
        // Make sure the TextMeshPro component is assigned.
        if (scoreText == null)
        {
            Debug.LogWarning(
                "ScoreDisplay: Score Text is not assigned.",
                this
            );

            return;
        }


        // Get the latest score.
        lastScore =
            ScoreManager.CurrentScore;


        // Display the score.
        scoreText.text =
            "Score: " +
            lastScore;
    }
}