namespace Nine.Animations;

public readonly struct ParametricCubicCurveKey<TValue>(
    IParametric<float> position,
    IParametric<TValue> value,
    CurveKeyType type,
    IParametric<TValue>? gradient = null)
    : IParametric<CubicCurveKey<TValue>>, IEquatable<ParametricCubicCurveKey<TValue>>
    where TValue : struct, IEquatable<TValue>
{
    public readonly IParametric<float> Position = position;

    public readonly IParametric<TValue> Value = value;

    public readonly CurveKeyType Type = type;

    public readonly IParametric<TValue>? Gradient = gradient;

    public CubicCurveKey<TValue> Bake(IDictionary<string, object?>? parameters = null)
        => new(
            Position.Bake(parameters),
            Value.Bake(parameters),
            Type,
            Gradient?.Bake(parameters)
        );

    #region IEquatable

    public bool Equals(ParametricCubicCurveKey<TValue> other)
        => Position.Equals(other.Position) && Value.Equals(other.Value) && Type == other.Type &&
           Equals(Gradient, other.Gradient);

    public override bool Equals(object? obj)
        => obj is ParametricCubicCurveKey<TValue> other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Position, Value, (int)Type, Gradient);

    public static bool operator ==(ParametricCubicCurveKey<TValue> left, ParametricCubicCurveKey<TValue> right)
        => left.Equals(right);

    public static bool operator !=(ParametricCubicCurveKey<TValue> left, ParametricCubicCurveKey<TValue> right)
        => !(left == right);

    #endregion
}