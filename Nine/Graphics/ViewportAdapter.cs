using Microsoft.Xna.Framework.Graphics;
using Nine.Drawing;

namespace Nine.Graphics;

public static class ViewportAdapter
{
    public static Viewport Adapt(in SizeF expectSize, in Viewport viewport,
                                 float maxScale = float.PositiveInfinity, float minScale = 0)
    {
        var scaleX = viewport.Width / expectSize.Width;
        var scaleY = viewport.Height / expectSize.Height;

        float scale;
        if (scaleX > scaleY)
            scale = MathF.Max(minScale, MathF.Min(maxScale, scaleY));
        else
            scale = MathF.Max(minScale, MathF.Min(maxScale, scaleX));
        var actualWidth = (int)(expectSize.Width * scale);
        var actualHeight = (int)(expectSize.Height * scale);

        return new((viewport.Width - actualWidth) / 2, (viewport.Height - actualHeight) / 2, actualWidth, actualHeight);
    }
}
