using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Nine.Animations;

namespace Nine.Assets.Animation;

public class SingleCubicKeyFrameCurveLoader(
    JsonConverter<float>? valueConverter, JsonConverter<float>? gradientConverter = null)
    : KeyFrameCurveLoader<float, float, SingleCubicKeyFrameCurve>(
        valueConverter, gradientConverter ?? valueConverter
    )
{ }

public class Vector2CubicKeyFrameCurveLoader(
    JsonConverter<Vector2>? valueConverter, JsonConverter<Vector2>? gradientConverter = null)
    : KeyFrameCurveLoader<Vector2, Vector2, Vector2CubicKeyFrameCurve>(
        valueConverter, gradientConverter ?? valueConverter
    )
{ }

public class Vector3CubicKeyFrameCurveLoader(
    JsonConverter<Vector3>? valueConverter, JsonConverter<Vector3>? gradientConverter = null)
    : KeyFrameCurveLoader<Vector3, Vector3, Vector3CubicKeyFrameCurve>(
        valueConverter, gradientConverter ?? valueConverter
    )
{ }

public class SphereKeyFrameCurveLoader(
    JsonConverter<Quaternion>? valueConverter, JsonConverter<Vector3>? gradientConverter)
    : KeyFrameCurveLoader<Quaternion, Vector3, SphereKeyFrameCurve>(
        valueConverter, gradientConverter
    )
{ }
