namespace Nine.Animations;

public readonly struct CurveKey<T>(float position, T value, CurveKeyType type, T? gradient = default)
{
    public readonly float Position = position;

    public readonly T Value = value;

    public readonly CurveKeyType Type = type;

    public readonly T? Gradient = gradient;
}