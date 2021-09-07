using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [Tooltip("Speed of obstacles")]
    [SerializeField]
    private float _obstaclesSpeed = 0.0f;

    [Tooltip("")]
    [SerializeField]
    private float _obstacleSpawnFreqMin = 0.0f;

    [Tooltip("")]
    [SerializeField]
    private float _obstacleSpawnFreqMax = 0.0f;

    [Tooltip("UI obstacles spawner point")]
    [SerializeField]
    private Transform _spawnerPoint;

    [Tooltip("List of obstacles")]
    [SerializeField]
    private List<ObstacleMove> _obstacles = new List<ObstacleMove>();

    [SerializeField]
    private GameObject _winGuy = null;

    private float currentTime = 0.0f;
    private float nextTime = 0.0f;

    private void Awake()
    {
        _winGuy.SetActive(false);
        RunnerDistance.GameWin += EnableWinGuy;
        nextTime = Random.Range(_obstacleSpawnFreqMin, _obstacleSpawnFreqMax);
    }

    private void Update()
    {
        if (RunnerGameState.IsGameRunning)
        {
            currentTime += Time.deltaTime;

            if (currentTime > nextTime)
            {
                currentTime = 0;
                SpawnObstacle();

                nextTime = Random.Range(_obstacleSpawnFreqMin, _obstacleSpawnFreqMax);
            }
        }
    }

    private void EnableWinGuy()
    {
        _winGuy.SetActive(true);
    }

    private void SpawnObstacle()
    {
        var randomIndex = Random.Range(0, _obstacles.Count);
        ObstacleMove newObstacle = Instantiate(_obstacles[randomIndex], _spawnerPoint);
        newObstacle.SetSpeed(_obstaclesSpeed);
    }

    private void OnDestroy()
    {
        RunnerDistance.GameWin -= EnableWinGuy;
    }
}
