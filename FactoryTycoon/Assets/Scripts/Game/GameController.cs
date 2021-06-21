using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController singleton;
    public static GameState gameState;

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

    public static void SetHelper()
    {
        GameHelper.SetHelperList(HelperTexts.allTexts[GameState.singleton._currentLevel]);
        GameHelper.GetSingleton().ShowHelper();
    }
}
