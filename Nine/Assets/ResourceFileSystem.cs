using System.Reflection;
using Zio;
using Zio.FileSystems;

namespace Nine.Assets;

public class ResourceFileSystem : ReadOnlyFileSystem
{
    public Assembly Assembly { get; }
    public string Namesapce { get; }

    private readonly string _prefix;

    public ResourceFileSystem(Assembly assembly, string @namespace) : base(new MemoryFileSystem(), false)
    {
        Assembly = assembly;
        Namesapce = @namespace;
        _prefix = $"{assembly.GetName().Name}.{@namespace}";

        foreach (var asset in Assembly.GetManifestResourceNames())
        {
            var path = new UPath(asset[_prefix.Length..].Replace('.', '/'));
            var directory = path.GetDirectory();
            FallbackSafe.CreateDirectory(directory);
            FallbackSafe.CreateFile(path).Dispose();
        }
    }

    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
    {
        if (mode != FileMode.Open)
            throw new IOException(FileSystemIsReadOnly);
        if ((access & FileAccess.Write) != 0)
            throw new IOException(FileSystemIsReadOnly);

        var res = ((string)path).Replace('/', '.').Insert(0, _prefix);
        return Assembly.GetManifestResourceStream(res)!;
    }
}
