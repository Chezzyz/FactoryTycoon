using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchThreeCollectable
{
    GameObject GetGameObject();
    void SelfDestroy();
}
