namespace Nine.Animations;

public class ParametricCubicCurve<TValue> : IParametricCurve<TValue> where TValue : struct, IEquatable<TValue>
{
    public List<ParametricCubicCurveKey<TValue>> Keys { get; set; } = [];

    public CubicCurve<TValue> Bake(IDictionary<string, object?>? parameters = null)
    {
        var result = new CubicCurve<TValue>();

        foreach (var rawKey in Keys)
            result.Keys.Add(rawKey.Bake(parameters));

        return result;
    }

    ICurve<TValue> IParametric<ICurve<TValue>>.Bake(IDictionary<string, object?>? parameters)
        => Bake(parameters);

    public TValue Evaluate(float position) { throw new NotImplementedException(); }
}
