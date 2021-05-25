public class Division : IOperation
{
    private readonly float leftOperand;
    private readonly float rightOperand;
    private float result;

    public Division(float left, float right)
    {
        leftOperand = left;
        rightOperand = right;

        result = left / right;
    }

    public Division(IOperation left, float right) : this(left.Result, right)
    {
    }

    public float Result => result;
}