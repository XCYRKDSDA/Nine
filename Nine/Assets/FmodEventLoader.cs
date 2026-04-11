using FMOD;
using FMOD.Studio;
using Zio;

namespace Nine.Assets;

public sealed class SafeFmodEventDescription(EventDescription eventDescriptionHandle) : IDisposable
{
    public EventDescription Native => eventDescriptionHandle;

    private bool _disposed = false;

    public void Dispose()
    {
        if (_disposed)
            return;

        var flag = RESULT.OK;

        // 世界中创建的 instance, 如果没有自行释放掉的话, 在此处强行全部释放
        flag = eventDescriptionHandle.releaseAllInstances();
        if (flag != RESULT.OK)
            throw new Exception($"Failed to release instances of a description with {flag}");

        flag = eventDescriptionHandle.unloadSampleData();
        if (flag != RESULT.OK)
            throw new Exception($"Failed to unload sample data of a description with {flag}");

        _disposed = true;
    }
}

public class FmodEventLoader(FMOD.Studio.System fmodSystem) : IAssetLoader<SafeFmodEventDescription>
{
    private FMOD.Studio.System _fmodSystem = fmodSystem;

    private RESULT _fmodFlag;

    public SafeFmodEventDescription Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        if (!path.FullName.Contains(':'))
            throw new Exception("Invalid path");
        var parts = path.FullName.Split(':');
        if (parts.Length != 2)
            throw new Exception("Invalid path");

        var bankPath = parts[0];
        var directory = path.GetDirectory();
        _ = assets.Load<SafeFmodBank>(UPath.Combine(directory, bankPath));

        var eventPath = Guid.TryParse(parts[1], out _) ? parts[1] : $"event:{parts[1]}";
        _fmodFlag = _fmodSystem.getEvent(eventPath, out var eventDescription);
        if (_fmodFlag != RESULT.OK)
            throw new Exception($"Failed to load event {eventPath} with {_fmodFlag}");

        _fmodFlag = eventDescription.loadSampleData();
        if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
            throw new Exception($"Failed to load sample data {eventPath} with {_fmodFlag}");

        return new SafeFmodEventDescription(eventDescription);
    }
}
