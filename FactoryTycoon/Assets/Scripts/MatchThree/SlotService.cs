using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotService 
{
    private readonly SlotController _slotController;
    public int PosX { get; private set; }
    public int PosY { get; private set; }

    public IMatchThreeItem CurrentItemController { get; private set; }

    public SlotService(int x, int y, SlotController controller)
    {
        PosX = x;
        PosY = y;
        _slotController = controller;
    }

    public void SetItemController(IMatchThreeItem controller, bool deletePrevSlot = true)
    {
        var curControllerSlot = controller?.GetSlot();
        if(curControllerSlot != _slotController && curControllerSlot != null && deletePrevSlot) curControllerSlot.SetItemController(null); // null previous slot item

        CurrentItemController = controller;
        
        if (controller != null) //change transform of item
        {
            var itemTransform = controller.GetGameObject().transform;
            itemTransform.SetParent(_slotController.transform);
            //itemTransform.localPosition = Vector3.zero; // without animation
        }
    }

    public bool IsSameTypeNeighborWith(int x, int y)
    {
        return
            MatchThreeController.gridController.HaveSlotAt(x, y) &&
            MatchThreeController.gridController.HaveItemAt(x, y) &&
            IsNeighborFor(x, y) &&
            IsSameTypeWith(x, y);
    }

    public bool IsNeighborFor(int x, int y)
    {
        return Math.Abs(PosX - x) + Math.Abs(PosY - y) == 1;
    }

    private bool IsSameTypeWith(int x, int y)
    {
        var thisTrash = _slotController.TrashController;
        var otherTrash = MatchThreeController.gridController.GetSlotByPosition(x, y).TrashController;

        if (thisTrash == null || otherTrash == null) return false;

        return thisTrash.GetItemType() == otherTrash.GetItemType();
    }

    public List<SlotController> GetSameTypeNeighbors()
    {
        var neighborsList = new List<SlotController>();
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (IsSameTypeNeighborWith(PosX + i, PosY + j)) 
                {
                    neighborsList.Add(MatchThreeController.gridController.GetSlotByPosition(PosX + i, PosY + j));
                }
            }
        }

        return neighborsList;
    }

    public int GetNeighborsCountOfType(string type)
    {
        var count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (IsNeighborFor(PosX + i, PosY + j) &&
                    MatchThreeController.gridController.HaveSlotAt(PosX + i, PosY + j) &&
                    MatchThreeController.gridController.HaveItemAt(PosX + i, PosY + j) &&
                    MatchThreeController.gridController.GetSlotByPosition(PosX + i, PosY + j).TrashController.GetItemType() == type.ToString())
                {
                        count++;
                    
                }
            }
        }
        return count;
    }

    public bool IsAbleToMatchBetweenSlots()
    {
        List<SlotController> slotControllers = new List<SlotController>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!(i == 0 && j == 0) &&
                    !IsNeighborFor(PosX + i, PosY + j) &&
                    MatchThreeController.gridController.HaveSlotAt(PosX + i, PosY + j) &&
                    MatchThreeController.gridController.HaveItemAt(PosX + i, PosY + j) &&
                    MatchThreeController.gridController.GetSlotByPosition(PosX + i, PosY + j)
                    .TrashController.GetItemType() == _slotController.TrashController.GetItemType())
                {
                    slotControllers.Add(MatchThreeController.gridController.GetSlotByPosition(PosX + i, PosY + j));
                }
            }
        }

        if (slotControllers.Count > 1)
        {
            foreach (var first in slotControllers)
            {
                foreach (var second in slotControllers)
                {
                    if (first.PosX == second.PosX || first.PosY == second.PosY)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
