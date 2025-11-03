using Microsoft.Xna.Framework.Graphics;
using Zio;

namespace Nine.Assets;

public class Texture2DLoader : IAssetLoader<Texture2D>
{
    public GraphicsDevice GraphicsDevice { get; }

    public Texture2DLoader(GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;
    }

    public Texture2D Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        return Texture2D.FromStream(GraphicsDevice, fileStream, DefaultColorProcessors.PremultiplyAlpha);
    }
}
