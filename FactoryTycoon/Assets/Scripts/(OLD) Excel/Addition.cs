public class Addition : IOperation
{
    private readonly float leftOperand;
    private readonly float rightOperand;
    private float result;

    public Addition(float left, float right)
    {
        leftOperand = left;
        rightOperand = right;

        result = left + right;
    }
    
    public Addition(IOperation left, float right) : this(left.Result, right)
    {
    }
    
    public float Result => result;
}