using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Nine.Assets.Serialization;

public class RectangleJsonConverter : JsonConverter<Rectangle>
{
    public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert,
                                   JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        var rectangle = new Rectangle();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return rectangle;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var property = reader.GetString();

                reader.Read();
                var value = reader.GetInt32();

                if (property == "x" || property == "X")
                    rectangle.X = value;
                else if (property == "y" || property == "Y")
                    rectangle.Y = value;
                else if (property == "w" || property == "W")
                    rectangle.Width = value;
                else if (property == "h" || property == "H")
                    rectangle.Height = value;
                else
                    throw new JsonException();
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Rectangle value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
