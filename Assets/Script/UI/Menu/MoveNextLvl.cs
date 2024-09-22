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
    void NextLevel(){
            //move to next level
            AudioManager.Instance.PlaySFX("Win");
            SceneManager.LoadScene(nextSceneLoad);
    }

}
