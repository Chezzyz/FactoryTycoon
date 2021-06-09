using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController 
{
    private GridService _gridService;
    private int _fieldSize;

    public List<SlotController> slots { get; private set; }

    public GridController(int fieldSize)
    {
        _fieldSize = fieldSize;
        _gridService = Object.FindObjectOfType<GridService>();
        slots = new List<SlotController>();
    }

    public void Swap(IMatchThreeItem first, IMatchThreeItem second) => _gridService.Swap(first, second);

    public void CreateGrid() => _gridService.CreateGrid(_fieldSize, slots);

}
