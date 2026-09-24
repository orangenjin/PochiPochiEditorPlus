using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelGrid
    {
        public int UnitSize
        {
            get => _unitSize;
            set
            {
                if (_unitSize != value)
                {
                    _unitSize = value;
                    _panel.Invalidate();
                }
            }
        }
        public Func<Point> ScrollOffsetProvider { get; set; }

        private Panel _panel = null;
        private int _unitSize = 0;

        public PanelGrid(
            Panel panel,
            EventBinder eventBinder)
        {
            _panel = panel;

            eventBinder.BindCustom(
                () => _panel.Paint += Panel_Paint,
                () => _panel.Paint -= Panel_Paint);
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            // サイズが未定義ならスキップ
            if (_unitSize <= 0) return;

            // X軸とY軸のスクロールオフセットを取得
            var offset = ScrollOffsetProvider?.Invoke() ?? Point.Empty;

            // 画面に見えている範囲とオフセットから座標の範囲を計算
            int startX = ((e.ClipRectangle.Left + offset.X) / _unitSize) * _unitSize;
            int startY = ((e.ClipRectangle.Top + offset.Y) / _unitSize) * _unitSize;
            int endX = e.ClipRectangle.Right + offset.X;
            int endY = e.ClipRectangle.Bottom + offset.Y;

            using (var brush = new SolidBrush(Color.FromArgb(80, Color.Gray)))
            {
                for (int y = startY; y < endY; y += _unitSize)
                {
                    for (int x = startX; x < endX; x += _unitSize)
                    {
                        int drawX = x - offset.X;
                        int drawY = y - offset.Y;

                        // 四辺に対して描画
                        e.Graphics.FillRectangle(brush, drawX, drawY, _unitSize, 1);
                        e.Graphics.FillRectangle(brush, drawX, drawY + _unitSize - 1, _unitSize, 1);
                        e.Graphics.FillRectangle(brush, drawX, drawY + 1, 1, _unitSize - 2);
                        e.Graphics.FillRectangle(brush, drawX + _unitSize - 1, drawY + 1, 1, _unitSize - 2);
                    }
                }
            }
        }

        public void ClearGrid()
        {
            UnitSize = 0;
        }
    }
}
