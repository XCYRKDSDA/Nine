using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Nine.Animations;
using Nine.Animations.Parametric;

namespace Nine.Assets.Animation;

public class ParametricSingleCubicKeyFrameCurveLoader(
    JsonConverter<IParametric<float>>? valueConverter, JsonConverter<IParametric<float>>? gradientConverter = null)
    : ParametricKeyFrameCurveLoader<float, float, SingleCubicKeyFrameCurve>(
        valueConverter, gradientConverter ?? valueConverter
    )
{ }

public class ParametricVector2CubicKeyFrameCurveLoader(
    JsonConverter<IParametric<Vector2>>? valueConverter, JsonConverter<IParametric<Vector2>>? gradientConverter = null)
    : ParametricKeyFrameCurveLoader<Vector2, Vector2, Vector2CubicKeyFrameCurve>(
        valueConverter, gradientConverter ?? valueConverter
    )
{ }

public class ParametricVector3CubicKeyFrameCurveLoader(
    JsonConverter<IParametric<Vector3>>? valueConverter, JsonConverter<IParametric<Vector3>>? gradientConverter = null)
    : ParametricKeyFrameCurveLoader<Vector3, Vector3, Vector3CubicKeyFrameCurve>(
        valueConverter, gradientConverter ?? valueConverter
    )
{ }

public class ParametricIntegerCubicKeyFrameCurveLoader(
    JsonConverter<IParametric<int>>? valueConverter, JsonConverter<IParametric<float>>? gradientConverter)
    : ParametricKeyFrameCurveLoader<int, float, IntegerCubicKeyFrameCurve>(
        valueConverter, gradientConverter
    )
{ }

public class ParametricSphereKeyFrameCurveLoader(
    JsonConverter<IParametric<Quaternion>>? valueConverter, JsonConverter<IParametric<Vector3>>? gradientConverter)
    : ParametricKeyFrameCurveLoader<Quaternion, Vector3, SphereKeyFrameCurve>(
        valueConverter, gradientConverter
    )
{ }
