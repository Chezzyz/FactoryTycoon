using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputService : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            MouseProperties.isLeftButtonDown = true;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            MouseProperties.isLeftButtonDown = false;
    }
}
