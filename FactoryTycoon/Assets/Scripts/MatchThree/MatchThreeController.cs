using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchThreeController : MonoBehaviour
{
    [SerializeField] int fieldSize = 5;
    [SerializeField] GridController GridController;
    
    //InputService InputService => GetComponent<InputService>();

    public static GridController gridController;
    public static AnimationController animationController;
    void Start()
    {
        gridController = GridController;
        gridController.InitService(fieldSize);

        gridController.CreateGrid();
        gridController.FillGrid();

        animationController = GetComponent<AnimationController>();
        animationController.InitService();
    }
}
