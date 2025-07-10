using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject quitDialog;
    public void PlayGame()
    {
        SceneManager.LoadScene("Level-1");
    }

    public void QuitGame()
    {
        quitDialog.SetActive(true);
    }

    public void LevelScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void Sound()
    {
        Debug.Log("Sound Check!!");
    }
    public void Music()
    {
        Debug.Log("Music Check!!");
    }

    public void YesExit()
    {
#if UNITY_EDITOR
        // Stop playing in the editor
        EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        // WebGL can't quit, but you can redirect or show a message
        Debug.Log("Quit requested in WebGL build.");
        // Optionally: Application.OpenURL("https://your-website.com");
        Application.OpenURL("https://buddhadebchhetri.itch.io/bulletbound01"); // Redirect to a URL or show a message
#else
        // Quit for standalone builds (Windows, Mac, etc.)
        Application.Quit();
#endif
    }
    public void NoExit()
    {
        quitDialog.SetActive(false);
    }
    public void MainMenuSection()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
