using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int ammo = 7;
    [SerializeField] private int maxAmmo = 7;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI showAmmo;


    private void Update()
    {
        UpdateAmmoUI();

        if (Input.GetKeyDown(KeyCode.F) && ammo > 0)
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        ammo--;

        Debug.Log(
            "Shot fired. Ammo: " +
            ammo +
            "/" +
            maxAmmo
        );
    }


    private void UpdateAmmoUI()
    {
        if (showAmmo == null)
            return;

        showAmmo.text =
            "Bullet: " +
            ammo +
            "/" +
            maxAmmo;
    }
}