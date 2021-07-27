using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchThreeController : MonoBehaviour, ICheckable
{
    [SerializeField] int fieldSize = 5;
    [SerializeField] int CollectableCount = 3;
    [SerializeField] GridController GridController;
    [SerializeField] TextMeshProUGUI collectablesCountText;
    //InputService InputService => GetComponent<InputService>();

    public static int collectableCount;
    public static GridController gridController;
    public static AnimationController animationController;

    void OnEnable()
    {
        gridController = GridController;
        gridController.InitService(fieldSize);

        gridController.CreateGrid();
        gridController.FillGrid();

        collectableCount = CollectableCount;
        CollectableController.SpawnControllers(collectableCount);
        CollectableController.FillGridWithCollectable(collectableCount);

        collectablesCountText.text = string.Format("Осталось собрать:\n {0}", collectableCount);

        animationController = GetComponent<AnimationController>();
        animationController.InitService();
    }

    public bool CheckAnswer()
    {
        if(collectableCount == 0)
        {
            Invoke(nameof(DestroyPrefab),2f);
            return true;
        }
        return false; 
    }

    private void DestroyPrefab()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void Collect()
    {
        collectableCount -= 1;
        collectablesCountText.text = string.Format("Осталось собрать:\n {0}", collectableCount);
    }
}
