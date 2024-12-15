using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations;
using Nine.Assets.Serialization;
using Zio;

namespace Nine.Assets;

public abstract class ParametricAnimationClipLoaderBase<TObject> : IAssetLoader<ParametricAnimationClip<TObject>>
{
    #region IProperty Builder

    public delegate TValue Getter<out TValue>(in TObject obj);

    public delegate void Setter<TValue>(ref TObject obj, in TValue value);

    protected abstract (IProperty<TObject>, Type) ParsePropertyImpl(string property);

    #endregion

    private class JsonCurveKeyFrame
    {
        public IParametric<float> Time { get; set; }

        public JsonElement Value { get; set; }

        public CurveKeyType Type { get; set; } = CurveKeyType.Linear;

        public JsonElement? Gradient { get; set; }
    }

    private class JsonAnimationClip
    {
        public Dictionary<string, IParametric<float>> Variables { get; set; } = [];

        public IParametric<float> Duration { get; set; }

        public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;

        public Dictionary<string, JsonCurveKeyFrame[]> Curves { get; set; } = [];
    }

    private static IParametric<TCurve> LoadCurve<TValue, TCurve>(
        JsonCurveKeyFrame[] jsonKeys, JsonConverter<IParametric<TValue>>? parser)
        where TCurve : ICurve<TValue>, new() where TValue : struct, IEquatable<TValue>
    {
        var curve = new ParametricCurve<TCurve, TValue>();

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        if (parser is not null) jsonSerializerOptions.Converters.Add(parser);

        foreach (var jsonKey in jsonKeys)
        {
            var key = new ParametricCurveKey<TValue>(
                jsonKey.Time,
                jsonKey.Value.Deserialize<IParametric<TValue>>(jsonSerializerOptions)!,
                jsonKey.Type,
                jsonKey.Gradient?.Deserialize<IParametric<TValue>>(jsonSerializerOptions));
            curve.Keys.Add(key);
        }

        return curve;
    }

    private static readonly MethodInfo _loadCurveMethod =
        typeof(ParametricAnimationClipLoaderBase<TObject>).GetMethod("LoadCurve",
                                                                     BindingFlags.Static | BindingFlags.NonPublic)!;

    public Dictionary<Type, (JsonConverter? Parser, Type CurveType)> ValueTypes { get; } = [];

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new AnimationLoopModeJsonConverter(),
            new ParametricFloatJsonConverter(),
            new JsonStringEnumConverter<CurveKeyType>()
        }
    };

    public ParametricAnimationClip<TObject> Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var jsonClip = JsonSerializer.Deserialize<JsonAnimationClip>(stream, _jsonSerializerOptions) ??
                       throw new JsonException();

        var clip = new ParametricAnimationClip<TObject> { Length = jsonClip.Duration, LoopMode = jsonClip.LoopMode };

        foreach (var (propertyKey, keys) in jsonClip.Curves)
        {
            var (property, valueType) = ParsePropertyImpl(propertyKey);
            var (parser, curveType) = ValueTypes[valueType];

            var loadCurveMethod = _loadCurveMethod.MakeGenericMethod(valueType, curveType);
            var parametricCurve = (loadCurveMethod.Invoke(null, [keys, parser]) as IParametric<ICurve>)!;

            clip.Tracks.Add((property, valueType), parametricCurve);
        }

        foreach (var (variableName, defaultValue) in jsonClip.Variables)
            clip.Parameters[variableName] = defaultValue.Bake(clip.Parameters);

        return clip;
    }
}
