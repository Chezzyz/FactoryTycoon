using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrashService;

public class GridService : MonoBehaviour
{
    [SerializeField] GameObject SlotPrefab;
    [SerializeField] GameObject TrashPrefab;
    public static List<SlotController> slots { get; private set; }

    private int _fieldSize;
    private int _slotCount => (int) Mathf.Pow(_fieldSize, 2);

    public void CreateGrid(int fieldSize)
    {
        _fieldSize = fieldSize;
        slots = new List<SlotController>();

        var layoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layoutGroup.constraintCount = fieldSize;

        for(int i = 0; i < _fieldSize; i++)
        {
            for (int j = 0; j < _fieldSize; j++)
            {
                var newSlot = SpawnSlot(j, i);
                slots.Add(newSlot);
            }
        }
    }

    private SlotController SpawnSlot(int x, int y)
    {
        var newSlotGO = Instantiate(SlotPrefab, transform);
        var slotController = newSlotGO.GetComponent<SlotController>();
        slotController.InitService(x, y);
        return slotController;
    }

    public void Swap(IMatchThreeItem first, IMatchThreeItem second)
    {
        var bufFirst = first;

        var firstSlot = first.GetSlot();
        var secondSlot = second.GetSlot();

        firstSlot.SetItemController(second);
        secondSlot.SetItemController(bufFirst);
    }

    public void FillGrid()
    {
        for (int i = 0; i < _slotCount; i++)
        {
            var newTrash = SpawnTrash(slots[i]);
            slots[i].SetItemController(newTrash);
        }
    }

    private TrashController SpawnTrash(SlotController slot)
    {
        var rnd = new UnityEngine.Random();
        var typeCount = Enum.GetNames(typeof(TrashType)).Length;

        var trashGo = Instantiate(TrashPrefab, slot.transform);
        var newTrash = trashGo.GetComponent<TrashController>();

        newTrash.InitService((TrashType) UnityEngine.Random.Range(0,typeCount));
        newTrash.InitView();

        return newTrash;
    }

    public bool IsNeighborsSlots(SlotController first, SlotController second)
    {
        return Math.Abs(first.posX - second.posX) + Math.Abs(first.posY - second.posY) == 1;
    }
}
