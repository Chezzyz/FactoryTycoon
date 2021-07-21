using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableService
{
    public readonly List<SlotController> SlotControllers;

    private readonly CollectableController _collectableController;

    private static readonly List<CollectableController> s_collectables = new List<CollectableController>();

    private static Color _collectableColor = new Color(0.5965201f, 0.990566f, 0.5934051f);

    private static Color _defaultSlotColor = new Color(0.1886792f, 0.1886792f, 0.1886792f);

    private static int s_currentDelta;

    private static readonly Vector2Int[] _deltas = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    private int FieldSize => MatchThreeController.gridController.FieldSize;

    private GridController GridController => MatchThreeController.gridController;

    public CollectableService(CollectableController controller)
    {
        _collectableController = controller;
        SlotControllers = new List<SlotController>();
        GridService.OnDestroyLinesEvent += TryToCollect;
    }

    private void TryToCollect(HashSet<SlotController> slotControllers)
    {
        if (slotControllers.Contains(SlotControllers[0]) &&
            slotControllers.Contains(SlotControllers[1]))
        {
            Collect();
        }
    }

    private void Collect()
    {
        SlotControllers[0].SlotImage.color = _defaultSlotColor;
        SlotControllers[1].SlotImage.color = _defaultSlotColor;

        MonoBehaviour.FindObjectOfType<MatchThreeController>().Collect(); //Подумать

        CreateVFX();

        GridService.OnDestroyLinesEvent -= TryToCollect;
        //MatchThreeController.CheckWin();
    }

    //Лучше бы в какой-нибудь view или animation скрипт перенести
    private void CreateVFX()
    {
        var collectEffectPref = Resources.Load<ParticleSystem>("CollectVFX");
        var effect1 = MonoBehaviour.Instantiate(collectEffectPref, SlotControllers[0].transform);
        effect1.transform.localPosition = new Vector3(0, 0, 0);
        var effect2 = MonoBehaviour.Instantiate(collectEffectPref, SlotControllers[1].transform);
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
        var firstSlotX = UnityEngine.Random.Range(0, FieldSize);
        var firstSlotY = UnityEngine.Random.Range(0, FieldSize);
        s_currentDelta = (s_currentDelta + 1) % 4;

        if (!IsEmptySlot(firstSlotX, firstSlotY))
        {
            SpawnCollectable();
            return;
        }

        var secondSlotX = firstSlotX + _deltas[s_currentDelta].x;
        var secondSlotY = firstSlotY + _deltas[s_currentDelta].y;

        if (!GridController.HaveSlotAt(secondSlotX, secondSlotY) ||
            !IsEmptySlot(secondSlotX, secondSlotY))
        {
            SpawnCollectable();
            return;
        }

        var firstSlot = GridController.GetSlotByPosition(firstSlotX, firstSlotY);
        var secondSlot = GridController.GetSlotByPosition(secondSlotX, secondSlotY);
        SlotControllers.Add(firstSlot);
        SlotControllers.Add(secondSlot);

        firstSlot.SlotImage.color = _collectableColor;
        secondSlot.SlotImage.color = _collectableColor;

        s_collectables.Add(_collectableController);
    }

    private static bool IsEmptySlot(int x, int y)
    {
        bool isEmptySlot = true;

        foreach (var collectable in s_collectables)
        {
            //collectable.slotControllers.Select(slot => isEmptySlot = (slot.posX == x && slot.posY == y) ?  false : true);

            foreach (var slot in collectable.SlotControllers)
            {
                if (slot.PosX == x && slot.PosY == y)
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
