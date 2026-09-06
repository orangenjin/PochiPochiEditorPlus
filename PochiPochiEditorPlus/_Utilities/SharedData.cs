using System;
using PochiPochiEditorPlus._Managers;

namespace PochiPochiEditorPlus._Utilities
{
    public class SharedData
    {
        public byte[] RomData { get; set; }　// 後入れ
        public bool IsRomLoaded { get; set; }
        public ConfigManager Config { get; }
        public CharmapManager Charmap { get; }

        public SharedData(
            ConfigManager config,
            CharmapManager charmap)
        {
            Config = config;
            Charmap = charmap;

            IsRomLoaded = false;
        }

        public void LoadRom(byte[] romData)
        {
            RomData = romData;
            IsRomLoaded = true;
        }

        public void ClearRom()
        {
            RomData = Array.Empty<byte>();
            IsRomLoaded = false;
        }
    }
}
