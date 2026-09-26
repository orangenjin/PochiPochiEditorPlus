using System;
using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    /// <summary>
    /// コピーしたブロック画像を保持し、パネル間で共有する。
    /// </summary>
    public sealed class ClipboardData : IDisposable
    {
        // 範囲選択に対応するための2次元配列
        public Bitmap[,] CopiedImages { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// 選択されている範囲の画像をコピーする。
        /// </summary>
        public void Copy(SelectLayer selectLayer, BlockImageLayer blockImageLayer)
        {
            var bounds = selectLayer.SelectedGrids;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // コピーを破棄
            Dispose();

            Width = bounds.Width;
            Height = bounds.Height;
            CopiedImages = new Bitmap[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int gridX = bounds.Left + x;
                    int gridY = bounds.Top + y;

                    var img = blockImageLayer.GetBlockImage(gridX, gridY);
                    if (img != null)
                    {
                        CopiedImages[x, y] = (Bitmap)img.Clone();
                    }
                }
            }
        }

        public void Dispose()
        {
            if (CopiedImages != null)
            {
                foreach (var img in CopiedImages)
                {
                    img?.Dispose();
                }
                CopiedImages = null;
            }
        }
    }
}
