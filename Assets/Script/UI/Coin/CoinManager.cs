using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // =========================================================
    // SINGLETON
    // =========================================================

    // Global reference to the CoinManager.
    public static CoinManager Instance;


    // =========================================================
    // COINS
    // =========================================================

    [Header("Coins")]

    // Current number of coins owned by the Player.
    [SerializeField] private int coins = 0;


    // =========================================================
    // UI
    // =========================================================

    [Header("UI")]

    // Text used to display the current coin count.
    [SerializeField] private TextMeshProUGUI coinText;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // Prevent duplicate CoinManager objects.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Register this CoinManager as the active instance.
        Instance = this;
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Update the coin UI when the scene starts.
        UpdateCoinUI();
    }


    // =========================================================
    // ADD COINS
    // =========================================================

    public void AddCoins(int amount)
    {
        // Ignore zero or negative values.
        if (amount <= 0)
            return;


        // Add coins to the current total.
        coins += amount;


        // Debug information.
        Debug.Log(
            "Coins collected: +" +
            amount
        );

        Debug.Log(
            "Total Coins: " +
            coins
        );


        // Update the UI.
        UpdateCoinUI();
    }


    // =========================================================
    // SPEND COINS
    // =========================================================

    public bool SpendCoins(int amount)
    {
        // Invalid amount.
        if (amount <= 0)
        {
            Debug.LogWarning(
                "CoinManager: Cannot spend zero or negative coins."
            );

            return false;
        }


        // Check whether the Player has enough coins.
        if (coins < amount)
        {
            Debug.Log(
                "Not enough coins!"
            );

            return false;
        }


        // Remove the purchased amount.
        coins -= amount;


        // Debug information.
        Debug.Log(
            "Coins spent: " +
            amount
        );

        Debug.Log(
            "Remaining Coins: " +
            coins
        );


        // Update the UI.
        UpdateCoinUI();


        // Purchase was successful.
        return true;
    }


    // =========================================================
    // CHECK COINS
    // =========================================================

    public bool HasEnoughCoins(int amount)
    {
        // Invalid amounts cannot be purchased.
        if (amount <= 0)
            return false;


        return coins >= amount;
    }


    // =========================================================
    // GET COINS
    // =========================================================

    public int GetCoins()
    {
        return coins;
    }


    // =========================================================
    // UPDATE UI
    // =========================================================

    private void UpdateCoinUI()
    {
        // Make sure the UI reference exists.
        if (coinText == null)
        {
            Debug.LogWarning(
                "CoinManager: Coin Text is not assigned!",
                this
            );

            return;
        }


        // Display the current coin amount.
        coinText.text =
            coins.ToString();


        // Debug information.
        Debug.Log(
            "Coin UI updated: " +
            coinText.text
        );
    }
}