namespace Nine.Animations;

public readonly struct ParametricCubicCurveKey<TValue>(
    IParametric<float> position, IParametric<TValue> value, IParametric<TValue>? gradient = null)
    : IParametric<CubicCurveKey<TValue>>, IEquatable<ParametricCubicCurveKey<TValue>>
    where TValue : struct, IEquatable<TValue>
{
    public readonly IParametric<float> Position = position;

    public readonly IParametric<TValue> Value = value;

    public readonly IParametric<TValue>? Gradient = gradient;

    public CubicCurveKey<TValue> Bake(IDictionary<string, object?>? parameters = null)
        => new(
            Position.Bake(parameters),
            Value.Bake(parameters),
            Gradient?.Bake(parameters)
        );

    #region IEquatable

    public readonly bool Equals(ParametricCubicCurveKey<TValue> other)
        => Position.Equals(other.Position) && Value.Equals(other.Value) && Gradient!.Equals(other.Gradient);

    public readonly override bool Equals(object? obj) => obj is CubicCurveKey<TValue> other && Equals(other);

    public static bool operator ==(ParametricCubicCurveKey<TValue> left, ParametricCubicCurveKey<TValue> right)
        => left.Equals(right);

    public static bool operator !=(ParametricCubicCurveKey<TValue> left, ParametricCubicCurveKey<TValue> right)
        => !(left == right);

    public readonly override int GetHashCode()
        => Position.GetHashCode() ^ Value.GetHashCode() ^ Gradient!.GetHashCode();

    #endregion
}
