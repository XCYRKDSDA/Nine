using System.Reflection;
using Zio;
using Zio.FileSystems;

namespace Nine.Assets;

public class ResourceFileSystem : ReadOnlyFileSystem
{
    public Assembly Assembly { get; }

    public ResourceFileSystem(Assembly assembly) : base(new MemoryFileSystem())
    {
        Assembly = assembly;

        foreach (var asset in Assembly.GetManifestResourceNames())
        {
            var path = new UPath(asset).ToAbsolute();
            var directory = path.GetDirectory();
            FallbackSafe.CreateDirectory(directory);
            FallbackSafe.CreateFile(path).Dispose();
        }
    }

    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access,
                                           FileShare share = FileShare.None)
    {
        if (mode != FileMode.Open)
            throw new IOException(FileSystemIsReadOnly);
        if ((access & FileAccess.Write) != 0)
            throw new IOException(FileSystemIsReadOnly);

        return Assembly.GetManifestResourceStream(path.ToRelative().FullName)!;
    }
}
