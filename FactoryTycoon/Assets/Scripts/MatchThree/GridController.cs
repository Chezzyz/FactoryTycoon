using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController 
{
    private GridService _gridService;
    public int fieldSize;
    public List<SlotController> slots => GridService.slots;
    
    public GridController(int fieldSize)
    {
        this.fieldSize = fieldSize;
        _gridService = Object.FindObjectOfType<GridService>();
    }

    public void Swap(IMatchThreeItem first, IMatchThreeItem second) => _gridService.Swap(first, second);

    public void CreateGrid() => _gridService.CreateGrid(fieldSize);

    public void FillGrid() => _gridService.FillGrid();

    public bool IsSlotNeighborsSlots(SlotController first, SlotController second) => _gridService.IsNeighborsSlots(first, second);

}
