namespace Nine.Animations.Parametric;

public class ParametricKeyFrameCurve<TValue, TGradient, TCurve>
    : IParametric<KeyFrameCurve<TValue, TGradient>> where TValue : struct
                                                    where TGradient : struct
                                                    where TCurve : KeyFrameCurve<TValue, TGradient>, new()
{
    public List<ParametricKeyFrame<TValue, TGradient>> KeyFrames { get; } = [];

    public KeyFrameCurve<TValue, TGradient> Bake(IDictionary<string, object?>? parameters = null)
    {
        var result = new TCurve();

        foreach (var parametricKeyFrame in KeyFrames)
            result.KeyFrames.Add(parametricKeyFrame.Bake(parameters));

        return result;
    }
}
