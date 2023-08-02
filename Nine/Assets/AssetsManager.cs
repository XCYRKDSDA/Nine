namespace Nine.Assets;

public class AssetsManager : IAssetsManager
{
    private readonly IAssetResolver _assetResolver;
    private readonly Dictionary<Type, object> _assetLoaders = new();
    private readonly Dictionary<string, object> _assetsCache = new();

    public AssetsManager(IAssetResolver assetResolver)
    {
        _assetResolver = assetResolver;
    }

    public void RegisterLoader<T>(IAssetLoader<T> loader)
    {
        _assetLoaders.Add(typeof(T), loader);
    }

    public T Load<T>(string path, bool cache = true)
    {
        if (_assetsCache.TryGetValue(path, out var cached))
            return (T)cached;

        var loader = _assetLoaders[typeof(T)] as IAssetLoader<T> ?? throw new NullReferenceException();
        var asset = loader.Load(_assetResolver, path) ?? throw new NullReferenceException();

        if (cache)
            _assetsCache.Add(path, asset);

        return asset;
    }

    public void Unload(string path)
    {
        _assetsCache.Remove(path);
    }

    public void ClearCache()
    {
        _assetsCache.Clear();
    }
}
