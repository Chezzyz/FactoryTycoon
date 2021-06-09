using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState singleton;

    private int _currentGoal = 1;
    private int _currentTable = 1;

    public int _currentLevel = 0;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
        DontDestroyOnLoad(singleton);

        SceneLoader.OnLevelEnterEvent += SetLevelNumber;
        Goal.OnEndGoalEvent += OnEndGoal;
    }

    public void IncrementGoalNumber()
    {
        _currentGoal++;
    }

    public void SetGoalNumber(int num)
    {
        _currentGoal = num;
    }

    public string GetCurrentGoalNumber(bool onlyNumber)
    {
        if (onlyNumber) return _currentGoal.ToString();
        return _currentTable + "_" + _currentGoal;
    }

    public void IncrementTableNumber()
    {
        _currentTable++;
    }

    public void SetLevelNumber(int level)
    {
        _currentLevel = level;
    }

    private void OnEndGoal(bool lastGoal)
    {
        IncrementGoalNumber();

        if (lastGoal)  //change to new table goals
        {
            IncrementTableNumber();
            SetGoalNumber(1);
        }
    }

}
