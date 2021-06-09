using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridService : MonoBehaviour
{
    [SerializeField] GameObject SlotPrefab;

    public void Swap(IMatchThreeItem first, IMatchThreeItem second)
    {

    }

    public void CreateGrid(int fieldSize, List<SlotController> slotList)
    {
        var layoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layoutGroup.constraintCount = fieldSize;

        for(int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                var newSlot = Spawn(j, i);
                slotList.Add(newSlot);
            }
        }
    }

    public SlotController Spawn(int x, int y)
    {
        var newSlotGO = Instantiate(SlotPrefab, transform);
        var slotController = newSlotGO.GetComponent<SlotController>();
        slotController.InitService(x, y);
        return slotController;
    }
}
