using FontStashSharp;
using Zio;

namespace Nine.Assets;

public class FontSystemLoader : IAssetLoader<FontSystem>
{
    public FontSystem Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        var fontSystem = new FontSystem();

        var directory = path.GetDirectory();
        foreach (var file in path.GetName().Split(':'))
            fontSystem.AddFont(fs.OpenFile(UPath.Combine(directory, file), FileMode.Open, FileAccess.Read));

        return fontSystem;
    }
}
