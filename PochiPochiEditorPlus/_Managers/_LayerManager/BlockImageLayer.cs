using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class BlockImageLayer : LayerBase
    {
        // 各ブロックが持つ画像
        private Bitmap[] _blockImages = null;

        /// <summary>
        /// 初期化して配列を確保する。
        /// </summary>
        public void Allocate()
        {
            int total = Data.ValidItemCount;

            // 配列の画像を破棄
            ClearImages();
            _blockImages = new Bitmap[total];
        }

        /// <summary>
        /// 任意のマス座標の画像を書き換える。
        /// </summary>
        public void SetBlockImage(int gridX, int gridY, Bitmap image)
        {
            if (!Data.IsValidGrid(gridX, gridY)) return;
            int index = gridY * Data.Columns + gridX;

            _blockImages[index]?.Dispose();
            _blockImages[index] = image != null 
                ? (Bitmap)image.Clone()
                : null;
        }

        public Bitmap GetBlockImage(int gridX, int gridY)
        {
            if (!Data.IsValidGrid(gridX, gridY)) return null;
            return _blockImages[gridY * Data.Columns + gridX];
        }

        public override void Draw(Graphics gfx)
        {
            if (_blockImages == null) return;

            int cols = Data.Columns;
            if (cols <= 0) return;
            int size = Data.ScaledGridSize;

            for (int i = 0; i < _blockImages.Length; i++)
            {
                var bmp = _blockImages[i];
                if (bmp == null) continue;

                int gridX = i % cols;
                int gridY = i / cols;
                gfx.DrawImage(bmp,
                    new Rectangle(gridX * size, gridY * size, size, size),
                    new Rectangle(0, 0, Data.GridSize, Data.GridSize),
                    GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 配列の画像を破棄して、参照を切る。
        /// </summary>
        public void ClearImages()
        {
            if (_blockImages != null)
            {
                foreach (var img in _blockImages)
                {
                    img?.Dispose();
                }
                _blockImages = null;
            }
        }
    }
}
