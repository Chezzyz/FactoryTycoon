using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static TrashService;

public class GridService
{

    public const string VERTICAL_LINE = "Vertical";
    public const string HORIZONTAL_LINE = "Horizontal";

    public readonly int FieldSize;

    public delegate void OnDestroyLines(HashSet<SlotController> slotControllers);
    public static event OnDestroyLines OnDestroyLinesEvent;

    private GridController _gridController;

    private readonly GameObject _slotPrefab;
    private readonly GameObject _slotImage;
    private readonly GameObject _trashPrefab;

    private static SlotController[,] s_slots;
    private static SlotController[] s_spawnSlots;

    private int _fallStageIndex; 

    public GridService(int fieldSize, GridController controller, GameObject slotPrefab, GameObject trashPrefab, GameObject slotImage)
    {
        _gridController = controller;
        this.FieldSize = fieldSize;
        _slotPrefab = slotPrefab;
        _trashPrefab = trashPrefab;
        _slotImage = slotImage;

        
        AnimationService.OnAnimationSwapEndEvent += AfterSwapCheck;
        AnimationService.OnAnimationFallEndEvent += LetItemsDown;
    }

    public void CreateGrid()
    {
        s_slots = new SlotController[FieldSize, FieldSize];
        s_spawnSlots = new SlotController[FieldSize];

        SetLayoutGridColumn(_gridController.gameObject.GetComponent<GridLayoutGroup>());
        SetLayoutGridColumn(_gridController.GridImages.gameObject.GetComponent<GridLayoutGroup>());

        for(int x = 0; x < FieldSize; x++)
        {
            var spawnSlot = SpawnSlot(x, -1);
            UnityEngine.Object.Instantiate(_slotImage, _gridController.GridImages);
            s_spawnSlots[x] = spawnSlot;
        }

        for(int y = 0; y < FieldSize; y++)
        {
            for (int x = 0; x < FieldSize; x++)
            {
                var newSlot = SpawnSlot(x, y);
                newSlot.SlotImage = UnityEngine.Object.Instantiate(_slotImage, _gridController.GridImages).GetComponent<Image>();
                s_slots[y,x] = newSlot;
            }
        }
    }

    private void SetLayoutGridColumn(GridLayoutGroup gridLayoutGroup)
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = FieldSize;
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
        foreach(var slot in s_spawnSlots)
        {
            SpawnTrash(slot);
        }

        foreach(var slot in s_slots)
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
        foreach (var slot in s_slots)
        {
            UnityEngine.Object.Destroy(slot.TrashController.GetGameObject());
            slot.SetItemController(null);
            SpawnTrash(slot);
        }
    }

    private bool IsAbleToTurn()
    {
        foreach(var slot in s_slots)
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
                    if (HaveSlotAt(slot.PosX * 2 - neighbor.PosX, slot.PosX * 2 - neighbor.PosY)) //slot + diff between slot and neighbor
                    { 
                        var aroundNeighbor = GetSlotByPosition(slot.PosX * 2 - neighbor.PosX, slot.PosX * 2 - neighbor.PosY);

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
            s_FirstSwapped.Swap(s_SecondSwapped, false);
        }
    }

    public void LetItemsDown(bool firstTime)
    {
        if (firstTime) _fallStageIndex = 0;
        if (_fallStageIndex < FieldSize)
        {
            for (int y = FieldSize - 2; y >= 0; y--) //from down to up
            {
                for (int x = 0; x < FieldSize; x++) //fall line
                {
                    s_slots[y, x].TrashController?.FallDown();
                }
            }

            FillUpAfterFall();

            _fallStageIndex++;
        }
    }

    private void FillUpAfterFall()
    {
        SlotController last = null;

        foreach (var slot in s_spawnSlots)
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

        OnDestroyLinesEvent?.Invoke(destroyList);

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

        for (int i = 0; i < FieldSize; i++)
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
        for (int i = 1; i < FieldSize; i++)
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

        return s_slots[y, x];
    }

    public bool HaveSlotAt(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= FieldSize || y >= FieldSize);
    }

    public bool HaveItemAt(int x, int y)
    {
        return HaveSlotAt(x, y) &&
        s_slots[y, x].TrashController != null;
    }

    public void OnDestroy()
    {
        AnimationService.OnAnimationSwapEndEvent -= AfterSwapCheck;
        AnimationService.OnAnimationFallEndEvent -= LetItemsDown;
    }
}
