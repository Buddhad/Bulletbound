using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false;
    //[SerializeField]GameObject audioMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        SceneManager.sceneLoaded += ResetGameState;

    }
    public void Resume()
    {
        // Resume the game timer if it exists
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Restart()
    {
        // Restart the game timer if it exists
        GameStartTimer.GameStarted = false; // 🔁 Reset the static flag BEFORE scene reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void AudioMenu()
    {
        //audioMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }
    void ResetGameState(Scene scene, LoadSceneMode mode)
    {
        GameStartTimer.GameStarted = false;
        SceneManager.sceneLoaded -= ResetGameState;
    }

}
