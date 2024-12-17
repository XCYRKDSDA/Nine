using System.Text.Json;
using Nine.Animations;
using Nine.Animations.Parametric;

namespace Nine.Assets.Animation;

public interface IParametricCurveLoader
{
    IParametric<ICurve> Load(in JsonElement jsonElement);
}

public interface IParametricCurveLoader<TValue> : IParametricCurveLoader where TValue : struct
{
    new IParametric<ICurve<TValue>> Load(in JsonElement jsonElement);

    IParametric<ICurve> IParametricCurveLoader.Load(in JsonElement jsonElement) => Load(in jsonElement);
}
