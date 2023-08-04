namespace Nine.Assets;

public interface IAssetLoader<T>
{
    T Load(AssetsContext context, string asset);
}
