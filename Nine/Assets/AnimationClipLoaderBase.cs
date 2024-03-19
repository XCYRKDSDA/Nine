using System.Reflection;
using System.Text.Json;
using Nine.Animations;
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

        public JsonElement? Gradient { get; set; }
    }

    private class JsonAnimationClip
    {
        public float Duration { get; set; } = float.NaN;

        public Dictionary<string, JsonCurveKeyFrame[]> Curves { get; set; } = [];
    }

    protected abstract ValueT ParseValueImpl<ValueT>(in JsonElement json);

    private CubicCurve<ValueT> LoadCurve<ValueT>(JsonCurveKeyFrame[] jsonKeys) where ValueT : struct, IEquatable<ValueT>
    {
        var curve = new CubicCurve<ValueT>();

        foreach (var jsonKey in jsonKeys)
        {
            var key = new CubicCurveKey<ValueT>(jsonKey.Time,
                                                ParseValueImpl<ValueT>(jsonKey.Value),
                                                jsonKey.Gradient.HasValue ? ParseValueImpl<ValueT>(jsonKey.Gradient.Value) : null);
            curve.Keys.Add(key);
        }

        return curve;
    }

    private static readonly MethodInfo _loadCurveMethod =
        typeof(AnimationClipLoaderBase<ObjectT>).GetMethod("LoadCurve", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public AnimationClip<ObjectT> Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var jsonClip = JsonSerializer.Deserialize<JsonAnimationClip>(stream, _jsonSerializerOptions) ?? throw new JsonException();

        var clip = new AnimationClip<ObjectT> { Length = jsonClip.Duration };

        foreach (var (propertyKey, keys) in jsonClip.Curves)
        {
            var (property, type) = ParsePropertyImpl(propertyKey);
            clip.Tracks.Add((property, type), (ICurve)_loadCurveMethod.MakeGenericMethod(type).Invoke(this, [keys])!);
        }

        return clip;
    }
}
