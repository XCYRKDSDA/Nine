namespace Nine.Assets;

public class StringLoader : IAssetLoader<string>
{
    public string Load(IAssetResolver context, string path)
    {
        using var fileStream = context.Open(path);
        using var textReader = new StreamReader(fileStream);

        return textReader.ReadToEnd();
    }
}
