using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations;

namespace Nine.Assets.Animation;

public class KeyFrameCurveLoader<TValue, TGradient, TCurve>
    : ICurveLoader<TValue> where TValue : struct
                           where TGradient : struct
                           where TCurve : KeyFrameCurve<TValue, TGradient>, new()
{
    public KeyFrameCurveLoader(JsonConverter<TValue>? valueConverter,
                               JsonConverter<TGradient>? gradientConverter)
    {
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter<KeyFrameType>() }
        };
        if (valueConverter is not null) _jsonSerializerOptions.Converters.Add(valueConverter);
        if (gradientConverter is not null) _jsonSerializerOptions.Converters.Add(gradientConverter);
    }

    private class JsonKeyFrame
    {
        public float Time { get; set; }

        public TValue Value { get; set; }

        public KeyFrameType Type { get; set; } = KeyFrameType.Linear;

        public TGradient? Gradient { get; set; }
    }

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ICurve<TValue> Load(in JsonElement jsonElement)
    {
        var jsonKeyFrames = jsonElement.Deserialize<JsonKeyFrame[]>(_jsonSerializerOptions) ??
                            throw new JsonException();

        var curve = new TCurve();
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
