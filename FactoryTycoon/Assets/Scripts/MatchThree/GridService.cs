using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static TrashService;

public class GridService
{
    private GridController _gridController;

    private GameObject _slotPrefab;
    private GameObject _slotImage;
    private GameObject _trashPrefab;

    private static SlotController[,] slots;

    public const string VERTICAL_LINE = "Vertical";
    public const string HORIZONTAL_LINE = "Horizontal";

    public readonly int _fieldSize;

    public delegate void OnAction();
    public static event OnAction OnActionEvent;

    public GridService(int fieldSize, GridController controller, GameObject slotPrefab, GameObject trashPrefab, GameObject slotImage)
    {
        _gridController = controller;
        _fieldSize = fieldSize;
        _slotPrefab = slotPrefab;
        _trashPrefab = trashPrefab;
        _slotImage = slotImage;

        //OnActionEvent += AfterActionCheck;
        AnimationService.OnAnimationEndEvent += AfterActionCheck;
    }

    public void CreateGrid()
    {
        slots = new SlotController[_fieldSize, _fieldSize];

        SetLayoutGridColumn(_gridController.gameObject.GetComponent<GridLayoutGroup>());
        SetLayoutGridColumn(_gridController.GridImages.gameObject.GetComponent<GridLayoutGroup>());

        for(int i = 0; i < _fieldSize; i++)
        {
            for (int j = 0; j < _fieldSize; j++)
            {
                var newSlot = SpawnSlot(j, i);
                UnityEngine.Object.Instantiate(_slotImage, _gridController.GridImages);
                slots[i,j] = newSlot;
            }
        }
    }

    private void SetLayoutGridColumn(GridLayoutGroup gridLayoutGroup)
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = _fieldSize;
    }

    private SlotController SpawnSlot(int x, int y)
    {
        var newSlotGO = UnityEngine.Object.Instantiate(_slotPrefab, _gridController.transform);
        var slotController = newSlotGO.GetComponent<SlotController>();
        slotController.InitService(x, y);
        return slotController;
    }

    public void FillGrid()
    {
        foreach(var slot in slots)
        {
            var newTrash = SpawnTrash(slot);
            slot.SetItemController(newTrash);
        }
    }

    private TrashController SpawnTrash(SlotController slot)
    {
        var rnd = new UnityEngine.Random();
        var typeCount = Enum.GetNames(typeof(TrashType)).Length;

        var trashGo = UnityEngine.Object.Instantiate(_trashPrefab, slot.transform);
        var newTrash = trashGo.GetComponent<TrashController>();

        newTrash.InitService((TrashType) UnityEngine.Random.Range(0,typeCount));
        newTrash.InitView();

        return newTrash;
    }

    private void AfterActionCheck()
    {
        DestroyLines();
        LetItemsDown();
    }

    private void LetItemsDown()
    {
        for(int i = _fieldSize - 2; i >= 0; i--) //from down to up
        {
            var fallList = GetLineForFall(i);
            foreach(var controller in fallList)
            {
                controller.FallDown();
            }
        }
    }

    private void FillUpAfterDestroy()
    {

    }

    private void DestroyLines()
    {
        var destroyList = new List<SlotController>();

        for (int i = 0; i < _fieldSize; i++)
        {
            var horizontalList = GetLineForDestroy(HORIZONTAL_LINE, i);
            var verticalList = GetLineForDestroy(VERTICAL_LINE, i);
            destroyList = destroyList.Concat(horizontalList).Concat(verticalList).ToList(); //add horizonatal and vertical items to list
        }

        foreach (var slot in destroyList)
        {
            slot.TrashController.SelfDestroy();
        }
    }

    private List<SlotController> GetLineForDestroy(string lineType, int lineIndex)
    {
        if (lineType != HORIZONTAL_LINE && lineType != VERTICAL_LINE)
        {
            throw new Exception("Incorrect type of line, use const strings of this class");
        }

        var lineList = new List<SlotController>();

        var isCreatingLine = false;
        for (int i = 1; i < _fieldSize; i++)
        {
            var currentX = lineType == HORIZONTAL_LINE ? i : lineIndex;
            var currentY = lineType == VERTICAL_LINE ? i : lineIndex;
            var deltaX = lineType == HORIZONTAL_LINE ? 1 : 0;
            var deltaY = lineType == VERTICAL_LINE ? 1 : 0;

            SlotController currentSlot = GetSlotByPosition(currentX, currentY);

            if (currentSlot.IsSameTypeNeighborWith(currentX - deltaX, currentY - deltaY))
            {
                if (currentSlot.IsSameTypeNeighborWith(currentX + deltaX, currentY + deltaY) && !isCreatingLine) //add current and prev if next same
                {
                    isCreatingLine = true;
                    lineList.Add(currentSlot);
                    lineList.Add(GetSlotByPosition(currentX - deltaX, currentY - deltaY)); 
                }
                else if (isCreatingLine) lineList.Add(currentSlot);
            }
            else isCreatingLine = false;
        }
        if (!(lineList.Count == 0 || lineList.Count > 2)) throw new Exception("Invalid line item count");
        return lineList;
    }

    private List<IMatchThreeItem> GetLineForFall(int lineIndex)
    {
        List<IMatchThreeItem> fallList = new List<IMatchThreeItem>();
        for(int i = 0; i < _fieldSize; i++)
        {
            if (HaveItemAt(i, lineIndex))
            {
                fallList.Add(GetSlotByPosition(i, lineIndex).TrashController);
            }
        }

        return fallList;
    }
    
    public SlotController GetSlotByPosition(int x, int y)
    {
        if (!HaveSlotAt(x, y))
        {
            throw new Exception($"There is no slot at {x},{y}");
        }

        return slots[y, x];
    }

    public bool HaveSlotAt(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= _fieldSize || y >= _fieldSize);
    }

    public bool HaveItemAt(int x, int y)
    {
        return HaveSlotAt(x, y) &&
        slots[y, x].TrashController != null;
    }
}
