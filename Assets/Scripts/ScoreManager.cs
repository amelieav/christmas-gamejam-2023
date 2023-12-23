using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public UnityEngine.UI.Text scoreText;
    private int score = 0; // The current score

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void IncrementScore(int amount)
    {
        score += amount;
        scoreText.text = "Coal collected: " + score;
    }
}
