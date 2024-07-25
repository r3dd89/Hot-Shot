using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickups : MonoBehaviour
{
    [SerializeField] Pickups pickups;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerUI playerUI;

    int maxAmmo;
    int ammoCount;
    int currentHealth;
    int maxHealth;
    private void Start()
    {
        playerMovement.maxAmmo = maxAmmo;
        playerMovement.ammoCount = ammoCount;
        playerHealth.currentHealth = currentHealth;
        playerHealth.maxHealth = maxHealth;
        Debug.Log(ammoCount + ": ammoCount");
        Debug.Log(maxAmmo + ": maxAmmo");
    }
    private void OnTriggerEnter(Collider other)
    {
        // adding ammo to the current amount
        if (other.CompareTag("Player") && pickups.AmmoBox)
        {
            if (pickups.AmmoBox != null)
            {
                Debug.Log("Ammo picked up");
                playerMovement.PickupAmmo(pickups.AmmoAmount);
                playerUI.UpdateAmmo(playerMovement.ammoCount); // Update the UI
                Destroy(gameObject);
            }

        }

        // adding health to the player up to the maximum amount
        if (other.CompareTag("Player") && pickups.HealthBox)
        {
            currentHealth += pickups.HealAmount;
            if (currentHealth > maxHealth)
            {
               currentHealth = maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
