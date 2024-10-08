using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager sample;

    public int score;
    public int sayı;

    private Text scoreText;

    private void Awake()
    {
        makeSingleton();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    private void makeSingleton() // Sahne değiştiğinde skorun sıfırlanmaması için
    {
        if (sample != null)
        {
            Destroy(gameObject);
        }
        else
        {
            sample = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        addScore(0);
    }

    void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }
        sayı = score;
    }

    public void addScore(int value)
    {
        score += value;
        if (score > PlayerPrefs.GetInt("HighScore", 0)) // Score , HighScore dan büyük ise HighScore u değiştir
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        scoreText.text = score.ToString();
    }

    public void ResetScore() 
    {
        score = 0;
    }
}
