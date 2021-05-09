using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CellController : Button
{
    private string link => 
        string.Join("", GetComponentInParent<Table>().gameObject.name, 
            GetComponentInParent<VerticalLayoutGroup>().gameObject.name,
            transform.GetSiblingIndex() + 1);

    public delegate void OnSelectExcel(string link, PointerEventData eventData);
    public static event OnSelectExcel DeselectSelected;
    public static bool isStretchMode;
    public static Cell startCell;
    private bool isSelected;
    private GameObject inputFieldGO => GetComponentInChildren<InputField>(true).gameObject;

    protected override void Start()
    {
        //GetComponentInChildren<Text>().text = link;
        DeselectSelected += DeselectExcel;
    }
    public void StretchModeOn()
    {
        isStretchMode = true;
        startCell = GetComponent<Cell>();
    }

    private void DeselectExcel(string link, PointerEventData eventData)
    {
        if (this.link != link)
        {
            isSelected = false;
            inputFieldGO.SetActive(false);
            base.OnDeselect(eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

        if (MouseProperties.isLeftButtonDown) //Mouse0 Holding
        {
            if (isStretchMode)
            {
                if (startCell.CellType == CellType.Value)
                {
                    GetComponent<Cell>().SetValue(startCell.Value);
                    inputFieldGO.GetComponent<InputField>().text = startCell.Value.ToString();
                }
            }
            base.OnSelect(eventData);
        }
        


        //base.OnPointerEnter(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        DeselectSelected?.Invoke(link, eventData);
        if (isSelected)
        {
            inputFieldGO.SetActive(true);
            inputFieldGO.GetComponent<InputField>().OnPointerDown(eventData);
        }
        else
        {
            isSelected = true;
        }
        base.OnPointerDown(eventData);
        
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
