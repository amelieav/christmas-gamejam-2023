using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public GameObject endScreen;
    public TMP_Text scoreText;

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

    public static void IncrementScore(int amount)
    {
        instance.score += amount;
        instance.scoreText.text = instance.score.ToString();
        if (instance.score >= 100)
        {
            instance.endScreen.SetActive(true);
            instance.enabled = false;
        }
    }
}
