using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveNextLvl : MonoBehaviour
{
    public int nextSceneLoad;
    void Start()
    {
        nextSceneLoad= SceneManager.GetActiveScene().buildIndex+1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player")
        {
            //Last Level of the game
            if(SceneManager.GetActiveScene().buildIndex==11)
            {
                SceneManager.LoadScene("GameComplete");
            }
            else
            {
            //move to next level
            AudioManager.Instance.PlaySFX("Win");
            SceneManager.LoadScene(nextSceneLoad);
            //setting int for index
            if(nextSceneLoad>PlayerPrefs.GetInt("levelAt")){
                PlayerPrefs.SetInt("levelAt",nextSceneLoad);
                }
            }
        }
    }

}
