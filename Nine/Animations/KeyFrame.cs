namespace Nine.Animations;

public readonly struct KeyFrame<TValue, TGradient>(
    float position, TValue value, KeyFrameType frameType, TGradient? gradient = null)
    where TValue : struct where TGradient : struct
{
    public readonly float Position = position;

    public readonly TValue Value = value;

    public readonly KeyFrameType FrameType = frameType;

    public readonly TGradient? Gradient = gradient;
}
