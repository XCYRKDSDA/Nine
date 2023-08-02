namespace Nine.Assets;

public interface IAssetResolver
{
    bool Exists(string path);

    Stream Open(string path);
}
