using FMOD;
using FMOD.Studio;
using Zio;

namespace Nine.Assets;

public class FmodBankLoader(FMOD.Studio.System fmodSystem) : IAssetLoader<Bank>
{
    private FMOD.Studio.System _fmodSystem = fmodSystem;

    private RESULT _fmodFlag;

    public Bank Load(IFileSystem fs, IAssetsManager assets, in UPath path)
    {
        var bankBytes = fs.ReadAllBytes(path);
        _fmodFlag = _fmodSystem.loadBankMemory(bankBytes, LOAD_BANK_FLAGS.NORMAL, out var bank);
        if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
            throw new Exception($"Failed to load bank {path}");
        _fmodFlag = bank.loadSampleData();
        if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
            throw new Exception($"Failed to load sample data {path}");

        var stringsBankPath = path.FullName.Replace(".bank", ".strings.bank");
        if (fs.FileExists(stringsBankPath))
        {
            var stringsBankBytes = fs.ReadAllBytes(stringsBankPath);
            _fmodFlag = _fmodSystem.loadBankMemory(stringsBankBytes, LOAD_BANK_FLAGS.NORMAL, out var stringsBank);
            if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
                throw new Exception($"Failed to load bank {stringsBankPath}");
            _fmodFlag = stringsBank.loadSampleData();
            if (_fmodFlag != RESULT.OK && _fmodFlag != RESULT.ERR_EVENT_ALREADY_LOADED)
                throw new Exception($"Failed to load sample data {stringsBankPath}");
        }

        return bank;
    }
}
