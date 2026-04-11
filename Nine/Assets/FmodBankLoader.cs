using FMOD;
using FMOD.Studio;
using Zio;

namespace Nine.Assets;

public sealed class SafeFmodBank(Bank bankHandle) : IDisposable
{
    public Bank Native => bankHandle;

    private bool _disposed = false;

    public void Dispose()
    {
        if (_disposed)
            return;

        var flag = RESULT.OK;

        flag = bankHandle.unloadSampleData();
        if (flag != RESULT.OK)
            throw new Exception($"Failed to unload sample data of a bank with {flag}");

        flag = bankHandle.unload();
        if (flag != RESULT.OK)
            throw new Exception($"Failed to unload a bank with {flag}");

        _disposed = true;
    }
}

public class FmodBankLoader(FMOD.Studio.System fmodSystem) : IAssetLoader<SafeFmodBank>
{
    private FMOD.Studio.System _fmodSystem = fmodSystem;

    private RESULT _fmodFlag;

    public SafeFmodBank Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        var bankBytes = fs.ReadAllBytes(path);
        _fmodFlag = _fmodSystem.loadBankMemory(bankBytes, LOAD_BANK_FLAGS.NORMAL, out var bank);
        if (_fmodFlag != RESULT.OK)
            throw new Exception($"Failed to load bank {path} with {_fmodFlag}");
        _fmodFlag = bank.loadSampleData();
        if (_fmodFlag != RESULT.OK)
            throw new Exception($"Failed to load sample data {path} with {_fmodFlag}");

        if (!path.FullName.EndsWith(".strings.bank"))
        {
            // 当资产不是 strings 时, 尝试加载它的 strings 资产
            var stringsBankPath = path.FullName.Replace(".bank", ".strings.bank");
            if (fs.FileExists(stringsBankPath))
                _ = assets.Load<SafeFmodBank>(stringsBankPath);
        }

        return new SafeFmodBank(bank);
    }
}
