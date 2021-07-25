using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private const string CELL_TAG = "Cell";
    private RectTransform _rectTransform;
    public GameObject _startParent;
    private Vector3 _startPosition;
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    public bool IsInCell = false;
    public CellSlot cellSlot;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.transform.position;
        _canvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _startParent = GetComponentInParent<HorizontalLayoutGroup>().gameObject;
    }

    public void PutCardOnSlot(CellSlot slot)
    {
        if (cellSlot != null && cellSlot != slot) // Попали в другой слот
        {
            slot.cardGO.GetComponent<DragAndDrop>().SwapCard(cellSlot);
        }
        if(cellSlot == null && slot.cardGO != null)
        {
            slot.cardData.DragController.ReturnCardToHand();
            slot.cardGO = gameObject;
        }
        transform.position = slot.transform.position;
        transform.SetParent(slot.transform);
        cellSlot = slot;
        IsInCell = true;
    }

    public void SwapCard(CellSlot slot)
    {
        slot.cardGO = this.gameObject;
        transform.position = slot.transform.position;
        transform.SetParent(slot.transform);
        cellSlot = slot;
        IsInCell = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var data = PointerRaycast(Input.mousePosition);

        if (data != null && !data.CompareTag(CELL_TAG))
        {
            ReturnCardToHand();
        }
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }

    public void ReturnCardToHand()
    {
        if (IsInCell)
        {
            IsInCell = false;
            //cellSlot.cardGO.Remove(gameObject);
            cellSlot.RemoveCard();
            cellSlot = null;
            CellSlot.OnCardDropFunc();
        }
        //transform.position = _startPosition;
        transform.SetParent(null);
        transform.SetParent(_startParent.transform);
        //transform.localPosition = new Vector3(0, 0, 0);
    }

    GameObject PointerRaycast(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> resultsData = new List<RaycastResult>();
        pointerData.position = position;
        EventSystem.current.RaycastAll(pointerData, resultsData);

        if (resultsData.Count > 0)
        {
            return resultsData[0].gameObject;
        }

        return null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ////Spawn new card under this when PointerDown

        //if (!IsInCell)
        //{
        //    Instantiate(gameObject, transform.parent);
        //}
    }
}

