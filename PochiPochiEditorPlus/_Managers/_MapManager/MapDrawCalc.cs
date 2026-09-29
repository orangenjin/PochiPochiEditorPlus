using System;
using System.Collections.Generic;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._MapManager
{
    public static class MapDrawCalc
    {
        // MapGridDataのビットフィールド
        public enum MapGridBits
        {
            CollIndex = 6,
            BlockIndex = 10,
        }

        /// <summary>
        /// バイト配列をマップマスデータに変換する。
        /// </summary>
        public static MapGridData BytesToMapGridData(FieldValueHolder fieldValue)
        {
            var ushortValue = (ushort)IoHelper.ReadBytesAsLong(
                fieldValue.BinaryData,
                0,
                (DataSize)fieldValue.Lengths.EntryLength);

            // マッピングされたビットフィールドの辞書を取得
            var bits = ConvHelper.BitExtract<MapGridBits>(ushortValue);

            // インスタンスを生成
            return new MapGridData(
                collIndex: (int)bits[MapGridBits.CollIndex],
                blockIndex: (int)bits[MapGridBits.BlockIndex]);
        }

        /// <summary>
        /// マップマスデータをバイト配列に変換する。
        /// </summary>
        public static byte[] MapGridDataToBytes(
            MapGridData dataValue,
            FieldValueHolder fieldValue)
        {
            // 値を辞書に格納する
            var bits = new Dictionary<MapGridBits, uint>
            {
                { MapGridBits.CollIndex, (uint)dataValue.CollIndex },
                { MapGridBits.BlockIndex, (uint)dataValue.BlockIndex }
            };

            // uintに統合する
            uint combined = ConvHelper.BitCombine(bits);

            // 戻り値に書き込む
            byte[] result = new byte[fieldValue.Lengths.EntryLength];
            IoHelper.WriteLongAsBytes(
                result,
                0,
                (long)combined,
                (DataSize)result.Length);

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
