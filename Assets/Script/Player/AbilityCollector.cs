using UnityEngine;

public class AbilityCollector : MonoBehaviour
{
    // =========================================================
    // COIN COLLECTION SETTINGS
    // =========================================================

    [Header("Coin Collection")]

    // Distance around the Player where coins can be collected.
    [SerializeField] private float collectionRadius = 1f;


    // =========================================================
    // PLAYER REFERENCES
    // =========================================================

    // Reference to the PlayerAbilityManager.
    private PlayerAbilityManager abilityManager;


    // =========================================================
    // INITIALIZE
    // =========================================================

    private void Awake()
    {
        // Get PlayerAbilityManager from the same GameObject.
        abilityManager = GetComponent<PlayerAbilityManager>();


        // If it is not on this GameObject,
        // check the parent GameObject.
        if (abilityManager == null)
        {
            abilityManager =
                GetComponentInParent<PlayerAbilityManager>();
        }


        // Warn if PlayerAbilityManager cannot be found.
        if (abilityManager == null)
        {
            Debug.LogWarning(
                "AbilityCollector: PlayerAbilityManager not found.",
                this
            );
        }
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // Continuously check for nearby coins.
        CheckForCoins();
    }


    // =========================================================
    // CHECK FOR COINS
    // =========================================================

    private void CheckForCoins()
    {
        // Search for all colliders around the Player.
        //
        // No LayerMask is used here.
        // Therefore, the collector does not depend on
        // the Ability layer being assigned correctly.
        Collider2D[] nearbyObjects =
            Physics2D.OverlapCircleAll(
                transform.position,
                collectionRadius
            );


        // Check every detected collider.
        foreach (Collider2D nearbyObject in nearbyObjects)
        {
            // Only collect the normal physical coin.
            //
            // The Ability_Double_Coins object is ignored
            // because it has a different tag.
            if (!nearbyObject.CompareTag("Ability_Coin"))
                continue;


            // Collect the coin.
            CollectCoin(nearbyObject.gameObject);
        }
    }


    // =========================================================
    // COLLECT COIN
    // =========================================================

    private void CollectCoin(GameObject coin)
    {
        // -----------------------------------------------------
        // CHECK COIN MANAGER
        // -----------------------------------------------------

        if (CoinManager.Instance == null)
        {
            Debug.LogError(
                "AbilityCollector: CoinManager.Instance is missing!"
            );

            return;
        }


        // -----------------------------------------------------
        // DETERMINE COIN VALUE
        // -----------------------------------------------------

        // Normally, one physical coin gives +1.
        int coinValue = 1;


        // -----------------------------------------------------
        // CHECK DOUBLE COINS
        // -----------------------------------------------------

        if (abilityManager != null &&
            abilityManager.IsDoubleCoinActive())
        {
            // Double Coins does NOT create another physical coin.
            //
            // The same one physical coin is worth +2.
            coinValue = 2;

            Debug.Log(
                "Double Coins ACTIVE! Ability_Coin = +2"
            );
        }
        else
        {
            Debug.Log(
                "Ability_Coin collected = +1"
            );
        }


        // -----------------------------------------------------
        // PLAY COIN COLLECTION SFX
        // -----------------------------------------------------

        if (AudioManager.Instance != null)
        {
            // The Sound name in AudioManager must be:
            // "CoinSFX"
            AudioManager.Instance.PlaySFX("CoinSFX");
        }
        else
        {
            Debug.LogWarning(
                "AbilityCollector: AudioManager.Instance is missing."
            );
        }


        // -----------------------------------------------------
        // ADD COINS
        // -----------------------------------------------------

        CoinManager.Instance.AddCoins(coinValue);


        Debug.Log(
            "Coins added: +" +
            coinValue
        );


        // -----------------------------------------------------
        // DESTROY PHYSICAL COIN
        // -----------------------------------------------------

        // Remove the collected physical coin.
        Destroy(coin);
    }


    // =========================================================
    // DEBUG VIEW
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        // Draw the collection radius in the Scene view.
        Gizmos.DrawWireSphere(
            transform.position,
            collectionRadius
        );
    }
}