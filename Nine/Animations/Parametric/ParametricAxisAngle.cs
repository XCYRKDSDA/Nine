using Microsoft.Xna.Framework;

namespace Nine.Animations.Parametric;

public class ParametricAxisAngle(IParametric<float> x, IParametric<float> y, IParametric<float> z)
    : IParametric<Quaternion>
{
    public IParametric<float> X => x;
    public IParametric<float> Y => y;
    public IParametric<float> Z => z;

    public Quaternion Bake(IDictionary<string, object?>? parameters = null)
    {
        var vec = new Vector3(X.Bake(parameters), Y.Bake(parameters), Z.Bake(parameters));
        return Quaternion.CreateFromAxisAngle(Vector3.Normalize(vec), vec.Length());
    }
}
