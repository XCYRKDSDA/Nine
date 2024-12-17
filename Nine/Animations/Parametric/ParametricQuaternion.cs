using Microsoft.Xna.Framework;

namespace Nine.Animations.Parametric;

public sealed class ParametricQuaternion(
    IParametric<float> x, IParametric<float> y, IParametric<float> z,
    IParametric<float> w) : IParametric<Quaternion>
{
    public IParametric<float> X => x;
    public IParametric<float> Y => y;
    public IParametric<float> Z => z;
    public IParametric<float> W => w;

    public Quaternion Bake(IDictionary<string, object?>? parameters = null)
        => new Quaternion(X.Bake(parameters), Y.Bake(parameters), Z.Bake(parameters), W.Bake(parameters));
}
