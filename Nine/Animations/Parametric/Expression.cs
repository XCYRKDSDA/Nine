using NCalc;

namespace Nine.Animations.Parametric;

public class Expression(string expression) : IParametric<float>
{
    private readonly NCalc.Expression _expression = new(expression);

    public float Bake(IDictionary<string, object?>? parameters = null)
    {
        if (parameters is not null)
            _expression.Parameters = parameters;
        var result = _expression.Evaluate()! ?? throw new InvalidOperationException();
        return Convert.ToSingle(result);
    }
}
