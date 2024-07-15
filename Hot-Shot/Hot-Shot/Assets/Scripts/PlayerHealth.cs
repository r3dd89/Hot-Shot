using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamage
{
    // Player's maximum health
    [SerializeField] public int maxHealth = 100;

    // Player's current health
    public int currentHealth;

    // Reference to the lose menu in the UI
    [SerializeField] GameObject menuLose;

    // Reference to the damage flash image
    [SerializeField] public GameObject damageFlashImage;

    // Duration of the damage flash effect
    [SerializeField] float flashDuration = 0.2f;



    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player's current health to max health
        currentHealth = maxHealth;

        // Ensure the damage flash image is initially disabled
        if (damageFlashImage != null)
        {
            damageFlashImage.SetActive(false);
        }
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
        // Reduce the current health by the damage amount
        currentHealth -= amount;

        // Trigger the health update event
        OnHealthUpdate?.Invoke(currentHealth);

        // Flash the damage effect
        StartCoroutine(FlashDamageEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Show the lose menu
        menuLose.SetActive(true);
    }

    IEnumerator FlashDamageEffect()
    {
        if (damageFlashImage != null)
        {
            damageFlashImage.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            damageFlashImage.SetActive(false);
        }
    }
}


