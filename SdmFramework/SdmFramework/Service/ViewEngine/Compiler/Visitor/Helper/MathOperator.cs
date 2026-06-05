namespace SdmFramework.Resource.Visitor.Helper;

public class MathOperator
{
    public static object? Add(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l + r;

        if (left is float lf && right is float rf)
            return lf + rf;

        if (left is float lFloat && right is int rInt)
            return lFloat + rInt;
        
        if (left is int lInt && right is float rFloat)
            return lInt + rFloat;

        throw new Exception($"Can not add these values");
    }
    
    public static object? Subtract(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l - r;

        if (left is float lf && right is float rf)
            return lf - rf;

        if (left is float lFloat && right is int rInt)
            return lFloat - rInt;
        
        if (left is int lInt && right is float rFloat)
            return lInt - rFloat;

        throw new Exception($"Can not add these values");
    }
    
    public static object? Multiplication(object? left, object? right)
    {
        if (left is int l && right is float r)
            return l * r;

        if (left is float lf && right is int ri)
            return lf * ri;

        if (left is float lFloat && right is float rFloat)
            return lFloat * rFloat;

        if (left is int lInt && right is int rInt)
            return lInt * rInt;

        throw new Exception($"Cannot multiply these values");
    }
    
    public static object? Division(object? left, object? right)
    {
        if (right is null || (right is int rightInt && rightInt == 0) || (right is float rightFloat && rightFloat == 0))
        {
            throw new Exception("Division by zero is not allowed");
        }

        if (left is int l && right is float r)
            return l / r;

        if (left is float lf && right is int ri)
            return lf / ri;

        if (left is float lFloat && right is float rFloat)
            return lFloat / rFloat;

        if (left is int lInt && right is int rInt)
            return lInt / rInt;

        throw new Exception($"Cannot divide these values");
    }
}