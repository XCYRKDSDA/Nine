namespace Nine.Animations;

public class TrackCollection<ObjectT> : Dictionary<(IProperty<ObjectT>, Type), ICurve>, IEnumerable<(IProperty<ObjectT>, Type, ICurve)>
{
    public new IEnumerator<(IProperty<ObjectT>, Type, ICurve)> GetEnumerator()
    {
        return (from KeyValuePair<(IProperty<ObjectT>, Type), ICurve> pair in this
                select (pair.Key.Item1, pair.Key.Item2, pair.Value)).GetEnumerator();
    }
}
