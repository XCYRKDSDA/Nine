using Microsoft.Xna.Framework;

namespace Nine.Animations.Parametric;

public class ParametricEuler(string order, IParametric<float>[] angles)
    : IParametric<Quaternion>
{
    public string Order => order;
    public IParametric<float>[] Angles => angles;

    private static Vector3 GetAxis(char c)
        => c switch
        {
            'x' => Vector3.UnitX,
            'y' => Vector3.UnitY,
            'z' => Vector3.UnitZ,
            _ => throw new ArgumentOutOfRangeException(nameof(c))
        };

    public Quaternion Bake(IDictionary<string, object?>? parameters = null)
    {
        var rot = Quaternion.Identity;
        for (int i = 0; i < 3; i++)
            rot = Quaternion.Multiply(rot, Quaternion.CreateFromAxisAngle(GetAxis(order[i]), angles[i].Bake()));
        return rot;
    }
}
