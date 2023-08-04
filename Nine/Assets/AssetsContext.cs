using System.Runtime.CompilerServices;

namespace Nine.Assets;

public class AssetsContext : IAssetResolver, IAssetsManager
{
    private readonly IAssetResolver _assetResolver;
    private readonly IAssetsManager _assetsManager;
    private readonly string _directory;

    public AssetsContext(IAssetResolver assetResolver, IAssetsManager assetsManager, string directory)
    {
        _assetResolver = assetResolver;
        _assetsManager = assetsManager;
        _directory = directory;
    }

    #region IAssetResolver

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(string path) => _assetResolver.Exists(Path.Combine(_directory, path));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream Open(string path) => _assetResolver.Open(Path.Combine(_directory, path));

    #endregion

    #region IAssetsManager

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Load<T>(string path, bool cache = true) => _assetsManager.Load<T>(Path.Combine(_directory, path), cache);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Unload(string path) => _assetsManager.Unload(Path.Combine(_directory, path));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearCache() => _assetsManager.ClearCache();

    #endregion
}
