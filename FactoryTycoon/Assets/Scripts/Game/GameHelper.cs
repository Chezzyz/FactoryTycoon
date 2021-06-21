using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    [SerializeField] GameObject textWindow;
    private GameObject _helperImageGO;
    private TextMeshProUGUI _helperText;
    private GameObject _nextButtonGO;
    private TextMeshProUGUI _nextButtonText => _nextButtonGO.transform.GetComponentInChildren<TextMeshProUGUI>();
    private IReadOnlyList<string> _currentHelperList;
    public int _helperListIndex = 0;
    private bool _waitForClose = false;

    private static GameHelper singleton;

    [SerializeField] List<TipMovement> tip;
    [SerializeField] List<int> waitIndexesList;
    private TipMovement _currentTip;

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
        textWindow.SetActive(true);
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

        //if (closeIndexesList.Contains(singleton._helperListIndex))
        //{
        //    CloseHelper();
        //    StopPreviousAnim();
        //    return;
        //}

        StopPreviousAnim();
        TryPause();

        if (singleton._helperListIndex == singleton._currentHelperList.Count - 1) //last text
        {
            singleton._nextButtonText.text = "Понятно";
            singleton._waitForClose = true;
        }
    }

    private void TryPause()
    {
        //Если на этой фразе нужно подождать
        if (waitIndexesList.Contains(singleton._helperListIndex))
        {
            // Выкл кнопки "далее", следующая подсказка будет вызвана из КАКОГО-ТО другого места
            singleton._nextButtonGO.SetActive(false);

            //Порядковый номер элемента паузы в списке, чтобы включить нужную анимацию
            var index = waitIndexesList.IndexOf(singleton._helperListIndex);

            if (tip[index])
            {
                tip[index].StartTipAnimation();
                _currentTip = tip[index];
            }
        }
        else
        {
            singleton._nextButtonGO.SetActive(true);
        }
    }

    private void StopPreviousAnim()
    {
        if (_currentTip)
        {
            _currentTip.StopAnimation();
        }
    }

    private void CloseHelper()
    {
        singleton.textWindow.SetActive(false);
        singleton._helperText.enabled = false;
        singleton._helperImageGO.SetActive(false);
        singleton._nextButtonGO.SetActive(false);
        singleton._currentHelperList = null;
        singleton._waitForClose = false;
        singleton._helperListIndex = 0;
    }
}
