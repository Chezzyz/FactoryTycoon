public class Subtraction : IOperation
{
    private readonly float leftOperand;
    private readonly float rightOperand;
    private float result;

    public Subtraction(float left, float right)
    {
        leftOperand = left;
        rightOperand = right;

        result = left - right;
    }

    public Subtraction(IOperation left, float right) : this(left.Result, right)
    {
    }

    public float Result => result;
}