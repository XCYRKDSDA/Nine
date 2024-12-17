using System.Text.Json;
using Nine.Animations;
using Nine.Animations.Parametric;
using Nine.Assets.Animation;
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

    private class JsonAnimationClip
    {
        public Dictionary<string, IParametric<float>> Variables { get; set; } = [];

        public IParametric<float> Duration { get; set; }

        public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;

        public Dictionary<string, JsonElement> Curves { get; set; } = [];
    }

    public Dictionary<Type, IParametricCurveLoader> CurveLoaders { get; } = [];

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

        foreach (var (propertyKey, definition) in jsonClip.Curves)
        {
            var (property, valueType) = ParsePropertyImpl(propertyKey);

            var curveLoader = CurveLoaders[valueType];
            var curve = curveLoader.Load(in definition);

            clip.Tracks.Add((property, valueType), curve);
        }

        foreach (var (variableName, defaultValue) in jsonClip.Variables)
            clip.Parameters[variableName] = defaultValue.Bake(clip.Parameters);

        return clip;
    }
}
