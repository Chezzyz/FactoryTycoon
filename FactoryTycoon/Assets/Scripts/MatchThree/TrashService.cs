using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashService
{
    public TrashType _trashType { get; private set; }

    private TrashController _trashController;

    private static TrashController selectedController;

    public static IMatchThreeItem firstSwapped;
    public static IMatchThreeItem secondSwapped;

    private bool _isAnimationPlaying = false;

    public enum TrashType
    {
        bolt,
        screw,
        nail,
        gear,
        //wire
    }

    public TrashService(TrashType trashType, TrashController controller)
    {
        _trashType = trashType;
        _trashController = controller;
        AnimationService.OnAnimationDestroyEndEvent += DestroyObject;
        AnimationService.OnAnimationStateChangeEvent += SetAnimationPlaying;
    }

    private void SetAnimationPlaying(bool isPlay)
    {
        _isAnimationPlaying = isPlay;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        if (!_isAnimationPlaying)
        {
            selectedController = _trashController;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAbleToSwap() && !_isAnimationPlaying)
        {
            Swap(selectedController);
            selectedController = null;
        }
    }

    public void Swap(IMatchThreeItem second, bool sendEvent = true)
    {
        var position = second.GetGameObject().transform.position;
        position.Set(position.x,position.y,position.z - 1); //set on front

        var bufFirst = (IMatchThreeItem) _trashController;

        var firstSlot = _trashController.GetSlot();
        var secondSlot = second.GetSlot();

        firstSlot.SetItemController(second, false);
        secondSlot.SetItemController(bufFirst, false);

        firstSwapped = _trashController;
        secondSwapped = second;

        MatchThreeController.animationController.Swap(second, bufFirst, sendEvent);
    }
    
    private bool IsAbleToSwap()
    {
        return 
            MouseProperties.isLeftButtonDown &&
            selectedController != null &&
            _trashController.Slot.IsNeighborWith(selectedController.Slot.posX, selectedController.Slot.posY);
    }

    public void SelfDestroy()
    {
        _trashController.Slot.SetItemController(null);
        MatchThreeController.animationController.SelfDestroy(_trashController);
    }

    private void DestroyObject(IMatchThreeItem item)
    {
        if (_trashController != null && item.GetGameObject() == _trashController.GetGameObject())
        {
            Object.Destroy(_trashController.gameObject);
            AnimationService.OnAnimationDestroyEndEvent -= DestroyObject;
        }
    }

    public void FallDown(bool isLast = false)
    {
        var downSlotX = _trashController.Slot.posX;
        var downSlotY = _trashController.Slot.posY + 1;

        if (IsAbleToFall())
        {
            MatchThreeController.gridController.GetSlotByPosition(downSlotX, downSlotY)
                .SetItemController(_trashController);
            MatchThreeController.animationController.Fall(_trashController, isLast);
            //MonoBehaviour.print($"Fall trash to [{downSlotY},{downSlotX}]");
        }
    }

    public bool IsAbleToFall()
    {
        var downSlotX = _trashController.Slot.posX;
        var downSlotY = _trashController.Slot.posY + 1;
        return
            MatchThreeController.gridController.HaveSlotAt(downSlotX, downSlotY) &&
           !MatchThreeController.gridController.HaveItemAt(downSlotX, downSlotY);
    }
}
