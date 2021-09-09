using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState Singleton;
    public int CurrentLevel {get; private set;}
    public bool IsLoadedGame { get; private set; }

    private HashSet<string> _completedTips = new HashSet<string>();

    private HashSet<string> _completedStages = new HashSet<string>();

    private Dictionary<int, string> _stageNames = new Dictionary<int, string>()
    {
        { 0, "Map" }, {1, "Table1"}, {2, "Table2"}, {3, "Table3"}, 
        {4, "Table4"}, {5, "Table5"}, {6, "Table6"}, {7, "Table7"}, 
        {8, "Table8"}, {9, "Menu"},
    };

    private int _currentGoal = 1;
    public int _currentTable = 1; //public for debug


    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            CurrentLevel = 0;
        }
        DontDestroyOnLoad(Singleton);

        Goal.OnEndGoalEvent += OnEndGoal;
    }

    public void SetLoadedGame(int level)
    {
        _currentTable = level;
        IsLoadedGame = true;
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

    public int GetTable()
    {
        return _currentTable;
    }

    public string GetNameByBuildIndex(int buildIndex) => _stageNames[buildIndex];

    public bool IsStageCompleted(string stage) => _completedStages.Contains(stage);

    public bool IsStageTipsCompleted(string stage) => _completedTips.Contains(stage);

    public void CompleteTipsOfStage(int buildIndex) => _completedTips.Add(GetNameByBuildIndex(buildIndex));

    private void OnEndGoal(bool lastGoal)
    {
        IncrementGoalNumber();

        if (lastGoal)  //change to new table goals
        {
            _completedStages.Add(GetNameByBuildIndex(_currentTable));
            IncrementTableNumber();
            SetGoalNumber(1);
        }
    }
}
