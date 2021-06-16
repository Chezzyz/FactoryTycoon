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

    private static SlotController[,] _slots;
    private static SlotController[] _spawnSlots;

    public const string VERTICAL_LINE = "Vertical";
    public const string HORIZONTAL_LINE = "Horizontal";

    public readonly int fieldSize;

    public delegate void OnAction();
    public static event OnAction OnActionEvent;

    private int _fallStageIndex; 
    public GridService(int fieldSize, GridController controller, GameObject slotPrefab, GameObject trashPrefab, GameObject slotImage)
    {
        _gridController = controller;
        this.fieldSize = fieldSize;
        _slotPrefab = slotPrefab;
        _trashPrefab = trashPrefab;
        _slotImage = slotImage;

        
        AnimationService.OnAnimationSwapEndEvent += AfterSwapCheck;
        AnimationService.OnAnimationFallEndEvent += LetItemsDown;
    }

    public void CreateGrid()
    {
        _slots = new SlotController[fieldSize, fieldSize];
        _spawnSlots = new SlotController[fieldSize];

        SetLayoutGridColumn(_gridController.gameObject.GetComponent<GridLayoutGroup>());
        SetLayoutGridColumn(_gridController.GridImages.gameObject.GetComponent<GridLayoutGroup>());

        for(int x = 0; x < fieldSize; x++)
        {
            var spawnSlot = SpawnSlot(x, -1);
            UnityEngine.Object.Instantiate(_slotImage, _gridController.GridImages);
            _spawnSlots[x] = spawnSlot;
        }

        for(int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                var newSlot = SpawnSlot(x, y);
                newSlot.slotImage = UnityEngine.Object.Instantiate(_slotImage, _gridController.GridImages).GetComponent<Image>();
                _slots[y,x] = newSlot;
            }
        }
    }

    private void SetLayoutGridColumn(GridLayoutGroup gridLayoutGroup)
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = fieldSize;
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
        foreach(var slot in _spawnSlots)
        {
            SpawnTrash(slot);
        }

        foreach(var slot in _slots)
        {
            SpawnTrash(slot);
        }

        while(GetLinesForDestroy().Count > 0 || !IsAbleToTurn())
        {
            ReFillGrid();
        }
    }

    private void ReFillGrid()
    {
        foreach (var slot in _slots)
        {
            UnityEngine.Object.Destroy(slot.TrashController.GetGameObject());
            slot.SetItemController(null);
            SpawnTrash(slot);
        }
    }

    private bool IsAbleToTurn()
    {
        foreach(var slot in _slots)
        {
            if (slot.IsAbleToMatchBetweenSlots())
            {
                return true;
            }

            var neighbors = slot.GetSameTypeNeighbors();
            if (neighbors.Count > 0)
            {
                foreach (var neighbor in neighbors) 
                {
                    if (HaveSlotAt(slot.posX * 2 - neighbor.posX, slot.posX * 2 - neighbor.posY)) //slot + diff between slot and neighbor
                    { 
                        var aroundNeighbor = GetSlotByPosition(slot.posX * 2 - neighbor.posX, slot.posX * 2 - neighbor.posY);

                        if(aroundNeighbor.GetNeighborsCountOfType(slot.TrashController.GetItemType()) > 1)
                        {
                            return true;
                        }
                        
                    }
                
                }
            }
        }

        return false;
    }
 
    private void SpawnTrash(SlotController slot)
    {
        var trashGo = UnityEngine.Object.Instantiate(_trashPrefab, slot.transform);
        var newTrash = trashGo.GetComponent<TrashController>();
        
        var typeCount = Enum.GetNames(typeof(TrashType)).Length;

        newTrash.InitService((TrashType) UnityEngine.Random.Range(0,typeCount));
        newTrash.InitView();

        slot.SetItemController(newTrash);
    }

    private void AfterSwapCheck()
    {
        if (GetLinesForDestroy().Count > 0)
        {
            DestroyLines();
            LetItemsDown(true);
        }
        else
        {
            firstSwapped.Swap(secondSwapped, false);
        }
    }

    public void LetItemsDown(bool firstTime)
    {
        if (firstTime) _fallStageIndex = 0;
        if (_fallStageIndex < fieldSize)
        {
            for (int y = fieldSize - 2; y >= 0; y--) //from down to up
            {
                for (int x = 0; x < fieldSize; x++) //fall line
                {
                    _slots[y, x].TrashController?.FallDown();
                }
            }

            FillUpAfterFall();

            _fallStageIndex++;
        }
    }

    private void FillUpAfterFall()
    {
        SlotController last = null;

        foreach (var slot in _spawnSlots)
        {
            if (slot.TrashController.IsAbleToFall())
            {
                if (last != null)
                {
                    last.TrashController.FallDown();
                    SpawnTrash(last);
                }
                last = slot;
            }
        }

        if (last != null)
        {
            last.TrashController.FallDown(true);
            SpawnTrash(last);
        }
        if (last == null && GetLinesForDestroy().Count > 0) //stop moving and have smth to destroy
        {
            AfterSwapCheck();
        }
        if(last == null && GetLinesForDestroy().Count == 0 && !IsAbleToTurn())
        {
           Reshuffle();
        }
    }

    public void Reshuffle()
    {
        MatchThreeController.animationController.Reshuffle(_gridController.ReshuffleImage);
        
        while (GetLinesForDestroy().Count > 0 || !IsAbleToTurn())
        {
            ReFillGrid();
        }
    }


    private bool DestroyLines()
    {
        var destroyList = GetLinesForDestroy();
        
        if(destroyList.Count == 0) return false;

        foreach (var slot in destroyList)
        {
            slot.TrashController.SelfDestroy();
            //MonoBehaviour.print($"Destroy trash at [{slot.posY},{slot.posX}]");
        }

        return true;
    }

    private HashSet<SlotController> GetLinesForDestroy()
    {
        var destroyList = new HashSet<SlotController>();

        for (int i = 0; i < fieldSize; i++)
        {
            var horizontalList = GetLineForDestroy(HORIZONTAL_LINE, i);
            var verticalList = GetLineForDestroy(VERTICAL_LINE, i);
            var list = horizontalList.Concat(verticalList).Select(slot => destroyList.Add(slot)).ToList(); //add horizonatal and vertical items to list
        }

        return destroyList;
    }

    private List<SlotController> GetLineForDestroy(string lineType, int lineIndex)
    {
        if (lineType != HORIZONTAL_LINE && lineType != VERTICAL_LINE)
        {
            throw new Exception("Incorrect type of line, use const strings of this class");
        }

        var lineList = new List<SlotController>();

        var isCreatingLine = false;
        for (int i = 1; i < fieldSize; i++)
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

        if (lineList.Any(slot => slot.TrashController.IsAbleToFall()))
        { 
            return new List<SlotController>(); 
        }

        return lineList;
    }

    public SlotController GetSlotByPosition(int x, int y)
    {
        if (!HaveSlotAt(x, y))
        {
            throw new Exception($"There is no slot at {x},{y}");
        }

        return _slots[y, x];
    }

    public bool HaveSlotAt(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= fieldSize || y >= fieldSize);
    }

    public bool HaveItemAt(int x, int y)
    {
        return HaveSlotAt(x, y) &&
        _slots[y, x].TrashController != null;
    }
}
