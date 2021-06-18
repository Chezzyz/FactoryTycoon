using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotService 
{
    private SlotController _slotController;
    public int posX { get; private set; }
    public int posY { get; private set; }

    public IMatchThreeItem currentItemController { get; private set; }

    public SlotService(int x, int y, SlotController controller)
    {
        posX = x;
        posY = y;
        _slotController = controller;
    }

    public void SetItemController(IMatchThreeItem controller, bool deletePrevSlot = true)
    {
        var curControllerSlot = controller?.GetSlot();
        if(curControllerSlot != _slotController && curControllerSlot != null && deletePrevSlot) curControllerSlot.SetItemController(null); // null previous slot item

        currentItemController = controller;
        
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
        return Math.Abs(posX - x) + Math.Abs(posY - y) == 1;
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
                if (IsSameTypeNeighborWith(posX + i, posY + j)) 
                {
                    neighborsList.Add(MatchThreeController.gridController.GetSlotByPosition(posX + i, posY + j));
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
                if (IsNeighborFor(posX + i, posY + j) &&
                    MatchThreeController.gridController.HaveSlotAt(posX + i, posY + j) &&
                    MatchThreeController.gridController.HaveItemAt(posX + i, posY + j) &&
                    MatchThreeController.gridController.GetSlotByPosition(posX + i, posY + j).TrashController.GetItemType() == type.ToString())
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
                    !IsNeighborFor(posX + i, posY + j) &&
                    MatchThreeController.gridController.HaveSlotAt(posX + i, posY + j) &&
                    MatchThreeController.gridController.HaveItemAt(posX + i, posY + j) &&
                    MatchThreeController.gridController.GetSlotByPosition(posX + i, posY + j)
                    .TrashController.GetItemType() == _slotController.TrashController.GetItemType())
                {
                    slotControllers.Add(MatchThreeController.gridController.GetSlotByPosition(posX + i, posY + j));
                }
            }
        }

        if (slotControllers.Count > 1)
        {
            foreach (var first in slotControllers)
            {
                foreach (var second in slotControllers)
                {
                    if (first.posX == second.posX || first.posY == second.posY)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
