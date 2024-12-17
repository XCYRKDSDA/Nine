using Microsoft.Xna.Framework;

namespace Nine.Animations.Parametric;

public sealed class ParametricVector2(IParametric<float> x, IParametric<float> y)
    : IParametric<Vector2>
{
    public IParametric<float> X { get; } = x;
    public IParametric<float> Y { get; } = y;

    public Vector2 Bake(IDictionary<string, object?>? parameters = null)
        => new(
            X.Bake(parameters),
            Y.Bake(parameters)
        );
}
