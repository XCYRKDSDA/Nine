using Microsoft.Xna.Framework.Graphics;

namespace Nine.Assets;

public class Texture2DLoader : IAssetLoader<Texture2D>
{
    public GraphicsDevice GraphicsDevice { get; }

    public Texture2DLoader(GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;
    }

    public Texture2D Load(AssetsContext context, string asset)
    {
        using var fileStream = context.Open(asset);

        return Texture2D.FromStream(GraphicsDevice, fileStream);
    }
}
