using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAnimation : MonoBehaviour
{
    public void OnEnable()
    {
        GetComponent<Animator>().SetTrigger("Awake");
    }
}
