﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    public int posX => _slotService.posX;
    public int posY => _slotService.posY;

    private SlotService _slotService;

    public IMatchThreeItem TrashController => _slotService.currentItemController;
    private GridController _gridController => MatchThreeController.gridController;

    public void InitService(int x, int y)
    {
        _slotService = new SlotService(x, y, this);
    }

    public void SetItemController(IMatchThreeItem itemController) => _slotService.SetItemController(itemController);
    
}
