public class Multiplication : IOperation
{
    private readonly float leftOperand;
    private readonly float rightOperand;
    private float result;

    public Multiplication(float left, float right)
    {
        leftOperand = left;
        rightOperand = right;

        result = left * right;
    }

    public Multiplication(IOperation left, float right) : this(left.Result, right)
    {
    }

    public float Result => result;
}