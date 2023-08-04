namespace Nine.Assets;

public class StringLoader : IAssetLoader<string>
{
    public string Load(AssetsContext context, string asset)
    {
        using var fileStream = context.Open(asset);
        using var textReader = new StreamReader(fileStream);

        return textReader.ReadToEnd();
    }
}
