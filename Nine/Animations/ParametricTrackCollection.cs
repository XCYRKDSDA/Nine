namespace Nine.Animations;

public class ParametricTrackCollection<TObject>
    : Dictionary<(IProperty<TObject>, Type), IParametric<ICurve>>, IParametric<TrackCollection<TObject>>
{
    public TrackCollection<TObject> Bake(IDictionary<string, object?>? parameters = null)
    {
        var result = new TrackCollection<TObject>();

        foreach (var (propertyInfo, rawCurve) in this)
            result.Add(propertyInfo, rawCurve.Bake(parameters));

        return result;
    }
}
