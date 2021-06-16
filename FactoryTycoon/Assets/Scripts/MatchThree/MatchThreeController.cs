using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchThreeController : MonoBehaviour
{
    [SerializeField] int fieldSize = 5;
    [SerializeField] int CollectableCount = 3;
    [SerializeField] GridController GridController;

    //InputService InputService => GetComponent<InputService>();

    public static int collectableCount;
    public static GridController gridController;
    public static AnimationController animationController;
    void Start()
    {
        gridController = GridController;
        gridController.InitService(fieldSize);

        gridController.CreateGrid();
        gridController.FillGrid();

        collectableCount = CollectableCount;
        CollectableController.SpawnControllers(collectableCount);
        CollectableController.FillGridWithCollectable(collectableCount);

        animationController = GetComponent<AnimationController>();
        animationController.InitService();
    }
}
