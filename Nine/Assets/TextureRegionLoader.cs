using Microsoft.Xna.Framework.Graphics;
using Nine.Graphics;
using Zio;

namespace Nine.Assets;

public class TextureRegionLoader : IAssetLoader<TextureRegion>
{
    public TextureRegion Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        var file = path.GetName();
        if (!file.Contains(':'))
            return new(assets.Load<Texture2D>(path));

        var parts = file.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException();
        var atlasFile = parts[0];
        var subTextureName = parts[1];

        var directory = path.GetDirectory();
        var textureAtlas = assets.Load<TextureAtlas>(UPath.Combine(directory, atlasFile));
        return textureAtlas[subTextureName];
    }
}
