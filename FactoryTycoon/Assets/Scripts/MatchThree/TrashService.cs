using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashService 
{
    public TrashType _trashType { get; private set; }

    private TrashController _trashController;

    private static TrashController selectedController;
    
    public enum TrashType
    {
        bolt,
        screw,
        nail,
        gear,
        wire
    }

    public TrashService(TrashType trashType, TrashController controller)
    {
        _trashType = trashType;
        _trashController = controller;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        selectedController = _trashController;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAbleToSwap())
        {
            Swap(selectedController);
            selectedController = null;
        }
    }

    public void Swap(IMatchThreeItem second)
    {
        var bufFirst = (IMatchThreeItem) _trashController;

        var firstSlot = _trashController.GetSlot();
        var secondSlot = second.GetSlot();

        firstSlot.SetItemController(second, false);
        secondSlot.SetItemController(bufFirst, false);

        MatchThreeController.animationController.Swap(second, bufFirst);
    }
    
    private bool IsAbleToSwap()
    {
        return 
            MouseProperties.isLeftButtonDown &&
            selectedController != null &&
            _trashController.slot.IsNeighborWith(selectedController.slot);
    }

    public void SelfDestroy()
    {
        _trashController.slot.SetItemController(null);
        Object.Destroy(_trashController.gameObject);
    }

    public void FallDown()
    {
        var downSlotX = _trashController.slot.posX;
        var downSlotY = _trashController.slot.posY + 1;

        if (IsAbleToFall(downSlotX, downSlotY))
        {
            MatchThreeController.gridController.GetSlotByPosition(downSlotX, downSlotY)
                .SetItemController(_trashController);
            MatchThreeController.animationController.Fall(_trashController);
        }
    }

    private bool IsAbleToFall(int downSlotX, int downSlotY)
    {
        return
            MatchThreeController.gridController.HaveSlotAt(downSlotX, downSlotY) &&
           !MatchThreeController.gridController.HaveItemAt(downSlotX, downSlotY);
    }
}
