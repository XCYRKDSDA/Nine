namespace Nine.Animations;

public interface IKeyFrameCollection<TValue, TGradient>
    : IReadOnlyList<KeyFrame<TValue, TGradient>>, ICollection<KeyFrame<TValue, TGradient>>
    where TValue : struct where TGradient : struct
{ }
