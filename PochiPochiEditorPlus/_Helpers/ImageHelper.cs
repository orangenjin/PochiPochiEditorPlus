using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditor2._Helpers
{
    public static class ImageHelper
    {
        private const int LZ77HeaderSize = 0x4;
        private const int LZ77HeaderIdentifier = 0x10;
        private const int LZ77MaxDistance = 4096;
        private const int LZ77MaxLength = 18;
        private const int LZ77MinMatchLength = 3;
        private const int LZ77MinSafeDistance = 2;
        private const int LZ77CompressedUnitSize = 2;

        /// <summary>
        /// LZ77圧縮されたデータを解凍する。
        /// </summary>
        public static byte[] DecompressLZ77(byte[] data, int offset = 0)
        {
            // ヘッダの読み込み
            // 先頭1バイトは識別子(LZ77HeaderIdentifier)
            int header = (int)IoHelper.ReadBytesAsInt(data, offset, LZ77HeaderSize);
            // 残り3バイトは解凍後のサイズ
            int decompressedSize = header >> Constants.BitsPerByte;
            var result = new byte[decompressedSize];

            int srcPos = offset + LZ77HeaderSize;
            int dstPos = 0;

            while (dstPos < decompressedSize)
            {
                // フラグバイトを読み込む
                // 後続する8ブロックの圧縮状態を示す
                byte flagByte = data[srcPos++];

                // 左端のビットから1ビットずつ確認
                for (int i = 0; i < Constants.BitsPerByte; i++)
                {
                    if (dstPos >= decompressedSize) break;

                    // 対象ビットが1であれば圧縮、0であれば非圧縮
                    bool isCompressed =
                        (flagByte & (1 << (Constants.BitsPerByte - 1 - i))) != 0;

                    if (isCompressed)
                    {
                        // 圧縮データを2バイト読み込む
                        byte byte0 = data[srcPos];
                        byte byte1 = data[srcPos + 1];
                        srcPos += LZ77CompressedUnitSize;

                        // 上位4ビットから長さ(3から18)を計算
                        int length = 
                            (byte0 >> Constants.NibbleShift) 
                            + LZ77MinMatchLength;

                        // 下位12ビットから相対距離を計算
                        int distance =
                            (((byte0 & Constants.NibbleMask) << Constants.BitsPerByte) 
                            | byte1) + 1;

                        // 位置を特定
                        int copySrc = dstPos - distance;

                        // 解凍したデータから1バイトずつコピー
                        for (int j = 0; j < length; j++)
                        {
                            if (dstPos >= decompressedSize) break;
                            result[dstPos++] = result[copySrc++];
                        }
                    }
                    else
                    {
                        // 非圧縮データを1バイトコピー
                        result[dstPos++] = data[srcPos++];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// データをLZ77に圧縮する。
        /// </summary>
        public static byte[] CompressLZ77(byte[] data)
        {
            // 圧縮データ格納用
            int length = data.Length;
            var result = new List<byte>(length);

            // ヘッダの書き込み
            // 先頭に識別子(LZ77HeaderIdentifier)
            // 続く3バイトに非圧縮時のサイズ
            result.Add((byte)LZ77HeaderIdentifier);
            for (int i = 0; i < LZ77HeaderSize - 1; i++)
            {
                result.Add((byte)((length >> (i * Constants.BitsPerByte)) & Constants.ByteMask));
            }

            int pos = 0;
            while (pos < length)
            {
                // フラグバイトの位置と値を仮置き
                int flagPos = result.Count;
                byte flag = 0;
                result.Add(Constants.PaddingByte);

                // 8ブロック分の圧縮処理
                for (int i = 0; i < Constants.BitsPerByte; i++)
                {
                    if (pos >= length) break;

                    // データから最大の距離と長さを探索する
                    var (bestDistance, bestLength) = FindLongestMatch(data, pos);

                    if (bestLength >= LZ77MinMatchLength)
                    {
                        // 長さが最小(3バイト)を満たす場合、圧縮フラグを立てる
                        flag |= (byte)(1 << (Constants.BitsPerByte - 1 - i));

                        int distanceValue = bestDistance - 1;
                        int lengthValue = bestLength - LZ77MinMatchLength;

                        // [長さ4bit][距離の上位4bit]
                        result.Add((byte)(
                                ((lengthValue & Constants.NibbleMask) << Constants.NibbleShift)
                                | ((distanceValue >> Constants.BitsPerByte) & Constants.NibbleMask)));
                        // [距離の下位8bit]
                        result.Add((byte)(distanceValue & Constants.ByteMask));

                        pos += bestLength;
                    }
                    else
                    {
                        // 圧縮できない場合はそのまま書き込む
                        result.Add(data[pos++]);
                    }
                }

                // 仮置きしたフラグバイトを上書き
                result[flagPos] = flag;
            }

            // データサイズが4の倍数バイトになるように調整
            while (result.Count % Constants.UIntSize != 0)
            {
                result.Add(Constants.PaddingByte);
            }

            return result.ToArray();
        }

        /// <summary>
        /// バッファから最大の距離と長さを探索する。
        /// </summary>
        private static (int distance, int length) FindLongestMatch(byte[] data, int pos)
        {
            // 最大検索範囲(LZ77MaxDistance)と最大長(LZ77MaxLength)を調整
            int maxDistance = Math.Min(pos, LZ77MaxDistance);
            int maxLength = Math.Min(data.Length - pos, LZ77MaxLength);

            // 近すぎる場合を除外
            if (maxDistance < LZ77MinSafeDistance || maxLength < LZ77MinMatchLength) return (0, 0);

            int bestLength = 0;
            int bestDistance = 0;

            // 最小距離から最大距離までを解析
            for (int distance = LZ77MinSafeDistance; distance <= maxDistance; distance++)
            {
                // 現在位置とデータが一致する長さを計測
                int length = 0;
                while (length < maxLength && data[pos - distance + length] == data[pos + length])
                {
                    length++;
                }

                // より長い一致が見つかったら更新
                if (length > bestLength)
                {
                    bestDistance = distance;
                    bestLength = length;

                    // 最大長(LZ77MaxLength)になったら終了
                    if (bestLength == LZ77MaxLength) break;
                }
            }

            return (bestDistance, bestLength);
        }

        /// <summary>
        /// データからパレットデータ（圧縮と非圧縮）を読み込む。
        /// </summary>
        public static byte[] DecompressPalette(
            byte[] data,
            int offset = 0,
            bool isCompressed = true)
        {
            if (isCompressed)
            {
                return DecompressLZ77(data, offset);
            }

            var paletteData = new byte[Constants.PalColorCount * Constants.BytesPerColor];
            Array.Copy(data, offset, paletteData, 0, paletteData.Length);
            return paletteData;
        }

        /// <summary>
        /// パレットデータを書き込み用に変換する。（圧縮指定可能）
        /// </summary>
        public static byte[] CompressPalette(
            byte[] data,
            bool isCompressed = true)
        {
            return isCompressed
                ? CompressLZ77(data)
                : data;
        }

        /// <summary>
        /// タイルデータとパレットデータからBitmap(4bppインデックスカラー)を生成する。
        /// </summary>
        public static Bitmap CreateBitmap(
            byte[] tileData,
            byte[] paletteData,
            int width,
            int height,
            bool showBackColor = true)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format4bppIndexed);

            ColorPalette bmpPalette = bmp.Palette;
            int paletteCount = Math.Min(paletteData.Length / Constants.BytesPerColor, Constants.PalColorCount);

            // パレット変換処理
            // GBA15ビット(RGB各5ビット)からARGB
            for (int i = 0; i < paletteCount; i++)
            {
                int byteIndex = i * Constants.BytesPerColor;
                if (byteIndex + 1 >= paletteData.Length) break;

                // 2バイトから1つの色データ(15bit)を合成
                int temp = (paletteData[byteIndex + 1] << Constants.BitsPerByte) | paletteData[byteIndex];

                // 5ビット(0-31)を8ビット(0-255)にするため8倍する
                int r = ((temp & Constants.RedMask) >> Constants.RedShift) * Constants.ColorChannelMulti;
                int g = ((temp & Constants.GreenMask) >> Constants.GreenShift) * Constants.ColorChannelMulti;
                int b = ((temp & Constants.BlueMask) >> Constants.BlueShift) * Constants.ColorChannelMulti;

                // インデックス0は背景色
                // showBackColorがfalseならアルファを0にする
                bmpPalette.Entries[i] = (i == 0 && !showBackColor)
                    ? Color.FromArgb(0, r, g, b)
                    : Color.FromArgb(255, r, g, b);
            }

            // 余ったパレットは適当に黒で埋める
            for (int i = paletteCount; i < Constants.PalColorCount; i++)
            {
                bmpPalette.Entries[i] = Color.Black;
            }

            bmp.Palette = bmpPalette;

            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format4bppIndexed);

            var pixels = new byte[bmpData.Stride * height];
            int dataIndex = 0;

            // タイル変換処理
            // 8x8で保存されている
            // (yTile, xTile)から(yPixel, xPixel)の順の4重ループ
            for (int yTile = 0; yTile < height; yTile += Constants.TileSize)
            {
                for (int xTile = 0; xTile < width; xTile += Constants.TileSize)
                {
                    for (int yPixel = 0; yPixel < Constants.TileSize; yPixel++)
                    {
                        // 4bppの場合、1バイトで2ピクセル分
                        for (int xPixel = 0; xPixel < Constants.TileSize; xPixel += Constants.PixelsPerByte4Bpp)
                        {
                            if (dataIndex >= tileData.Length) break;

                            byte temp = tileData[dataIndex++];

                            // 1バイトのデータからパレットインデックスを取得
                            int leftIndex = temp & Constants.NibbleMask;
                            int rightIndex = (temp >> Constants.NibbleShift) & Constants.NibbleMask;

                            // Bitmapの書き込み位置を計算
                            int byteIndex = (yTile + yPixel) * bmpData.Stride + ((xTile + xPixel) / Constants.PixelsPerByte4Bpp);
                            pixels[byteIndex] = (byte)((leftIndex << Constants.NibbleShift) | rightIndex);
                        }
                    }
                }
            }

            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// Bitmapからタイルデータとパレットデータを抽出する。
        /// </summary>
        public static bool ExtractTileAndPalette(
            Bitmap bmp,
            int expectedWidth,
            int expectedHeight,
            out byte[] tileData,
            out byte[] paletteData)
        {
            tileData = null;
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
            paletteData = new byte[Constants.PalColorCount * Constants.BytesPerColor];

            for (int i = 0; i < Constants.PalColorCount; i++)
            {
                Color c = (i < pal.Entries.Length)
                    ? pal.Entries[i]
                    : Color.FromArgb(255, 0, 0, 0);

                // 8ビット(0-255)を5ビット(0-31)に変換
                int r = c.R / Constants.ColorChannelMulti;
                int g = c.G / Constants.ColorChannelMulti;
                int b = c.B / Constants.ColorChannelMulti;

                // B, G, R の順でビットシフトする
                ushort gbaColor = (ushort)(
                    (b << Constants.BlueShift) |
                    (g << Constants.GreenShift) |
                    (r << Constants.RedShift));

                // バイト配列に上書き
                IoHelper.WriteIntAsBytes(
                    paletteData,
                    i * Constants.BytesPerColor,
                    Constants.BytesPerColor,
                    gbaColor);
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
            for (int yTile = 0; yTile < expectedHeight; yTile += Constants.TileSize)
            {
                for (int xTile = 0; xTile < expectedWidth; xTile += Constants.TileSize)
                {
                    for (int yPixel = 0; yPixel < Constants.TileSize; yPixel++)
                    {
                        for (int xPixel = 0; xPixel < Constants.TileSize; xPixel += Constants.PixelsPerByte4Bpp)
                        {
                            // Bitmap上の位置
                            int byteIndex = (yTile + yPixel) * bmpData.Stride + ((xTile + xPixel) / Constants.PixelsPerByte4Bpp);
                            byte pixelByte = pixels[byteIndex];

                            // パレットインデックスを分離
                            int p1 = (pixelByte >> Constants.Bpp4) & Constants.NibbleMask;
                            int p2 = pixelByte & Constants.NibbleMask;

                            // 左ピクセルが下位4ビットに相当ので、マージする
                            dataList.Add((byte)((p2 << Constants.NibbleShift) | p1));
                        }
                    }
                }
            }

            tileData = dataList.ToArray();
            return true;
        }

        /// <summary>
        /// 背景色の透過状態をリセットし、画像を出力する。
        /// </summary>
        public static void ExportIndexedImage(Bitmap bmp, string filePath)
        {
            if (bmp == null) return;

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
                var ext = Path.ChangeExtension(null, Constants.BmpExt);
                var format = Path.GetExtension(filePath).ToLower() == ext
                    ? ImageFormat.Bmp
                    : ImageFormat.Png;

                exportBmp.Save(filePath, format);
            }
        }

        /// <summary>
        /// Bitmapを拡大常表示する。
        /// </summary>
        public static Bitmap ScaleBitmap(
            Bitmap originalBmp,
            int scaleFactor = Constants.DefaultScale)
        {
            int newWidth = originalBmp.Width * scaleFactor;
            int newHeight = originalBmp.Height * scaleFactor;
            var scaledBmp = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(scaledBmp))
            {
                // ぼやかさずに拡大する設定
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(originalBmp, new Rectangle(0, 0, newWidth, newHeight));
            }

            return scaledBmp;
        }
    }
}
