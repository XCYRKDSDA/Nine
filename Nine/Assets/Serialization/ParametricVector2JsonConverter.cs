using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Nine.Animations;

namespace Nine.Assets.Serialization;

public sealed class ParametricVector2JsonConverter : JsonConverter<IParametric<Vector2>>
{
    private readonly ParametricFloatJsonConverter _floatJsonConverter = new();

    public override IParametric<Vector2>? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                               JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();
        reader.Read();

        // 将数组前两个值记录为x和y
        var x = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();
        var y = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();

        // 丢弃多余的数组成员
        while (reader.TokenType != JsonTokenType.EndArray)
            reader.Read();

        return new ParametricVector2(x, y);
    }

    public override void Write(Utf8JsonWriter writer, IParametric<Vector2> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
