using System.Data;
using Nine.Graphics;
using Zio;

namespace Nine.Assets;

public class NinePatchRegionLoader : IAssetLoader<NinePatchRegion>
{
    public NinePatchRegion Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        var file = path.GetName();
        if (!file.Contains(':'))
            throw new NotImplementedException(); // TODO: 允许单独定义一个json

        var parts = file.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException();
        var atlasFile = parts[0];
        var subTextureName = parts[1];

        var directory = path.GetDirectory();
        var textureAtlas = assets.Load<TextureAtlas>(UPath.Combine(directory, atlasFile));
        return textureAtlas[subTextureName] as NinePatchRegion ?? throw new DataException();
    }
}
