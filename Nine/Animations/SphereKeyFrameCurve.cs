using Microsoft.Xna.Framework;

namespace Nine.Animations;

internal static class QuaternionUtils
{
    public static Vector3 ToAxisAngle(this Quaternion q)
    {
        var θ = 2 * MathF.Acos(q.W);
        var dir = Vector3.Normalize(new(q.X, q.Y, q.Z));
        return θ * dir;
    }
}

public class SphereKeyFrameCurve : KeyFrameCurve<Quaternion, Vector3>
{
    private static Vector3 CalculateRatio(Quaternion q1, Quaternion q2, float t)
    {
        return Quaternion.Divide(q2, q1).ToAxisAngle() / t;
    }

    private static Quaternion Hermite(float x, Quaternion q0, Quaternion q1, Vector3 w0, Vector3 w1)
    {
        var x2 = x * x;
        var x3 = x2 * x;

        var h1 = 3 * x2 - 2 * x3;
        var h2 = x3 - 2 * x2 + x;
        var h3 = x3 - x2;

        var dr = Quaternion.Divide(q1, q0).ToAxisAngle();
        var r = h1 * dr + h2 * w0 + h3 * w1;

        var result = Quaternion.Multiply(Quaternion.CreateFromAxisAngle(Vector3.Normalize(r), r.Length()), q0);
        return result;
    }

    protected override Vector3 Difference(in Quaternion p0, in Quaternion p1)
        => Quaternion.Divide(p1, p0).ToAxisAngle();

    protected override Quaternion LinearInterpolate(in Quaternion p0, in Quaternion p1, float k)
        => Quaternion.Slerp(p0, p1, k);

    protected override Quaternion SmoothInterpolate(in Quaternion p0, in Vector3 m0, in Quaternion p1, in Vector3 m1,
                                                    float x)
    {
        var x2 = x * x;
        var x3 = x2 * x;

        var h1 = 3 * x2 - 2 * x3;
        var h2 = x3 - 2 * x2 + x;
        var h3 = x3 - x2;

        var dr = Quaternion.Divide(p1, p0).ToAxisAngle();
        var r = h1 * dr + h2 * m0 + h3 * m1;

        var result = Quaternion.Multiply(Quaternion.CreateFromAxisAngle(Vector3.Normalize(r), r.Length()), p0);
        return result;
    }
}
