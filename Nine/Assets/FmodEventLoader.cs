using FMOD;
using FMOD.Studio;
using Zio;

namespace Nine.Assets;

public class FmodEventLoader(FMOD.Studio.System fmodSystem) : IAssetLoader<EventDescription>
{
    private FMOD.Studio.System _fmodSystem = fmodSystem;

    private RESULT _fmodFlag;

    public EventDescription Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        if (!path.FullName.Contains(':')) throw new Exception("Invalid path");
        var parts = path.FullName.Split(':');
        if (parts.Length != 2) throw new Exception("Invalid path");

        var bankPath = parts[0];
        var directory = path.GetDirectory();
        _ = assets.Load<Bank>(UPath.Combine(directory, bankPath));

        var eventPath = Guid.TryParse(parts[1], out _) ? parts[1] : $"event:{parts[1]}";
        _fmodFlag = _fmodSystem.getEvent(eventPath, out var eventDescription);
        if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
            throw new Exception($"Failed to load event {eventPath}");

        _fmodFlag = eventDescription.loadSampleData();
        if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
            throw new Exception($"Failed to load sample data {eventPath}");

        return eventDescription;
    }
}
