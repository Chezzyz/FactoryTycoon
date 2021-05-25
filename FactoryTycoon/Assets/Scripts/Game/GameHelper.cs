using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    private GameObject _helperImageGO;
    private TextMeshProUGUI _helperText;
    private GameObject _nextButtonGO;
    private TextMeshProUGUI _nextButtonText => _nextButtonGO.transform.GetComponentInChildren<TextMeshProUGUI>();
    private IReadOnlyList<string> _currentHelperList;
    private int _helperListIndex = 0;
    private bool _waitForClose = false;

    private static GameHelper singleton;


    public static GameHelper GetSingleton()
    {
        if (!singleton || singleton._currentHelperList == null) throw new System.Exception("HelperList Can't be null, set a list to Helper");
        singleton._helperImageGO = singleton.transform.GetComponentInChildren<Image>(true).gameObject;
        singleton._nextButtonGO = singleton.transform.GetComponentInChildren<Button>(true).gameObject;
        singleton._helperText = singleton.transform.GetComponentInChildren<TextMeshProUGUI>(true);
        return singleton;
    }

    private static void CreateSingleton() 
    {
        if (!singleton) singleton = GameObject.Find("GameHelper").GetComponent<GameHelper>();
    }
    
    public static void SetHelperList(List<string> list)
    {
        CreateSingleton();
        singleton._currentHelperList = list;
    }

    public void ShowHelper()
    {
        singleton._helperText.enabled = true;
        singleton._helperText.text = singleton._currentHelperList[singleton._helperListIndex];
        singleton._helperImageGO.SetActive(true);
        singleton._nextButtonGO.SetActive(true);
    }

    public void NextTip()//after last text
    {
        if (_waitForClose)
        {
            CloseHelper();
            return;
        }

        singleton._helperListIndex += 1;
        singleton._helperText.text = _currentHelperList[_helperListIndex];

        if (singleton._helperListIndex == singleton._currentHelperList.Count - 1) //last text
        {
            singleton._nextButtonText.text = "Понятно";
            singleton._waitForClose = true;
        }
    }
    
    private void CloseHelper()
    {
        singleton._helperText.enabled = false;
        singleton._helperImageGO.SetActive(false);
        singleton._nextButtonGO.SetActive(false);
        singleton._currentHelperList = null;
        singleton._waitForClose = false;
        singleton._helperListIndex = 0;
    }
}
