using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    // =========================================================
    // UI REFERENCES
    // =========================================================

    [Header("Score UI")]

    // Displays the score achieved before the player died.
    [SerializeField] private TextMeshProUGUI currentScoreText;

    // Displays the highest score saved on this device.
    [SerializeField] private TextMeshProUGUI highScoreText;


    // =========================================================
    // UNITY EVENT
    // =========================================================

    private void OnEnable()
    {
        // Make sure the game is running normally.
        // This is important if the Game Over panel appears
        // after the game was paused.
        Time.timeScale = 1f;

        // Update the score and high score.
        UpdateGameOverUI();
    }


    // =========================================================
    // UPDATE GAME OVER UI
    // =========================================================

    private void UpdateGameOverUI()
    {
        // Get the current score from ScoreManager.
        int currentScore =
            ScoreManager.CurrentScore;


        // Get the saved high score.
        //
        // If there is no saved high score yet,
        // the default value will be 0.
        int highScore =
            PlayerPrefs.GetInt(
                "HighScore",
                0
            );


        // =====================================================
        // CHECK FOR NEW HIGH SCORE
        // =====================================================

        // If the current score is greater than
        // the saved high score, save it as the new high score.
        if (currentScore > highScore)
        {
            highScore = currentScore;

            // Save the new high score.
            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );

            // Make sure PlayerPrefs is written to disk.
            PlayerPrefs.Save();

            Debug.Log(
                "New High Score: " +
                highScore
            );
        }


        // =====================================================
        // CURRENT SCORE TEXT
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
                "GameOverUI: Current Score Text is not assigned.",
                this
            );
        }


        // =====================================================
        // HIGH SCORE TEXT
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
                "GameOverUI: High Score Text is not assigned.",
                this
            );
        }
    }


    // =========================================================
    // RESTART BUTTON
    // =========================================================

    public void Restart()
    {
        Debug.Log(
            "GameOverUI: Restart button clicked."
        );


        // -----------------------------------------------------
        // RESTORE GAME TIME
        // -----------------------------------------------------

        // Make sure the game is not paused.
        Time.timeScale = 1f;


        // -----------------------------------------------------
        // CLOSE GAME OVER PANEL
        // -----------------------------------------------------

        // Because this script is attached to the GameOver panel,
        // "gameObject" refers to the GameOver panel itself.
        //
        // This immediately hides the panel before the scene
        // reloads.
        gameObject.SetActive(false);


        // -----------------------------------------------------
        // GET CURRENT SCENE
        // -----------------------------------------------------

        // Get the Build Index of the level currently being played.
        int currentScene =
            SceneManager.GetActiveScene().buildIndex;


        Debug.Log(
            "Restarting Level: " +
            currentScene
        );


        // =====================================================
        // RESET PERSISTENT PLAYER
        // =====================================================

        // Your Player persists between levels using
        // PersistentPlayer.
        //
        // Make sure the Player is active and usable again
        // before the level is reloaded.
        if (PersistentPlayer.Instance != null)
        {
            GameObject player =
                PersistentPlayer.Instance.gameObject;


            // -------------------------------------------------
            // ENABLE PLAYER
            // -------------------------------------------------

            // Make sure the Player GameObject is active.
            player.SetActive(true);


            // -------------------------------------------------
            // PLAYER MOVEMENT
            // -------------------------------------------------

            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                // Enable the movement component.
                movement.enabled = true;

                // Allow movement.
                movement.SetMovementEnabled(true);

                // Put the player into the normal idle state.
                movement.ForceIdle();
            }


            // -------------------------------------------------
            // PLAYER SHOOTER
            // -------------------------------------------------

            PlayerShooter shooter =
                player.GetComponent<PlayerShooter>();

            if (shooter != null)
            {
                // Enable shooting again.
                shooter.enabled = true;

                // Make sure the shooter is not stuck in
                // the reloading state.
                shooter.isReloading = false;
            }


            // -------------------------------------------------
            // PLAYER SPRITES
            // -------------------------------------------------

            // Get all SpriteRenderers on the Player and
            // its children.
            SpriteRenderer[] renderers =
                player.GetComponentsInChildren<SpriteRenderer>(true);


            // Make every Player sprite visible again.
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning(
                "GameOverUI: PersistentPlayer.Instance was not found."
            );
        }


        // =====================================================
        // RELOAD CURRENT LEVEL
        // =====================================================

        // Reload the same level.
        //
        // The PersistentPlayer will remain alive because
        // it uses DontDestroyOnLoad.
        //
        // PersistentPlayer will then find the PlayerSpawnPoint
        // in the newly loaded level.
        SceneManager.LoadScene(currentScene);
    }


    // =========================================================
    // MAIN MENU BUTTON
    // =========================================================

    public void MainMenu()
    {
        Debug.Log(
            "GameOverUI: Main Menu button clicked."
        );


        // -----------------------------------------------------
        // RESTORE GAME TIME
        // -----------------------------------------------------

        // Make sure the game is running normally.
        Time.timeScale = 1f;


        // -----------------------------------------------------
        // CLOSE GAME OVER PANEL
        // -----------------------------------------------------

        // Hide the Game Over panel before changing scenes.
        gameObject.SetActive(false);


        // -----------------------------------------------------
        // ENABLE PERSISTENT PLAYER
        // -----------------------------------------------------

        // Make sure the persistent Player is active.
        if (PersistentPlayer.Instance != null)
        {
            PersistentPlayer.Instance.gameObject.SetActive(true);
        }


        // -----------------------------------------------------
        // LOAD MAIN MENU
        // -----------------------------------------------------

        SceneManager.LoadScene("MainMenu");
    }
}