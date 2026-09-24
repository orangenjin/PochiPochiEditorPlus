using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelGrid
    {
        public int Size { get; set; }
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    _panel.Invalidate();
                }
            }
        }
        public Func<Point> ScrollOffsetProvider { get; set; }

        private Panel _panel = null;
        private bool _visible = false;

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
            // 非表示ならスキップ
            if (!Visible) return;

            // X軸とY軸のスクロールオフセットを取得
            var offset = ScrollOffsetProvider?.Invoke() ?? Point.Empty;

            // 画面に見えている範囲とオフセットから座標の範囲を計算
            int startX = ((e.ClipRectangle.Left + offset.X) / Size) * Size;
            int startY = ((e.ClipRectangle.Top + offset.Y) / Size) * Size;
            int endX = e.ClipRectangle.Right + offset.X;
            int endY = e.ClipRectangle.Bottom + offset.Y;

            using (var brush = new SolidBrush(Color.FromArgb(80, Color.Gray)))
            {
                for (int y = startY; y < endY; y += Size)
                {
                    for (int x = startX; x < endX; x += Size)
                    {
                        int drawX = x - offset.X;
                        int drawY = y - offset.Y;

                        // 四辺に対して描画
                        e.Graphics.FillRectangle(brush, drawX, drawY, Size, 1);
                        e.Graphics.FillRectangle(brush, drawX, drawY + Size - 1, Size, 1);
                        e.Graphics.FillRectangle(brush, drawX, drawY + 1, 1, Size - 2);
                        e.Graphics.FillRectangle(brush, drawX + Size - 1, drawY + 1, 1, Size - 2);
                    }
                }
            }
        }
    }
}
