using System.Text.Json;
using Microsoft.Xna.Framework;
using Nine.Animations;
using Nine.Assets.Animation;
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

    private class JsonAnimationClip
    {
        public float Duration { get; set; } = float.NaN;

        public AnimationLoopMode LoopMode { get; set; } = AnimationLoopMode.RunOnce;

        public Dictionary<string, JsonElement> Curves { get; set; } = [];
    }

    public Dictionary<Type, ICurveLoader> CurveLoaders { get; set; } = [];

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new AnimationLoopModeJsonConverter() }
    };

    public AnimationClip<ObjectT> Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var jsonClip = JsonSerializer.Deserialize<JsonAnimationClip>(stream, _jsonSerializerOptions) ??
                       throw new JsonException();

        var clip = new AnimationClip<ObjectT> { Length = jsonClip.Duration, LoopMode = jsonClip.LoopMode };

        foreach (var (propertyKey, definition) in jsonClip.Curves)
        {
            var (property, valueType) = ParsePropertyImpl(propertyKey);

            var curveLoader = CurveLoaders[valueType];
            var curve = curveLoader.Load(in definition);

            clip.Tracks.Add((property, valueType), curve);
        }

        return clip;
    }
}
