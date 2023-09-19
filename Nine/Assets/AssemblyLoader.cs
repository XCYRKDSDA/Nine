using System.Reflection;
using System.Runtime.Loader;
using Zio;

namespace Nine.Assets;

public class AssemblyLoader : IAssetLoader<Assembly>
{
    public AssemblyLoadContext Domain { get; }

    public AssemblyLoader(AssemblyLoadContext domain)
    {
        Domain = domain;
    }

    public Assembly Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        using var fileStream = fs.OpenFile(path, FileMode.Open, FileAccess.Read);

        return Domain.LoadFromStream(fileStream);
    }
}
