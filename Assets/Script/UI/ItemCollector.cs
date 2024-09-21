using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int _coins;
    private bool doublePickup;
    public TextMeshProUGUI CoinsText;

    private void Start() {
        doublePickup=false;
    }

    private void Update() {
        CoinsText.text="Coins: "+_coins;
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
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Double")){
            doublePickup=true;
            other.gameObject.SetActive(false);
            
        }
        if(other.gameObject.CompareTag("Coin")&&!doublePickup){
            _coins+=1;
            other.gameObject.SetActive(false);
            //CoinsText.text="Coins: "+_coins;
            //CoinCollectSound.Play();
            //AudioManager.Instance.PlaySFX("Coin");
        }
        if(other.gameObject.CompareTag("Coin")&&doublePickup){
            _coins+=2;
            other.gameObject.SetActive(false);
        }
    }



}
