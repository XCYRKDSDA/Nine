namespace Nine.Animations;

public class TrackCollection<ObjectT> : Dictionary<(IProperty<ObjectT>, Type), ICurve>
{ }
