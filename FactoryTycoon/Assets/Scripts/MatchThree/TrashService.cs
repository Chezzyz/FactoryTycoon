using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashService
{
    public TrashType ItemType { get; private set; }

    public static IMatchThreeItem s_FirstSwapped;

    public static IMatchThreeItem s_SecondSwapped;

    private readonly TrashController _trashController;

    private bool _isAnimationPlaying = false;

    private static TrashController s_selectedController;

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
        ItemType = trashType;
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
            s_selectedController = _trashController;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsAbleToSwap() && !_isAnimationPlaying)
        {
            Swap(s_selectedController);
            s_selectedController = null;
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

        s_FirstSwapped = _trashController;
        s_SecondSwapped = second;

        MatchThreeController.animationController.Swap(second, bufFirst, sendEvent);
    }
    
    private bool IsAbleToSwap()
    {
        return 
            MouseProperties.isLeftButtonDown &&
            s_selectedController != null &&
            _trashController.Slot.IsNeighborWith(s_selectedController.Slot.PosX, s_selectedController.Slot.PosY);
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
        var downSlotX = _trashController.Slot.PosX;
        var downSlotY = _trashController.Slot.PosY + 1;

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
        var downSlotX = _trashController.Slot.PosX;
        var downSlotY = _trashController.Slot.PosY + 1;
        return
            MatchThreeController.gridController.HaveSlotAt(downSlotX, downSlotY) &&
           !MatchThreeController.gridController.HaveItemAt(downSlotX, downSlotY);
    }
}
