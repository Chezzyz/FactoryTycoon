using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CellController : Button
{
    //private Button button;
    //private Action<string> link = Link => {
    //    string.Join("", GetComponentInParent<Table>().gameObject.name, GetComponentInParent<VerticalLayoutGroup>().gameObject.name, );
    //}

    public override void OnPointerEnter(PointerEventData eventData)
    {

        if (MouseProperties.isLeftButtonDown)
            base.OnSelect(eventData);


        //base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        //base.OnPointerExit(eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        //base.OnMove(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }


}
