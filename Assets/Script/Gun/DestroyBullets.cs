using UnityEngine;

public class DestroyBullets : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}