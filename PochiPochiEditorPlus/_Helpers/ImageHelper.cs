using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers
{
    public static class ImageHelper
    {
        private const int LZ77HeaderSize = 4;
        private const byte LZ77HeaderIdentifier = 0x10;
        private const int LZ77UnitSize = 2;
        private const int LZ77MinLength = LZ77UnitSize + 1;
        private const int LZ77MaxLength = LZ77MinLength + ((1 << (int)LZ77UnitField.Length) - 1);
        private const int LZ77MinDistance = 2;
        private const int LZ77MaxDistance = 1 << (int)LZ77UnitField.Distance;

        public enum LZ77UnitField
        {
            Length = 4,
            Distance = 12,
        }

        /// <summary>
        /// LZ77圧縮されたデータを解凍する。
        /// </summary>
        public static byte[] DecompressLZ77(byte[] buffer, int offset = 0)
        {
            // ヘッダの読み込み
            var header = (uint)IoHelper.ReadBytesAsLong(buffer, offset, (DataSize)LZ77HeaderSize);
            // 先頭1バイトは識別子(LZ77HeaderIdentifier)
            // 後半残り3バイトは解凍後のサイズ
            int decompressedSize = (int)header >> BinaryConstants.BitsPerByte;

            // 戻り値を確保
            var result = new byte[decompressedSize];
            // 戻り値の位置
            int dstPos = 0;

            // 参照元の位置
            int srcPos = offset + LZ77HeaderSize;

            while (dstPos < decompressedSize)
            {
                // フラグバイトを読み込む
                // 後続する8個のデータの圧縮状態を示す
                int flagByte = (int)buffer[srcPos++];

                // 左端のビットから1ビットずつ確認
                for (int i = 0; i < BinaryConstants.BitsPerByte; i++)
                {
                    if (dstPos >= decompressedSize) break;

                    // 対象ビット(位置i)が1であれば圧縮、0であれば非圧縮
                    bool isCompressed = (flagByte & (1 << (BinaryConstants.BitsPerByte - 1 - i))) != 0;

                    if (isCompressed)
                    {
                        // 圧縮データを2バイト分読み込む(ビッグエンディアン)
                        var value = (uint)IoHelper.ReadBytesAsLong(
                            buffer,
                            srcPos,
                            (DataSize)LZ77UnitSize,
                            isLittleEndian: false);
                        srcPos += LZ77UnitSize;

                        // ビットを分割して、解析する
                        var fields = ConvHelper.BitExtract(value, LZ77UnitField.Length, LZ77UnitField.Distance);

                        // 上位4ビットから長さ(3から18の範囲)を計算
                        int length = (int)fields[LZ77UnitField.Length] + LZ77MinLength;

                        // 下位12ビットから距離を計算
                        int distance = (int)fields[LZ77UnitField.Distance] + 1;

                        // コピー元の位置を特定
                        int copyPos = dstPos - distance;

                        // 解凍したデータから1バイトずつコピー
                        for (int j = 0; j < length; j++)
                        {
                            if (dstPos >= decompressedSize) break;
                            result[dstPos++] = result[copyPos++];
                        }
                    }
                    else
                    {
                        // 非圧縮データを1バイトコピー
                        result[dstPos++] = buffer[srcPos++];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// データをLZ77に圧縮する。
        /// </summary>
        public static byte[] CompressLZ77(byte[] buffer)
        {
            // 圧縮データ格納用
            var result = new List<byte>(buffer.Length);

            // ヘッダの書き込み
            // 先頭に識別子(LZ77HeaderIdentifier)
            // 続く3バイトに非圧縮時のサイズ
            result.Add(LZ77HeaderIdentifier);
            for (int i = 0; i < LZ77HeaderSize - 1; i++)
            {
                result.Add((byte)((buffer.Length >> (i * BinaryConstants.BitsPerByte)) & BinaryConstants.ByteMask));
            }

            int currentPos = 0;
            while (currentPos < buffer.Length)
            {
                // フラグバイトの位置と値を仮置き
                int flagPos = result.Count;
                byte flagValue = 0;
                result.Add(flagValue);

                // 8個のデータの圧縮処理
                for (int i = 0; i < BinaryConstants.BitsPerByte; i++)
                {
                    if (currentPos >= buffer.Length) break;

                    // データから最大の距離と長さを探索する
                    var (distance, length) = FindLongestMatch(buffer, currentPos);

                    if (length >= LZ77MinLength)
                    {
                        // 長さが最小(3バイト)を満たす場合、圧縮フラグを立てる
                        flagValue |= (byte)(1 << (BinaryConstants.BitsPerByte - 1 - i));

                        int distanceValue = distance - 1;
                        int lengthValue = length - LZ77MinLength;

                        // 圧縮用の値を辞書に設定
                        var unitValues = new Dictionary<LZ77UnitField, uint>
                        {
                            { LZ77UnitField.Length, (uint)lengthValue },
                            { LZ77UnitField.Distance, (uint)distanceValue }
                        };

                        // uintに値を結合
                        uint combinedValue = ConvHelper.BitCombine(
                            unitValues,
                            LZ77UnitField.Length,
                            LZ77UnitField.Distance);

                        // ビッグエンディアンで書き込み
                        var tempBuffer = new byte[LZ77UnitSize];
                        IoHelper.WriteLongAsBytes(
                            tempBuffer,
                            0,
                            combinedValue,
                            (DataSize)LZ77UnitSize,
                            isLittleEndian: false);
                        result.AddRange(tempBuffer);

                        currentPos += length;
                    }
                    else
                    {
                        // 圧縮できない場合はそのまま書き込む
                        result.Add(buffer[currentPos++]);
                    }
                }

                // 仮置きしたフラグバイトを上書き
                result[flagPos] = flagValue;
            }

            // データサイズが4の倍数バイトになるように調整
            while (result.Count % (int)DataSize.UInt != 0)
            {
                result.Add(BinaryConstants.PaddingByte);
            }

            return result.ToArray();
        }

        /// <summary>
        /// バッファから最大の距離と長さを探索する。
        /// </summary>
        private static (int Distance, int Length) FindLongestMatch(byte[] buffer, int currentPos)
        {
            // 最大範囲(LZ77MaxDistance)と最大長(LZ77MaxLength)を調整
            int maxDistance = Math.Min(currentPos, LZ77MaxDistance);
            int maxLength = Math.Min(buffer.Length - currentPos, LZ77MaxLength);

            // 近すぎる場合を除外
            if (maxDistance < LZ77MinDistance || maxLength < LZ77MinLength) return (0, 0);

            int resultDistance = 0;
            int resultLength = 0;

            // 最小距離から最大距離までを解析
            for (int tempDistance = LZ77MinDistance; tempDistance <= maxDistance; tempDistance++)
            {
                // 現在位置とデータが一致する長さを計測
                int tempLength = 0;
                while (tempLength < maxLength 
                    && buffer[currentPos - tempDistance + tempLength] == buffer[currentPos + tempLength])
                {
                    tempLength++;
                }

                // より長い一致が見つかったら更新
                if (tempLength > resultLength)
                {
                    resultDistance = tempDistance;
                    resultLength = tempLength;

                    // 最大長(LZ77MaxLength)になったら終了
                    if (resultLength == LZ77MaxLength) break;
                }
            }

            return (resultDistance, resultLength);
        }

        /// <summary>
        /// データからパレットデータ（圧縮と非圧縮）を読み込む。
        /// </summary>
        public static byte[] DecompressPalette(
            byte[] buffer,
            int offset = 0,
            bool isCompressed = true)
        {
            if (isCompressed)
            {
                return DecompressLZ77(buffer, offset);
            }

            var paletteData = new byte[ImageConstants.PalColorCount * ImageConstants.BytesPerColor];
            Array.Copy(buffer, offset, paletteData, 0, paletteData.Length);
            return paletteData;
        }

        /// <summary>
        /// パレットデータを書き込み用に変換する。(圧縮指定可能)
        /// </summary>
        public static byte[] CompressPalette(
            byte[] buffer,
            bool isCompressed = true)
        {
            return isCompressed
                ? CompressLZ77(buffer)
                : buffer;
        }

        /// <summary>
        /// 画像データとパレットデータからBitmap(4bppインデックスカラー)を生成する。
        /// </summary>
        public static Bitmap CreateBitmap(
            byte[] imageData,
            byte[] paletteData,
            int width,
            int height,
            bool showBackColor = true)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format4bppIndexed);

            // まずパレットを適用する
            ApplyPalette(bmp, paletteData, showBackColor);

            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format4bppIndexed);

            var pixels = new byte[bmpData.Stride * height];
            int dataIndex = 0;

            // タイル変換処理
            // 8x8で保存されている
            // (yTile, xTile)から(yPixel, xPixel)の順の4重ループ
            for (int yTile = 0; yTile < height; yTile += ImageConstants.TileSize)
            {
                for (int xTile = 0; xTile < width; xTile += ImageConstants.TileSize)
                {
                    for (int yPixel = 0; yPixel < ImageConstants.TileSize; yPixel++)
                    {
                        // 4bppの場合、1バイトで2ピクセル分
                        for (int xPixel = 0; xPixel < ImageConstants.TileSize; xPixel += ImageConstants.PixelsPerByte)
                        {
                            if (dataIndex >= imageData.Length) break;
                            byte temp = imageData[dataIndex++];

                            // 1バイトのデータからパレットインデックスを取得
                            int leftIndex = temp & BinaryConstants.NibbleMask;
                            int rightIndex = (temp >> BinaryConstants.NibbleShift) & BinaryConstants.NibbleMask;

                            // Bitmapの書き込み位置を計算
                            int byteIndex = (yTile + yPixel) * bmpData.Stride + ((xTile + xPixel) / ImageConstants.PixelsPerByte);
                            pixels[byteIndex] = (byte)((leftIndex << BinaryConstants.NibbleShift) | rightIndex);
                        }
                    }
                }
            }

            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// Bitmapにパレットデータを適用・更新する。
        /// </summary>
        public static void ApplyPalette(
            Bitmap bmp,
            byte[] paletteData,
            bool showBackColor = true)
        {
            ColorPalette bmpPalette = bmp.Palette;
            int paletteCount = Math.Min(paletteData.Length / ImageConstants.BytesPerColor, ImageConstants.PalColorCount);

            // パレット変換処理
            // GBA15ビット(RGB各5ビット)からARGB
            for (int i = 0; i < paletteCount; i++)
            {
                int byteIndex = i * ImageConstants.BytesPerColor;
                if (byteIndex + 1 >= paletteData.Length) break;

                // 2バイトから1つの色データ(15bit)を合成
                int temp = (paletteData[byteIndex + 1] << BinaryConstants.BitsPerByte) | paletteData[byteIndex];

                // 5ビット(0-31)を8ビット(0-255)にするため8倍する
                int r = ((temp & ImageConstants.RedMask) >> ImageConstants.RedShift) * ImageConstants.ColorChannelMulti;
                int g = ((temp & ImageConstants.GreenMask) >> ImageConstants.GreenShift) * ImageConstants.ColorChannelMulti;
                int b = ((temp & ImageConstants.BlueMask) >> ImageConstants.BlueShift) * ImageConstants.ColorChannelMulti;

                // インデックス0は背景色
                // showBackColorがfalseならアルファを0にする
                bmpPalette.Entries[i] = (i == 0 && !showBackColor)
                    ? Color.FromArgb(0, r, g, b)
                    : Color.FromArgb(255, r, g, b);
            }

            // 余ったパレットは適当に黒で埋める
            for (int i = paletteCount; i < ImageConstants.PalColorCount; i++)
            {
                bmpPalette.Entries[i] = Color.Black;
            }

            // 更新したパレットをBitmapに戻す
            bmp.Palette = bmpPalette;
        }

        /// <summary>
        /// Bitmapから画像データとパレットデータを抽出する。
        /// </summary>
        public static bool ExtractImageAndPalette(
            Bitmap bmp,
            int expectedWidth,
            int expectedHeight,
            out byte[] imageData,
            out byte[] paletteData)
        {
            imageData = null;
            paletteData = null;

            if (bmp.Width != expectedWidth || bmp.Height != expectedHeight)
            {
                MessageBox.Show(
                    $"画像サイズは {expectedWidth}x{expectedHeight} である必要があります。",
                    "サイズエラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (bmp.PixelFormat != PixelFormat.Format4bppIndexed)
            {
                MessageBox.Show(
                    "4bpp(16色)のインデックスカラー画像を使用してください。",
                    "パレットエラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            // パレット変換(ARGBからRGB15ビット)
            ColorPalette pal = bmp.Palette;
            paletteData = new byte[ImageConstants.PalColorCount * ImageConstants.BytesPerColor];

            for (int i = 0; i < ImageConstants.PalColorCount; i++)
            {
                Color c = (i < pal.Entries.Length)
                    ? pal.Entries[i]
                    : Color.FromArgb(255, 0, 0, 0);

                // 8ビット(0-255)を5ビット(0-31)に変換
                int r = c.R / ImageConstants.ColorChannelMulti;
                int g = c.G / ImageConstants.ColorChannelMulti;
                int b = c.B / ImageConstants.ColorChannelMulti;

                // B, G, R の順でビットシフトする
                ushort gbaColor = (ushort)(
                    (b << ImageConstants.BlueShift) |
                    (g << ImageConstants.GreenShift) |
                    (r << ImageConstants.RedShift));

                // バイト配列に上書き
                IoHelper.WriteLongAsBytes(
                    paletteData,
                    i * ImageConstants.BytesPerColor,
                    gbaColor,
                    (DataSize)ImageConstants.BytesPerColor);
            }

            // タイル変換(8x8)
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format4bppIndexed);

            var pixels = new byte[bmpData.Stride * bmp.Height];
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);
            bmp.UnlockBits(bmpData);

            var dataList = new List<byte>();

            // タイル単位で解析して抽出する
            for (int yTile = 0; yTile < expectedHeight; yTile += ImageConstants.TileSize)
            {
                for (int xTile = 0; xTile < expectedWidth; xTile += ImageConstants.TileSize)
                {
                    for (int yPixel = 0; yPixel < ImageConstants.TileSize; yPixel++)
                    {
                        for (int xPixel = 0; xPixel < ImageConstants.TileSize; xPixel += ImageConstants.PixelsPerByte)
                        {
                            // Bitmap上の位置
                            int byteIndex = (yTile + yPixel) * bmpData.Stride + ((xTile + xPixel) / ImageConstants.PixelsPerByte);
                            byte pixelByte = pixels[byteIndex];

                            // パレットインデックスを分離
                            int p1 = (pixelByte >> ImageConstants.Bpp4) & BinaryConstants.NibbleMask;
                            int p2 = pixelByte & BinaryConstants.NibbleMask;

                            // 左ピクセルが下位4ビットに相当ので、マージする
                            dataList.Add((byte)((p2 << BinaryConstants.NibbleShift) | p1));
                        }
                    }
                }
            }

            imageData = dataList.ToArray();
            return true;
        }

        /// <summary>
        /// 背景色の透過状態をリセットし、画像を出力する。
        /// </summary>
        public static void ExportIndexedImage(Bitmap bmp, string filePath)
        {
            using (var exportBmp = (Bitmap)bmp.Clone())
            {
                // すべてのパレットカラーのアルファ値を255(不透明)に戻す
                ColorPalette pal = exportBmp.Palette;
                for (int i = 0; i < pal.Entries.Length; i++)
                {
                    Color e = pal.Entries[i];
                    pal.Entries[i] = Color.FromArgb(255, e.R, e.G, e.B);
                }
                exportBmp.Palette = pal;

                // .bmp
                var ext = Path.ChangeExtension(null, ExtConstants.BmpExt);
                var format = Path.GetExtension(filePath).ToLower() == ext
                    ? ImageFormat.Bmp
                    : ImageFormat.Png;

                exportBmp.Save(filePath, format);
            }
        }

        /// <summary>
        /// Bitmapをぼやかさずに拡大する。
        /// </summary>
        public static Bitmap ScaleBitmap(
            Bitmap bmp,
            int xOffset = 0,
            int yOffset = 0,
            int scaleFactor = ImageConstants.DefaultScale)
        {
            int newWidth = bmp.Width * scaleFactor;
            int newHeight = bmp.Height * scaleFactor;

            Bitmap scaledBmp = new Bitmap(newWidth, newHeight);

            using (Graphics gfx = Graphics.FromImage(scaledBmp))
            {
                // ぼやかさずに拡大する設定
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.DrawImage(bmp, new Rectangle(xOffset, yOffset, newWidth, newHeight));
            }

            return scaledBmp;
        }
    }
}
