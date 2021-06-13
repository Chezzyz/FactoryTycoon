using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static TrashService;

public class TrashController : MonoBehaviour, IMatchThreeItem, IPointerEnterHandler, IPointerDownHandler 
{
    public GridController GridController => MatchThreeController.gridController;

    private TrashService _trashService;
    private TrashView _trashView;
    public TrashType TrashType => _trashService._trashType;
    public Image Image => GetComponent<Image>();
    public SlotController Slot => GetComponentInParent<SlotController>();

    //public bool selected => _trashService.selected;

    public void InitService(TrashType trashType)
    {
        _trashService = new TrashService(trashType, this);
    }

    public void InitView()
    {
        _trashView = new TrashView(TrashType, this);
        _trashView.SetSpriteToImage(Image);
    }

    public void SelfDestroy() => _trashService.SelfDestroy();

    public void Swap(IMatchThreeItem otherItem, bool sendEvent) => _trashService.Swap(otherItem, sendEvent);

    public SlotController GetSlot() => Slot;

    public GameObject GetGameObject() => gameObject;

    public string GetItemType() => TrashType.ToString();

    public void FallDown(bool isLast) => _trashService.FallDown(isLast);

    public bool IsAbleToFall() => _trashService.IsAbleToFall();

    public void OnPointerDown(PointerEventData eventData) => _trashService.OnPointerDown(eventData);

    public void OnPointerEnter(PointerEventData eventData) => _trashService.OnPointerEnter(eventData);
}
