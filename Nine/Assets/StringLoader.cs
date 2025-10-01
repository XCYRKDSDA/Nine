using Zio;

namespace Nine.Assets;

public class StringLoader : IAssetLoader<string>
{
    public string Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);
        using var textReader = new StreamReader(fileStream);

        return textReader.ReadToEnd();
    }
}
