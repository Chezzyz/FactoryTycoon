using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotService 
{
    private SlotController slotController;
    public int posX { get; private set; }
    public int posY { get; private set; }

    public IMatchThreeItem currentItemController { get; private set; }

    public SlotService(int x, int y, SlotController controller)
    {
        posX = x;
        posY = y;
        slotController = controller;
    }

    public void SetItemController(IMatchThreeItem controller)
    {
        currentItemController = controller;
        //change transform of item
        var itemTransform = controller.GetGameObject().transform;
        itemTransform.SetParent(slotController.transform);
        itemTransform.localPosition = Vector3.zero;
    }
}
