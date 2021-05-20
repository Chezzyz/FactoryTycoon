using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public static List<CardData> allCards;
    public DragAndDrop _dragController => GetComponent<DragAndDrop>();
    public string _name;
    private bool isAchieved;
}
