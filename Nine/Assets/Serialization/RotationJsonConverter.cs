using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Nine.Assets.Serialization;

public class RotationJsonConverter : JsonConverter<Quaternion>
{
    public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert,
                                    JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        // 如果第一个值为字符串，则判定为欧拉角，字符串指定了顺序；
        // 否则，如果总共有三个元素，则判定为轴角；
        // 否则，如果总共有四个元素，则判定为四元数
        var rot = Quaternion.Identity;

        reader.Read();
        if (reader.TokenType == JsonTokenType.String)
        {
            var order = reader.GetString()!;
            for (int i = 0; i < 3; i++)
            {
                reader.Read();
                var angle = reader.GetSingle();
                var axis = order[i];
                rot = Quaternion.Multiply(
                    rot, Quaternion.CreateFromAxisAngle(
                        axis switch
                        {
                            'x' => Vector3.UnitX,
                            'y' => Vector3.UnitY,
                            'z' => Vector3.UnitZ,
                            _ => throw new ArgumentOutOfRangeException()
                        }, angle
                    )
                );
            }

            goto DONE;
        }

        var x = reader.GetSingle();
        reader.Read();
        var y = reader.GetSingle();
        reader.Read();
        var z = reader.GetSingle();

        reader.Read();
        if (reader.TokenType == JsonTokenType.EndArray)
        {
            var vec = new Vector3(x, y, z);
            rot = Quaternion.CreateFromAxisAngle(Vector3.Normalize(vec), vec.Length());
            goto DONE;
        }

        var w = reader.GetSingle();
        rot = new Quaternion(x, y, z, w);

        DONE:

        // 丢弃多余的数组成员
        while (reader.TokenType != JsonTokenType.EndArray)
            reader.Read();

        return rot;
    }

    public override void Write(Utf8JsonWriter writer, Quaternion value,
                               JsonSerializerOptions options)
        => throw new NotImplementedException();
}
