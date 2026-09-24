using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelSelector
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
        public Size MaxSelectSize { get; set; }
        public Rectangle SelectedGrids { get; set; } // 選択範囲の座標情報
        public Func<Point> ScrollOffsetProvider { get; set; }

        private Panel _panel = null;
        private int _unitSize = 0;

        // 座標制御用
        private bool _isDragging = false;
        private Point _startGridPoint;
        private Point _currentGridPoint;

        public PanelSelector(Panel panel, EventBinder eventBinder)
        {
            _panel = panel;

            eventBinder.BindCustom(
                () =>
                {
                    _panel.MouseDown += Panel_MouseDown;
                    _panel.MouseMove += Panel_MouseMove;
                    _panel.MouseUp += Panel_MouseUp;
                    _panel.Paint += Panel_Paint;
                },
                () =>
                {
                    _panel.MouseDown -= Panel_MouseDown;
                    _panel.MouseMove -= Panel_MouseMove;
                    _panel.MouseUp -= Panel_MouseUp;
                    _panel.Paint -= Panel_Paint;
                });
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (_unitSize <= 0) return;

            // 右クリックで選択開始
            if (e.Button == MouseButtons.Right)
            {
                var offset = ScrollOffsetProvider?.Invoke() ?? Point.Empty;
                int gridX = (e.X + offset.X) / _unitSize;
                int gridY = (e.Y + offset.Y) / _unitSize;

                _startGridPoint = new Point(gridX, gridY);
                _currentGridPoint = _startGridPoint;
                _isDragging = true;

                UpdateSelection();
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _unitSize <= 0) return;

            var offset = ScrollOffsetProvider?.Invoke() ?? Point.Empty;

            // パネル外のマイナス座標も考慮し、パネル範囲内に収束させる
            int rawX = e.X + offset.X;
            int rawY = e.Y + offset.Y;
            int gridX = rawX < 0 
                ? (rawX - _unitSize + 1) / _unitSize 
                : rawX / _unitSize;
            int gridY = rawY < 0 
                ? (rawY - _unitSize + 1) / _unitSize 
                : rawY / _unitSize;

            // 表示されている範囲の最小・最大マス座標を計算
            int minGridX = offset.X / _unitSize;
            int minGridY = offset.Y / _unitSize;
            int maxGridX = (_panel.Width - 1 + offset.X) / _unitSize;
            int maxGridY = (_panel.Height - 1 + offset.Y) / _unitSize;

            // 選択可能サイズを計算
            int limitX = Math.Max(0, MaxSelectSize.Width - 1);
            int limitY = Math.Max(0, MaxSelectSize.Height - 1);

            // 選択最大範囲に留める
            gridX = Math.Max(
                _startGridPoint.X - limitX,
                Math.Min(gridX, _startGridPoint.X + limitX));
            gridY = Math.Max(
                _startGridPoint.Y - limitY,
                Math.Min(gridY, _startGridPoint.Y + limitY));

            // パネルの表示領域内に留める
            gridX = Math.Max(minGridX, Math.Min(gridX, maxGridX));
            gridY = Math.Max(minGridY, Math.Min(gridY, maxGridY));

            // マス位置が変位した場合のみ再描画
            if (_currentGridPoint.X != gridX || _currentGridPoint.Y != gridY)
            {
                _currentGridPoint = new Point(gridX, gridY);
                UpdateSelection();
            }
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _isDragging)
            {
                _isDragging = false;
            }
        }

        private void UpdateSelection()
        {
            // 始点と終点から矩形範囲を計算
            int minX = Math.Min(_startGridPoint.X, _currentGridPoint.X);
            int minY = Math.Min(_startGridPoint.Y, _currentGridPoint.Y);
            int maxX = Math.Max(_startGridPoint.X, _currentGridPoint.X);
            int maxY = Math.Max(_startGridPoint.Y, _currentGridPoint.Y);

            SelectedGrids = new Rectangle(
                minX,
                minY,
                maxX - minX + 1, 
                maxY - minY + 1);
            _panel.Invalidate();
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            if (_unitSize <= 0 || SelectedGrids.Width <= 0 || SelectedGrids.Height <= 0) return;

            var offset = ScrollOffsetProvider?.Invoke() ?? Point.Empty;

            // 矩形座標を計算
            int drawX = (SelectedGrids.X * _unitSize) - offset.X;
            int drawY = (SelectedGrids.Y * _unitSize) - offset.Y;
            int drawWidth = SelectedGrids.Width * _unitSize;
            int drawHeight = SelectedGrids.Height * _unitSize;

            // 赤枠を描画
            using (var brush = new SolidBrush(Color.Red))
            {
                e.Graphics.FillRectangle(brush, drawX, drawY, drawWidth, 1); 
                e.Graphics.FillRectangle(brush, drawX, drawY + drawHeight - 1, drawWidth, 1); 
                e.Graphics.FillRectangle(brush, drawX, drawY + 1, 1, drawHeight - 2); 
                e.Graphics.FillRectangle(brush, drawX + drawWidth - 1, drawY + 1, 1, drawHeight - 2); 
            }
        }

        /// <summary>
        /// 選択状態をクリアし、再描画する。
        /// </summary>
        public void ClearSelect()
        {
            UnitSize = 0;
            MaxSelectSize = Size.Empty;
            SelectedGrids = Rectangle.Empty;
        }
    }
}
