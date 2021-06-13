using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public interface IMatchThreeItem
{
    void Swap(IMatchThreeItem otherItem, bool sendEvent = true);

    SlotController GetSlot();

    GameObject GetGameObject();

    string GetItemType();

    void FallDown(bool isLast = false);

    bool IsAbleToFall();

    void SelfDestroy();
}

