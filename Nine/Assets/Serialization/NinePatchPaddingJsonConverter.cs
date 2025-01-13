using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Graphics;

namespace Nine.Assets.Serialization;

public class NinePatchPaddingJsonConverter : JsonConverter<NinePatchPadding>
{
    public override NinePatchPadding Read(ref Utf8JsonReader reader, Type typeToConvert,
                                          JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        int[] paddings = [0, 0, 0, 0];
        int i = 0;
        for (; i < 4; i++)
        {
            reader.Read();

            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            paddings[i] = reader.GetInt32();
        }

        return i switch
        {
            1 => new NinePatchPadding(paddings[0]),
            2 => new NinePatchPadding(paddings[0], paddings[1]),
            4 => new NinePatchPadding(paddings[0], paddings[1], paddings[2], paddings[3]),
            _ => throw new NotImplementedException()
        };
    }

    public override void Write(Utf8JsonWriter writer, NinePatchPadding value,
                               JsonSerializerOptions options)
        => throw new NotImplementedException();
}
