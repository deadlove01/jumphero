using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text curScoreText;
    public Text bestScoreText;
    public Text ingameScoreText;


    public static ScoreManager Instance;


    private int curScore = 0;
    private int bestScore = 0;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore");
    }


    public void AddScore()
    {
        curScore++;
        ingameScoreText.text = curScore.ToString();
    }

    public void UpdateFinalScore()
    {
        if (curScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", curScore);
        }

        curScoreText.text = curScore.ToString();
        bestScoreText.text = bestScore.ToString();
    }
}
