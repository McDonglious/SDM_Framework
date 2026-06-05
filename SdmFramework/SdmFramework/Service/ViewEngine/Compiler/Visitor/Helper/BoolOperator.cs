namespace SdmFramework.Resource.Visitor.Helper;

public class BoolOperator
{
    private const double Tolerance = 0.001;
    
    public static bool OrOp(object? left, object? right)
    {
        if (left is bool lBool && right is bool rBool)
        {
            return lBool || rBool;
        }
        throw new Exception($"both types must be boolean: left expresion {left}, right expression : {right}");
    }

    public static bool AndOp(object? left, object? right)
    {
        if (left is bool lBool && right is bool rBool)
        {
            return lBool && rBool;
        }
        throw new Exception($"both types must be boolean: left expresion {left}, right expression : {right}");
    }

    public static bool LessorEqualThen(object? left, object? right)
    {
        if (left is int l && right is float r)
            return l <= r;

        if (left is float lf && right is int ri)
            return lf <= ri;

        if (left is float lFloat && right is float rFloat)
            return lFloat <= rFloat;

        if (left is int lInt && right is int rInt)
            return lInt <= rInt;
        throw new NotImplementedException();
    }

    public static bool GreaterOrEqualThen(object? left, object? right)
    {
        if (left is int l && right is float r)
            return l >= r;

        if (left is float lf && right is int ri)
            return lf >= ri;

        if (left is float lFloat && right is float rFloat)
            return lFloat >= rFloat;

        if (left is int lInt && right is int rInt)
            return lInt >= rInt;
        
        throw new NotImplementedException();
    }
    
    public static bool GreaterThen(object? left, object? right)
    {
        if (left is int l && right is float r)
            return l > r;

        if (left is float lf && right is int ri)
            return lf > ri;

        if (left is float lFloat && right is float rFloat)
            return lFloat > rFloat;

        if (left is int lInt && right is int rInt)
            return lInt > rInt;
        
        throw new NotImplementedException();
    }

    public static bool LessThen(object? left, object? right)
    {
        if (left is int l && right is float r)
            return l < r;

        if (left is float lf && right is int ri)
            return lf < ri;

        if (left is float lFloat && right is float rFloat)
            return lFloat < rFloat;

        if (left is int lInt && right is int rInt)
            return lInt < rInt;
        
        throw new NotImplementedException();
    }

    public static bool IsNotEqual(object? left, object? right)
    {
        if (left is int l && right is float r)
            return Math.Abs(l - r) > Tolerance; 

        if (left is float lf && right is int ri)
            return Math.Abs(lf - ri) > Tolerance;

        if (left is float lFloat && right is float rFloat)
            return Math.Abs(lFloat - rFloat) > Tolerance;

        if (left is int lInt && right is int rInt)
            return lInt != rInt;
        
        throw new NotImplementedException();
    }

    public static bool IsEqual(object? left, object? right)
    {
        if (left is int l && right is float r)
            return Math.Abs(l - r) < Tolerance; 

        if (left is float lf && right is int ri)
            return Math.Abs(lf - ri) < Tolerance;

        if (left is float lFloat && right is float rFloat)
            return Math.Abs(lFloat - rFloat) < Tolerance;

        if (left is int lInt && right is int rInt)
            return lInt == rInt;
        
        throw new NotImplementedException();
    }
}