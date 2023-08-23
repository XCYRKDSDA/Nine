namespace Nine.Animations;

public struct CubicCurveKey<T> : IEquatable<CubicCurveKey<T>>
    where T : struct, IEquatable<T>
{
    public float Position;

    public T Value;

    public T? Gradient;

    public CubicCurveKey(float position, T value, T? gradient = null)
    {
        Position = position;
        Value = value;
        Gradient = gradient;
    }

    public readonly bool Equals(CubicCurveKey<T> other) => Position.Equals(other.Position) && Value.Equals(other.Value) && Gradient!.Equals(other.Gradient);

    public override readonly bool Equals(object? obj) => obj is CubicCurveKey<T> other && Equals(other);

    public static bool operator ==(CubicCurveKey<T> left, CubicCurveKey<T> right) => left.Equals(right);

    public static bool operator !=(CubicCurveKey<T> left, CubicCurveKey<T> right) => !(left == right);

    public override readonly int GetHashCode() => Position.GetHashCode() ^ Value.GetHashCode() ^ Gradient!.GetHashCode();

}
