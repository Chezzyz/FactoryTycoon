using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject checkableProperty;
    [SerializeField] int table;
    [SerializeField] bool lastGoal;
    private Image _checkImage;

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
            //CheckImage = GreenCheckImage
            _checkImage.color = new Color(0.6f, 1f, 0.6f);
            GameState.singleton.IncrementGoalNumber();

            if (lastGoal)  //change to new table goals
            {            
                GameState.singleton.IncrementTableNumber();
                GameState.singleton.SetGoalNumber(1);
            }
        }
    }

}
