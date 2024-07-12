using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlayerStatus : MonoBehaviour
{
    public int playerHP = 100;
    public int playerAmmo = 50;

    public delegate void PlayerStatusUpdate(int hp, int ammo);
    public event PlayerStatusUpdate OnPlayerStatusUpdate;

    // Example method to simulate taking damage
    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        if (playerHP < 0) playerHP = 0;
        // Raise the event to notify subscribers (e.g., PlayerUI)
        OnPlayerStatusUpdate?.Invoke(playerHP, playerAmmo);
    }

    // Example method to simulate firing a weapon
    public void FireWeapon()
    {
        if (playerAmmo > 0)
        {
            playerAmmo--;
            // Raise the event to notify subscribers (e.g., PlayerUI)
            OnPlayerStatusUpdate?.Invoke(playerHP, playerAmmo);
        }
    }
}