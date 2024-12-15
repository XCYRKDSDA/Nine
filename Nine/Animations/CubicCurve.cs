using System.ComponentModel;

namespace Nine.Animations;

public class CubicCurve<ValueT> : ICurve<ValueT>
{
    private readonly CurveKeyCollection<ValueT> _keys = [];

    public CurveKeyCollection<ValueT> Keys => _keys;

    ICurveKeyCollection<ValueT> ICurve<ValueT>.Keys => _keys;

    private (int, int) FindKeysIndices(float position)
    {
        if (position < _keys[0].Position)
            return (-1, 0);
        if (position > _keys[^1].Position)
            return (_keys.Count - 1, -1);

        int l = 0, r = _keys.Count - 1;
        while (true)
        {
            if (r == l + 1)
                return (l, r);

            var m = (l + r) / 2;

            if (_keys[m].Position < position)
                l = m;
            else if (_keys[m].Position > position)
                r = m;
            else
                return (m, m);
        }
    }

    private ValueT GetGradient(int idx)
    {
        var key = _keys[idx];

        if (key.Type == CurveKeyType.Step)
            return GenericMathHelper<ValueT>.Zero;

        if (key.Type == CurveKeyType.Linear)
        {
            if (idx >= _keys.Count - 1)
                return GenericMathHelper<ValueT>.Zero;

            var nextKey = _keys[idx + 1];
            return GenericMathHelper<ValueT>.Div(
                GenericMathHelper<ValueT>.Sub(in nextKey.Value, in key.Value),
                nextKey.Position - key.Position
            );
        }

        if (key.Type == CurveKeyType.Smooth)
        {
            if (key.Gradient is not null)
                return key.Gradient;

            if (idx <= 0 || idx >= _keys.Count - 1)
                return GenericMathHelper<ValueT>.Zero;

            var prevKey = _keys[idx - 1];
            var nextKey = _keys[idx + 1];

            return GenericMathHelper<ValueT>.Div(GenericMathHelper<ValueT>.Sub(in nextKey.Value, in prevKey.Value),
                                                 nextKey.Position - prevKey.Position);
        }

        throw new InvalidEnumArgumentException();
    }

    public ValueT Evaluate(float position)
    {
        // 首先找到位置两侧的关键帧
        var (leftKeyIdx, rightKeyIdx) = FindKeysIndices(position);

        // 处理特殊情况
        if (leftKeyIdx < 0) // 在序列前
            return Keys[0].Value;
        else if (rightKeyIdx < 0) // 在序列后
            return Keys[^1].Value;
        else if (leftKeyIdx == rightKeyIdx) // 直接找到关键帧
            return Keys[leftKeyIdx].Value;

        var leftKey = Keys[leftKeyIdx];
        var rightKey = Keys[rightKeyIdx];

        // 阶跃关键帧
        if (leftKey.Type == CurveKeyType.Step)
            return leftKey.Value;

        // 线性关键帧
        if (leftKey.Type == CurveKeyType.Linear)
        {
            var k = (position - leftKey.Position) / (rightKey.Position - leftKey.Position);
            return GenericMathHelper<ValueT>.Add(
                GenericMathHelper<ValueT>.Mul(in leftKey.Value, 1 - k),
                GenericMathHelper<ValueT>.Mul(in rightKey.Value, k)
            );
        }

        // 三次关键帧
        if (leftKey.Type == CurveKeyType.Smooth)
        {
            // 确定两侧关键帧上的值和斜率
            var p0 = leftKey.Value;
            var p1 = rightKey.Value;
            var m0 = GetGradient(leftKeyIdx);
            var m1 = GetGradient(rightKeyIdx);

            // 标准化
            var u = rightKey.Position - leftKey.Position;
            m0 = GenericMathHelper<ValueT>.Mul(in m0, u);
            m1 = GenericMathHelper<ValueT>.Mul(in m1, u);
            var t = (position - leftKey.Position) / u;

            // 计算结果
            var t2 = t * t;
            var t3 = t2 * t;
            var h00 = 2 * t3 - 3 * t2 + 1;
            var h10 = t3 - 2 * t2 + t;
            var h01 = -2 * t3 + 3 * t2;
            var h11 = t3 - t2;
            return GenericMathHelper<ValueT>.Add(
                GenericMathHelper<ValueT>.Add(GenericMathHelper<ValueT>.Mul(in p0, h00),
                                              GenericMathHelper<ValueT>.Mul(in m0, h10)),
                GenericMathHelper<ValueT>.Add(GenericMathHelper<ValueT>.Mul(in p1, h01),
                                              GenericMathHelper<ValueT>.Mul(in m1, h11))
            );
        }

        throw new InvalidEnumArgumentException();
    }
}
