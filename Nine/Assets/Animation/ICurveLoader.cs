using System.Text.Json;
using Nine.Animations;

namespace Nine.Assets.Animation;

public interface ICurveLoader
{
    ICurve Load(in JsonElement jsonElement);
}

public interface ICurveLoader<TValue> : ICurveLoader where TValue : struct
{
    new ICurve<TValue> Load(in JsonElement jsonElement);

    ICurve ICurveLoader.Load(in JsonElement jsonElement) => Load(in jsonElement);
}
