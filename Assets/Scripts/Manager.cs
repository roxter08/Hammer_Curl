using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
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
    [SerializeField] Transform uiRootTransform;

    [Space(30)]

    //Audio Related Data
    [Header("AUDIO DATA")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip matchedAudio;
    [SerializeField] AudioClip mismatchedAudio;
    [SerializeField] AudioClip gameOverAudio;

    private GameController gameController;
    private ScoreController scoreController;
    private ViewController viewController;
    private GameCallbacks gameCallbacks;

    private int totalCardsMatched;
    private int totalNumberOfPairs;

    private void Awake()
    {
        totalCardsMatched = 0;
        totalNumberOfPairs = (int)((rows * columns) * 0.5f);

        gameCallbacks = new GameCallbacks();
        scoreController = new ScoreController();
        viewController = new ViewController(uiRootTransform);
        gameController = new GameController(gridRoot, rows, columns, cardImages, cardPrefab, gameCallbacks);
        SoundManager.GetInstance().Initialize(audioSource);
    }

    private void OnEnable()
    {
        GameCallbacks.OnMatchFound += DoScoreUpdate;
        GameCallbacks.OnMatchFound += CheckWinLoseCondition;
        GameCallbacks.OnTurnsUpdated += DoTurnsUpdate;
    }

    private void OnDisable()
    {
        GameCallbacks.OnMatchFound -= DoScoreUpdate;
        GameCallbacks.OnMatchFound -= CheckWinLoseCondition;
        GameCallbacks.OnTurnsUpdated -= DoTurnsUpdate;
    }

    private void Start()
    {
        gameController.Initialize();
        viewController.AddListenerOnExitButtonClicked(ExitGame);
        viewController.AddListenerOnRestartButtonClicked(RestartGame);
    }

    
    private void ExitGame()
    {
        //Exit Game
        Application.Quit();
    }
    private void RestartGame()
    {
        //Reload Level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        gameController.CheckForMatch();
    }

    private void DoScoreUpdate(bool value)
    {
        if (value == false)
        {
            SoundManager.GetInstance().PlayAudio(mismatchedAudio);
        }
        else
        {
            SoundManager.GetInstance().PlayAudio(matchedAudio);
            scoreController.UpdateScore(10);
            viewController.DisplayNewScore(scoreController.GetScore());
        }
    }
    private void DoTurnsUpdate()
    {
        scoreController.UpdateTurns();
        viewController.DisplayTurns(scoreController.GetTurnsTaken());
    }
    private void CheckWinLoseCondition(bool value) 
    {
        if(value == true)
        {
            totalCardsMatched++;
            if (totalCardsMatched == totalNumberOfPairs)
            {
                viewController.DisplayGameCompletion();
                SoundManager.GetInstance().PlayAudio(gameOverAudio);
            }
        }
    }
}
