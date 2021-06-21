using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Discription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject discriptionWindow;

    private bool isOnCard = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        discriptionWindow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        discriptionWindow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        discriptionWindow.SetActive(false);
    }
}
