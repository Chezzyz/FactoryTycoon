using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RatioController : MonoBehaviour, ICheckable
{
    [SerializeField] List<TextMeshProUGUI> Ratios = new List<TextMeshProUGUI>();
   
    [SerializeField] List<TextMeshProUGUI> Machines = new List<TextMeshProUGUI>();
   
    [SerializeField] List<Image> Plates = new List<Image>();


    private const int _machinesCount = 22;
    private readonly int[] _machinesCurrentCount = new int[3] { 20, 18, 14 };
    private int[] _machinesCurrentCountStart;
    private int _currentIndex;

    private readonly Color GREEN = new Color(0.5f, 0.9f, 0.5f);
    private readonly Color RED = new Color(0.9f, 0.5f, 0.5f);
    private readonly Color YELLOW = new Color(0.9f, 0.9f, 0.5f);

    private void Start()
    {
        _machinesCurrentCountStart = (int[])_machinesCurrentCount.Clone();

        var coefCount = 3;

        for(int i = 0; i < coefCount; i++)
        {
            _currentIndex = i;
            ChangeMachineCurrentCount(0);
        }

        _currentIndex = -1;
    }

    public void SetIndex(int index) => _currentIndex = index;

    public void ChangeMachineCurrentCount(int difference)
    {
        var currentCount = _machinesCurrentCount[_currentIndex];

        currentCount += difference;

        if (currentCount < 0)
        {
            currentCount = 0;
        }
        if(currentCount > _machinesCount)
        {
            currentCount = _machinesCount;
        }

        ChangeMachineCountText(_currentIndex, currentCount);

        ChangeRatioText(_currentIndex, currentCount);

        ChangePlateColor(_currentIndex, currentCount);

        _machinesCurrentCount[_currentIndex] = currentCount;
    }

    private void ChangePlateColor(int index, int count)
    {
        var currentRatio = (float)count / _machinesCount;

        if (currentRatio >= 0.8 && currentRatio <= 0.85)
        {
            Plates[index].color = GREEN;
        }
        else if (currentRatio < 0.8)
        {
            Plates[index].color = YELLOW;
        }
        else if (currentRatio > 0.85)
        {
            Plates[index].color = RED;
        }
    }

    private void ChangeMachineCountText(int index, int count)
    {
        int diff = count - _machinesCurrentCountStart[index];
        char sign = Math.Sign(diff) == 1 ? '+' : '-';

        Machines[index].text = $"N {sign} {Math.Abs(diff)}";
    }

    private void ChangeRatioText(int index, int count) => Ratios[index].text = $"{((float)count / _machinesCount):F2}";

    public bool CheckAnswer()
    {
        foreach(var count in _machinesCurrentCount)
        {
            var currentRatio = (float)count / _machinesCount;

            if (!(currentRatio >= 0.8 && currentRatio <= 0.85))
            {
                return false;
            }
        }

        return true;
    }
}
