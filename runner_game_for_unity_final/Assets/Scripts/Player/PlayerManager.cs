using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;
    public Text scoreText;


    public static int numberOfCoins;
    public int score = 0;


    public Text coinsText;
    internal static bool isGamePaused;

    void Start()
    {


        gameOver = false;
        Time.timeScale = 1;

        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()

    {


        //Update UI
        scoreText.text ="Score:"+(score + numberOfCoins);

        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        coinsText.text = "Coins:" + numberOfCoins;

        if(SwipeManager.tap)
        {
            isGameStarted = true;   
            Destroy(startingText);
        }
    }
}
