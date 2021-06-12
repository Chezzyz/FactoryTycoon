using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public interface IMatchThreeItem
{
    void Swap(IMatchThreeItem otherItem);

    SlotController GetSlot();

    GameObject GetGameObject();

    void FallDown();

    void SelfDestroy();
}

