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
    public static class TilesetDataConv
    {
        /// <summary>
        /// バイト配列をブロックデータに変換する。
        /// </summary>
        public static TilesetBlockData BytesToTilesetBlockData(
            FieldValueHolder fieldValue)
        {
            var byteValue = (ushort)IoHelper.ReadBytesAsInt(
                fieldValue.BinaryData,
                0,
                fieldValue.Lengths.EntryLength);

            // ビット演算で各データを抽出
            int tileIndex = byteValue & 0x3FF;          // Bit 0-9
            bool reverseX = (byteValue & 0x400) != 0;   // Bit 10
            bool reverseY = (byteValue & 0x800) != 0;   // Bit 11
            int paletteIndex = (byteValue >> 12) & 0xF; // Bit 12-15

            // クラスの生成[cite: 3]
            return new TilesetBlockData(
                tileIndex,
                paletteIndex,
                reverseX,
                reverseY);
        }

        /// <summary>
        /// ブロックデータをバイト配列に変換する。
        /// </summary>
        public static byte[] TilesetBlockDataToBytes(
            TilesetBlockData dataValue,
            FieldValueHolder fieldValue)
        {
            // ushortに結合
            ushort byteValue = (ushort)(
                (dataValue.TileIndex & 0x3FF) |
                (dataValue.ReverseX ? 0x400 : 0) |
                (dataValue.ReverseY ? 0x800 : 0) |
                ((dataValue.PaletteIndex & 0xF) << 12)
            );

            // 戻り値用に整形
            byte[] result = new byte[fieldValue.Lengths.EntryLength];
            IoHelper.WriteIntAsBytes(
                result,
                0,
                byteValue,
                result.Length);
            return result;
        }
    }
}
