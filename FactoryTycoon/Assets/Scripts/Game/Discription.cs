using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Discription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject discriptionWindow;

    private bool isOnCard = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        discriptionWindow.SetActive(true);
        isOnCard = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        discriptionWindow.SetActive(false);
        isOnCard = false;
    }
    void Update()
    {
        if (isOnCard)
        {
            discriptionWindow.transform.position = Input.mousePosition;
        }
    }
}
