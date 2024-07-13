using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamage
{
    // Player's maximum health
    [SerializeField] public int maxHealth =100;

    // Player's current health
    public int currentHealth;

    // TODO: References the health bar in the UI

    // TODO: Reference the lose menu in the UI
    [SerializeField] GameObject menuLose;

    // Start is called before the first frame update
    void Start()
    {
        // Starts the players current health to max health
        currentHealth = maxHealth;

        // TODO: Set health slider's max value to max health

        // TODO: Set the health slider value equal to the player's current health
        
    }

    // Event for health updates
    public delegate void HealthUpdate(int currentHealth);
    public event HealthUpdate OnHealthUpdate;

    void Update()
    {
        OnHealthUpdate?.Invoke(currentHealth);
    }
    public void takeDamage(int amount)
    {
        // Reduces the current health by the damage amount
        currentHealth -= amount;

        // Trigger the health update event
        OnHealthUpdate?.Invoke(currentHealth);

        // TODO: This will update the health slider's value

        if (currentHealth <= 0)
        {
            Die();
        }
    }  
    
    void Die()
    {
        // Shows lose menu
        menuLose.SetActive(true);
    }

    
}
