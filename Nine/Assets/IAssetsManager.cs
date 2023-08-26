namespace Nine.Assets;

public interface IAssetsManager
{
    T Load<T>(string path, bool cache = true);

    void Unload<T>(string path);

    void ClearCache();
}
