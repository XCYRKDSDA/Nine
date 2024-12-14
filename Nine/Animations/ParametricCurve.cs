namespace Nine.Animations;

public class ParametricCurve<TCurve, TValue> : IParametric<TCurve>
    where TCurve : ICurve<TValue>, new() where TValue : struct, IEquatable<TValue>
{
    public List<ParametricCurveKey<TValue>> Keys { get; set; } = [];

    public TCurve Bake(IDictionary<string, object?>? parameters = null)
    {
        var result = new TCurve();

        foreach (var rawKey in Keys)
            result.Keys.Add(rawKey.Bake(parameters));

        return result;
    }
}