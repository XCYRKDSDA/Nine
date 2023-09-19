using Zio;

namespace Nine.Assets;

public class ByteArrayLoader : IAssetLoader<byte[]>
{
    public byte[] Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);
        using var memoryStream = new MemoryStream();

        fileStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
