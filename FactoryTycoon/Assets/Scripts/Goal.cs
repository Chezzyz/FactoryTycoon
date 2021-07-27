using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Goal : MonoBehaviour
{
    [SerializeField] List<GameObject> CheckableObjects;
    [SerializeField] int Table;
    [SerializeField] bool IsLastGoal;
    [SerializeField] GameObject Discription;
    [SerializeField] TextMeshProUGUI GoalText;
    [SerializeField] Goal NextGoal;
    [SerializeField] GameObject CurrentScreen;
    [SerializeField] GameObject NextScreen;
    [SerializeField] GameObject WinWindow;

    private Image _checkImage;
    private Button _button => GetComponent<Button>();
    public delegate void OnEndGoal(bool last);
    public static event OnEndGoal OnEndGoalEvent;

    private void Start()
    {
        _checkImage = GetComponentsInChildren<Image>()[1];   //second after background
    }

    private bool CheckProperty()
    {
        foreach (var checkableObject in CheckableObjects)
        { 
            ICheckable checkable = checkableObject.GetComponent<ICheckable>();
            if (!checkable.CheckAnswer())
            {
                return false;
            }
        }
        return true;
    }

    public void CheckGoal()
    {
        if(CheckProperty())
        {
            //CheckImage = GreenCheckImage
            _checkImage.color = new Color(0.6f, 1f, 0.6f);
            GoalText.color = new Color(0.5f, 0.5f, 0.5f);
            GoalText.fontStyle = FontStyles.Superscript;
            WinWindow.SetActive(true);
        }
    }

    public void ToNextGoal()
    {
        OnEndGoalEvent?.Invoke(IsLastGoal);

        Discription.SetActive(false);
        NextGoal.Discription.SetActive(true);

        _button.interactable = false;
        if (!IsLastGoal) NextGoal._button.interactable = true;

        ChangeScreenStates();
    }

    private void ChangeScreenStates()
    {
        var screenAnimator = CurrentScreen.GetComponent<Animator>();

        if(screenAnimator)
        {
            screenAnimator.SetTrigger("Close");
        }

        DisableCurrentScreen();
        NextScreen.SetActive(true);

        screenAnimator = NextScreen.GetComponent<Animator>();

        if (screenAnimator)
        {
            screenAnimator.SetTrigger("Open");
        }
    }

    private void DisableCurrentScreen()
    {
        CurrentScreen.SetActive(false);
    }
}
