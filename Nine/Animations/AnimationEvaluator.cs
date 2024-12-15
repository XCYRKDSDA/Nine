namespace Nine.Animations;

public static class AnimationEvaluator<ObjectT>
{
    public static float WrapTime(float t, AnimationClip<ObjectT> clip)
        => clip.LoopMode switch
        {
            AnimationLoopMode.RunOnce => t > clip.Length ? clip.Length : t,
            AnimationLoopMode.LoopForever => t % clip.Length,
            _ => throw new NotSupportedException()
        };

    public static void EvaluateAndSet(ref ObjectT obj, AnimationClip<ObjectT>? clip, float t)
    {
        if (clip is null)
            return;

        t = WrapTime(t, clip);
        TrackEvaluator.EvaluateAndSet(ref obj, clip.Tracks, t);
    }

    public static void TweenAndSet(ref ObjectT obj,
                                   AnimationClip<ObjectT>? clip1, float t1, AnimationClip<ObjectT>? clip2, float t2,
                                   ICurve<float>? tweener, float k)
    {
        if (clip1 is null && clip2 is null)
            return;

        if (clip1 is not null)
            t1 = WrapTime(t1, clip1);
        if (clip2 is not null)
            t2 = WrapTime(t2, clip2);

        TrackEvaluator.TweenAndSet(ref obj, clip1 is null ? [] : clip1.Tracks, t1, clip2 is null ? [] : clip2.Tracks,
                                   t2, tweener, k);
    }
}
