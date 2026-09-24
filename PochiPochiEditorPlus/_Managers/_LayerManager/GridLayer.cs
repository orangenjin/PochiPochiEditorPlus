using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class GridLayer : LayerBase
    {
        public override void Draw(Graphics gfx, LayerData data)
        {
            if (data.ValidItemCount <= 0) return;

            // 拡大後のグリッドサイズを使用
            int size = data.ScaledGridSize;

            // アイテム数から必要な行数を切り上げて計算
            int rows = (data.ValidItemCount + data.Columns - 1) / data.Columns;
            int totalGridCells = rows * data.Columns;

            using (var brush = new SolidBrush(Color.FromArgb(80, Color.Gray)))
            {
                for (int i = 0; i < totalGridCells; i++)
                {
                    int gridX = i % data.Columns;
                    int gridY = i / data.Columns;
                    int drawX = gridX * size;
                    int drawY = gridY * size;

                    // 四辺に対して描画
                    gfx.FillRectangle(brush, drawX, drawY, size, 1);
                    gfx.FillRectangle(brush, drawX, drawY + size - 1, size, 1);
                    gfx.FillRectangle(brush, drawX, drawY + 1, 1, size - 2);
                    gfx.FillRectangle(brush, drawX + size - 1, drawY + 1, 1, size - 2);
                }
            }
        }
    }
}
