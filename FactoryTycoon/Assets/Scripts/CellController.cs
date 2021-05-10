using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CellController : Button
{
    private string link => 
        string.Join("", GetComponentInParent<Table>().gameObject.name, ":", 
            GetComponentInParent<VerticalLayoutGroup>().gameObject.name,
            transform.GetSiblingIndex() + 1);

    public char column => link[link.Length - 2];
    public char row => link[link.Length - 1];

    public static GameObject startSelectGO;
    public static CellController startSelect => startSelectGO?.GetComponent<CellController>();

    public delegate void OnSelectExcel(string link, PointerEventData eventData);
    public static event OnSelectExcel DeselectSelected;
    public static List<string> selectedList = new List<string>();
    public static Dictionary<string,CellController> controllerDict = new Dictionary<string, CellController>();
    private bool isSelected;
    private GameObject inputFieldGO => GetComponentInChildren<InputField>(true).gameObject;
    private GameObject stretchButtonGO => GetComponentInChildren<StretchController>(true).gameObject;

    protected override void Start()
    {
        //GetComponentInChildren<Text>().text = link;
        controllerDict.Add(link, this);
        DeselectSelected += DeselectExcel;
    }
    public static CellController GetCellController(string link) => controllerDict[link];
    

    private void DeselectExcel(string link, PointerEventData eventData)
    {
        if (this.link != link)
        {
            selectedList.Remove(this.link);
            isSelected = false;
            inputFieldGO.SetActive(false);
            stretchButtonGO.SetActive(false);
            base.OnDeselect(eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

        if (MouseProperties.isLeftButtonDown) //Mouse0 Holding
        {
            if (startSelect.column == column || startSelect.row == row)
            {
                if (StretchController.isStretchMode)
                {
                    if (StretchController.startCell.CellType == CellType.Value)
                    {
                        GetComponent<Cell>().SetValue(StretchController.startCell.Value);
                        inputFieldGO.GetComponent<InputField>().text = StretchController.startCell.Value.ToString();
                    }
                }
                base.OnSelect(eventData);
                isSelected = true;
                selectedList.Add(link);
            }
        }
        //base.OnPointerEnter(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        DeselectSelected?.Invoke(link, eventData);
        stretchButtonGO.SetActive(true);
        startSelectGO = gameObject;
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

    public override void OnPointerUp(PointerEventData eventData)
    {
        StretchController.isStretchMode = false;
        StretchController.startCellGO?.GetComponentInChildren<StretchController>()?.OnDeselect(eventData); // pizda
        base.OnPointerUp(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (MouseProperties.isLeftButtonDown)
        { 
            
        }
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
