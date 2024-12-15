using System.Collections;

namespace Nine.Animations;

public sealed class CurveKeyCollection<TValue> : ICurveKeyCollection<TValue>
    where TValue : struct
{
    private readonly SortedList<float, CurveKey<TValue>> _keys = [];

    public int Count => _keys.Count;

    bool ICollection<CurveKey<TValue>>.IsReadOnly => false;

    public CurveKey<TValue> this[int index] => _keys.Values[index];

    public void Add(CurveKey<TValue> item) => _keys.Add(item.Position, item);

    public void Clear() => _keys.Clear();

    public bool Contains(CurveKey<TValue> item) => _keys.ContainsKey(item.Position);

    public void CopyTo(CurveKey<TValue>[] array, int arrayIndex) => _keys.Values.CopyTo(array, arrayIndex);

    public IEnumerator<CurveKey<TValue>> GetEnumerator() => _keys.Values.GetEnumerator();

    public bool Remove(CurveKey<TValue> item) => _keys.Remove(item.Position);

    IEnumerator IEnumerable.GetEnumerator() => _keys.Values.GetEnumerator();
}
