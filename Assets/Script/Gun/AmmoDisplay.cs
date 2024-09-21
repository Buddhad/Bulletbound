using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    
    public int ammo;
    [SerializeField]
    private bool isFiring;
    public TextMeshProUGUI showAmmo;

    // Update is called once per frame
    void Update()
    {
        UIammo();
    }

    private void UIammo(){
        showAmmo.text=ammo.ToString("Bullet: "+ammo+"/7");
        if(Input.GetKeyDown(KeyCode.F) && !isFiring && ammo>0){
            isFiring=true;
            ammo--;
            isFiring=false;
        }
    }
}
