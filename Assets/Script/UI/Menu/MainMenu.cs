using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Application.Quit();
    }
    public void NoExit()
    {
        quitDialog.SetActive(false);
    }
    public void MainMenuSection(){
        SceneManager.LoadScene("MainMenu");
    }


}
