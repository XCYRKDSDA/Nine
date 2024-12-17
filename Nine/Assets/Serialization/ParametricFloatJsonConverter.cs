using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations;
using Nine.Animations.Parametric;

namespace Nine.Assets.Serialization;

public class ParametricFloatJsonConverter : JsonConverter<IParametric<float>>
{
    public override IParametric<float>? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                             JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Number => new Constant<float>(reader.GetSingle()),
            JsonTokenType.String => new Expression<float>(reader.GetString() ?? throw new JsonException()),
            _ => throw new JsonException()
        };

    public override void Write(Utf8JsonWriter writer, IParametric<float> value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
