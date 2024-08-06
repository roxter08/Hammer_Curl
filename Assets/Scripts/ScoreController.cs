using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController
{
    private int score;
    private int turns;

    public ScoreController() 
    {
        //Constructor
    }

    public int GetScore()
    {
        return score;
    }
    public int GetTurnsTaken()
    {
        return turns;
    }

    public void UpdateScore(int value)
    {
        score += value;
    }

    /// <summary>
    /// Turn will always be updated by one
    /// </summary>
    /// <returns></returns>
    public void UpdateTurns()
    {
        turns++;
    }
}
