using NCalc;

namespace Nine.Animations.Parametric;

public class IntExpression(string expression) : IParametric<int>
{
    private readonly NCalc.Expression _expression = new(expression);

    public int Bake(IDictionary<string, object?>? parameters = null)
    {
        if (parameters is not null)
            _expression.Parameters = parameters;
        var result = _expression.Evaluate()! ?? throw new InvalidOperationException();
        return Convert.ToInt32(result);
    }
}
