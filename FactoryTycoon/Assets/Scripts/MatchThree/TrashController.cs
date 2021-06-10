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
    public Image image => GetComponent<Image>();
    public SlotController slot => GetComponentInParent<SlotController>();

    public bool selected => _trashService.selected;

    public void InitService(TrashType trashType)
    {
        _trashService = new TrashService(trashType, this);
    }

    public void InitView()
    {
        _trashView = new TrashView(TrashType, this);
        _trashView.SetSpriteToImage(image);
    }

    public void SelfDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void Swap(IMatchThreeItem otherItem)
    {
        GridController.Swap(this, otherItem);
    }

    public SlotController GetSlot() => slot;

    public GameObject GetGameObject() => gameObject;

    public void OnPointerDown(PointerEventData eventData) => _trashService.OnPointerDown(eventData);

    public void OnPointerEnter(PointerEventData eventData) => _trashService.OnPointerEnter(eventData);
}
