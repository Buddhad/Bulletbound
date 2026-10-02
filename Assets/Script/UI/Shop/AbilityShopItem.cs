using UnityEngine;

public class AbilityShopItem : MonoBehaviour
{
    // =========================================================
    // SHOP ITEM TYPE
    // =========================================================

    public enum ShopItemType
    {
        SpeedBoost,
        FireRateBoost,
        Shield,
        HealthBoost,
        DoubleCoins,
        Bullets
    }


    // =========================================================
    // PURCHASE SETTINGS
    // =========================================================

    [Header("Purchase")]

    // Select what this button sells.
    [SerializeField] private ShopItemType itemType;

    // Coin cost of the selected item.
    [SerializeField] private int coinCost = 10;


    // =========================================================
    // ABILITY SETTINGS
    // =========================================================

    [Header("Ability Settings")]

    // Duration for temporary abilities.
    [SerializeField] private float duration = 5f;


    // =========================================================
    // BULLET SETTINGS
    // =========================================================

    [Header("Bullet Settings")]

    // Number of reserve bullets given when purchased.
    [SerializeField] private int bulletAmount = 30;


    // =========================================================
    // BUY ITEM
    // =========================================================

    public void BuyItem()
    {
        // -----------------------------------------------------
        // CHECK COIN MANAGER
        // -----------------------------------------------------

        if (CoinManager.Instance == null)
        {
            Debug.LogError("CoinManager not found.");
            return;
        }


        // -----------------------------------------------------
        // CHECK COINS
        // -----------------------------------------------------

        if (!CoinManager.Instance.HasEnoughCoins(coinCost))
        {
            Debug.Log(
                "Not enough coins to buy " +
                itemType +
                "."
            );

            return;
        }


        // -----------------------------------------------------
        // FIND PLAYER
        // -----------------------------------------------------

        GameObject player =
            GameObject.FindWithTag("Player");


        if (player == null)
        {
            Debug.LogError("Player not found.");
            return;
        }


        // =====================================================
        // BULLET PURCHASE
        // =====================================================

        if (itemType == ShopItemType.Bullets)
        {
            // Find PlayerShooter.
            PlayerShooter shooter =
                player.GetComponent<PlayerShooter>();


            if (shooter == null)
            {
                Debug.LogError(
                    "PlayerShooter not found on Player."
                );

                return;
            }


            // Spend coins.
            bool purchased =
                CoinManager.Instance.SpendCoins(coinCost);


            if (!purchased)
                return;


            // Add bullets to reserve ammunition.
            shooter.AddReserveAmmo(
                bulletAmount
            );


            Debug.Log(
                "Purchased " +
                bulletAmount +
                " bullets for " +
                coinCost +
                " coins."
            );


            return;
        }


        // =====================================================
        // ABILITY PURCHASE
        // =====================================================

        // Find PlayerAbilityManager.
        PlayerAbilityManager abilityManager =
            player.GetComponent<PlayerAbilityManager>();


        if (abilityManager == null)
        {
            Debug.LogError(
                "PlayerAbilityManager not found on Player."
            );

            return;
        }


        // -----------------------------------------------------
        // SPEND COINS
        // -----------------------------------------------------

        bool abilityPurchased =
            CoinManager.Instance.SpendCoins(
                coinCost
            );


        if (!abilityPurchased)
            return;


        // =====================================================
        // ACTIVATE ABILITY
        // =====================================================

        switch (itemType)
        {
            // -------------------------------------------------
            // SPEED BOOST
            // -------------------------------------------------

            case ShopItemType.SpeedBoost:

                abilityManager.ActivateSpeedBoost(
                    duration
                );

                Debug.Log(
                    "Purchased Speed Boost for " +
                    coinCost +
                    " coins."
                );

                break;


            // -------------------------------------------------
            // FIRE RATE BOOST
            // -------------------------------------------------

            case ShopItemType.FireRateBoost:

                abilityManager.ActivateFireRateBoost(
                    duration
                );

                Debug.Log(
                    "Purchased Fire Rate Boost for " +
                    coinCost +
                    " coins."
                );

                break;


            // -------------------------------------------------
            // SHIELD
            // -------------------------------------------------

            case ShopItemType.Shield:

                abilityManager.ActivateShield();

                Debug.Log(
                    "Purchased Shield for " +
                    coinCost +
                    " coins."
                );

                break;


            // -------------------------------------------------
            // HEALTH BOOST
            // -------------------------------------------------

            case ShopItemType.HealthBoost:

                abilityManager.ActivateHealthBoost();

                Debug.Log(
                    "Purchased Health Boost for " +
                    coinCost +
                    " coins."
                );

                break;


            // -------------------------------------------------
            // DOUBLE COINS
            // -------------------------------------------------

            case ShopItemType.DoubleCoins:

                abilityManager.ActivateDoubleCoins(
                    duration
                );

                Debug.Log(
                    "Purchased Double Coins for " +
                    coinCost +
                    " coins."
                );

                break;
        }
    }
}