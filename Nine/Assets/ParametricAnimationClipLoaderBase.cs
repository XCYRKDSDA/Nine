using System.Reflection;
using System.Text.Json;
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

        public JsonElement? Gradient { get; set; }
    }

    private class JsonAnimationClip
    {
        public Dictionary<string, IParametric<float>> Variables { get; set; } = [];

        public IParametric<float> Duration { get; set; }

        public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;

        public Dictionary<string, JsonCurveKeyFrame[]> Curves { get; set; } = [];
    }

    protected abstract IParametric<TValue> ParseValueImpl<TValue>(in JsonElement json);

    private ParametricCubicCurve<TValue> LoadCurve<TValue>(JsonCurveKeyFrame[] jsonKeys)
        where TValue : struct, IEquatable<TValue>
    {
        var curve = new ParametricCubicCurve<TValue>();

        foreach (var jsonKey in jsonKeys)
        {
            var key = new ParametricCubicCurveKey<TValue>(
                jsonKey.Time,
                ParseValueImpl<TValue>(jsonKey.Value),
                jsonKey.Gradient.HasValue ? ParseValueImpl<TValue>(jsonKey.Gradient.Value) : null);
            curve.Keys.Add(key);
        }

        return curve;
    }

    private static readonly MethodInfo _loadCurveMethod = typeof(ParametricAnimationClipLoaderBase<TObject>)
        .GetMethod("LoadCurve", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new AnimationLoopModeJsonConverter(),
            new ParametricFloatJsonConverter()
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
            var (property, type) = ParsePropertyImpl(propertyKey);
            clip.Tracks.Add((property, type),
                            (IParametricCurve)_loadCurveMethod.MakeGenericMethod(type).Invoke(this, [keys])!);
        }

        foreach (var (variableName, defaultValue) in jsonClip.Variables)
            clip.Parameters[variableName] = defaultValue.Bake(clip.Parameters);

        return clip;
    }
}
