using Microsoft.Xna.Framework;
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

    private static byte ApplyAlpha(byte color, byte alpha)
    {
        var fc = color / 255.0f;
        var fa = alpha / 255.0f;
        var fr = (int)(255.0f * fc * fa);
        if (fr < 0)
            fr = 0;
        else if (fr > 255)
            fr = 255;
        return (byte)fr;
    }

    public Texture2D Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        var rawTexture = Texture2D.FromStream(GraphicsDevice, fileStream);

        var data = new Color[rawTexture.Width * rawTexture.Height];
        rawTexture.GetData(data);
        for (int i = 0; i < data.Length; ++i)
        {
            byte a = data[i].A;

            data[i].R = ApplyAlpha(data[i].R, a);
            data[i].G = ApplyAlpha(data[i].G, a);
            data[i].B = ApplyAlpha(data[i].B, a);
        }

        var result = new Texture2D(GraphicsDevice, rawTexture.Width, rawTexture.Height);
        result.SetData(data);
        return result;
    }
}
