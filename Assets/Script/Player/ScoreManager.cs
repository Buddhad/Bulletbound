using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int CurrentScore = 0;


    public static void AddScore(int amount)
    {
        float multiplier = 1f;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerAbilityManager abilityManager = player.GetComponent<PlayerAbilityManager>();
            if (abilityManager != null)
            {
                multiplier = abilityManager.GetScoreMultiplier();
            }
        }

        CurrentScore += Mathf.RoundToInt(amount * multiplier);
    }

    public static void ResetScore()
    {
        CurrentScore = 0;
    }

}