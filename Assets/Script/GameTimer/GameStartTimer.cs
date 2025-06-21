using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class GameStartTimer : MonoBehaviour
{
    public float countdownTime = 15f;
    public GameObject howToPlayUI;
    public GameObject player;
    public TextMeshProUGUI timerText;
    public Button skipButton;

    public static bool GameStarted { get; set; } = false;
    public static event Action OnGameStarted;

    private void Start()
    {
        Debug.Log("GameStartTimer initialized");
        InitializeCountdown();
    }

    public void RestartTimer()
    {
        ScoreManager.ResetScore();
        StopAllCoroutines();
        GameStarted = false;
        InitializeCountdown();
    }

    void InitializeCountdown()
    {
        countdownTime = 15f;

        howToPlayUI.SetActive(true);
        timerText.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);

        if (player.TryGetComponent(out PlayerMovement move))
            move.enabled = false;
        if (player.TryGetComponent(out PlayerShooter shooter))
            shooter.enabled = false;

        DisableAllEnemies();

        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipIntro);

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0 && !GameStarted)
        {
            timerText.text = "Game starts in " + countdownTime.ToString("F0") + "s";
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        if (!GameStarted)
        {
            StartGame();
        }
    }

    void SkipIntro()
    {
        if (!GameStarted)
        {
            countdownTime = 0;
            StartGame();
        }
    }

    void StartGame()
    {
        GameStarted = true;

        howToPlayUI.SetActive(false);
        timerText.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        if (player.TryGetComponent(out PlayerMovement move))
            move.enabled = true;

        if (player.TryGetComponent(out PlayerShooter shooter))
            shooter.enabled = true;

        EnableAllEnemies();

        OnGameStarted?.Invoke(); // 🔔 Notify subscribers
    }

    void DisableAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.TryGetComponent(out EmemyMovement move))
                move.canMove = false;
        }
    }

    void EnableAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.TryGetComponent(out EmemyMovement move))
                move.canMove = true;
        }
    }

    private void OnEnable()
    {
        GameStarted = false;
    }
}