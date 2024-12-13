namespace Nine.Animations;

public struct CubicCurveKey<T> : IEquatable<CubicCurveKey<T>>
    where T : struct, IEquatable<T>
{
    public float Position;

    public T Value;
    
    public CurveKeyType Type;

    public T? Gradient;

    public CubicCurveKey(float position, T value, CurveKeyType type, T? gradient = null)
    {
        Position = position;
        Value = value;
        Type = type;
        Gradient = gradient;
    }

    public bool Equals(CubicCurveKey<T> other)
        => Position.Equals(other.Position) && Value.Equals(other.Value) && Type == other.Type && Nullable.Equals(Gradient, other.Gradient);

    public override bool Equals(object? obj)
        => obj is CubicCurveKey<T> other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Position, Value, (int)Type, Gradient);

    public static bool operator ==(CubicCurveKey<T> left, CubicCurveKey<T> right)
        => left.Equals(right);

    public static bool operator !=(CubicCurveKey<T> left, CubicCurveKey<T> right)
        => !(left == right);
}
