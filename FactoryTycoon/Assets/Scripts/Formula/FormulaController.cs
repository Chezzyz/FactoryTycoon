using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaController : MonoBehaviour, ICheckable
{
    [SerializeField] List<CellSlot> numeratorCellSlots = new List<CellSlot>();
    [SerializeField] List<CellSlot> denumeratorCellSlots = new List<CellSlot>();
    [SerializeField] List<CellSlot> sumCellSlots = new List<CellSlot>();

    [SerializeField] List<CardData> correctNumeratorList = new List<CardData>();
    [SerializeField] List<CardData> correctDenumeratorList = new List<CardData>();
    [SerializeField] List<CardData> correctSumList = new List<CardData>();

    private HashSet<string> correctNumeratorSet => ConvertListToStringSet(correctNumeratorList);
    private HashSet<string> correctDenumeratorSet => ConvertListToStringSet(correctDenumeratorList);
    private HashSet<string> correctSumSet => ConvertListToStringSet(correctSumList);

    private HashSet<string> numeratorSet = new HashSet<string>();
    private HashSet<string> denumeratorSet = new HashSet<string>();
    private HashSet<string> sumSet = new HashSet<string>();

    void Start()
    {
        CellSlot.OnCardDropEvent += CheckAnswer;
    }

    private HashSet<string> ConvertListToStringSet(List<CardData> list)
    {
        HashSet<string> set = new HashSet<string>(list.Select(card => card._name));
        return set;
    }

    public bool CheckAnswer()
    {
        numeratorSet.Clear();
        denumeratorSet.Clear();
        sumSet.Clear();

        FillSets();

        var numCheck = numeratorSet.SetEquals(correctNumeratorSet); //comment!
        var denumCheck = denumeratorSet.SetEquals(correctDenumeratorSet);
        var sumCheck = sumSet.SetEquals(correctSumSet);

        return numCheck && denumCheck && sumCheck;
    }

    private void FillSets()
    {
        FillSetWithData(numeratorCellSlots, numeratorSet);
        FillSetWithData(denumeratorCellSlots, denumeratorSet);
        FillSetWithData(sumCellSlots, sumSet);
    }

    private void FillSetWithData(List<CellSlot> list, HashSet<string> set)
    {
        var list1 = list.Select(slot => slot.cardData).ToList();
        var list2 = list1.Select(card => card ? set.Add(card._name) : card).ToList();
    }
}
