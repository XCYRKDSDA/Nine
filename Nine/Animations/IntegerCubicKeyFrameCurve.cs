namespace Nine.Animations;

public class IntegerCubicKeyFrameCurve : KeyFrameCurve<int, float>
{
    protected override float Difference(in int p0, in int p1) => p1 - p0;

    protected override int LinearInterpolate(in int p0, in int p1, float k) => (int)(p0 * (1 - k) + p1 * k);

    protected override int SmoothInterpolate(in int p0, in float m0, in int p1, in float m1, float t)
    {
        // 计算结果
        var t2 = t * t;
        var t3 = t2 * t;
        var h00 = 2 * t3 - 3 * t2 + 1;
        var h10 = t3 - 2 * t2 + t;
        var h01 = -2 * t3 + 3 * t2;
        var h11 = t3 - t2;
        return (int)(p0 * h00 + m0 * h10 + p1 * h01 + m1 * h11);
    }
}
