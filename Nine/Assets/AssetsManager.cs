using Zio;

namespace Nine.Assets;

public class AssetsManager : IAssetsManager
{
    private readonly IFileSystem _assetsFileSystem;
    private readonly Dictionary<Type, object> _assetLoaders = new();
    private readonly Dictionary<Type, Dictionary<UPath, object>> _assetsCache = new();

    public AssetsManager(IFileSystem assetsFileSystem)
    {
        _assetsFileSystem = assetsFileSystem;
    }

    public void RegisterLoader<T>(IAssetLoader<T> loader)
    {
        _assetLoaders.Add(typeof(T), loader);
    }

    public T Load<T>(in UPath path, bool cache = true)
    {
        var absPath = path.ToAbsolute(); //以绝对路径为准

        if (!_assetsCache.TryGetValue(typeof(T), out var typeSpecifiedCache))
        {
            typeSpecifiedCache = new();
            _assetsCache.Add(typeof(T), typeSpecifiedCache);
        }

        if (typeSpecifiedCache.TryGetValue(absPath, out var cached))
            return (T)cached;

        var loader = _assetLoaders[typeof(T)] as IAssetLoader<T> ?? throw new NullReferenceException();
        var asset = loader.Load(_assetsFileSystem, this, absPath) ?? throw new NullReferenceException();

        if (cache)
            typeSpecifiedCache.Add(absPath, asset);

        return asset;
    }

    public void Unload<T>(in UPath path)
    {
        _assetsCache[typeof(T)].Remove(path.ToAbsolute());
    }

    public void ClearCache()
    {
        _assetsCache.Clear();
    }
}
