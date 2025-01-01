using System.ComponentModel;

namespace Nine.Animations;

public abstract class KeyFrameCurve<TValue, TGradient> : ICurve<TValue>
    where TValue : struct where TGradient : struct
// TGradient must be located in a linear space
{
    public KeyFrameCollection<TValue, TGradient> KeyFrames { get; } = [];

    private (int, int) FindKeysIndices(float position)
    {
        if (position < KeyFrames[0].Position)
            return (-1, 0);
        if (position > KeyFrames[^1].Position)
            return (KeyFrames.Count - 1, -1);

        int l = 0, r = KeyFrames.Count - 1;
        while (true)
        {
            if (r == l + 1)
                return (l, r);

            var m = (l + r) / 2;

            if (KeyFrames[m].Position < position)
                l = m;
            else if (KeyFrames[m].Position > position)
                r = m;
            else
                return (m, m);
        }
    }

    protected abstract TGradient Difference(in TValue p0, in TValue p1);
    protected abstract TValue LinearInterpolate(in TValue p0, in TValue p1, float k);
    protected abstract TValue SmoothInterpolate(in TValue p0, in TGradient m0, in TValue p1, in TGradient m1, float t);

    private TGradient GetGradient(int idx)
    {
        var key = KeyFrames[idx];

        if (key.Gradient is not null)
            return key.Gradient.Value;

        if (key.FrameType == KeyFrameType.Step)
            return GenericMathHelper<TGradient>.Zero;

        if (key.FrameType == KeyFrameType.Linear)
        {
            if (idx >= KeyFrames.Count - 1)
                return GenericMathHelper<TGradient>.Zero;

            var nextKey = KeyFrames[idx + 1];
            return GenericMathHelper<TGradient>.Div(Difference(in key.Value, in nextKey.Value),
                                                    nextKey.Position - key.Position);
        }

        if (key.FrameType == KeyFrameType.Smooth)
        {
            var prevKey = idx == 0 ? key : KeyFrames[idx - 1];
            var nextKey = idx == KeyFrames.Count - 1 ? key : KeyFrames[idx + 1];

            return GenericMathHelper<TGradient>.Div(Difference(in prevKey.Value, in nextKey.Value),
                                                    nextKey.Position - prevKey.Position);
        }

        throw new InvalidEnumArgumentException();
    }

    public TValue Evaluate(float position)
    {
        // 首先找到位置两侧的关键帧
        var (leftKeyIdx, rightKeyIdx) = FindKeysIndices(position);

        // 处理特殊情况
        if (leftKeyIdx < 0) // 在序列前
            return KeyFrames[0].Value;
        else if (rightKeyIdx < 0) // 在序列后
            return KeyFrames[^1].Value;
        else if (leftKeyIdx == rightKeyIdx) // 直接找到关键帧
            return KeyFrames[leftKeyIdx].Value;

        var leftKey = KeyFrames[leftKeyIdx];
        var rightKey = KeyFrames[rightKeyIdx];

        // 阶跃关键帧
        if (leftKey.FrameType == KeyFrameType.Step)
            return leftKey.Value;

        // 线性关键帧
        if (leftKey.FrameType == KeyFrameType.Linear)
        {
            var k = (position - leftKey.Position) / (rightKey.Position - leftKey.Position);
            return LinearInterpolate(in leftKey.Value, in rightKey.Value, k);
        }

        // 曲线关键帧
        if (leftKey.FrameType == KeyFrameType.Smooth)
        {
            var p0 = leftKey.Value;
            var p1 = rightKey.Value;
            var m0 = GetGradient(leftKeyIdx);
            var m1 = GetGradient(rightKeyIdx);

            // 归一化
            var u = rightKey.Position - leftKey.Position;
            m0 = GenericMathHelper<TGradient>.Mul(in m0, u);
            m1 = GenericMathHelper<TGradient>.Mul(in m1, u);
            var t = (position - leftKey.Position) / u;

            return SmoothInterpolate(in p0, in m0, in p1, in m1, t);
        }

        throw new InvalidEnumArgumentException();
    }
}
