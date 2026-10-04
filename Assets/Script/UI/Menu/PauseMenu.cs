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

    private bool isPaused = false;


    // =========================================================
    // UPDATE
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
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        Time.timeScale = 0f;

        isPaused = true;
    }


    // =========================================================
    // RESUME
    // =========================================================

    public void Resume()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        Time.timeScale = 1f;

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
    // HOME
    // =========================================================

    public void Home()
    {
        Time.timeScale = 1f;

        isPaused = false;

        // Make sure the persistent Player is active
        // before going back to Main Menu.
        if (PersistentPlayer.Instance != null)
        {
            PersistentPlayer.Instance.gameObject.SetActive(true);
        }

        SceneManager.LoadScene("MainMenu");
    }


    // =========================================================
    // RESTART
    // =========================================================

    public void Restart()
    {
        // Always restore normal time.
        Time.timeScale = 1f;

        isPaused = false;

        // -----------------------------------------------------
        // REACTIVATE PERSISTENT PLAYER
        // -----------------------------------------------------

        if (PersistentPlayer.Instance != null)
        {
            GameObject player =
                PersistentPlayer.Instance.gameObject;

            // The Player may have been disabled by Game Over
            // or another gameplay system.
            player.SetActive(true);

            // Re-enable Player Movement.
            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.enabled = true;
                movement.SetMovementEnabled(true);
                movement.ForceIdle();
            }

            // Re-enable Player Shooter.
            PlayerShooter shooter =
                player.GetComponent<PlayerShooter>();

            if (shooter != null)
            {
                shooter.enabled = true;
                shooter.isReloading = false;
            }

            // Make sure Player graphics are visible.
            SpriteRenderer[] renderers =
                player.GetComponentsInChildren<SpriteRenderer>(
                    true
                );

            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = true;
            }

            Debug.Log(
                "PauseMenu: Persistent Player reactivated for restart."
            );
        }

        // -----------------------------------------------------
        // RELOAD CURRENT LEVEL
        // -----------------------------------------------------

        int currentScene =
            SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }


    // =========================================================
    // AUDIO MENU
    // =========================================================

    public void AudioMenu()
    {
        // Keep the game paused while the audio menu is open.
        Time.timeScale = 0f;
    }
}