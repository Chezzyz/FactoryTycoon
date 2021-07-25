using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public static List<CardData> s_AllCards;
    public DragAndDrop DragController => GetComponent<DragAndDrop>();
    public string Name => gameObject.name;
}
