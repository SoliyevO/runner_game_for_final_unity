using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;      // Score qiymati
    public Text scoreText;     // UI da score ko'rsatish uchun

    void Start()
    {
        //UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        //UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        //UpdateScoreUI();
    }

    //void UpdateScoreUI()
    //{
    //    scoreText.text = "Score: " + score;
    //}
}
