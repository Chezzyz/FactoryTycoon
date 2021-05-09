using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Rendering;

public class Formula
{
    private readonly List<IOperation> _operations;
    public string formulaText;
    public Formula(float value)
    {
        _operations = new List<IOperation>{new Addition(0, value)};
    }
    
    public Formula() : this(0){}

    private Formula(IEnumerable<IOperation> operations)
    {
        _operations = operations.ToList();
    }

    public override string ToString()
    {
        return formulaText;
    }

    public Formula Plus(float value)
    {
        return DoOperation(value, OperationType.Addiction);        
    }
    public Formula Minus(float value)
    {
        return DoOperation(value, OperationType.Subtraction);
    }
    public Formula Multiply(float value)
    {
        return DoOperation(value, OperationType.Multiplication);
    }
    public Formula Divide(float value)
    {
        return DoOperation(value, OperationType.Division);        
    }

    public  float Sum(IEnumerable<float> values)
    {
        return values.Sum();
    }

    #region Open to See Methods with Formula as argument
    
    public Formula Plus(Formula formula)
    {
        return DoOperation(formula.Calculate(), OperationType.Addiction);        
    }
    public Formula Minus(Formula formula)
    {
        return DoOperation(formula.Calculate(), OperationType.Subtraction);
    }
    public Formula Multiply(Formula formula)
    {
        return DoOperation(formula.Calculate(), OperationType.Multiplication);
    }
    public Formula Divide(Formula formula)
    {
        return DoOperation(formula.Calculate(), OperationType.Division);        
    }

    public float Sum(IEnumerable<Formula> formulas)
    {
        return Sum(formulas.Select(formula => formula.Calculate()));
    }
    

    #endregion
    

    public float Calculate()
    {
        return _operations.Last().Result;
    }

    private Formula DoOperation(float value, OperationType type)
    {
        var newOperationsList =  new List<IOperation>(_operations);
        IOperation operationToAdd;

        switch (type)
        {
            case OperationType.Addiction:
                operationToAdd = new Addition(_operations.Last(), value);
                break;
            case OperationType.Subtraction:
                operationToAdd = new Subtraction(_operations.Last(), value);
                break;
            case OperationType.Multiplication:
                operationToAdd = new Multiplication(_operations.Last(), value);
                break;
            case OperationType.Division:
                operationToAdd = new Division(_operations.Last(), value);
                break;
            default:
                throw new ArgumentException("Invalid operation");

        }

        newOperationsList.Add(operationToAdd);
        return new Formula(newOperationsList);
    }
    
    
    private enum OperationType
    {
        Addiction,
        Subtraction,
        Multiplication,
        Division, 
    }
}