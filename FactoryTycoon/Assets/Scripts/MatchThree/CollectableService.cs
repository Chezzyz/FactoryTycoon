using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableService
{
    private CollectableController _collectableController;

    public List<SlotController> _slotControllers { get; private set; }

    private static List<CollectableController> collectables = new List<CollectableController>();

    private static Color collectableColor = new Color(0.45f, 0.8f, 0.45f);

    private static Color defaultSlotColor = new Color(0.1886792f, 0.1886792f, 0.1886792f);

    private static int _currentDelta;

    private static Vector2Int[] _deltas = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    private int fieldSize => MatchThreeController.gridController.fieldSize;

    private GridController GridController => MatchThreeController.gridController;

    public CollectableService(CollectableController controller)
    {
        _collectableController = controller;
        _slotControllers = new List<SlotController>();
        GridService.OnDestroyLinesEvent += TryToCollect;
    }

    private void TryToCollect(HashSet<SlotController> slotControllers)
    {
        if (slotControllers.Contains(_slotControllers[0]) &&
            slotControllers.Contains(_slotControllers[1]))
        {
            Collect();
        }
    }

    private void Collect()
    {
        _slotControllers[0].slotImage.color = defaultSlotColor;
        _slotControllers[1].slotImage.color = defaultSlotColor;

        MatchThreeController.collectableCount -= 1;

        CreateVFX();

        GridService.OnDestroyLinesEvent -= TryToCollect;
        //MatchThreeController.CheckWin();
    }

    //Лучше бы в какой-нибудь view или animation скрипт перенести
    private void CreateVFX()
    {
        var collectEffectPref = Resources.Load<ParticleSystem>("CollectVFX");
        var effect1 = MonoBehaviour.Instantiate(collectEffectPref, _slotControllers[0].transform);
        effect1.transform.localPosition = new Vector3(0, 0, 0);
        var effect2 = MonoBehaviour.Instantiate(collectEffectPref, _slotControllers[1].transform);
        effect2.transform.localPosition = new Vector3(0, 0, 0);
    }

    public static void FillGridWithCollectable(int count)
    {
        foreach (var controller in CollectableController.collectableControllers)
        {
            controller.SpawnCollectable();
        }
    }

    public void SpawnCollectable()
    {
        var firstSlotX = UnityEngine.Random.Range(0, fieldSize);
        var firstSlotY = UnityEngine.Random.Range(0, fieldSize);
        _currentDelta = (_currentDelta + 1) % 4;

        if (!IsEmptySlot(firstSlotX, firstSlotY))
        {
            SpawnCollectable();
            return;
        }

        var secondSlotX = firstSlotX + _deltas[_currentDelta].x;
        var secondSlotY = firstSlotY + _deltas[_currentDelta].y;

        if (!GridController.HaveSlotAt(secondSlotX, secondSlotY) ||
            !IsEmptySlot(secondSlotX, secondSlotY))
        {
            SpawnCollectable();
            return;
        }

        var firstSlot = GridController.GetSlotByPosition(firstSlotX, firstSlotY);
        var secondSlot = GridController.GetSlotByPosition(secondSlotX, secondSlotY);
        _slotControllers.Add(firstSlot);
        _slotControllers.Add(secondSlot);

        firstSlot.slotImage.color = collectableColor;
        secondSlot.slotImage.color = collectableColor;

        collectables.Add(_collectableController);
    }

    private static bool IsEmptySlot(int x, int y)
    {
        bool isEmptySlot = true;

        foreach (var collectable in collectables)
        {
            //collectable.slotControllers.Select(slot => isEmptySlot = (slot.posX == x && slot.posY == y) ?  false : true);

            foreach (var slot in collectable.slotControllers)
            {
                if (slot.posX == x && slot.posY == y)
                {
                    isEmptySlot = false;
                }
            }


            if (!isEmptySlot)
            {
                return false;
            }
        }

        return true;
    }
    public void SelfDestroy()
    {
        //MonoBehaviour.Destroy(_collectableController);
    }
}
