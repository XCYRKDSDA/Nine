using Zio;

namespace Nine.Assets;

public class AssetsManager(IFileSystem assetsFileSystem, bool inheritDefaultLoaders = true) : IAssetsManager
{
    private static readonly Dictionary<Type, object> _defaultAssetLoaders = [];

    public static void RegisterDefaultLoader(Type type, object loader)
    {
        _defaultAssetLoaders.Add(type, loader);
    }

    public static void RegisterDefaultLoader<T>(IAssetLoader<T> loader)
    {
        _defaultAssetLoaders.Add(typeof(T), loader);
    }

    private readonly Dictionary<Type, object> _assetLoaders = new();
    private readonly Dictionary<Type, Dictionary<UPath, object>> _assetsCache = new();

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

        var loader = _assetLoaders.TryGetValue(typeof(T), out var localLoader)
                         ? (IAssetLoader<T>)localLoader
                         : inheritDefaultLoaders && _defaultAssetLoaders.TryGetValue(typeof(T), out var defaultLoader)
                             ? (IAssetLoader<T>)defaultLoader
                             : throw new NullReferenceException();
        var asset = loader.Load(assetsFileSystem, this, absPath) ?? throw new NullReferenceException();

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
