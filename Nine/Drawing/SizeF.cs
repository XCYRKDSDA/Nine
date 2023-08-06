using Microsoft.Xna.Framework;

namespace Nine.Drawing;

public struct SizeF : IEquatable<SizeF>
{
    public static readonly SizeF Empty = new();

    public float Width = 0, Height = 0;

    public readonly bool IsEmpty => Width == 0 && Height == 0;

    public readonly bool IsValid => Width >= 0 && Height >= 0;

    public SizeF(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public static explicit operator SizeF(Vector2 vector) => new(vector.X, vector.Y);

    public static explicit operator Vector2(SizeF size) => new(size.Width, size.Height);

    #region IEquatable

    public readonly bool Equals(SizeF other) => Width == other.Width && Height == other.Height;

    public override readonly bool Equals(object? obj) => obj is SizeF other && Equals(other);

    public static bool operator ==(SizeF first, SizeF second) => first.Equals(second);
    public static bool operator !=(SizeF first, SizeF second) => !first.Equals(second);

    public override readonly int GetHashCode() => Width.GetHashCode() ^ Height.GetHashCode();

    #endregion

    #region Operations

    public static SizeF operator +(SizeF first, SizeF second) => new(first.Width + second.Width, first.Height + second.Height);
    public static SizeF operator -(SizeF first, SizeF second) => new(first.Width - second.Width, first.Height - second.Height);
    public static SizeF operator *(SizeF SizeF, float k) => new(SizeF.Width * k, SizeF.Height * k);
    public static SizeF operator /(SizeF SizeF, float k) => new(SizeF.Width / k, SizeF.Height / k);

    #endregion

    public override readonly string ToString() => $"Width: {Width}, Height: {Height}";
}