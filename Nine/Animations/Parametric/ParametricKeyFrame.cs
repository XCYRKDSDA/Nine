namespace Nine.Animations.Parametric;

public readonly struct ParametricKeyFrame<TValue, TGradient>(
    IParametric<float> position, IParametric<TValue> value, KeyFrameType type, IParametric<TGradient>? gradient = null)
    : IParametric<KeyFrame<TValue, TGradient>> where TValue : struct
                                               where TGradient : struct
{
    public readonly IParametric<float> Position = position;

    public readonly IParametric<TValue> Value = value;

    public readonly KeyFrameType Type = type;

    public readonly IParametric<TGradient>? Gradient = gradient;

    public KeyFrame<TValue, TGradient> Bake(IDictionary<string, object?>? parameters = null)
        => new(
            Position.Bake(parameters),
            Value.Bake(parameters),
            Type,
            Gradient?.Bake(parameters)
        );
}
