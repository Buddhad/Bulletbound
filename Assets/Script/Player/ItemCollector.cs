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
        if (other.CompareTag("Coin"))
        {
            _coins += 1;
            AudioManager.Instance.PlaySFX("CoinSFX");
            UpdateCoinUI();
            Destroy(other.gameObject);
            
        }
    }
}


    //[SerializeField] private AudioSource CoinCollectSound;
    /*
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.CompareTag("Coin")){
                Destroy(other.gameObject);
                _coins++;
                CoinsText.text="Coins: "+_coins;
                //CoinCollectSound.Play();
                //AudioManager.Instance.PlaySFX("Coin");
            }
        }
        */

    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Double"))
        {
            AudioManager.Instance.PlaySFX("PowerupB");
            doublePickup = true;
            other.gameObject.SetActive(false);

        }
        if (other.gameObject.CompareTag("Coin") && !doublePickup)
        {
            AudioManager.Instance.PlaySFX("CoinSFX");
            _coins += 1;
            other.gameObject.SetActive(false);

        }
        if (other.gameObject.CompareTag("Coin") && doublePickup)
        {
            AudioManager.Instance.PlaySFX("PowerupA");
            _coins += 2;
            other.gameObject.SetActive(false);
        }
    }
*/

