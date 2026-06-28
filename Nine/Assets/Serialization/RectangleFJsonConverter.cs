using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Graphics;

namespace Nine.Assets.Serialization;

public class RectangleFJsonConverter : JsonConverter<RectangleF>
{
    public override RectangleF Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        var x = 0f;
        var y = 0f;
        var w = 0f;
        var h = 0f;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new(x, y, w, h);

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var property = reader.GetString();

                reader.Read();
                var value = reader.GetSingle();

                if (property == "x" || property == "X")
                    x = value;
                else if (property == "y" || property == "Y")
                    y = value;
                else if (property == "w" || property == "W")
                    w = value;
                else if (property == "h" || property == "H")
                    h = value;
                else
                    throw new JsonException();
            }
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer,
        RectangleF value,
        JsonSerializerOptions options
    )
    {
        throw new NotImplementedException();
    }
}
