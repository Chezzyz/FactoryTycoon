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
            
        SetHelper();
    }

    void Start()
    {
    }

    private static void SetHelper()
    {
        GameHelper.SetHelperList(HelperTexts.allTexts[GameState.singleton._currentLevel]);
        GameHelper.GetSingleton().ShowHelper();
    }
}
