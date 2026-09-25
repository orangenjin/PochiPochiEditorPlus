using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Managers._LayerManager;
using PochiPochiEditorPlus._Managers._TilesetManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._TilesetManager
{
    public static class TilesetDataConv
    {
        // ビット位置
        private const int BlockDataPaletteShift = 12;

        // ビットマスク
        private const ushort BlockDataTileIndexMask = 0x03FF;    // Bit 0-9
        private const ushort BlockDataReverseXMask = 0x0400;     // Bit 10
        private const ushort BlockDataReverseYMask = 0x0800;     // Bit 11
        private const ushort BlockDataPaletteMask = 0xF000;      // Bit 12-15

        /// <summary>
        /// バイト配列をブロックデータに変換する。
        /// </summary>
        public static BlockTileData BytesToBlockLayerData(
            FieldValueHolder fieldValue)
        {
            var byteValue = (ushort)IoHelper.ReadBytesAsInt(
                fieldValue.BinaryData,
                0,
                fieldValue.Lengths.EntryLength);

            // ビット演算で各データを抽出
            int tileIndex = byteValue & BlockDataTileIndexMask;
            bool reverseX = (byteValue & BlockDataReverseXMask) != 0;
            bool reverseY = (byteValue & BlockDataReverseYMask) != 0;
            int paletteIndex = (byteValue & BlockDataPaletteMask) >> BlockDataPaletteShift;

            // インスタンスの生成
            return new BlockTileData(
                tileIndex,
                paletteIndex,
                reverseX,
                reverseY);
        }

        /// <summary>
        /// タイルデータを取得するメソッドを簡素化するため。
        /// </summary>
        public static BlockTileData GetBlockLayerData(dynamic value)
        {
            return value.GetData<BlockTileData>(
                converter: (Func<FieldValueHolder, BlockTileData>)BytesToBlockLayerData);
        }

        /// <summary>
        /// ブロックデータをバイト配列に変換する。
        /// </summary>
        public static byte[] BlockLayerDataToBytes(
            BlockTileData dataValue,
            FieldValueHolder fieldValue)
        {
            // ushortに結合
            ushort byteValue = (ushort)(
                (dataValue.TileIndex & BlockDataTileIndexMask) |
                (dataValue.ReverseX ? BlockDataReverseXMask : 0) |
                (dataValue.ReverseY ? BlockDataReverseYMask : 0) |
                ((dataValue.PaletteIndex & 0xF) << BlockDataPaletteShift)
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
