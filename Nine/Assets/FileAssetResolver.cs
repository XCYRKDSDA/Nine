namespace Nine.Assets;

public class FileAssetResolver : IAssetResolver
{
    public string BaseFolder { get; }

    public FileAssetResolver(string baseFolder)
    {
        BaseFolder = baseFolder;
    }

    public bool Exists(string path)
    {
        path = Path.Combine(BaseFolder, path);
        return File.Exists(path);
    }

    public Stream Open(string path)
    {
        path = Path.Combine(BaseFolder, path);
        return File.OpenRead(path);
    }
}
