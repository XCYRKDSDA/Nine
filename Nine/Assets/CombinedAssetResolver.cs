namespace Nine.Assets;

public class CombinedAssetResolver : IAssetResolver
{
    public IAssetResolver[] SubResolvers { get; }

    public CombinedAssetResolver(params IAssetResolver[] subResolvers)
    {
        SubResolvers = subResolvers;
    }

    public bool Exists(string path)
    {
        foreach (var resolver in SubResolvers)
        {
            if (resolver.Exists(path))
                return true;
        }
        return false;
    }

    public Stream Open(string path)
    {
        foreach (var resolver in SubResolvers)
        {
            if (resolver.Exists(path))
                return resolver.Open(path);
        }
        throw new FileNotFoundException("The path can not be found in any of sub-resolvers");
    }
}
