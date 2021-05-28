using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState singleton;

    private int currentGoal = 1;
    private int currentTable = 1;

    public int _currentLevel = 0;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
        DontDestroyOnLoad(singleton);

        SceneLoader.OnLevelEnterEvent += SetLevelNumber;
    }

    public void IncrementGoalNumber()
    {
        currentGoal++;
    }

    public void SetGoalNumber(int num)
    {
        currentGoal = num;
    }

    public string GetCurrentGoalNumber()
    {
        return currentTable + "_" + currentGoal;
    }

    public void IncrementTableNumber()
    {
        currentTable++;
    }

    public void SetLevelNumber(int level)
    {
        singleton._currentLevel = level;
    }

}
