using System;
using System.Collections.Generic;
using System.Linq;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers
{
    public sealed class TilesetManager
    {
        public Entry HeaderEntry { get; set; }
        public byte[] ImageData { get; set; }
        public List<byte[]> PaletteData { get; set; }
        public List<Entry> BlockDataEntries { get; set; }
        public List<Entry> BlockAttrEntries { get; set; }

        // コンストラクタで事前に計算
        public List<FieldValueHolder> _headerFields = null;
        public List<FieldValueHolder> _blockFields = null;
        public List<FieldValueHolder> _attrFields = null;
        // 定数用
        private int _baseHeaderOffset = 0;
        private int _headerEntryLength = 0;
        private int _blockDataEntryLength = 0;
        private int _blockAttrEntryLength = 0;
        // 計算用
        private const int TilesetImageWidth = 128;
        private const int Tileset1ImageHeight = 320;
        private const int Tileset2ImageMaxHeight = 192;
        private const int Tileset1BlockAmount = Tileset1ImageHeight * Constants.PixelsPerByte4Bpp;
        private const int Tileset2BlockMaxAmount = Tileset2ImageMaxHeight * Constants.PixelsPerByte4Bpp;
        private const int PaletteEntryCount = 16;
        // 簡易アクセス用
        dynamic _dynamicHeaderEntry = null;

        public enum PaletteKind
        {
            Palette0to6,
            Palette7to12
        }

        public TilesetManager(SharedData sharedData)
        {
            // ヘッダー定義を読み込み
            _headerFields = 
                GenerateContainer(FieldMetaDataReader.Create("TilesetHeaderEntry"));
            // ブロック定義を読み込み
            _blockFields = 
                GenerateContainer(FieldMetaDataReader.Create("BlockDataEntry"));
            // 属性定義を読み込み
            _attrFields =
                GenerateContainer(FieldMetaDataReader.Create("AttrDataEntry"));

            // ヘルパーメソッド
            List<FieldValueHolder> GenerateContainer(List<FieldMetaData> metaData)
            {
                var fields = new List<FieldValueHolder>();
                for (int i = 0; i < metaData.Count; i++)
                {
                    var fieldValue = 
                        new FieldValueHolder(metaData[i], sharedData);
                    fields.Add(fieldValue);
                }
                return fields;
            }

            // 定数を計算
            dynamic dynamicConfig = sharedData.Config;
            _baseHeaderOffset = dynamicConfig.TilesetHeaderBaseOffset;
            _headerEntryLength = _headerFields.Sum(f => f.Lengths.EntryLength);
            _blockDataEntryLength = _blockFields.Sum(f => f.Lengths.EntryLength);
            _blockAttrEntryLength = _attrFields.Sum(f => f.Lengths.EntryLength);
        }

        public void ReadHeader(int tilesetNo, SharedData sharedData)
        {
            var offset = CalcOffset(tilesetNo);

            // 単一エントリーとして読み込む
            HeaderEntry = new Entry(offset, 0, _headerFields);
            _dynamicHeaderEntry = HeaderEntry;

            // タイルデータとパレットデータを読み込む
            ImageData = LoadImage(sharedData);
            PaletteData = LoadPalettes(sharedData);

            // ブロックデータを読み込む
            BlockDataEntries =
                LoadBlockEntries(_dynamicHeaderEntry.BlockDataTableOffset.GetData<int>(), _blockFields);
            // ブロック属性データを読み込む
            BlockAttrEntries =
                LoadBlockEntries(_dynamicHeaderEntry.BlockAttrTableOffset.GetData<int>(), _attrFields);
        }

        private byte[] LoadImage(SharedData sharedData)
        {
            try
            {
                var imageOffset = _dynamicHeaderEntry.ImageOffset.GetData<int>();

                // 圧縮の場合
                if (Convert.ToBoolean(_dynamicHeaderEntry.ImageCompType.GetData<int>()))
                {
                    byte[] decompressed = 
                        ImageHelper.DecompressLZ77(sharedData.RomData, imageOffset);
                    return decompressed ?? Array.Empty<byte>();
                }

                // 非圧縮の場合
                var maxPixelCount = 
                    (_dynamicHeaderEntry.PaletteType.GetData<int>() == (int)PaletteKind.Palette0to6)
                        ? TilesetImageWidth * Tileset1ImageHeight
                        : TilesetImageWidth * Tileset2ImageMaxHeight;
                var maxByteLength = maxPixelCount / Constants.PixelsPerByte4Bpp;

                // バイト数を確定させる
                int byteLength;
                if (_dynamicHeaderEntry.PaletteType.GetData<int>() == (int)PaletteKind.Palette0to6)
                {
                    byteLength = maxByteLength;
                }
                else
                {
                    var paletteOffset = _dynamicHeaderEntry.PaletteOffset.GetData<int>();
                    // 画像データとパレットデータが順に並んでいると想定
                    var expectedBytes = paletteOffset - imageOffset;
                    byteLength = Math.Min(expectedBytes, maxByteLength);
                }

                var imageData = new byte[byteLength];
                Array.Copy(sharedData.RomData, imageOffset, imageData, 0, byteLength);

                return imageData;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        private List<byte[]> LoadPalettes(SharedData sharedData)
        {
            var paletteDataList = new List<byte[]>();
            var paletteDataLength = Constants.PalColorCount * Constants.BytesPerColor;

            try
            {
                var basePaletteOffset = _dynamicHeaderEntry.PaletteOffset.GetData<int>();
                for (int i = 0; i < PaletteEntryCount; i++)
                {
                    var currentPos = basePaletteOffset + i * paletteDataLength;
                    var paletteData = ImageHelper.DecompressPalette(
                        sharedData.RomData,
                        currentPos,
                        isCompressed: false);
                    paletteDataList.Add(paletteData ?? Array.Empty<byte>());
                }
                return paletteDataList;
            }
            catch
            {
                return new List<byte[]>();
            }
        }

        private List<Entry> LoadBlockEntries(int tableOffset, List<FieldValueHolder> fields) 
        {
            var blockCount = CalcBlockCount();
            var entries = new List<Entry>();

            for (int i = 0; i < blockCount; i++)
            {
                entries.Add(new Entry(tableOffset, i, fields));
            }

            return entries;

            // ブロック数を計算する
            int CalcBlockCount()
            {
                if (_dynamicHeaderEntry.PaletteType.GetData<int>() == (int)PaletteKind.Palette0to6)
                {
                    return Tileset1BlockAmount;
                }
                else
                {
                    // ブロックデータとブロック属性が順に並んでいると想定
                    var blockDataTableOffset = _dynamicHeaderEntry.BlockDataTableOffset.GetData<int>();
                    var blockAttrTableOffset = _dynamicHeaderEntry.BlockAttrTableOffset.GetData<int>();
                    var expectedCount = (blockAttrTableOffset - blockDataTableOffset) / _blockDataEntryLength;
                    return Math.Min(expectedCount, Tileset2BlockMaxAmount);
                }
            }
        }

        /// <summary>
        /// タイルセット番号からヘッダーオフセットを計算する。
        /// </summary>
        public int CalcOffset(int tilesetNo)
        {
            return _baseHeaderOffset + (tilesetNo * _headerEntryLength);
        }

        /// <summary>
        /// ヘッダーオフセットからタイルセット番号を計算する。
        /// 完全一致しない場合は失敗する。
        /// </summary>
        public bool TryCalcTilesetNo(int offset, out int tilesetNo)
        {
            tilesetNo = Constants.InvalidValue;

            // 番号0のヘッダーオフセットより小さいの場合
            if (offset < _baseHeaderOffset)
            {
                return false;
            }

            // 0x18の倍数でない場合
            int diff = offset - _baseHeaderOffset;
            if (diff % _headerEntryLength != 0)
            {
                return false;
            }

            tilesetNo = diff / _headerEntryLength;
            return true;
        }

        /// <summary>
        /// 指定されたオフセットに近いタイルセット番号を取得する。
        /// 失敗時は番号-1を返す。
        /// </summary>
        public int CalcNearestTilesetNo(int offset)
        {
            int diff = offset - _baseHeaderOffset;
            return diff < 0
                ? Constants.InvalidValue
                : (diff / _headerEntryLength) + 1;
        }
    }
}
