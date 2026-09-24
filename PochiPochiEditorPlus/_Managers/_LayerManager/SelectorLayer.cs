using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class SelectorLayer : LayerBase
    {
        // 選択可能な最大範囲
        public Size MaxSelectSize { get; set; }
        public Rectangle SelectedGrids { get; set; }

        private Point _currentGridPoint;
        private bool _isDragging = false;
        private Point _startGridPoint;
        private Action _requestInvalidate;

        public SelectorLayer(Action requestInvalidate)
        {
            _requestInvalidate = requestInvalidate;
        }

        public void SelectSingleItem(int index, LayerData data)
        {
            // インデックスが有効な範囲内か判定
            if (index < 0 || index >= data.ValidItemCount)
            {
                ClearSelect();
                return;
            }

            // マス座標を逆算する
            int gridX = index % data.Columns;
            int gridY = index / data.Columns;

            SelectedGrids = new Rectangle(gridX, gridY, 1, 1);
            _startGridPoint = new Point(gridX, gridY);
            _currentGridPoint = new Point(gridX, gridY);

            _requestInvalidate?.Invoke();
        }

        public List<int> GetSelectedIndex(LayerData data)
        {
            var selectedIndices = new List<int>();

            // 選択範囲が存在しない場合
            if (SelectedGrids.Width <= 0 || SelectedGrids.Height <= 0)
            {
                return selectedIndices;
            }

            for (int girdY = SelectedGrids.Top; girdY < SelectedGrids.Bottom; girdY++)
            {
                for (int gridX = SelectedGrids.Left; gridX < SelectedGrids.Right; gridX++)
                {
                    // 座標からインデックスを計算
                    int index = girdY * data.Columns + gridX;

                    // 有効なアイテム範囲内かどうかを確認
                    if (index < data.ValidItemCount)
                    {
                        selectedIndices.Add(index);
                    }
                }
            }

            return selectedIndices;
        }

        public override void Draw(Graphics gfx, LayerData data)
        {
            if (SelectedGrids.Width <= 0 || SelectedGrids.Height <= 0) return;

            int drawX = SelectedGrids.X * data.ScaledGridSize;
            int drawY = SelectedGrids.Y * data.ScaledGridSize;
            int drawWidth = SelectedGrids.Width * data.ScaledGridSize;
            int drawHeight = SelectedGrids.Height * data.ScaledGridSize;

            using (var brush = new SolidBrush(Color.Red))
            {
                gfx.FillRectangle(brush, drawX, drawY, drawWidth, 1);
                gfx.FillRectangle(brush, drawX, drawY + drawHeight - 1, drawWidth, 1);
                gfx.FillRectangle(brush, drawX, drawY + 1, 1, drawHeight - 2);
                gfx.FillRectangle(brush, drawX + drawWidth - 1, drawY + 1, 1, drawHeight - 2);
            }
        }

        public override void OnMouseDown(MouseEventArgs e, LayerData data)
        {
            if (e.Button != MouseButtons.Right) return;

            // マス座標を取得
            var gridPoint = data.GetGridPoint(e.Location);

            // 有効なマスかどうかを判定
            if (data.IsValidGrid(gridPoint.X, gridPoint.Y))
            {
                _startGridPoint = gridPoint;
                _currentGridPoint = gridPoint;
                _isDragging = true;
                UpdateSelection();
            }
        }

        public override void OnMouseMove(MouseEventArgs e, LayerData data)
        {
            if (!_isDragging) return;

            // マス座標を取得
            var gridPoint = data.GetGridPoint(e.Location);

            // 選択可能サイズを取得
            if (MaxSelectSize.Width > 0 && MaxSelectSize.Height > 0)
            {
                int limitX = MaxSelectSize.Width - 1;
                int limitY = MaxSelectSize.Height - 1;

                gridPoint.X = Math.Max(
                    _startGridPoint.X - limitX, 
                    Math.Min(gridPoint.X, _startGridPoint.X + limitX));
                gridPoint.Y = Math.Max(
                    _startGridPoint.Y - limitY,
                    Math.Min(gridPoint.Y, _startGridPoint.Y + limitY));
            }

            // 開始点を基準に、選択範囲が最大範囲を超えないようにする
            int maxGridX = Math.Max(0, data.Columns - 1);
            int maxGridY = Math.Max(0, data.Rows - 1);
            gridPoint.X = Math.Max(0, Math.Min(gridPoint.X, maxGridX));
            gridPoint.Y = Math.Max(0, Math.Min(gridPoint.Y, maxGridY));

            // 有効アイテム数の範囲内に収める
            if (data.ValidItemCount > 0)
            {
                int currentIndex = gridPoint.Y * data.Columns + gridPoint.X;
                int maxIndex = data.ValidItemCount - 1;

                if (currentIndex > maxIndex)
                {
                    gridPoint.X = maxIndex % data.Columns;
                    gridPoint.Y = maxIndex / data.Columns;
                }
            }

            if (_currentGridPoint != gridPoint)
            {
                _currentGridPoint = gridPoint;
                UpdateSelection();
            }
        }

        public override void OnMouseUp(MouseEventArgs e, LayerData data)
        {
            if (e.Button == MouseButtons.Right && _isDragging)
            {
                _isDragging = false;
            }
        }

        private void UpdateSelection()
        {
            int minX = Math.Min(_startGridPoint.X, _currentGridPoint.X);
            int minY = Math.Min(_startGridPoint.Y, _currentGridPoint.Y);
            int maxX = Math.Max(_startGridPoint.X, _currentGridPoint.X);
            int maxY = Math.Max(_startGridPoint.Y, _currentGridPoint.Y);

            SelectedGrids = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
            _requestInvalidate?.Invoke();
        }

        public void ClearSelect()
        {
            SelectedGrids = Rectangle.Empty;
            _requestInvalidate?.Invoke();
        }
    }
}
