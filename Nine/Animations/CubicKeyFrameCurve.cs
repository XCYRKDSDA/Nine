namespace Nine.Animations;

public class CubicKeyFrameCurve<TValue> : KeyFrameCurve<TValue, TValue>
    where TValue : struct
{
    protected override TValue Difference(in TValue p0, in TValue p1)
        => GenericMathHelper<TValue>.Sub(in p1, in p0);

    protected override TValue LinearInterpolate(in TValue p0, in TValue p1, float k)
        => GenericMathHelper<TValue>.Add(GenericMathHelper<TValue>.Mul(in p0, 1 - k),
                                         GenericMathHelper<TValue>.Mul(in p1, k));

    protected override TValue SmoothInterpolate(in TValue p0, in TValue m0, in TValue p1, in TValue m1, float t)
    {
        // 计算结果
        var t2 = t * t;
        var t3 = t2 * t;
        var h00 = 2 * t3 - 3 * t2 + 1;
        var h10 = t3 - 2 * t2 + t;
        var h01 = -2 * t3 + 3 * t2;
        var h11 = t3 - t2;
        return GenericMathHelper<TValue>.Add(
            GenericMathHelper<TValue>.Add(GenericMathHelper<TValue>.Mul(in p0, h00),
                                          GenericMathHelper<TValue>.Mul(in m0, h10)),
            GenericMathHelper<TValue>.Add(GenericMathHelper<TValue>.Mul(in p1, h01),
                                          GenericMathHelper<TValue>.Mul(in m1, h11))
        );
    }
}
