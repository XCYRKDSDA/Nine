using Microsoft.Xna.Framework;

namespace Nine.Animations;

public sealed class ParametricVector3(IParametric<float> x, IParametric<float> y, IParametric<float> z)
    : IParametric<Vector3>
{
    public IParametric<float> X { get; } = x;
    public IParametric<float> Y { get; } = y;
    public IParametric<float> Z { get; } = z;

    public Vector3 Bake(IDictionary<string, object?>? parameters = null)
        => new(
            X.Bake(parameters),
            Y.Bake(parameters),
            Z.Bake(parameters)
        );
}
