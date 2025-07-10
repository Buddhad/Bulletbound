using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthbar;
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
            AudioManager.Instance.PlaySFX("Player_Die");
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
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(_damage);
        }
    }
*/
    public void TakeDamage(float damage)
    {
        PlayerAbilityManager abilities = GetComponent<PlayerAbilityManager>();
        if (abilities != null && abilities.IsShieldActive())
        {
            Debug.Log("💥 Hit Blocked by Shield!");
            return; // ignore damage
        }

        AudioManager.Instance.PlaySFX("Player_Damage");
        health -= damage;
    }
}
