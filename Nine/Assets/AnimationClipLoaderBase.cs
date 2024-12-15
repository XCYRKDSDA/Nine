using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations;
using Nine.Assets.Serialization;
using Zio;

namespace Nine.Assets;

public abstract class AnimationClipLoaderBase<ObjectT> : IAssetLoader<AnimationClip<ObjectT>>
{
    #region IProperty Builder

    public delegate ValueT Getter<ValueT>(in ObjectT obj);

    public delegate void Setter<ValueT>(ref ObjectT obj, in ValueT value);

    protected abstract (IProperty<ObjectT>, Type) ParsePropertyImpl(string property);

    #endregion

    private class JsonCurveKeyFrame
    {
        public float Time { get; set; }

        public JsonElement Value { get; set; }

        public CurveKeyType Type { get; set; } = CurveKeyType.Linear;

        public JsonElement? Gradient { get; set; }
    }

    private class JsonAnimationClip
    {
        public float Duration { get; set; } = float.NaN;

        public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;

        public Dictionary<string, JsonCurveKeyFrame[]> Curves { get; set; } = [];
    }

    private static CurveT LoadCurve<ValueT, CurveT>(JsonCurveKeyFrame[] jsonKeys, JsonConverter<ValueT>? parser)
        where CurveT : ICurve<ValueT>, new()
    {
        var curve = new CurveT();

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        if (parser is not null) jsonSerializerOptions.Converters.Add(parser);

        foreach (var jsonKey in jsonKeys)
        {
            var key = new CurveKey<ValueT>(
                jsonKey.Time,
                jsonKey.Value.Deserialize<ValueT>(jsonSerializerOptions)!,
                jsonKey.Type,
                jsonKey.Gradient is null ? default : jsonKey.Gradient.Value.Deserialize<ValueT>(jsonSerializerOptions)
            );
            curve.Keys.Add(key);
        }

        return curve;
    }

    private static readonly MethodInfo _loadCurveMethod =
        typeof(AnimationClipLoaderBase<ObjectT>).GetMethod("LoadCurve", BindingFlags.Static | BindingFlags.NonPublic)!;

    public Dictionary<Type, (JsonConverter? Parser, Type CurveType)> ValueTypes { get; } = [];

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new AnimationLoopModeJsonConverter(),
            new JsonStringEnumConverter<CurveKeyType>()
        }
    };

    public AnimationClip<ObjectT> Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var jsonClip = JsonSerializer.Deserialize<JsonAnimationClip>(stream, _jsonSerializerOptions) ??
                       throw new JsonException();

        var clip = new AnimationClip<ObjectT> { Length = jsonClip.Duration, LoopMode = jsonClip.LoopMode };

        foreach (var (propertyKey, keys) in jsonClip.Curves)
        {
            var (property, valueType) = ParsePropertyImpl(propertyKey);
            var (parser, curveType) = ValueTypes[valueType];

            var loadCurveMethod = _loadCurveMethod.MakeGenericMethod(valueType, curveType);
            var curve = (loadCurveMethod.Invoke(null, [keys, parser]) as ICurve)!;

            clip.Tracks.Add((property, valueType), curve);
        }

        return clip;
    }
}
