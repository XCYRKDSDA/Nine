namespace Nine.Animations;

public readonly struct ParametricCurveKey<TValue>(
    IParametric<float> position,
    IParametric<TValue> value,
    CurveKeyType type,
    IParametric<TValue>? gradient = null)
    : IParametric<CurveKey<TValue>>, IEquatable<ParametricCurveKey<TValue>>
{
    public readonly IParametric<float> Position = position;

    public readonly IParametric<TValue> Value = value;

    public readonly CurveKeyType Type = type;

    public readonly IParametric<TValue>? Gradient = gradient;

    public CurveKey<TValue> Bake(IDictionary<string, object?>? parameters = null)
        => new(
            Position.Bake(parameters),
            Value.Bake(parameters),
            Type,
            Gradient is null ? default : Gradient.Bake(parameters)
        );

    #region IEquatable

    public bool Equals(ParametricCurveKey<TValue> other)
        => Position.Equals(other.Position) && Value.Equals(other.Value) && Type == other.Type &&
           Equals(Gradient, other.Gradient);

    public override bool Equals(object? obj)
        => obj is ParametricCurveKey<TValue> other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Position, Value, (int)Type, Gradient);

    public static bool operator ==(ParametricCurveKey<TValue> left, ParametricCurveKey<TValue> right)
        => left.Equals(right);

    public static bool operator !=(ParametricCurveKey<TValue> left, ParametricCurveKey<TValue> right)
        => !(left == right);

    #endregion
}
