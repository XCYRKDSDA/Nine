using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations;
using Nine.Animations.Parametric;
using Nine.Assets.Serialization;

namespace Nine.Assets.Animation;

public class ParametricKeyFrameCurveLoader<TValue, TGradient, TCurve>
    : IParametricCurveLoader<TValue> where TValue : struct
                                     where TGradient : struct
                                     where TCurve : KeyFrameCurve<TValue, TGradient>, new()
{
    public ParametricKeyFrameCurveLoader(JsonConverter<IParametric<TValue>>? valueConverter,
                                         JsonConverter<IParametric<TGradient>>? gradientConverter)
    {
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter<KeyFrameType>(),
                new ParametricFloatJsonConverter()
            }
        };
        if (valueConverter is not null) _jsonSerializerOptions.Converters.Add(valueConverter);
        if (gradientConverter is not null) _jsonSerializerOptions.Converters.Add(gradientConverter);
    }

    private class JsonParametricKeyFrame
    {
        public IParametric<float> Time { get; set; }

        public IParametric<TValue> Value { get; set; }

        public KeyFrameType Type { get; set; } = KeyFrameType.Linear;

        public IParametric<TGradient>? Gradient { get; set; }
    }

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public IParametric<ICurve<TValue>> Load(in JsonElement jsonElement)
    {
        var jsonKeyFrames = jsonElement.Deserialize<JsonParametricKeyFrame[]>(_jsonSerializerOptions) ??
                            throw new JsonException();

        var curve = new ParametricKeyFrameCurve<TValue, TGradient, TCurve>();
        foreach (var jsonKeyFrame in jsonKeyFrames)
        {
            curve.KeyFrames.Add(
                new(jsonKeyFrame.Time,
                    jsonKeyFrame.Value,
                    jsonKeyFrame.Type,
                    jsonKeyFrame.Gradient
                )
            );
        }

        return curve;
    }
}
