using Microsoft.Xna.Framework.Graphics;
using Nine.Graphics;

namespace Nine.Assets;

public class TextureRegionLoader : IAssetLoader<TextureRegion>
{

    public TextureRegion Load(AssetsContext context, string asset)
    {
        if (!asset.Contains(':'))
            return new(context.Load<Texture2D>(asset));

        var parts = asset.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException();
        var atlasFile = parts[0];
        var subTextureName = parts[1];

        var textureAtlas = context.Load<TextureAtlas>(atlasFile);
        return textureAtlas[subTextureName];
    }
}
