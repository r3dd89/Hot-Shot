using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageTester : MonoBehaviour
{
    [SerializeField] int damageAmount = 10;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            IDamage playerDamage = GetComponent<IDamage>();
            if (playerDamage != null)
            {
                playerDamage.takeDamage(damageAmount);
                Debug.Log("Applied " + damageAmount + "damage to the player.");
            }
            else
            {
                Debug.LogError("IDamage component not found on player");
            }
        }
    }
}
