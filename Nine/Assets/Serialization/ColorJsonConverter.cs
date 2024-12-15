using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Nine.Assets.Serialization;

public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert,
                               JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException();

        return new()
        {
            R = byte.Parse(value.Substring(1, 2), NumberStyles.HexNumber),
            G = byte.Parse(value.Substring(3, 2), NumberStyles.HexNumber),
            B = byte.Parse(value.Substring(5, 2), NumberStyles.HexNumber),
            A = value.Length > 7 ? byte.Parse(value.Substring(7, 2), NumberStyles.HexNumber) : (byte)255
        };
    }

    public override void Write(Utf8JsonWriter writer, Color value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
