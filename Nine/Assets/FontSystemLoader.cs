using FontStashSharp;

namespace Nine.Assets;

public class FontSystemLoader : IAssetLoader<FontSystem>
{
    public FontSystem Load(AssetsContext context, string asset)
    {
        var fontSystem = new FontSystem();
        foreach (var font in asset.Split(','))
            fontSystem.AddFont(context.Open(font));

        return fontSystem;
    }
}
