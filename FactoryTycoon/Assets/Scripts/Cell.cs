using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Cell : MonoBehaviour
{
    public float? value;
    Formula formula;
    public string id;
}

public class Formula
{
    public float? result;

    List<float> coefs = new List<float>();
    List<string> cells = new List<string>();
    List<Operation> operations = new List<Operation>();

    //public float GetResult()
    //{
    //    foreach(var operation in operations)
    //    {
    //        operation.doOperation
    //    }
    //}

    class Operation
    {
        enum Func
        {
            Sum,
            Multiply,
            Division,
        }



        private float Sum(List<Cell> cells)
        {
            var result = 0f;
            var values = cells.Select(cell => cell.value == null ? 0f : cell.value);
            foreach(var num in values)
            {
                result += (float) num;
            }
            return result;
        }

        private float Multiply(float num1, float num2) => num1 * num2;
        private float Division(float num1, float num2) => num1 / num2;
        //public float doOperation(Func<float,float,float> operation,  )
        //{

        //}

    }
}
