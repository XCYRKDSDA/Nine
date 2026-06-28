using Microsoft.Xna.Framework;

namespace Nine.Graphics;

public readonly struct RectangleF(float x, float y, float width, float height)
{
    public float X { get; } = x;

    public float Y { get; } = y;

    public float Width { get; } = width;

    public float Height { get; } = height;

    public static implicit operator RectangleF(Rectangle rect) =>
        new(rect.X, rect.Y, rect.Width, rect.Height);

    public Rectangle ToRectangle() => new((int)X, (int)Y, (int)Width, (int)Height);
}
