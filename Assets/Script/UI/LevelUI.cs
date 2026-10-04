using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [Header("Level UI")]
    [SerializeField] private TMP_Text levelText;

    [Header("Level Complete UI")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private TMP_Text levelCompleteText;

    private void Start()
    {
        UpdateLevelUI();
    }

    private void UpdateLevelUI()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        // Main Menu = Build Index 0
        if (buildIndex == 0)
        {
            return;
        }

        // Level-1 = Build Index 1
        // Level-2 = Build Index 2
        // Level-3 = Build Index 3
        // Level-4 = Build Index 4
        // Level-5 = Build Index 5

        int levelNumber = buildIndex;

        if (levelText != null)
        {
            levelText.text = "Level " + levelNumber;
        }

        // Hide completion panel when the level starts.
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
    }

    public void ShowLevelComplete()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex == 0)
        {
            return;
        }

        int levelNumber = buildIndex;

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        if (levelCompleteText != null)
        {
            levelCompleteText.text =
                "Level " + levelNumber + " Complete!";
        }
    }
}