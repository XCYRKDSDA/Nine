namespace Nine.Assets;

public interface IAssetLoader<T>
{
    T Load(IAssetResolver context, string path);
}
