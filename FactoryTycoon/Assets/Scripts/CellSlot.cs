﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CellSlot : MonoBehaviour, IDropHandler
{
    public GameObject cardGO;
    //public GameObject rightFill;
    public bool rightCell = false;

    public void OnDrop(PointerEventData eventData)
    {
        DragAndDrop card;
        if (eventData.pointerDrag != null) //Если слот не содержит эту карту
        {
            card = eventData.pointerDrag.GetComponent<DragAndDrop>();
            card.PutCardOnSlot(this);
            cardGO = eventData.pointerDrag;
        }
        else
        {
            card = eventData.pointerDrag.GetComponent<DragAndDrop>();
            card.ReturnCardToHand();
            //items.Remove(eventData.pointerDrag);
            //Destroy(eventData.pointerDrag);

        }
    }

    //Переписать кринж
    #region Cringe
    //public void SetCorrectPositions()
    //{
    //    var rectTransform = GetComponent<RectTransform>();
    //    switch (cardGO.Count)
    //    {
    //        case 1:
    //            cardGO[0].transform.localPosition = new Vector3(0, 0, 0);
    //            break;
    //        case 2:
    //            cardGO[0].transform.localPosition = new Vector3(-rectTransform.rect.width / 4,
    //                rectTransform.rect.height / 4, 0);
    //            cardGO[1].transform.localPosition = new Vector3(rectTransform.rect.width / 4,
    //                -rectTransform.rect.height / 4, 0);
    //            break;
    //        case 3:
    //            cardGO[0].transform.localPosition = new Vector3(-rectTransform.rect.width / 4,
    //                rectTransform.rect.height / 4, 0);
    //            cardGO[1].transform.localPosition = new Vector3(rectTransform.rect.width / 4,
    //                rectTransform.rect.height / 4, 0);
    //            cardGO[2].transform.localPosition = new Vector3(0,
    //                -rectTransform.rect.height / 4, 0);
    //            break;
    //        case 4:
    //            cardGO[0].transform.localPosition = new Vector3(-rectTransform.rect.width / 4,
    //                rectTransform.rect.height / 4, 0);
    //            cardGO[1].transform.localPosition = new Vector3(rectTransform.rect.width / 4,
    //                rectTransform.rect.height / 4, 0);
    //            cardGO[2].transform.localPosition = new Vector3(-rectTransform.rect.width / 4,
    //                -rectTransform.rect.height / 4, 0);
    //            cardGO[3].transform.localPosition = new Vector3(rectTransform.rect.width / 4,
    //                -rectTransform.rect.height / 4, 0);
    //            break;
    //        default:
    //            break;
    //    }
    //}
    #endregion
}
