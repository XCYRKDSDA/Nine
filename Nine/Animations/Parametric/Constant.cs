namespace Nine.Animations.Parametric;

public class Constant<TValue>(TValue value) : IParametric<TValue>
{
    public TValue Value => value;

    public TValue Bake(IDictionary<string, object?>? parameters = null) => value;
}
