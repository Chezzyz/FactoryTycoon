using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Globalization;

//Ячейка только для чтения. Если нужно изменить значение ячейки ячейку нужно пересоздать


public class Cell : MonoBehaviour 
{
    public void Start()
    {
        //var example1 = new Formula(6).Plus(10).Divide(20).Multiply(new Formula(15).Plus(15)).Divide(15);
        //print($"The result of example1 is {example1.Calculate()}");

        //var example2 = new Formula().Sum(new float[] {1, 2, 3, 4, 5}); //return float
        //print($"The result of example2 is {example2}");

        //var example3 = new Formula(Formula.Sum(new float[] {1, 2, 3, 4})) // (1+2+3+4)*(5+6+7+8+9+10)
        //            .Multiply(Formula.Sum(new float[] {5, 6, 7, 8, 9, 10}));
        //print($"The result of example2 is {example3}");
    }

    public CellType CellType { get; set; }
    public Formula Formula { get; set; }
    public float? Value { get; private set; }
    public void SetValue(float? value) => Value = value;

    public void SetValue(InputField inputField)
    {
        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        float value;
        float.TryParse(inputField.text, NumberStyles.Any, ci, out value);
        Value = value;
        print(Value);
    }
    public Cell(Formula formula)
    {
        CellType = CellType.Formula;
        Formula = formula;
    }
    
    public Cell(float value)
    {
        CellType = CellType.Value;
        Value = value;
    }
}

public enum CellType
{
    Formula,
    Value
}
