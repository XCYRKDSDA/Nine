using Zio;

namespace Nine.Assets;

public interface IAssetLoader<T>
{
    T Load(IFileSystem fs, IAssetsManager assets, in UPath path);
}
