namespace Nine.Animations.Parametric;

public class ParametricAnimationClip<TObject> : IParametric<AnimationClip<TObject>>, ICloneable
{
    /// <summary>
    /// 默认参数。当烘焙时默认采用该字典中指定的参数
    /// </summary>
    public Dictionary<string, object?> Parameters { get; set; } = new();

    /// <summary>
    /// 该动画剪辑的所有轨道
    /// </summary>
    public ParametricTrackCollection<TObject> Tracks { get; set; } = new();

    /// <summary>
    /// 该动画剪辑的长度
    /// </summary>
    public IParametric<float> Length { get; set; }

    /// <summary>
    /// 该动画剪辑的循环模式
    /// </summary>
    public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;

    /// <summary>
    /// 根据指定参数烘焙非参数化的确定动画
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public AnimationClip<TObject> Bake(IDictionary<string, object?>? parameters = null)
    {
        // 将给定的参数和默认参数层叠起来
        Dictionary<string, object?> aggregatedParameters;
        if (parameters is null)
            aggregatedParameters = Parameters;
        else
        {
            aggregatedParameters = Parameters.ToDictionary();
            foreach (var (key, value) in parameters)
                aggregatedParameters[key] = value;
        }

        return new AnimationClip<TObject>
        {
            Tracks = Tracks.Bake(aggregatedParameters),
            Length = Length.Bake(aggregatedParameters),
            LoopMode = LoopMode
        };
    }

    public ParametricAnimationClip<TObject> Clone()
        => new()
        {
            Parameters = new(Parameters),
            Tracks = Tracks,
            Length = Length,
            LoopMode = LoopMode
        };

    object ICloneable.Clone() => Clone();
}
