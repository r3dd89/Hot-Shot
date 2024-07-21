using UnityEngine;

public class pPlayerController : MonoBehaviour
{
    public pPlayerStatus playerStatus;
    public pPlayerScore playerScore;
    public pLevelManager levelManager;

    void Update()
    {
        // Example input handling to fire a weapon
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStatus.FireWeapon();
        }

        // Example input handling to simulate taking damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStatus.TakeDamage(10); // Simulate taking 10 damage
        }

        // Example input handling to add score
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerScore.AddScore(100); // Simulate scoring 100 points
        }

        // Example input handling to simulate defeating an enemy
        if (Input.GetKeyDown(KeyCode.E))
        {
            levelManager.EnemyDefeated(); // Simulate defeating an enemy
        }
    }
}