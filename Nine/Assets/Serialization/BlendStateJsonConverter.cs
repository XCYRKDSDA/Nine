using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Assets.Serialization;

public class BlendStateJsonConverter : JsonConverter<BlendState>
{
    public override BlendState? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                     JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException();

        return value switch
        {
            "AlphaBlend" => BlendState.AlphaBlend,
            "Additive" => BlendState.Additive,
            "NonPremultiplied" => BlendState.NonPremultiplied,
            "Opaque" => BlendState.Opaque,
            _ => throw new KeyNotFoundException()
        };
    }

    public override void Write(Utf8JsonWriter writer, BlendState value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
