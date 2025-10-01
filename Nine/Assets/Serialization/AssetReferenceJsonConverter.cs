using System.Text.Json;
using System.Text.Json.Serialization;
using Zio;

namespace Nine.Assets.Serialization;

public class AssetReferenceJsonConverter<T> : JsonConverter<T> where T : class
{
    private readonly IAssetsManager _assets;
    private readonly UPath _directory;

    public AssetReferenceJsonConverter(IAssetsManager assets, in UPath directory)
    {
        _assets = assets;
        _directory = directory;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert,
                            JsonSerializerOptions options)
    {
        var path = reader.GetString();

        if (string.IsNullOrEmpty(path))
            return null;

        return _assets.Load<T>(UPath.Combine(_directory, path));
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
