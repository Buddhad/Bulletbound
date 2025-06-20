using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthbar;
    public float _damage = 20;
    [SerializeField] GameObject GameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        if (health <= 0)
        {
            //If health is zero then show menu and destory the player
            gameObject.SetActive(false);
            ShowGameOverScreen();
            AudioManager.Instance.PlaySFX("Hurt");
        }
    }
    void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);
        Invoke("GameOver", 1f);
        Time.timeScale = 0;
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerAbilityManager abilities = GetComponent<PlayerAbilityManager>();
            if (abilities != null && abilities.IsShieldActive())
            {
                Debug.Log("💥 Hit Blocked by Shield!");
                return; // ignore damage
            }

            AudioManager.Instance.PlaySFX("Die");
            health -= _damage;
        }
    }
}
