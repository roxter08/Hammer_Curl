using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewController
{
    private Transform gameHUDTransform;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI turnText;

    public ViewController(Transform gameHUD)
    {
        gameHUDTransform = gameHUD;

        scoreText = gameHUDTransform.Find("ScoreBoard/ScoreInfo/Score").GetComponent<TextMeshProUGUI>();
        turnText = gameHUDTransform.Find("ScoreBoard/TurnsInfo/Turns").GetComponent<TextMeshProUGUI>();
    }

    public void DisplayTurns(int turns)
    {
        turnText.text = turns.ToString();
    }

    public void DisplayNewScore(int value)
    {
        scoreText.text = value.ToString();
    }
}
