using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Goal : MonoBehaviour
{
    [SerializeField] List<GameObject> checkableObjects;
    [SerializeField] int table;
    [SerializeField] bool lastGoal;
    [SerializeField] GameObject discription;
    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] Goal nextGoal;
    [SerializeField] List<GameObject> currentScreens;
    [SerializeField] List<GameObject> nextScreens;
    [SerializeField] GameObject winWindow;

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
        foreach (var checkableObject in checkableObjects)
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
            goalText.color = new Color(0.5f, 0.5f, 0.5f);
            goalText.fontStyle = FontStyles.Superscript;
            winWindow.SetActive(true);
        }
    }

    public void ToNextGoal()
    {
        OnEndGoalEvent?.Invoke(lastGoal);

        discription.SetActive(false);
        nextGoal.discription.SetActive(true);

        _button.interactable = false;
        if (!lastGoal) nextGoal._button.interactable = true;

        ChangeScreenStates();
    }

    private void ChangeScreenStates()
    {
        foreach (var screen in currentScreens)
        {
            var screenAnimator = screen.GetComponent<Animator>();

            if(screenAnimator)
            {
                screenAnimator.SetTrigger("Close");
            }
        }

        foreach (var screen in nextScreens)
        {
            screen.SetActive(true);
            var screenAnimator = screen.GetComponent<Animator>();

            if (screenAnimator)
            {
                screenAnimator.SetTrigger("Open");
            }
        }
    }
}
