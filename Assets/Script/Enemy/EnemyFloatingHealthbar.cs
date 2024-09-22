using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFloatingHealthbar : MonoBehaviour
{
    [SerializeField]private Slider _slider;

    public void UpdateHealthBar(float currentValue,float maxValue) {
        _slider.value=currentValue/maxValue;
    }

}
