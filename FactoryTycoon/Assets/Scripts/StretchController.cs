using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StretchController : Button
{
    public static bool isStretchMode;
    public static GameObject startCellGO;
    public static Cell startCell => startCellGO.GetComponent<Cell>();

    public override void OnPointerDown(PointerEventData eventData)
    {
        isStretchMode = true;
        startCellGO = GetComponentInParent<Cell>().gameObject;
        base.OnPointerDown(eventData);
    }

}
