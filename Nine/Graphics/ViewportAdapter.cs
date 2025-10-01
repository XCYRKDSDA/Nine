using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Graphics;

public static class ViewportAdapter
{
    public static Viewport Adapt(in Point expectSize, in Viewport viewport,
                                 float maxScale = float.PositiveInfinity, float minScale = 0)
    {
        var scaleX = viewport.Width / expectSize.X;
        var scaleY = viewport.Height / expectSize.Y;

        float scale;
        if (scaleX > scaleY)
            scale = MathF.Max(minScale, MathF.Min(maxScale, scaleY));
        else
            scale = MathF.Max(minScale, MathF.Min(maxScale, scaleX));
        var actualWidth = (int)(expectSize.X * scale);
        var actualHeight = (int)(expectSize.Y * scale);

        return new((viewport.Width - actualWidth) / 2, (viewport.Height - actualHeight) / 2, actualWidth, actualHeight);
    }
}
