using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    public int posX => _slotService.posX;
    public int posY => _slotService.posY;

    private SlotService _slotService;

    public Image slotImage;
    public IMatchThreeItem TrashController => _slotService.currentItemController;
    private GridController _gridController => MatchThreeController.gridController;

    public void InitService(int x, int y)
    {
        _slotService = new SlotService(x, y, this);
    }

    public void SetItemController(IMatchThreeItem itemController, bool deletePrevSlot = true) => _slotService.SetItemController(itemController, deletePrevSlot);

    public bool IsNeighborWith(int x, int y) => _slotService.IsNeighborFor(x, y);

    public bool IsSameTypeNeighborWith(int x, int y) => _slotService.IsSameTypeNeighborWith(x, y);

    public List<SlotController> GetSameTypeNeighbors() => _slotService.GetSameTypeNeighbors();

    public int GetNeighborsCountOfType(string type) => _slotService.GetNeighborsCountOfType(type);

    public bool IsAbleToMatchBetweenSlots() => _slotService.IsAbleToMatchBetweenSlots();

    public override string ToString()
    {
        return $"[{posY},{posX}] {TrashController.GetItemType()}";
    }

}
