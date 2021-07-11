using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject introPanel, gamePanel, finishPanel;

    // The game is divided into states
    public enum GameState
    {
        Prepare,
        MainGame,
        FinishGame
    }
    private GameState _currentGameState;
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            switch (value)
            {
                case GameState.Prepare:
                    CameraManager.instance.FinishToIntro();
                    StartCoroutine(CameraManager.instance.IntroToMain_());
                    break;
                case GameState.MainGame:
                    break;
                case GameState.FinishGame:
                    CameraManager.instance.MainToFinish();
                    break;
                default:
                    break;
            }
            _currentGameState = value;
        }
    }
  
    private void Awake()
    {
        instance = this;
        CurrentGameState = GameState.Prepare;
    }
    
    // Game states are constantly checked in the update.
    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Prepare:
                PrepareGame();
                break;

            case GameState.MainGame:

                GamePlay();
                break;

            case GameState.FinishGame:
                GameOver();
                break;
            default:
                break;
        }
    }
    private void GameOver()
    {
        introPanel.SetActive(false);
        gamePanel.SetActive(false);
        finishPanel.SetActive(true);
    }
    private void GamePlay()
    {
        introPanel.SetActive(false);
        gamePanel.SetActive(true);
        finishPanel.SetActive(false);
    }
    private void PrepareGame()
    {
        introPanel.SetActive(true);
        gamePanel.SetActive(false);
        finishPanel.SetActive(false);
    }
    public void StartGame()
    {
        CurrentGameState = GameState.MainGame;

    }
    public void RestartGame()
    {
        CurrentGameState = GameState.Prepare;
    }


}
