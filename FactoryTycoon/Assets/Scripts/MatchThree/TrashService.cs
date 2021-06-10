using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashService 
{
    public TrashType _trashType { get; private set; }

    public bool selected { get; private set; }

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
        selected = false;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        selected = true;
        selectedController = _trashController;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAbleToSwap()) _trashController.Swap(selectedController);
    }
    
    private bool IsAbleToSwap()
    {
        return 
            MouseProperties.isLeftButtonDown &&
            selectedController != null &&
            MatchThreeController.gridController.IsSlotNeighborsSlots(selectedController.slot, _trashController.slot);
    }
}
