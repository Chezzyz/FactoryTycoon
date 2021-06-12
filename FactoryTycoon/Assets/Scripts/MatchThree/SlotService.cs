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

    public bool IsNeighborFor(SlotController otherSlot)
    {
        return Math.Abs(posX - otherSlot.posX) + Math.Abs(posY - otherSlot.posY) == 1;
    }

    public bool IsSameTypeNeighborWith(int x, int y)
    {
        return
            (x == posX || y == posY) && // on same line
            MatchThreeController.gridController.HaveSlotAt(x, y) &&
            MatchThreeController.gridController.HaveItemAt(x, y) &&
            IsSameTypeWith(x, y);
    }

    private bool IsSameTypeWith(int x, int y)
    {
        var thisTrash = (TrashController) _slotController.TrashController;
        var otherTrash = (TrashController)MatchThreeController.gridController.GetSlotByPosition(x, y).TrashController;

        return thisTrash.TrashType == otherTrash.TrashType;
    }
}
