using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class GridLayer : LayerBase
    {
        public override void Draw(Graphics gfx)
        {
            if (Data.ValidItemCount <= 0) return;

            // 拡大後のグリッドサイズを使用
            int size = Data.ScaledGridSize;

            using (var brush = new SolidBrush(Color.FromArgb(80, Color.Gray)))
            {
                for (int i = 0; i < Data.TotalGridCount; i++)
                {
                    int gridX = i % Data.Columns;
                    int gridY = i / Data.Columns;
                    int drawX = gridX * size;
                    int drawY = gridY * size;

                    // 四辺に対して描画
                    gfx.FillRectangle(
                        brush,
                        drawX, 
                        drawY, 
                        size, 
                        1);
                    gfx.FillRectangle(
                        brush, 
                        drawX, 
                        drawY + size - 1,
                        size, 
                        1);
                    gfx.FillRectangle(
                        brush,
                        drawX,
                        drawY + 1,
                        1, 
                        size - 2);
                    gfx.FillRectangle(
                        brush,
                        drawX + size - 1,
                        drawY + 1,
                        1,
                        size - 2);
                }
            }
        }
    }
}
