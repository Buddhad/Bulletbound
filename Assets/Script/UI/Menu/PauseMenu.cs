using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // =========================================================
    // UI
    // =========================================================

    [Header("Pause Menu")]

    // Main Pause Menu panel.
    [SerializeField] private GameObject pauseMenu;


    // =========================================================
    // STATE
    // =========================================================

    // Stores whether the game is currently paused.
    private bool isPaused = false;


    // =========================================================
    // UNITY UPDATE
    // =========================================================

    private void Update()
    {
        // Press Escape to pause/resume the game.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    // =========================================================
    // PAUSE
    // =========================================================

    public void Pause()
    {
        // Show Pause Menu.
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        // Stop gameplay.
        Time.timeScale = 0f;

        // Store pause state.
        isPaused = true;
    }


    // =========================================================
    // RESUME
    // =========================================================

    public void Resume()
    {
        // Hide Pause Menu.
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        // Resume gameplay.
        Time.timeScale = 1f;

        // Update pause state.
        isPaused = false;
    }


    // =========================================================
    // TOGGLE PAUSE
    // =========================================================

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }


    // =========================================================
    // HOME / MAIN MENU
    // =========================================================

    public void Home()
    {
        // Always restore normal time before changing scenes.
        Time.timeScale = 1f;

        // Reset pause state.
        isPaused = false;

        // Load Main Menu.
        SceneManager.LoadScene("MainMenu");
    }


    // =========================================================
    // RESTART
    // =========================================================

    public void Restart()
    {
        // Restore normal game speed.
        Time.timeScale = 1f;

        // Reset pause state.
        isPaused = false;

        // Reload the current Gameplay scene.
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


    // =========================================================
    // AUDIO MENU
    // =========================================================

    public void AudioMenu()
    {
        // If you later add a separate Audio Menu,
        // enable it here.
        //
        // Example:
        // audioMenu.SetActive(true);

        // Keep the game paused while the audio menu is open.
        Time.timeScale = 0f;
    }
}