using System.Reflection;

namespace Nine.Assets;

public class ResourceAssetResolver : IAssetResolver
{
    public Assembly Assembly { get; }
    public string Namesapce { get; }

    private readonly HashSet<string> assets_;

    public ResourceAssetResolver(Assembly assembly, string @namespace)
    {
        Assembly = assembly;
        Namesapce = @namespace;
        assets_ = new(assembly.GetManifestResourceNames());
    }

    private string ConvertToAssemblyPath(string path) => $"{Assembly.GetName().Name}.{Namesapce}.{path.Replace('/', '.').Replace('\\', '.')}".Replace("..", ".");

    public bool Exists(string path)
    {
        path = ConvertToAssemblyPath(path);
        return assets_.Contains(path);
    }

    public Stream Open(string path)
    {
        path = ConvertToAssemblyPath(path);
        return Assembly.GetManifestResourceStream(path) ?? throw new FileNotFoundException();
    }
}
