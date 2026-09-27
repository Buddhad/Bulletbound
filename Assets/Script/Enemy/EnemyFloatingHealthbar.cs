using UnityEngine;
using UnityEngine.UI;

public class EnemyFloatingHealthbar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (_slider == null)
        {
            Debug.LogWarning(
                "EnemyFloatingHealthbar: Slider is not assigned.",
                this
            );

            return;
        }

        if (maxValue <= 0f)
        {
            _slider.value = 0f;
            return;
        }

        _slider.value = Mathf.Clamp01(currentValue / maxValue);
    }
}