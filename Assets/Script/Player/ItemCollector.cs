using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int _coins;
    public TextMeshProUGUI CoinsText;

    private void Start()
    {
        UpdateCoinUI(); // show 0 at start
    }

    private void UpdateCoinUI()
    {
        CoinsText.text = "Score: " + _coins;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the tag "Coin"
        if (other.CompareTag("Coin"))
        {
            _coins += 1;
            AudioManager.Instance.PlaySFX("CoinSFX");
            UpdateCoinUI();
            Destroy(other.gameObject);
        }
    }
}


