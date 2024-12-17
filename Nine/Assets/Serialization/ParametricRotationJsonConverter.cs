using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Nine.Animations;
using Nine.Animations.Parametric;

namespace Nine.Assets.Serialization;

public class ParametricRotationJsonConverter : JsonConverter<IParametric<Quaternion>>
{
    private readonly ParametricFloatJsonConverter _floatJsonConverter = new();

    public override IParametric<Quaternion>? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                                  JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        // 如果第一个值为字符串，则判定为欧拉角，字符串指定了顺序；
        // 否则，如果总共有三个元素，则判定为轴角；
        // 否则，如果总共有四个元素，则判定为四元数
        IParametric<Quaternion>? rot = null;

        reader.Read();
        if (reader.TokenType == JsonTokenType.String)
        {
            var order = reader.GetString()!;
            var angles = new IParametric<float>[3];
            for (int i = 0; i < 3; i++)
            {
                reader.Read();
                angles[i] = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();
            }
            rot = new ParametricEuler(order, angles);

            goto DONE;
        }

        var x = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();
        reader.Read();
        var y = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();
        reader.Read();
        var z = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();

        reader.Read();
        if (reader.TokenType == JsonTokenType.EndArray)
        {
            rot = new ParametricAxisAngle(x, y, z);
            goto DONE;
        }

        var w = _floatJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException();
        rot = new ParametricQuaternion(x, y, z, w);

        DONE:

        // 丢弃多余的数组成员
        while (reader.TokenType != JsonTokenType.EndArray)
            reader.Read();

        return rot;
    }

    public override void Write(Utf8JsonWriter writer, IParametric<Quaternion> value,
                               JsonSerializerOptions options)
        => throw new NotImplementedException();
}
