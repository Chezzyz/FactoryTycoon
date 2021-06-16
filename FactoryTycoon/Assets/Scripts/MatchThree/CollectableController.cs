﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController
{
    private CollectableService _collectableService;
    public List<SlotController> slotControllers => _collectableService._slotControllers;
    public void SelfDestroy() => _collectableService.SelfDestroy();

    public static List<CollectableController> collectableControllers;

    public void SpawnCollectable() => _collectableService.SpawnCollectable();

    public static void FillGridWithCollectable(int count) => CollectableService.FillGridWithCollectable(count);

    public void InitService()
    {
        _collectableService = new CollectableService(this);
    }

    public static void SpawnControllers(int count)
    {
        collectableControllers = new List<CollectableController>();

        for (int i = 0; i < count; i++)
        {
            var controller = new CollectableController();
            collectableControllers.Add(controller);
            controller.InitService();
        }
    }
}
