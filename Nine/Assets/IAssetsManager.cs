using Zio;

namespace Nine.Assets;

public interface IAssetsManager
{
    T Load<T>(in UPath path, bool cache = true);

    void Unload<T>(in UPath path);

    void ClearCache();
}
