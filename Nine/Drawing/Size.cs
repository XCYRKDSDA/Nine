using Microsoft.Xna.Framework;

namespace Nine.Drawing;

public struct Size : IEquatable<Size>
{
    public static readonly Size Empty = new();

    public int Width = 0, Height = 0;

    public readonly bool IsEmpty => Width == 0 && Height == 0;

    public readonly bool IsValid => Width >= 0 && Height >= 0;

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public static explicit operator Size(Point point) => new(point.X, point.Y);

    public static explicit operator Point(Size size) => new(size.Width, size.Height);

    #region IEquatable

    public readonly bool Equals(Size other) => Width == other.Width && Height == other.Height;

    public override readonly bool Equals(object? obj) => obj is Size other && Equals(other);

    public static bool operator ==(Size first, Size second) => first.Equals(second);
    public static bool operator !=(Size first, Size second) => !first.Equals(second);

    public override readonly int GetHashCode() => Width.GetHashCode() ^ Height.GetHashCode();

    #endregion

    #region Operations

    public static Size operator +(Size first, Size second) =>
        new(first.Width + second.Width, first.Height + second.Height);

    public static Size operator -(Size first, Size second) =>
        new(first.Width - second.Width, first.Height - second.Height);

    public static Size operator *(Size size, int k) => new(size.Width * k, size.Height * k);
    public static Size operator /(Size size, int k) => new(size.Width / k, size.Height / k);

    #endregion

    public override readonly string ToString() => $"Width: {Width}, Height: {Height}";
}
