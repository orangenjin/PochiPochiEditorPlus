using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._TilesetManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._TilesetManager
{
    public sealed class TilesetBlockData
    {
        public int TileIndex { get; set; }
        public bool ReverseX { get; set; }
        public bool ReverseY { get; set; }
        public int PaletteIndex { get; set; }

        public TilesetBlockData(
            int tileIndex, 
            int paletteIndex,
            bool reverseX = false,
            bool reverseY = false)
        {
            TileIndex = tileIndex;
            PaletteIndex = paletteIndex;

            // 初期設定では反転なし
            ReverseX = reverseX;
            ReverseY = reverseY;
        }

        /// <summary>
        /// バイト配列をブロックデータに変換する。
        /// </summary>
        public static TilesetBlockData BytesToTilesetBlockData(
            FieldValueHolder fieldValue,
            int argIndex,
            CharmapManager charmap)
        {
            var  rawValue = (ushort)IoHelper.ReadBytesAsInt(
                fieldValue.BinaryData, 
                fieldValue.Offset, 
                Constants.UShortSize);

            // ビット演算で各データを抽出
            int tileIndex = rawValue & 0x3FF;          // Bit 0-9
            bool reverseX = (rawValue & 0x400) != 0;   // Bit 10
            bool reverseY = (rawValue & 0x800) != 0;   // Bit 11
            int paletteIndex = (rawValue >> 12) & 0xF; // Bit 12-15

            // クラスの生成[cite: 3]
            return new TilesetBlockData(
                tileIndex, 
                paletteIndex,
                reverseX,
                reverseY);
        }


    }
}
