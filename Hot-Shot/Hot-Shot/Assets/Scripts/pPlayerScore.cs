using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlayerScore : MonoBehaviour
{
    public int currentScore = 0;
    public int personalBest = 1000;

    public delegate void PlayerScoreUpdate(int currentScore, int personalBest);
    public event PlayerScoreUpdate OnPlayerScoreUpdate;

    // Example method to simulate scoring points
    public void AddScore(int points)
    {
        currentScore += points;
        if (currentScore > personalBest)
        {
            personalBest = currentScore;
        }
        // Raise the event to notify subscribers (e.g., PlayerUI)
        OnPlayerScoreUpdate?.Invoke(currentScore, personalBest);
    }
}