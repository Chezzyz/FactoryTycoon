using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : IMatchThreeItem
{
    GridController Grid => MatchThreeController.gridController;

    public void SelfDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void Swap(IMatchThreeItem otherItem)
    {
        Grid.Swap(this, otherItem);
    }
}
