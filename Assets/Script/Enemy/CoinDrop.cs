using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    // =========================================================
    // COIN SETTINGS
    // =========================================================

    [Header("Coin Settings")]

    // Coin prefab that will be spawned when the enemy dies.
    //
    // Assign your Coin prefab here in the Inspector.
    [SerializeField] private GameObject coinPrefab;


    // Layer used to identify the ground.
    //
    // IMPORTANT:
    // Set this to the "Ground" layer in the Inspector.
    [SerializeField] private LayerMask groundLayer;


    // Maximum distance the raycast will search for the ground.
    //
    // A large value such as 50 is useful because enemies
    // can potentially be positioned at different heights.
    [SerializeField] private float raycastDistance = 50f;


    // Small vertical offset used to place the coin slightly
    // above the ground.
    //
    // This prevents the coin from being partially buried
    // inside the ground collider.
    [SerializeField] private float groundOffset = 0.2f;


    // =========================================================
    // INTERNAL STATE
    // =========================================================

    // Prevents the same enemy from spawning multiple coins.
    //
    // This is especially useful if DropCoin() is accidentally
    // called more than once before the enemy is destroyed.
    private bool hasDropped = false;


    // =========================================================
    // DROP COIN
    // =========================================================

    public void DropCoin()
    {
        // -----------------------------------------------------
        // PREVENT MULTIPLE DROPS
        // -----------------------------------------------------

        // If a coin has already been dropped by this enemy,
        // don't create another one.
        if (hasDropped)
            return;


        // Mark the coin as dropped immediately.
        hasDropped = true;


        // -----------------------------------------------------
        // CHECK COIN PREFAB
        // -----------------------------------------------------

        // Make sure a Coin prefab has been assigned.
        if (coinPrefab == null)
        {
            Debug.LogWarning(
                "CoinDrop: Coin Prefab is not assigned.",
                this
            );

            return;
        }


        // =====================================================
        // RAYCAST SETUP
        // =====================================================

        // Start the raycast slightly above the enemy.
        //
        // This helps prevent the raycast from starting inside
        // the enemy's own collider.
        Vector2 origin =
            (Vector2)transform.position +
            Vector2.up;


        // Cast a ray straight downward to find the ground.
        //
        // The ray only detects objects on the Ground layer.
        RaycastHit2D hit =
            Physics2D.Raycast(
                origin,
                Vector2.down,
                raycastDistance,
                groundLayer
            );


        // =====================================================
        // GROUND FOUND
        // =====================================================

        if (hit.collider != null)
        {
            // The raycast successfully found the ground.
            //
            // Place the coin at the hit point and move it
            // slightly upward so it sits visibly above
            // the ground.
            Vector2 coinPosition =
                hit.point +
                Vector2.up * groundOffset;


            // Create the coin at the calculated position.
            Instantiate(
                coinPrefab,
                coinPosition,
                Quaternion.identity
            );


            Debug.Log(
                "Coin dropped on ground at: " +
                coinPosition
            );
        }


        // =====================================================
        // GROUND NOT FOUND
        // =====================================================

        else
        {
            // No Ground-layer object was detected below
            // the enemy.
            //
            // This can happen if:
            // - Ground Layer is not assigned correctly.
            // - The enemy is outside the ground.
            // - The raycast is too short.
            //
            // As a fallback, spawn the coin at the enemy's
            // current position instead of losing the coin.
            Debug.LogWarning(
                "CoinDrop: No ground detected below enemy. " +
                "Dropping coin at enemy position."
            );


            // Spawn the coin where the enemy currently is.
            Instantiate(
                coinPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}