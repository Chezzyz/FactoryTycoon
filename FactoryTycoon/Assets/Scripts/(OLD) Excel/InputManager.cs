﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            MouseProperties.isLeftButtonDown = true;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            MouseProperties.isLeftButtonDown = false;
    }
}