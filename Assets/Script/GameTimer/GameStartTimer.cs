using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class GameStartTimer : MonoBehaviour
{
    [Header("Countdown Settings")]
    public float countdownTime = 15f;

    [Header("UI References")]
    public GameObject howToPlayUI;
    public GameObject player;
    public TextMeshProUGUI timerText;
    public Button skipButton;

    [Header("Debug Settings")]
    [Tooltip("Enable this when testing the Gameplay scene directly.")]
    public bool debugStartImmediately = false;

    // Keep the setter public because PauseMenu and other scripts
    // in your current project use GameStarted directly.
    public static bool GameStarted { get; set; } = false;

    public static event Action OnGameStarted;

    private void Awake()
    {
        // Reset the game state when this Gameplay scene is initialized.
        GameStarted = false;
    }

    private void Start()
    {
        Debug.Log("GameStartTimer initialized.");

        // Debug mode allows you to start the Gameplay scene directly
        // without going through the Main Menu or countdown.
        if (debugStartImmediately)
        {
            Debug.Log("DEBUG MODE: Starting game immediately.");

            StartGame();
            return;
        }

        InitializeCountdown();
    }

    // =========================================================
    // RESTART TIMER
    // =========================================================

    public void RestartTimer()
    {
        Debug.Log("Restarting game timer.");

        ScoreManager.ResetScore();

        StopAllCoroutines();

        GameStarted = false;

        InitializeCountdown();
    }

    // =========================================================
    // INITIALIZE COUNTDOWN
    // =========================================================

    private void InitializeCountdown()
    {
        countdownTime = 15f;

        // Show countdown UI
        if (howToPlayUI != null)
            howToPlayUI.SetActive(true);

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        if (skipButton != null)
            skipButton.gameObject.SetActive(true);

        // Disable player movement
        DisablePlayer();

        // Disable enemies
        DisableAllEnemies();

        // Setup Skip button
        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(SkipIntro);
        }

        StartCoroutine(StartCountdown());
    }

    // =========================================================
    // COUNTDOWN
    // =========================================================

    private IEnumerator StartCountdown()
    {
        while (countdownTime > 0f && !GameStarted)
        {
            if (timerText != null)
            {
                timerText.text =
                    "Game starts in " +
                    Mathf.CeilToInt(countdownTime) +
                    "s";
            }

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        if (!GameStarted)
        {
            StartGame();
        }
    }

    // =========================================================
    // SKIP INTRO
    // =========================================================

    private void SkipIntro()
    {
        if (GameStarted)
            return;

        Debug.Log("Countdown skipped.");

        StopAllCoroutines();

        StartGame();
    }

    // =========================================================
    // START GAME
    // =========================================================

    private void StartGame()
    {
        // Prevent StartGame() from being called twice.
        if (GameStarted)
            return;

        GameStarted = true;

        Debug.Log("GAME STARTED");

        // Hide countdown UI
        if (howToPlayUI != null)
            howToPlayUI.SetActive(false);

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        if (skipButton != null)
            skipButton.gameObject.SetActive(false);

        // Enable player
        EnablePlayer();

        // Enable enemies
        EnableAllEnemies();

        // Notify other systems
        OnGameStarted?.Invoke();
    }

    // =========================================================
    // PLAYER
    // =========================================================

    private void DisablePlayer()
    {
        if (player == null)
            return;

        if (player.TryGetComponent(out PlayerMovement move))
            move.enabled = false;

        if (player.TryGetComponent(out PlayerShooter shooter))
            shooter.enabled = false;
    }

    private void EnablePlayer()
    {
        if (player == null)
            return;

        if (player.TryGetComponent(out PlayerMovement move))
            move.enabled = true;

        if (player.TryGetComponent(out PlayerShooter shooter))
            shooter.enabled = true;
    }

    // =========================================================
    // ENEMIES
    // =========================================================

    private void DisableAllEnemies()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.TryGetComponent(out EmemyMovement move))
            {
                move.canMove = false;
            }
        }
    }

    private void EnableAllEnemies()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.TryGetComponent(out EmemyMovement move))
            {
                move.canMove = true;
            }
        }
    }
}