using System.Text.Json;
using System.Text.Json.Serialization;
using Nine.Animations;

namespace Nine.Assets.Serialization;

public class AnimationLoopModeJsonConverter : JsonConverter<AnimationLoopMode>
{
    public override AnimationLoopMode Read(ref Utf8JsonReader reader, Type typeToConvert,
                                           JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException();

        return value switch
        {
            "OneShot" => AnimationLoopMode.RunOnce,
            "Loop" => AnimationLoopMode.LoopForever,
            _ => throw new KeyNotFoundException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AnimationLoopMode value,
                               JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
