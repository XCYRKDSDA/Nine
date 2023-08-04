using System.Reflection;
using System.Runtime.Loader;

namespace Nine.Assets;

public class AssemblyLoader : IAssetLoader<Assembly>
{
    public AssemblyLoadContext Domain { get; }

    public AssemblyLoader(AssemblyLoadContext domain)
    {
        Domain = domain;
    }

    public Assembly Load(AssetsContext context, string asset)
    {
        using var fileStream = context.Open(asset);

        return Domain.LoadFromStream(fileStream);
    }
}
