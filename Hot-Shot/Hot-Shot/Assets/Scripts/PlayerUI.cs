using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    // UI Text elements for displaying player stats and game information
    public TMP_Text hpText;
    public TMP_Text ammoText;
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text enemyText;

    // References to placeholder scripts that manage player status, score, and level information
    public PlayerHealth playerHealth;
    public pPlayerStatus ammo;
    public pPlayerScore playerScore;
    public pLevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize UI with values from other scripts
        UpdateHP(playerHealth.currentHealth);
        UpdateAmmo(ammo.playerAmmo);
        UpdateScore(playerScore.currentScore, playerScore.personalBest);
        UpdateLevel(levelManager.currentLevel);
        UpdateEnemies(levelManager.enemiesRemaining, levelManager.totalEnemies);

        // Subscribe to updates from other scripts
        playerHealth.OnHealthUpdate += HandlePlayerHealthUpdate;
        playerScore.OnPlayerScoreUpdate += HandlePlayerScoreUpdate;
        levelManager.OnLevelUpdate += HandleLevelUpdate;
        levelManager.OnEnemyUpdate += HandleEnemyUpdate;

        
    }

    // Update the HP text in the UI
    public void UpdateHP(int hp)
    {
        // Set the text of the hpText UI element to display the current HP

        hpText.text = "HP: " + hp.ToString();
    }

    // Update the ammo text in the UI
    public void UpdateAmmo(int ammo)
    {
        
        // Set the text of the ammoText UI element to display the current ammo count
        ammoText.text = "Ammo: " + ammo.ToString();
    }

    // Update the score text in the UI
    void UpdateScore(int currentScore, int personalBest)
    {
        // Set the text of the scoreText UI element to display the current score and personal best
        scoreText.text = currentScore.ToString() + " / " + personalBest.ToString();
    }

    // Update the level text in the UI
    void UpdateLevel(int level)
    {
        // Set the text of the levelText UI element to display the current level
        levelText.text = "Level: " + level.ToString();
    }

    // Update the enemies remaining text in the UI
    void UpdateEnemies(int currentEnemies, int totalEnemies)
    {
        // Set the text of the enemyText UI element to display the current number of enemies remaining and total enemies
        enemyText.text = currentEnemies.ToString() + " / " + totalEnemies.ToString();
    }


    // Handler for player status updates (HP and ammo)
    void HandlePlayerHealthUpdate(int currentHealth) //method to update the UI
    {
        UpdateHP(currentHealth);
    }

    // Handler for player score updates
    void HandlePlayerScoreUpdate(int currentScore, int personalBest)
    {
        // When the OnPlayerScoreUpdate event is triggered, update the score UI element
        UpdateScore(currentScore, personalBest);
    }

    // Handler for level updates
    void HandleLevelUpdate(int level)
    {
        // When the OnLevelUpdate event is triggered, update the level UI element
        UpdateLevel(level);
    }

    // Handler for enemy updates
    void HandleEnemyUpdate(int currentEnemies, int totalEnemies)
    {
        // When the OnEnemyUpdate event is triggered, update the enemy count UI element
        UpdateEnemies(currentEnemies, totalEnemies);
    }

    void OnDestroy()
    {
        // Unsubscribe from updates when the object is destroyed to prevent memory leaks

        // Unsubscribe the HandlePlayerStatusUpdate method from the OnPlayerStatusUpdate event
        playerHealth.OnHealthUpdate -= HandlePlayerHealthUpdate;

        // Unsubscribe the HandlePlayerScoreUpdate method from the OnPlayerScoreUpdate event
        playerScore.OnPlayerScoreUpdate -= HandlePlayerScoreUpdate;

        // Unsubscribe the HandleLevelUpdate method from the OnLevelUpdate event
        levelManager.OnLevelUpdate -= HandleLevelUpdate;

        // Unsubscribe the HandleEnemyUpdate method from the OnEnemyUpdate event
        levelManager.OnEnemyUpdate -= HandleEnemyUpdate;
    }
}