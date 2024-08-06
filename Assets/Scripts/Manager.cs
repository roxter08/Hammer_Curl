using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private GameController gameController;
    private ScoreController scoreController;
    private ViewController viewController;
    private GameCallbacks gameCallbacks;

    //Game Related Data
    [Header("GAME DATA")]
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] Card cardPrefab; // Prefab for the card
    [SerializeField] List<Sprite> cardImages; // List of card images

    [Space(30)]

    //UI Related Data
    [Header("UI DATA")]
    [SerializeField] GridLayoutGroup gridRoot;
    [SerializeField] Transform gameHUD;

    private void Awake()
    {
        gameCallbacks = new GameCallbacks();
        scoreController = new ScoreController();
        viewController = new ViewController(gameHUD);
        gameController = new GameController(gridRoot, rows, columns, cardImages, cardPrefab, gameCallbacks);
    }

    private void OnEnable()
    {
        GameCallbacks.OnMatchFound += DoScoreUpdate;
        GameCallbacks.OnTurnsUpdated += DoTurnsUpdate;
    }

    private void OnDisable()
    {
        GameCallbacks.OnMatchFound -= DoScoreUpdate;
        GameCallbacks.OnTurnsUpdated -= DoTurnsUpdate;
    }

    private void Start()
    {
        gameController.Initialize();
    }

    private void Update()
    {
        gameController.CheckForMatch();
    }

    private void DoScoreUpdate()
    {
        scoreController.UpdateScore(10);
        viewController.DisplayNewScore(scoreController.GetScore());
    }
    private void DoTurnsUpdate()
    {
        scoreController.UpdateTurns();
        viewController.DisplayTurns(scoreController.GetTurnsTaken());
    }
}
