using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Powerups/SpeedPow")]
public class SpeedPow : Powerups
{
    public float amount;
    public override void Apply(GameObject target){
        target.GetComponent<PlayerMovement>().moveSpeed+= amount;
    }
    
}
