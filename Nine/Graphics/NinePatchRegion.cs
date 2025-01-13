using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Graphics;

public readonly struct NinePatchPadding
{
    public readonly int Left, Right, Top, Bottom;

    public NinePatchPadding(int padding) { Left = Right = Top = Bottom = padding; }

    public NinePatchPadding(int horizontal, int vertical)
    {
        Left = Right = horizontal;
        Top = Bottom = vertical;
    }

    public NinePatchPadding(int left, int right, int top, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
}

public class NinePatchRegion(Texture2D texture, NinePatchPadding padding, Rectangle? region = null,
                             Vector2? logicalOrigin = null, Vector2? size = null)
    : TextureRegion(texture, region, logicalOrigin, size)
{
    public NinePatchPadding Padding { get; } = padding;
}
