using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    // =========================================================
    // UI REFERENCES
    // =========================================================

    [Header("UI")]

    [Tooltip("Quit confirmation dialog.")]
    [SerializeField] private GameObject quitDialog;

    [Tooltip("Audio settings panel.")]
    [SerializeField] private GameObject audioPanel;

    [Tooltip("How To Play panel.")]
    [SerializeField] private GameObject howToPlayUI;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Make sure panels are closed when Main Menu starts.

        if (quitDialog != null)
        {
            quitDialog.SetActive(false);
        }

        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }

        if (howToPlayUI != null)
        {
            howToPlayUI.SetActive(false);
        }
    }


    // =========================================================
    // BUTTON CLICK SOUND
    // =========================================================

    private void PlayButtonClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonClick");
        }
        else
        {
            Debug.LogWarning(
                "MainMenu: AudioManager.Instance is missing."
            );
        }
    }


    // =========================================================
    // PLAY GAME
    // =========================================================

    public void PlayGame()
    {
        // Play click sound.
        PlayButtonClick();

        // Load Level-1.
        SceneManager.LoadScene("Level-1");
    }


    // =========================================================
    // QUIT
    // =========================================================

    public void QuitGame()
    {
        // Play click sound.
        PlayButtonClick();

        // Open quit confirmation dialog.
        if (quitDialog != null)
        {
            quitDialog.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "MainMenu: Quit Dialog is not assigned."
            );
        }
    }


    // =========================================================
    // LEVEL SELECTION
    // =========================================================

    public void LevelScene()
    {
        // Play click sound.
        PlayButtonClick();

        // Load Level Selection scene.
        SceneManager.LoadScene("LevelSelection");
    }


    // =========================================================
    // OPEN AUDIO PANEL
    // =========================================================

    public void Sound()
    {
        // Play click sound.
        PlayButtonClick();

        // Open Audio panel.
        if (audioPanel != null)
        {
            audioPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "MainMenu: Audio Panel is not assigned."
            );
        }

        Debug.Log("Audio panel opened.");
    }


    // =========================================================
    // CLOSE AUDIO PANEL
    // =========================================================

    public void CloseAudio()
    {
        // Play click sound.
        PlayButtonClick();

        // Close Audio panel.
        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }

        Debug.Log("Audio panel closed.");
    }


    // =========================================================
    // OPEN HOW TO PLAY
    // =========================================================

    public void HowToPlay()
    {
        // Play click sound.
        PlayButtonClick();

        // Open How To Play panel.
        if (howToPlayUI != null)
        {
            howToPlayUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "MainMenu: How To Play UI is not assigned."
            );
        }

        Debug.Log("How To Play panel opened.");
    }


    // =========================================================
    // CLOSE HOW TO PLAY / GOT IT
    // =========================================================

    public void GotIt()
    {
        // Play click sound.
        PlayButtonClick();

        // Close How To Play panel.
        if (howToPlayUI != null)
        {
            howToPlayUI.SetActive(false);
        }

        Debug.Log("How To Play panel closed.");
    }


    // =========================================================
    // MUSIC BUTTON
    // =========================================================

    public void Music()
    {
        // Play click sound.
        PlayButtonClick();

        Debug.Log("Music Check!!");
    }


    // =========================================================
    // YES EXIT
    // =========================================================

    public void YesExit()
    {
        // Play click sound.
        PlayButtonClick();

#if UNITY_EDITOR

        // Stop playing in Unity Editor.
        EditorApplication.isPlaying = false;

#elif UNITY_WEBGL

        // WebGL cannot quit normally.
        Debug.Log("Quit requested in WebGL build.");

        Application.OpenURL(
            "https://buddhadebchhetri.itch.io/bulletbound01"
        );

#else

        // Quit standalone application.
        Application.Quit();

#endif
    }


    // =========================================================
    // NO EXIT
    // =========================================================

    public void NoExit()
    {
        // Play click sound.
        PlayButtonClick();

        // Close quit dialog.
        if (quitDialog != null)
        {
            quitDialog.SetActive(false);
        }
    }


    // =========================================================
    // MAIN MENU
    // =========================================================

    public void MainMenuSection()
    {
        // Play click sound.
        PlayButtonClick();

        // Make sure time is running.
        Time.timeScale = 1f;

        // Load Main Menu.
        SceneManager.LoadScene("MainMenu");
    }


    // =========================================================
    // CLOSE ALL PANELS
    // =========================================================

    public void CloseAllPanels()
    {
        // Play click sound.
        PlayButtonClick();

        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }

        if (howToPlayUI != null)
        {
            howToPlayUI.SetActive(false);
        }

        if (quitDialog != null)
        {
            quitDialog.SetActive(false);
        }
    }
}