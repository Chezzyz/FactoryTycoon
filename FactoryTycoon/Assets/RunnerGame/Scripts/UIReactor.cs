using UnityEngine;
using System;
using UnityEngine.UI;

public class UIReactor : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private GameObject _startWindow;

    [SerializeField]
    private GameObject _gameOverWindow;

    private void Awake()
    {
        _startWindow.SetActive(true);
        _gameOverWindow.SetActive(false);

        _startButton.onClick.AddListener(OnStartButtonClick);
        _restartButton.onClick.AddListener(OnRestartButtonClick);

        PlayerHealth.GameLose += OnGameLose;
    }

    private void OnStartButtonClick()
    {
        _startWindow.SetActive(false);
        RunnerGameState.IsGameRunning = true;
    }

    private void OnRestartButtonClick()
    {
        _gameOverWindow.SetActive(false);
        RunnerGameState.IsGameRunning = true;
    }

    private void OnGameLose()
    {
        _gameOverWindow.SetActive(true);
        RunnerGameState.IsGameRunning = false;
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
        _restartButton.onClick.RemoveAllListeners();
    }
}
