using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDrop : MonoBehaviour
{
    public GameObject coinPrefab; // Assign your coin prefab in the Inspector
    public LayerMask groundLayer; // Assign this in the Inspector (to your Ground layer)
    private bool hasDropped = false;

    public void DropCoin()
    {
        if (hasDropped) return; // ⛔ prevent double drop
        hasDropped = true;
        Vector2 origin = transform.position + Vector3.up;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 50f, groundLayer);

        if (hit.collider != null)
        {
            Vector2 coinPos = hit.point + Vector2.up * 0.2f;
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No ground detected below to drop the coin.");
            Instantiate(coinPrefab, transform.position, Quaternion.identity); // fallback
        }
    }

}
