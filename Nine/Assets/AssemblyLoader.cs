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

    public Assembly Load(IAssetResolver context, string path)
    {
        using var fileStream = context.Open(path);

        return Domain.LoadFromStream(fileStream);
    }
}
