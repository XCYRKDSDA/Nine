using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Nine.Assets.Serialization;

public class Vector3JsonConverter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert,
                                 JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        // 将数组前两个值记录为x和y
        reader.Read();
        var x = reader.GetSingle();
        reader.Read();
        var y = reader.GetSingle();
        reader.Read();
        var z = reader.GetSingle();

        // 丢弃多余的数组成员
        do
            reader.Read();
        while (reader.TokenType != JsonTokenType.EndArray);

        return new(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
