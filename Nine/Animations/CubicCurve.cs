using ValueT = System.Single;

namespace Nine.Animations;

public class CubicCurve : ICurve<ValueT>
{
    private readonly CubicCurveKeyCollection<ValueT> _keys = new();

    public CubicCurveKeyCollection<ValueT> Keys => _keys;

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
        if (idx <= 0 || idx >= _keys.Count - 1)
            return 0;

        var key = _keys[idx];

        if (key.Gradient.HasValue)
            return key.Gradient.Value;

        var prevKey = _keys[idx - 1];
        var nextKey = _keys[idx + 1];

        return (nextKey.Value - prevKey.Value) / (nextKey.Position - prevKey.Position);
    }

    public ValueT Evaluate(float position)
    {
        // 首先找到位置两侧的关键帧
        var (leftKeyIdx, rightKeyIdx) = FindKeysIndices(position);

        // 处理特殊情况
        if (leftKeyIdx < 0)
            return Keys[0].Value;
        else if (rightKeyIdx < 0)
            return Keys[^1].Value;
        else if (leftKeyIdx == rightKeyIdx)
            return Keys[leftKeyIdx].Value;

        var leftKey = Keys[leftKeyIdx];
        var rightKey = Keys[rightKeyIdx];

        // 确定两侧关键帧上的值和斜率
        var p0 = leftKey.Value;
        var p1 = rightKey.Value;
        var m0 = GetGradient(leftKeyIdx);
        var m1 = GetGradient(rightKeyIdx);

        // 标准化
        var u = rightKey.Position - leftKey.Position;
        m0 /= u;
        m1 /= u;
        var t = (position - leftKey.Position) / u;

        // 计算结果
        var t2 = t * t;
        var t3 = t2 * t;
        var h00 = 2 * t3 - 3 * t2 + 1;
        var h10 = t3 - 2 * t2 + t;
        var h01 = -2 * t3 + 3 * t2;
        var h11 = t3 - t2;
        return h00 * p0 + h10 * m0 + h01 * p1 + h11 * m1;
    }
}
