using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nine.Assets.Serialization;

public class AssetReferenceJsonConverter<T> : JsonConverter<T> where T : class
{
    private AssetsContext _context;

    public AssetReferenceJsonConverter(AssetsContext context)
    {
        _context = context;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert,
                            JsonSerializerOptions options)
    {
        var path = reader.GetString();

        if (string.IsNullOrEmpty(path))
            return null;

        return _context.Load<T>(path);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
