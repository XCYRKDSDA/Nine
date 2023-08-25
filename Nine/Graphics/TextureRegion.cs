using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Graphics;

public class TextureRegion
{
    public Texture SourceTexture { get; }

    public Rectangle SourceRegion { get; }

    public Rectangle VirtualFrame { get; }

    public int Width => VirtualFrame.Width;

    public int Height => VirtualFrame.Height;

    public Rectangle Bounds => new(0, 0, VirtualFrame.Width, VirtualFrame.Height);

    public TextureRegion(Texture2D texture, Rectangle? sourceRegion = null, Rectangle? virtualFrame = null)
    {
        SourceTexture = texture;
        SourceRegion = sourceRegion ?? texture.Bounds;
        VirtualFrame = virtualFrame ?? new(0, 0, SourceRegion.Width, SourceRegion.Height);
    }
}
