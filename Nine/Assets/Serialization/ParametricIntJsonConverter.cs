using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations.Parametric;

namespace Nine.Assets.Serialization;

public class ParametricIntJsonConverter : JsonConverter<IParametric<int>>
{
    public override IParametric<int>? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                           JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Number => new Constant<int>(reader.GetInt32()),
            JsonTokenType.String => new IntExpression(reader.GetString() ?? throw new JsonException()),
            _ => throw new JsonException()
        };

    public override void Write(Utf8JsonWriter writer, IParametric<int> value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
