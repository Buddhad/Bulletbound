using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 50f;
    [SerializeField] private float groundOffset = 0.2f;

    private bool hasDropped = false;

    public void DropCoin()
    {
        // Prevent multiple coin drops.
        if (hasDropped)
            return;

        hasDropped = true;

        // Check coin prefab.
        if (coinPrefab == null)
        {
            Debug.LogWarning(
                "CoinDrop: Coin Prefab is not assigned.",
                this
            );

            return;
        }

        Vector2 origin = (Vector2)transform.position + Vector2.up;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            raycastDistance,
            groundLayer
        );

        if (hit.collider != null)
        {
            Vector2 coinPosition =
                hit.point + Vector2.up * groundOffset;

            Instantiate(
                coinPrefab,
                coinPosition,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning(
                "CoinDrop: No ground detected below enemy. " +
                "Dropping coin at enemy position."
            );

            Instantiate(
                coinPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}