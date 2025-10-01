namespace Nine.Animations;

public class AnimationClip<ObjectT>
{
    /// <summary>
    /// 该动画剪辑的所有轨道
    /// </summary>
    public TrackCollection<ObjectT> Tracks { get; set; } = [];

    /// <summary>
    /// 该动画剪辑的长度
    /// </summary>
    public float Length { get; set; } = 0;

    /// <summary>
    /// 该动画剪辑的循环模式
    /// </summary>
    public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;
}
