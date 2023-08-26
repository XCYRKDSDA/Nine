using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nine.Graphics;

public class TextureRegion
{
    public Texture2D Texture { get; }

    public Rectangle Bounds { get; }

    public TextureRegion(Texture2D texture, Rectangle? region = null)
    {
        Texture = texture;
        Bounds = region ?? texture.Bounds;
    }
}
