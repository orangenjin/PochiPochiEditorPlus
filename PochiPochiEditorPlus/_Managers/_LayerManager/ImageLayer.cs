using System;
using System.Collections.Generic;
using System.Drawing;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class ImageLayer : LayerBase
    {
        // バイト配列の画像データ
        public byte[] ImageData { get; set; }
        public byte[] PaletteData { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public bool ShowBackColor { get; set; }

        // パレット切り替え時のために保持
        private Bitmap _cachedBitmap = null;

        /// <summary>
        /// 描画用Bitmapを生成する。
        /// </summary>
        public void SetImageData(
            byte[] imageData,
            byte[] paletteData, 
            int width, 
            int height,
            bool showBackColor)
        {
            ImageData = imageData;
            PaletteData = paletteData;
            ImageWidth = width;
            ImageHeight = height;
            ShowBackColor = showBackColor;

            // 画像を破棄
            _cachedBitmap?.Dispose();
            _cachedBitmap = null;

            // 画像を生成
            _cachedBitmap = ImageHelper.CreateBitmap(
                ImageData,
                PaletteData,
                ImageWidth,
                ImageHeight,
                ShowBackColor);
        }

        /// <summary>
        /// パレットデータのみを変更する。
        /// </summary>
        public void ApplyPalette(byte[] paletteData)
        {
            if (_cachedBitmap != null) return;
            PaletteData = paletteData;
            ImageHelper.ApplyPalette(_cachedBitmap, PaletteData, ShowBackColor);
        }

        /// <summary>
        /// ここで拡大率を考慮して、画像を表示する。
        /// </summary>
        public override void Draw(Graphics gfx, Rectangle rect, LayerData data)
        {
            if (_cachedBitmap == null) return;

            int scaledWidth = _cachedBitmap.Width * data.Scale;
            int scaledHeight = _cachedBitmap.Height * data.Scale;

            gfx.DrawImage(
                _cachedBitmap,
                new Rectangle(0, 0, scaledWidth, scaledHeight),
                new Rectangle(0, 0, _cachedBitmap.Width, _cachedBitmap.Height),
                GraphicsUnit.Pixel);
        }

        /// <summary>
        /// GridSize単位で、指定したマスの画像データを抽出する。
        /// </summary>
        public byte[] ExtractImageAtGrid(int gridX, int gridY, LayerData data)
        {
            if (ImageData == null) return null;
            if (!data.IsValidGrid(gridX, gridY)) return null;

            // 1マスにおけるタイル数を計算
            int tilesPerGridX = data.GridSize / Constants.TileSize;
            int tilesPerGridY = data.GridSize / Constants.TileSize;

            var extractedBytes = 
                new List<byte>(tilesPerGridX * tilesPerGridY * Constants.BytesPerTile);
            int totalTilesX = ImageWidth / Constants.TileSize;

            // グリッド内のY方向ループ
            for (int gridPosY = 0; gridPosY < tilesPerGridY; gridPosY++)
            {
                // 画像全体におけるY方向のタイル位置
                int tilePosY = (gridY * tilesPerGridY) + gridPosY;

                // グリッド内のX方向ループ
                for (int gridPosX = 0; gridPosX < tilesPerGridX; gridPosX++)
                {
                    // 画像全体におけるX方向のタイル位置
                    int tilePosX = (gridX * tilesPerGridX) + gridPosX;

                    int tileIndex = (tilePosY * totalTilesX) + tilePosX;
                    int byteOffset = tileIndex * Constants.BytesPerTile;

                    // 1タイル分を抽出
                    byte[] tileData = new byte[Constants.BytesPerTile];
                    Array.Copy(ImageData, byteOffset, tileData, 0, Constants.BytesPerTile);
                    extractedBytes.AddRange(tileData);
                }
            }

            return extractedBytes.ToArray();
        }
    }
}
