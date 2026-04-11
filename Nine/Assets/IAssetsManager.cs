using Zio;

namespace Nine.Assets;

public interface IAssetsManager : IDisposable
{
    T Load<T>(in UPath path, bool cache = true);

    void Unload<T>(in UPath path);
}
