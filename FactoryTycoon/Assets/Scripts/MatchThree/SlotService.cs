using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotService 
{
    private SlotController slotController;
    public int posX { get; private set; }
    public int posY { get; private set; }

    public SlotService(int x, int y)
    {
        posX = x;
        posY = y;
    }
}
