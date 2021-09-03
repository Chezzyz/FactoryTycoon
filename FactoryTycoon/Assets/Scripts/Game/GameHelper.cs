using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class GameHelper : MonoBehaviour
{
    [SerializeField] private Image ButtonBlock;
    [SerializeField] private List<TipMovement> Tips;
    [SerializeField] private List<int> WaitIndexesList;

    private TextMeshProUGUI _helperText;
    private GameObject _nextButtonGO;
    private TextMeshProUGUI _nextButtonText => _nextButtonGO.transform.GetComponentInChildren<TextMeshProUGUI>();
    private IReadOnlyList<string> _currentHelperList;
    private int _helperListIndex = 0;
    private bool _readyToClose = false;
    
    private Animator _animator;

    private static GameHelper Singleton;

    private TipMovement _currentTip;

    private void Start()
    {
        _nextButtonGO = transform.GetComponentInChildren<Button>(true).gameObject;
        _helperText = transform.GetComponentInChildren<TextMeshProUGUI>(true);
        _animator = GetComponent<Animator>();

        int table = GameState.Singleton.GetTable();

        if (HelperTexts.textsDict.ContainsKey(table))
        {
            TryShowHelper(table);
        }
    }

    private void TryShowHelper(int table)
    {
        if (!IsCurrentStageTipsCompleted())
        {
            SetHelperList(HelperTexts.textsDict[table]);
            ShowHelper();
        }
    }
    
    public void SetHelperList(List<string> list)
    {
        _currentHelperList = list;
    }

    public void ShowHelper()
    {
        _animator.SetTrigger("Awake");
        _helperText.enabled = true;
        _helperText.text = _currentHelperList[_helperListIndex];
        ButtonBlock.enabled = true;
        _nextButtonGO.SetActive(true);
    }

    public void NextTip()
    {
        if (_readyToClose || IsCurrentStageTipsCompleted())
        {
            CloseHelper();
            return;
        }

        _helperListIndex += 1;
        _helperText.text = _currentHelperList[_helperListIndex];

        StopPreviousAnim();

        TryPause();

        //Если последняя подсказка
        if (_helperListIndex == _currentHelperList.Count - 1) 
        {
            //Так как последней кнопки может не быть, считаем что подсказки закончились если показана последняя
            GameState.Singleton.CompleteTipsOfStage(SceneManager.GetActiveScene().buildIndex);
            _nextButtonText.text = "Понятно";
            _readyToClose = true;
        }
    }

    private void TryPause()
    {
        //Если на этой фразе нужно подождать
        if (WaitIndexesList.Contains(_helperListIndex))
        {
            // Выкл кнопки "далее", следующая подсказка будет вызвана из КАКОГО-ТО другого места
            _nextButtonGO.SetActive(false);

            //Порядковый номер элемента паузы в списке, чтобы включить нужную анимацию
            var index = WaitIndexesList.IndexOf(_helperListIndex);

            if (Tips[index])
            {
                Tips[index].StartTipAnimation();
                _currentTip = Tips[index];
            }

            ButtonBlock.enabled = false;
        }
        else
        {
            _nextButtonGO.SetActive(true);
            ButtonBlock.enabled = true;
        }
    }

    //Выключение анимации текущего пальца
    private void StopPreviousAnim()
    {
        if (_currentTip)
        {
            _currentTip.StopAnimation();
        }
    }

    private void CloseHelper()
    {
        _helperText.enabled = false;
        _nextButtonGO.SetActive(false);
        _currentHelperList = null;
        _readyToClose = false;
        _helperListIndex = 0;
        Destroy(this.gameObject);
    }

    private bool IsCurrentStageTipsCompleted() => 
        GameState.Singleton.IsStageTipsCompleted(GameState.Singleton.GetNameByBuildIndex(SceneManager.GetActiveScene().buildIndex));
}
