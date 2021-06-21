using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RatioController : MonoBehaviour, ICheckable
{
    [SerializeField] TextMeshProUGUI ratio1;
    [SerializeField] TextMeshProUGUI ratio3;
    [SerializeField] TextMeshProUGUI machines1;
    [SerializeField] TextMeshProUGUI machines3;
    [SerializeField] Image ratioBG1;
    [SerializeField] Image ratioBG3;
    private bool button1Clicked1;
    private bool button1Clicked3;

    public void Plus()
    {
        ratio3.text = "0.84";
        machines3.text = "N + 1";
        ratioBG3.color = new Color(0.6f, 0.9f, 0.55f);
        button1Clicked1 = true;
    }

    public void Minus()
    {
        ratio1.text = "0.81";
        machines1.text = "N - 1";
        ratioBG1.color = new Color(0.6f, 0.9f, 0.55f);
        button1Clicked3 = true;
    }

    public bool CheckAnswer()
    {
        return button1Clicked1 && button1Clicked3;
    }
}
