using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem damageEffect;

private void Update() {
    StartEffect();
}
    void StartEffect(){
        damageEffect.Play();
    }
}
