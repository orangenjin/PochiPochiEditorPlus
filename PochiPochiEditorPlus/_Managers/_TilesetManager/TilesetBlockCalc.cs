using System;
using System.Collections.Generic;
using System.Drawing;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._TilesetManager
{
    public static class TilesetBlockCalc
    {
        // BlockTileDataのビットフィールド
        public enum BlockTileBits
        {
            PaletteIndex = 4,
            ReverseY = 1, 
            ReverseX = 1,
            TileIndex = 10,
        }

        // LayerAndWildEncAttrのビットフィールド
        public enum LayerAndWildEncBits
        {
            Layer = 6, 
            WildEncWater = 1,
            WildEncGrass = 1,
        }

        /// <summary>
        /// バイト配列をブロックデータに変換する。
        /// </summary>
        public static BlockTileData BytesToBlockTileData(FieldValueHolder fieldValue)
        {
            var ushortValue = (ushort)IoHelper.ReadBytesAsLong(
                fieldValue.BinaryData,
                0,
                (DataSize)fieldValue.Lengths.EntryLength);

            // マッピングされたビットフィールドの辞書を取得
            var bits = ConvHelper.BitExtract(
                ushortValue,
                BlockTileBits.PaletteIndex,
                BlockTileBits.ReverseY,
                BlockTileBits.ReverseX,
                BlockTileBits.TileIndex);

            // インスタンスの生成
            return new BlockTileData(
                tileIndex: (int)bits[BlockTileBits.TileIndex],
                paletteIndex: (int)bits[BlockTileBits.PaletteIndex],
                reverseX: bits[BlockTileBits.ReverseX] != 0,
                reverseY: bits[BlockTileBits.ReverseY] != 0);
        }

        /// <summary>
        /// ブロックデータをバイト配列に変換する。
        /// </summary>
        public static byte[] BlockTileDataToBytes(
            BlockTileData dataValue,
            FieldValueHolder fieldValue)
        {
            // 値を辞書に格納する
            var bits = new Dictionary<BlockTileBits, uint>
                {
                    { BlockTileBits.PaletteIndex, (uint)dataValue.PaletteIndex },
                    { BlockTileBits.ReverseY, (uint)(dataValue.ReverseY ? 1 : 0) },
                    { BlockTileBits.ReverseX, (uint)(dataValue.ReverseX ? 1 : 0) },
                    { BlockTileBits.TileIndex, (uint)dataValue.TileIndex }
                };

            // uintに統合する
            uint combined = ConvHelper.BitCombine(
                bits,
                BlockTileBits.PaletteIndex,
                BlockTileBits.ReverseY,
                BlockTileBits.ReverseX,
                BlockTileBits.TileIndex);

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
            byte[] imageData,
            byte[] palData,
            int tileSize,
            bool showBackColor)
        {
            if (imageData == null || imageData.Length == 0 || palData == null) return null;

            Bitmap tileBmp = ImageHelper.CreateBitmap(
                imageData, palData, tileSize, tileSize, showBackColor: showBackColor);

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
        /// 各タイルとマス座標を遅延評価で順次返す。
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
                (DataSize)fieldValue.Lengths.EntryLength);

            // マッピングされたビットフィールドの辞書を取得
            var bits = ConvHelper.BitExtract(
                byteValue,
                LayerAndWildEncBits.Layer,
                LayerAndWildEncBits.WildEncWater,
                LayerAndWildEncBits.WildEncGrass);

            // インスタンスの生成
            return new LayerAndWildEncAttr(
                // 2ビット左シフトする(下位2ビットを0にする)
                layer: (byte)(bits[LayerAndWildEncBits.Layer] << 2),
                wildEncGrass: bits[LayerAndWildEncBits.WildEncGrass] != 0,
                wildEncWater: bits[LayerAndWildEncBits.WildEncWater] != 0);
        }

        /// <summary>
        /// レイヤーと野生設定の属性データをバイト配列に変換する。
        /// </summary>
        public static byte[] LayerAndWildEncAttrToBytes(
            LayerAndWildEncAttr dataValue,
            FieldValueHolder fieldValue)
        {
            // 値を辞書に格納する
            var bits = new Dictionary<LayerAndWildEncBits, uint>
            {
                // 2ビット右シフトする
                { LayerAndWildEncBits.Layer, (uint)(dataValue.Layer >> 2) },
                { LayerAndWildEncBits.WildEncWater, (uint)(dataValue.WildEncWater ? 1 : 0) },
                { LayerAndWildEncBits.WildEncGrass, (uint)(dataValue.WildEncGrass ? 1 : 0) }
            };

            // uintに統合する
            uint combined = ConvHelper.BitCombine(
                bits,
                LayerAndWildEncBits.Layer,
                LayerAndWildEncBits.WildEncWater,
                LayerAndWildEncBits.WildEncGrass);

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
        /// レイヤーと野生設定の属性データを取得するメソッドを簡素化するため。
        /// </summary>
        public static LayerAndWildEncAttr GetLayerAndWildEncAttr(dynamic value)
        {
            return value.GetData<LayerAndWildEncAttr>(
                converter: (Func<FieldValueHolder, LayerAndWildEncAttr>)BytesToLayerAndWildEncAttr);
        }
    }
}
