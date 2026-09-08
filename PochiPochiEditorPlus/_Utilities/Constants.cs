namespace PochiPochiEditorPlus._Utilities
{
    public static class Constants
    {
        // IO関連
        public const int HexBase = 16;
        public const int BitsPerByte = 8;
        public const int CharPerByte = 2;
        public const int NibbleShift = 4;
        public const int NibbleMask = 0xF;
        public const int ByteMask = 0xFF;
        public const int UShortMask = 0xFFFF;
        public const uint UIntMask = 0xFFFFFFFFU;
        public const int ByteSize = 1;
        public const int UShortSize = 2;
        public const int UIntSize = 4;
        public const uint BaseAddr = 0x8000000U;
        public const uint EndAddr = 0xFFFFFFFU;
        public const int InvalidValue = -1;
        public const int OffsetDigits = UIntSize * CharPerByte;

        // プレフィックス
        public const string HexPrefix = "0x";
        public const string ButtonPrefix = "btn";
        public const string MenuItemPrefix = "tsmi";
        // 拡張子
        public const string GbaExt = "gba";
        public const string BmpExt = "bmp";
        public const string DefExt = "def";
        public const string IniExt = "ini";

        // 文字バイト
        public const byte PaddingByte = 0x0;
        public const byte FreeSpaceByte = 0xFF;
        public const byte StrNewlineByte = 0xFE;
        public const byte StrTerminatorByte = 0xFF;

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
        public const int PixelsPerByte4Bpp = BitsPerByte / Bpp4;
        public const int SpriteSize = 64;
        public const int DefaultScale = 2;

        // タイルセット
        public const int TilesetImageWidth = 128;
        public const int Tileset1ImageHeight = 320;
        public const int Tileset2ImageMaxHeight = 192;
        public const int Tileset1BlockAmount = Tileset1ImageHeight * Constants.PixelsPerByte4Bpp;
        public const int Tileset2BlockMaxAmount = Tileset2ImageMaxHeight * Constants.PixelsPerByte4Bpp;
        public const int PaletteEntryCount = 16;

        // ダイアログフィルター
        public const string RomFileFilter = "ROMファイル|*.gba";
        public const string SpriteImportFilter = "画像ファイル (*.png;*.bmp)|*.png;*.bmp";
        public const string SpriteExportFilter = "PNG画像 (*.png)|*.png|BMP画像 (*.bmp)|*.bmp";
        public const string BinImportExportFilter = "BINファイル (*.bin)|*.bin";

        // その他
        public enum PartName{ Key, Value }
    }
}
