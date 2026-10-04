using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    // =========================================================
    // SINGLETON
    // =========================================================

    public static CoinManager Instance;


    // =========================================================
    // COINS
    // =========================================================

    [Header("Coins")]

    // Current number of coins.
    // This persists between levels.
    [SerializeField] private int coins = 0;


    // =========================================================
    // UI
    // =========================================================

    [Header("UI")]

    // This is the actual TextMeshPro object.
    // It belongs to the current level's Canvas.
    private TextMeshProUGUI coinText;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // Prevent duplicate CoinManagers.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep CoinManager alive between levels.
        DontDestroyOnLoad(gameObject);
    }


    // =========================================================
    // ENABLE
    // =========================================================

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // =========================================================
    // DISABLE
    // =========================================================

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        FindCoinUI();

        UpdateCoinUI();
    }


    // =========================================================
    // SCENE LOADED
    // =========================================================

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        Debug.Log(
            "CoinManager: Scene loaded: " +
            scene.name
        );

        // Find the new level's Coin Text.
        FindCoinUI();

        // Display the persistent coin amount.
        UpdateCoinUI();
    }


    // =========================================================
    // FIND COIN UI
    // =========================================================

    private void FindCoinUI()
    {
        // Clear old reference.
        coinText = null;

        // Find every TextMeshProUGUI in loaded scenes.
        TextMeshProUGUI[] texts =
            Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (TextMeshProUGUI text in texts)
        {
            if (text == null)
                continue;

            GameObject obj = text.gameObject;

            // Ignore objects that are not part of a scene.
            if (!obj.scene.IsValid())
                continue;

            // Ignore unloaded scenes.
            if (!obj.scene.isLoaded)
                continue;

            // -------------------------------------------------
            // IMPORTANT
            // -------------------------------------------------
            //
            // Your hierarchy is:
            //
            // Coin Score
            // ├── Icon Image
            // └── Coin Text
            //
            // Therefore we look for "Coin Text".
            // -------------------------------------------------

            if (obj.name == "Coin Text")
            {
                coinText = text;

                Debug.Log(
                    "CoinManager: Coin Text found in scene: " +
                    obj.scene.name
                );

                break;
            }
        }

        if (coinText == null)
        {
            Debug.LogWarning(
                "CoinManager: Coin Text was not found in the current scene."
            );
        }
    }


    // =========================================================
    // ADD COINS
    // =========================================================

    public void AddCoins(int amount)
    {
        if (amount <= 0)
            return;

        coins += amount;

        Debug.Log(
            "Coins collected: +" +
            amount
        );

        Debug.Log(
            "Total Coins: " +
            coins
        );

        UpdateCoinUI();
    }


    // =========================================================
    // SPEND COINS
    // =========================================================

    public bool SpendCoins(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning(
                "CoinManager: Cannot spend zero or negative coins."
            );

            return false;
        }

        if (coins < amount)
        {
            Debug.Log(
                "Not enough coins!"
            );

            return false;
        }

        coins -= amount;

        Debug.Log(
            "Coins spent: " +
            amount
        );

        Debug.Log(
            "Remaining Coins: " +
            coins
        );

        UpdateCoinUI();

        return true;
    }


    // =========================================================
    // CHECK COINS
    // =========================================================

    public bool HasEnoughCoins(int amount)
    {
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
        if (coinText == null)
        {
            Debug.LogWarning(
                "CoinManager: Cannot update Coin Text because UI reference is missing."
            );

            return;
        }

        coinText.text = coins.ToString();

        Debug.Log(
            "Coin UI updated: " +
            coinText.text
        );
    }
}