using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Powerups/HealthPow")]
public class HealthPow : Powerups
{   
    public float amount;
    public override void Apply(GameObject target){
        target.GetComponent<PlayerHealth>().health+= amount;
    }
}
