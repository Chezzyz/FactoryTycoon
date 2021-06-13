using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] GameObject SlotPrefab;
    [SerializeField] GameObject TrashPrefab;
    [SerializeField] GameObject SlotImage;
    [SerializeField] public Image ReshuffleImage;
    [SerializeField] public Transform GridImages;

    private GridService _gridService;

    public int fieldSize => _gridService.fieldSize;
    
    public void InitService(int fieldSize)
    {
        _gridService = new GridService(fieldSize, this, SlotPrefab, TrashPrefab, SlotImage);
    }

    public void CreateGrid() => _gridService.CreateGrid();

    public void FillGrid() => _gridService.FillGrid();

    public SlotController GetSlotByPosition(int x, int y) => _gridService.GetSlotByPosition(x, y);

    public bool HaveSlotAt(int x, int y) => _gridService.HaveSlotAt(x, y);

    public bool HaveItemAt(int x, int y) => _gridService.HaveItemAt(x, y);

}
