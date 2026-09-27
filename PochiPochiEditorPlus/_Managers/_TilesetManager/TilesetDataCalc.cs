using System;
using System.Collections.Generic;
using System.Drawing;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._TilesetManager
{
    public static class TilesetDataCalc
    {
        // ビット位置
        private const int BlockDataPaletteShift = 12;

        // ビットマスク
        private const ushort BlockDataTileIndexMask = 0x03FF;    // Bit 0-9
        private const ushort BlockDataReverseXMask = 0x0400;     // Bit 10
        private const ushort BlockDataReverseYMask = 0x0800;     // Bit 11
        private const ushort BlockDataPaletteMask = 0xF000;      // Bit 12-15
        private const byte WildEncGrassMask = 0x01;             // Bit 0
        private const byte WildEncWaterMask = 0x02;             // Bit 1
        private const byte LayerAndWildEncLayerMask = 0xFC;     // Bit 2-7

        /// <summary>
        /// バイト配列をブロックデータに変換する。
        /// </summary>
        public static BlockTileData BytesToBlockTileData(FieldValueHolder fieldValue)
        {
            var ushortValue = (ushort)IoHelper.ReadBytesAsLong(
                fieldValue.BinaryData,
                0,
                fieldValue.Lengths.EntryLength);

            // ビット演算で各データを抽出
            int tileIndex = ushortValue & BlockDataTileIndexMask;
            bool reverseX = (ushortValue & BlockDataReverseXMask) != 0;
            bool reverseY = (ushortValue & BlockDataReverseYMask) != 0;
            int paletteIndex = (ushortValue & BlockDataPaletteMask) >> BlockDataPaletteShift;

            // インスタンスの生成
            return new BlockTileData(
                tileIndex,
                paletteIndex,
                reverseX,
                reverseY);
        }

        /// <summary>
        /// ブロックデータをバイト配列に変換する。
        /// </summary>
        public static byte[] BlockTileDataToBytes(
            BlockTileData dataValue,
            FieldValueHolder fieldValue)
        {
            // ushortに結合
            ushort ushortValue = (ushort)(
                (dataValue.TileIndex & BlockDataTileIndexMask) |
                (dataValue.ReverseX ? BlockDataReverseXMask : 0) |
                (dataValue.ReverseY ? BlockDataReverseYMask : 0) |
                ((dataValue.PaletteIndex & Constants.NibbleMask) << BlockDataPaletteShift));

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
        /// タイルデータを取得するメソッドを簡素化するため。
        /// </summary>
        public static BlockTileData GetBlockTileData(dynamic value)
        {
            return value.GetData<BlockTileData>(
                converter: (Func<FieldValueHolder, BlockTileData>)BytesToBlockTileData);
        }

        /// <summary>
        /// エントリーからブロックデータを構築する。
        /// </summary>
        public static BlockData GetBlockData(int index, dynamic entry)
        {
            // 下位レイヤー
            var lowerLayer = new BlockLayer(
                GetBlockTileData(entry.LowerTopLeft),
                GetBlockTileData(entry.LowerTopRight),
                GetBlockTileData(entry.LowerBottomLeft),
                GetBlockTileData(entry.LowerBottomRight)
            );

            // 上位レイヤー
            var upperLayer = new BlockLayer(
                GetBlockTileData(entry.UpperTopLeft),
                GetBlockTileData(entry.UpperTopRight),
                GetBlockTileData(entry.UpperBottomLeft),
                GetBlockTileData(entry.UpperBottomRight)
            );

            return new BlockData(index, lowerLayer, upperLayer);
        }

        /// <summary>
        /// ブロックの画像とパレットと反転設定からタイル画像を生成する。
        /// </summary>
        public static Bitmap CreateTileImage(
            BlockTileData tileData,
            byte[] tileBytes,
            byte[] palData,
            int tileSize,
            bool showBackColor)
        {
            if (tileBytes == null || tileBytes.Length == 0 || palData == null) return null;

            Bitmap tileBmp = ImageHelper.CreateBitmap(
                tileBytes, palData, tileSize, tileSize, showBackColor: showBackColor);

            RotateFlipType flipType = RotateFlipType.RotateNoneFlipNone;
            if (tileData.ReverseX && tileData.ReverseY)
            {
                flipType = RotateFlipType.RotateNoneFlipXY;
            }
            else if (tileData.ReverseX)
            {
                flipType = RotateFlipType.RotateNoneFlipX;
            }
            else if (tileData.ReverseY)
            {
                flipType = RotateFlipType.RotateNoneFlipY;
            }

            if (flipType != RotateFlipType.RotateNoneFlipNone)
            {
                tileBmp.RotateFlip(flipType);
            }

            return tileBmp;
        }

        /// <summary>
        /// 各タイルとマス座標を順次返す。
        /// </summary>
        public static IEnumerable<(BlockTileData Tile, int OffsetX, int OffsetY)> GetTilesWithOffset(BlockLayer layer)
        {
            if (layer == null) yield break;
            yield return (layer.TopLeft, 0, 0);
            yield return (layer.TopRight, 1, 0);
            yield return (layer.BottomLeft, 0, 1);
            yield return (layer.BottomRight, 1, 1);
        }

        /// <summary>
        /// バイト配列をレイヤーと野生設定の属性データに変換する。
        /// </summary>
        public static LayerAndWildEncAttr BytesToLayerAndWildEncAttr(FieldValueHolder fieldValue)
        {
            var byteValue = (byte)IoHelper.ReadBytesAsLong(
                fieldValue.BinaryData,
                0,
                fieldValue.Lengths.EntryLength);

            // ビット演算で各データを抽出
            bool wildEncGrass = (byteValue & WildEncGrassMask) != 0;
            bool wildEncWater = (byteValue & WildEncWaterMask) != 0;
            byte layer = (byte)(byteValue & LayerAndWildEncLayerMask);

            // インスタンスの生成
            return new LayerAndWildEncAttr(
                layer,
                wildEncGrass,
                wildEncWater);
        }

        /// <summary>
        /// レイヤーと野生設定の属性データをバイト配列に変換する。
        /// </summary>
        public static byte[] LayerAndWildEncAttrToBytes(
            LayerAndWildEncAttr dataValue,
            FieldValueHolder fieldValue)
        {
            // byteに結合
            byte byteValue = (byte)(
                (dataValue.Layer & LayerAndWildEncLayerMask) |
                (dataValue.WildEncGrass ? WildEncGrassMask : 0) |
                (dataValue.WildEncWater ? WildEncWaterMask : 0));

            // 戻り値
            byte[] result = new byte[fieldValue.Lengths.EntryLength];
            IoHelper.WriteLongAsBytes(
                result,
                0,
                byteValue,
                result.Length);
            return result;
        }

        /// <summary>
        /// レイヤーと野生設定の属性データを取得するメソッドを簡素化するため。
        /// </summary>
        public static LayerAndWildEncAttr GetLayerAndWildEncAttr(dynamic value)
        {
            return value.GetData<LayerAndWildEncAttr>(
                converter: (Func<FieldValueHolder, LayerAndWildEncAttr>)BytesToLayerAndWildEncAttr);
        }
    }
}
