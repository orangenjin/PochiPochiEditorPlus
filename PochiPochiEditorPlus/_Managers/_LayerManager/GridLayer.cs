using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class GridLayer : LayerBase
    {
        public GridLayer(LayerData layerData)
        {
            _layerData = layerData;
        }

        public override void Draw(Graphics gfx)
        {
            if (_layerData.ValidItemCount <= 0) return;

            // 拡大後のグリッドサイズを使用
            int size = _layerData.ScaledGridSize;

            // グリッドの太さを調整
            int thickness = 1 * _layerData.Scale;

            using (var brush = new SolidBrush(Color.FromArgb(80, Color.Gray)))
            {
                for (int i = 0; i < _layerData.TotalGridCount; i++)
                {
                    int gridX = i % _layerData.Columns;
                    int gridY = i / _layerData.Columns;
                    int drawX = gridX * size;
                    int drawY = gridY * size;

                    // 四辺に対して描画
                    gfx.FillRectangle(
                        brush, 
                        drawX,
                        drawY,
                        size, 
                        thickness);
                    gfx.FillRectangle(
                        brush, 
                        drawX, 
                        drawY + size - thickness, 
                        size, 
                        thickness);
                    gfx.FillRectangle(
                        brush, 
                        drawX, 
                        drawY + thickness, 
                        thickness, 
                        size - (thickness * 2));
                    gfx.FillRectangle(
                        brush,
                        drawX + size - thickness,
                        drawY + thickness, 
                        thickness, 
                        size - (thickness * 2));
                }
            }
        }
    }
}
