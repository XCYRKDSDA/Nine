using System.Collections;

namespace Nine.Animations;

public sealed class KeyFrameCollection<TValue, TGradient> : IKeyFrameCollection<TValue, TGradient>
    where TValue : struct where TGradient : struct
{
    private readonly SortedList<float, KeyFrame<TValue, TGradient>> _keys = [];

    public int Count => _keys.Count;

    bool ICollection<KeyFrame<TValue, TGradient>>.IsReadOnly => false;

    public KeyFrame<TValue, TGradient> this[int index] => _keys.Values[index];

    public void Add(KeyFrame<TValue, TGradient> item) => _keys.Add(item.Position, item);

    public void Clear() => _keys.Clear();

    public bool Contains(KeyFrame<TValue, TGradient> item) => _keys.ContainsKey(item.Position);

    public void CopyTo(KeyFrame<TValue, TGradient>[] array, int arrayIndex) => _keys.Values.CopyTo(array, arrayIndex);

    public IEnumerator<KeyFrame<TValue, TGradient>> GetEnumerator() => _keys.Values.GetEnumerator();

    public bool Remove(KeyFrame<TValue, TGradient> item) => _keys.Remove(item.Position);

    IEnumerator IEnumerable.GetEnumerator() => _keys.Values.GetEnumerator();
}
