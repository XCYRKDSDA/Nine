namespace Nine.Assets;

public class ByteArrayLoader : IAssetLoader<byte[]>
{
    public byte[] Load(IAssetResolver context, string path)
    {
        using var fileStream = context.Open(path);
        using var memoryStream = new MemoryStream();

        fileStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
