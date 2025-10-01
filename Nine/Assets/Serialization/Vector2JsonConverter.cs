using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Nine.Assets.Serialization;

public class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert,
                                 JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();
        reader.Read();

        // 将数组前两个值记录为x和y
        var x = reader.GetSingle();
        reader.Read();
        var y = reader.GetSingle();
        reader.Read();

        // 丢弃多余的数组成员
        while (reader.TokenType != JsonTokenType.EndArray)
            reader.Read();

        return new(x, y);
    }

    public override void Write(Utf8JsonWriter writer, Vector2 value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
