using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject checkableProperty;
    [SerializeField] int table;
    [SerializeField] bool lastGoal;
    [SerializeField] GameObject discription;
    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] Goal nextGoal;

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
        ICheckable checkable = checkableProperty.GetComponent<ICheckable>();
        return checkable.CheckAnswer();
    }

    public void CheckGoal()
    {
        if(CheckProperty())
        {
            OnEndGoalEvent?.Invoke(lastGoal);

            //CheckImage = GreenCheckImage
            _checkImage.color = new Color(0.6f, 1f, 0.6f);
            goalText.color = new Color(0.5f, 0.5f, 0.5f);
            goalText.fontStyle = FontStyles.Superscript;

            discription.SetActive(false);

            _button.interactable = false;

            if(!lastGoal) nextGoal._button.interactable = true;
        }
    }

}
