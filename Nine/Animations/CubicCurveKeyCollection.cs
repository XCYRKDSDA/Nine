using System.Collections;

namespace Nine.Animations;

public class CubicCurveKeyCollection<T> : IReadOnlyList<CubicCurveKey<T>>, ICollection<CubicCurveKey<T>>, IEnumerable<CubicCurveKey<T>>, IEnumerable
    where T : struct, IEquatable<T>
{
    internal readonly SortedList<float, CubicCurveKey<T>> _keys = new();

    public int Count => _keys.Count;

    bool ICollection<CubicCurveKey<T>>.IsReadOnly => false;

    public CubicCurveKey<T> this[int index] => _keys.Values[index];

    public void Add(CubicCurveKey<T> item) => _keys.Add(item.Position, item);

    public void Clear() => _keys.Clear();

    public bool Contains(CubicCurveKey<T> item) => _keys.ContainsKey(item.Position);

    public void CopyTo(CubicCurveKey<T>[] array, int arrayIndex) => _keys.Values.CopyTo(array, arrayIndex);

    public IEnumerator<CubicCurveKey<T>> GetEnumerator() => _keys.Values.GetEnumerator();

    public bool Remove(CubicCurveKey<T> item) => _keys.Remove(item.Position);

    IEnumerator IEnumerable.GetEnumerator() => _keys.Values.GetEnumerator();
}
