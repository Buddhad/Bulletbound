using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletPlayerPref : MonoBehaviour
{
    private void Update() {
        // if(Input.GetKeyDown(KeyCode.L)){
            
        //     PlayerPrefs.DeleteAll();
        // }
    }

    public void ResetLevel(){
        Debug.Log("Delete level log!!");
        PlayerPrefs.DeleteAll();
    } 
}
