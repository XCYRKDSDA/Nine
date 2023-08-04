namespace Nine.Assets;

public class ByteArrayLoader : IAssetLoader<byte[]>
{
    public byte[] Load(AssetsContext context, string asset)
    {
        using var fileStream = context.Open(asset);
        using var memoryStream = new MemoryStream();

        fileStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
