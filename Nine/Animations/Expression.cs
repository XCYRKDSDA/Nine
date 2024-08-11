using NCalc;

namespace Nine.Animations;

public class Expression<TValue>(string expression) : IParametric<TValue>
{
    private readonly Expression _expression = new(expression);

    public TValue Bake(IDictionary<string, object?>? parameters = null)
    {
        if (parameters is not null)
            _expression.Parameters = parameters;
        var result = _expression.Evaluate()! ?? throw new InvalidOperationException();
        return (TValue)result;
    }
}
