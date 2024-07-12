using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pLevelManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int enemiesRemaining = 10;
    public int totalEnemies = 10;

    public delegate void LevelUpdate(int level);
    public event LevelUpdate OnLevelUpdate;

    public delegate void EnemyUpdate(int currentEnemies, int totalEnemies);
    public event EnemyUpdate OnEnemyUpdate;

    // Example method to simulate enemy defeat
    public void EnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining < 0) enemiesRemaining = 0;
        // Raise the event to notify subscribers (e.g., PlayerUI)
        OnEnemyUpdate?.Invoke(enemiesRemaining, totalEnemies);

        // Example logic to advance level when all enemies are defeated
        if (enemiesRemaining == 0)
        {
            AdvanceLevel();
        }
    }

    // Example method to advance to the next level
    public void AdvanceLevel()
    {
        currentLevel++;
        totalEnemies += 5; // Example logic for increasing enemies per level
        enemiesRemaining = totalEnemies;
        // Raise the events to notify subscribers (e.g., PlayerUI)
        OnLevelUpdate?.Invoke(currentLevel);
        OnEnemyUpdate?.Invoke(enemiesRemaining, totalEnemies);
    }
}