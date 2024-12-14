namespace Nine.Animations;

public interface ICurveKeyCollection<TValue> : IReadOnlyList<CurveKey<TValue>>, ICollection<CurveKey<TValue>>
{
}