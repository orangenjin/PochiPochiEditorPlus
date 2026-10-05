using System;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;

namespace PochiPochiEditorPlus._Managers._MapManager
{
    public static class MapDrawCalc
    {
        // ビット位置
        private const int MapGridCollIndexShift = 10;

        // ビットマスク
        private const ushort MapGridBlockIndexMask = 0x03FF;    // Bit 0-9
        private const ushort MapGridCollIndexMask = 0xFC00;     // Bit 10-15

        /// <summary>
        /// バイト配列をマップマスデータに変換する。
        /// </summary>
        public static MapGridData BytesToMapGridData(FieldValueHolder fieldValue)
        {
            var ushortValue = (ushort)IoHelper.ReadBytesAsLong(
                fieldValue.BinaryData,
                0,
                fieldValue.Lengths.EntryLength);

            // ビット演算で各データを抽出
            int blockIndex = ushortValue & MapGridBlockIndexMask;
            int collIndex = (ushortValue & MapGridCollIndexMask) >> MapGridCollIndexShift;

            // インスタンスを生成
            return new MapGridData(collIndex, blockIndex);
        }

        /// <summary>
        /// バイト配列をマップマスデータに変換するをバイト配列に変換する。
        /// </summary>
        public static byte[] MapGridDataToBytes(
            MapGridData dataValue,
            FieldValueHolder fieldValue)
        {
            // ushortに結合
            ushort ushortValue = (ushort)(
                (dataValue.BlockIndex & MapGridBlockIndexMask) |
                ((dataValue.CollIndex << MapGridCollIndexShift) & MapGridCollIndexMask));

            // 戻り値用に整形
            byte[] result = new byte[fieldValue.Lengths.EntryLength];

            IoHelper.WriteLongAsBytes(
                result,
                0,
                ushortValue,
                result.Length);

            return result;
        }


        /// <summary>
        /// マップマスデータを取得するメソッドを簡素化するため。
        /// </summary>
        public static MapGridData GetBlockTileData(dynamic value)
        {
            return value.GetData<MapGridData>(
                converter: (Func<FieldValueHolder, MapGridData>)BytesToMapGridData);
        }
    }
}
