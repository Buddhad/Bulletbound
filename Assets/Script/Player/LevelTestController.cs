using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTestController : MonoBehaviour
{
    [Header("Test Level")]
    [Tooltip("Build Index of the level you want to test.")]
    [SerializeField] private int testLevel = 5;

    [Header("Testing")]
    [SerializeField] private bool enableTesting = true;


    // =========================================================
    // TEST LEVEL
    // =========================================================

    public void TestLevel()
    {
        if (!enableTesting)
        {
            Debug.LogWarning(
                "LevelTestController: Testing is disabled."
            );

            return;
        }

        Time.timeScale = 1f;

        Debug.Log(
            "TEST: Loading Level-" + testLevel
        );

        SceneManager.LoadScene(testLevel);
    }


    // =========================================================
    // TEST LEVEL 1
    // =========================================================

    public void TestLevel1()
    {
        LoadLevel(1);
    }


    // =========================================================
    // TEST LEVEL 2
    // =========================================================

    public void TestLevel2()
    {
        LoadLevel(2);
    }


    // =========================================================
    // TEST LEVEL 3
    // =========================================================

    public void TestLevel3()
    {
        LoadLevel(3);
    }


    // =========================================================
    // TEST LEVEL 4
    // =========================================================

    public void TestLevel4()
    {
        LoadLevel(4);
    }


    // =========================================================
    // TEST LEVEL 5
    // =========================================================

    public void TestLevel5()
    {
        LoadLevel(5);
    }


    // =========================================================
    // LOAD LEVEL
    // =========================================================

    private void LoadLevel(int buildIndex)
    {
        if (!enableTesting)
        {
            Debug.LogWarning(
                "LevelTestController: Testing is disabled."
            );

            return;
        }

        if (buildIndex < 1 ||
            buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError(
                "LevelTestController: Invalid Build Index: " +
                buildIndex
            );

            return;
        }

        Time.timeScale = 1f;

        Debug.Log(
            "========================================"
        );

        Debug.Log(
            "TESTING LEVEL: " + buildIndex
        );

        Debug.Log(
            "========================================"
        );

        SceneManager.LoadScene(buildIndex);
    }
}