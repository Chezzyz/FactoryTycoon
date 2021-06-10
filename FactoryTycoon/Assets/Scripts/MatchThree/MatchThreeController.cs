using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchThreeController : MonoBehaviour
{
    [SerializeField] int fieldSize = 5;

    public static GridController gridController;
    public static InputController inputController;
    void Start()
    {
        gridController = new GridController(fieldSize);
        gridController.CreateGrid();
        gridController.FillGrid();
        inputController = new InputController();
        inputController.InitService();
    }
}
