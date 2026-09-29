namespace PochiPochiEditorPlus._Utilities
{
    public static class BinaryConstants
    {
        // 基数
        public const int HexBase = 16;
        public const int BitsPerByte = 8;
        public const int NibbleShift = 4;

        // 1バイトに存在する16進数文字数
        public const int HexCharsPerByte = BitsPerByte / NibbleShift;

        // ビットマスク
        public const int NibbleMask = 0x0F;
        public const int ByteMask = 0xFF;

        // アドレス範囲
        public const uint BaseAddr = 0x08000000U;
        public const uint EndAddr = 0x0FFFFFFFU;

        // 桁数
        public const int OffsetDigits = sizeof(uint) * HexCharsPerByte;

        // null値
        public const int InvalidValue = -1;

        // 文字バイト
        public const byte PaddingByte = 0x0;
        public const byte FreeSpaceByte = 0xFF;
        public const byte StrNewlineByte = 0xFE;
        public const byte StrTerminatorByte = 0xFF;
    }

    public static class PrefixConstants
    {
        public const string HexPrefix = "0x";
        public const string ButtonPrefix = "btn";
        public const string MenuItemPrefix = "tsmi";
    }

    public static class ExtConstants
    {
        public const string GbaExt = "gba";
        public const string BmpExt = "bmp";
        public const string DefExt = "def";
        public const string IniExt = "ini";
    }

    public static class FilterConstants
    {
        public const string RomFileFilter = "ROMファイル|*.gba";
        public const string SpriteImportFilter = "画像ファイル (*.png;*.bmp)|*.png;*.bmp";
        public const string SpriteExportFilter = "PNG画像 (*.png)|*.png|BMP画像 (*.bmp)|*.bmp";
        public const string BinImportExportFilter = "BINファイル (*.bin)|*.bin";
    }

    public static class ImageConstants
    {
        // パレット
        public const int PalColorCount = 16;
        public const int BytesPerColor = 2;
        public const int ColorChannelMulti = 8;
        public const int RedShift = 0;
        public const int GreenShift = 5;
        public const int BlueShift = 10;
        public const int RedMask = 0x1F;
        public const int GreenMask = 0x3E0;
        public const int BlueMask = 0x7C00;

        // 画像
        public const int TileSize = 8;
        public const int Bpp4 = 4;
        public const int PixelsPerByte = BinaryConstants.BitsPerByte / Bpp4;
        public const int BytesPerTile = TileSize * TileSize / PixelsPerByte;
        public const int SpriteSize = 64;
        public const int DefaultScale = 2;
    }

    public static class TilesetConstants
    {
        public const int TilesetImageWidth = 128;
        public const int Tileset1ImageHeight = 320;
        public const int Tileset2ImageMaxHeight = 192;
        public const int Tileset1BlockAmount = Tileset1ImageHeight * ImageConstants.PixelsPerByte;
        public const int Tileset2BlockMaxAmount = Tileset2ImageMaxHeight * ImageConstants.PixelsPerByte;
        public const int PaletteEntryCount = 16;
        public const int TilePerBlockSide = 2;
    }

    public enum DataSize
    {
        Byte = sizeof(byte),
        UShort = sizeof(ushort),
        UInt = sizeof(uint),
    }

    public enum PartName
    {
        Key = 0,
        Value = 1,
    }
}
