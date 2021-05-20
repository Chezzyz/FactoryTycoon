using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController singleton;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(singleton.gameObject);
        }
    }

    void Start()
    {
        GameHelper.SetHelperList(HelperTexts.level_1);
        GameHelper.GetSingleton().ShowHelper(); 
    }
}
