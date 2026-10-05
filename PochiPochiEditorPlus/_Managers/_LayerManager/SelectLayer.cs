using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class SelectLayer : LayerBase
    {
        // 選択可能な最大範囲
        // 0以下の場合は制限なし
        public Size MaxSelectSize { get; set; }
        // 選択されているグリッドの範囲
        public Rectangle SelectedGrids { get; private set; }
        // 選択範囲が確定した際に通知するイベント
        public Action SelectChanged { get; set; }

        private Point _currentGridPoint = Point.Empty;
        private bool _isDragging = false;
        private Point _startGridPoint = Point.Empty;

        /// <summary>
        /// 指定したインデックス(1マス)を選択状態にする。
        /// </summary>
        public void SelectSingleItem(int index)
        {
            // インデックスが有効な範囲内か判定
            if (index < 0 || index >= Data.ValidItemCount)
            {
                ClearSelect();
                return;
            }

            // マス座標を逆算する
            int gridX = index % Data.Columns;
            int gridY = index / Data.Columns;

            SelectedGrids = new Rectangle(gridX, gridY, 1, 1);
            _startGridPoint = new Point(gridX, gridY);
            _currentGridPoint = new Point(gridX, gridY);

            RequestInvalidate.Invoke();
        }

        private void ClearSelect()
        {
            SelectedGrids = Rectangle.Empty;
        }

        /// <summary>
        /// 選択されているすべての有効なインデックスのリストを取得する。
        /// </summary>
        public List<int> GetSelectedIndexList()
        {
            var selectedIndexList = new List<int>();

            // 選択範囲が存在しない場合
            if (SelectedGrids.Width <= 0 || SelectedGrids.Height <= 0)
            {
                return selectedIndexList;
            }

            for (int gridY = SelectedGrids.Top; gridY < SelectedGrids.Bottom; gridY++)
            {
                for (int gridX = SelectedGrids.Left; gridX < SelectedGrids.Right; gridX++)
                {
                    int index = gridY * Data.Columns + gridX;

                    // アイテムが存在する有効な範囲内のみ取得
                    if (index >= 0 && index < Data.ValidItemCount)
                    {
                        selectedIndexList.Add(index);
                    }
                }
            }

            return selectedIndexList;
        }

        /// <summary>
        /// 選択されているマス座標のリストを取得する。
        /// </summary>
        public List<Point> GetSelectedGridPoints()
        {
            var points = new List<Point>();
            if (SelectedGrids.Width <= 0 || SelectedGrids.Height <= 0)
            {
                return points;
            }

            for (int gridY = SelectedGrids.Top; gridY < SelectedGrids.Bottom; gridY++)
            {
                for (int gridX = SelectedGrids.Left; gridX < SelectedGrids.Right; gridX++)
                {
                    if (Data.IsValidGrid(gridX, gridY))
                    {
                        points.Add(new Point(gridX, gridY));
                    }
                }
            }
            return points;
        }

        public override void Draw(Graphics gfx)
        {
            if (SelectedGrids.Width <= 0 || SelectedGrids.Height <= 0) return;

            int drawX = SelectedGrids.X * Data.ScaledGridSize;
            int drawY = SelectedGrids.Y * Data.ScaledGridSize;
            int drawWidth = SelectedGrids.Width * Data.ScaledGridSize;
            int drawHeight = SelectedGrids.Height * Data.ScaledGridSize;

            using (var brush = new SolidBrush(Color.Red))
            {
                // 四辺を描画
                gfx.FillRectangle(
                    brush, 
                    drawX, 
                    drawY, 
                    drawWidth,
                    1);
                gfx.FillRectangle(
                    brush, 
                    drawX, 
                    drawY + drawHeight - 1, 
                    drawWidth,
                    1);
                gfx.FillRectangle(
                    brush, 
                    drawX,
                    drawY + 1,
                    1,
                    drawHeight - 2);
                gfx.FillRectangle(
                    brush, 
                    drawX + drawWidth - 1, 
                    drawY + 1, 
                    1, 
                    drawHeight - 2);
            }
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            // マス座標を取得
            var gridPoint = Data.GetGridPoint(e.Location);

            // 有効なマスかどうかを判定
            if (Data.IsValidGrid(gridPoint.X, gridPoint.Y))
            {
                _startGridPoint = gridPoint;
                _currentGridPoint = gridPoint;
                _isDragging = true;
                UpdateSelection();
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!_isDragging) return;

            // マス座標を取得
            var gridPoint = Data.GetGridPoint(e.Location);

            // 最大選択サイズの制限
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
            int maxGridX = Math.Max(0, Data.Columns - 1);
            int maxGridY = Math.Max(0, Data.Rows - 1);
            gridPoint.X = Math.Max(0, Math.Min(gridPoint.X, maxGridX));
            gridPoint.Y = Math.Max(0, Math.Min(gridPoint.Y, maxGridY));

            // 有効アイテム数の範囲内に収める
            if (Data.ValidItemCount > 0)
            {
                int currentIndex = gridPoint.Y * Data.Columns + gridPoint.X;
                int maxIndex = Data.ValidItemCount - 1;

                if (currentIndex > maxIndex)
                {
                    gridPoint.X = maxIndex % Data.Columns;
                    gridPoint.Y = maxIndex / Data.Columns;
                }
            }

            if (_currentGridPoint != gridPoint)
            {
                _currentGridPoint = gridPoint;
                UpdateSelection();
            }
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _isDragging)
            {
                _isDragging = false;
                SelectChanged?.Invoke();
            }
        }

        private void UpdateSelection()
        {
            int minX = Math.Min(_startGridPoint.X, _currentGridPoint.X);
            int minY = Math.Min(_startGridPoint.Y, _currentGridPoint.Y);
            int maxX = Math.Max(_startGridPoint.X, _currentGridPoint.X);
            int maxY = Math.Max(_startGridPoint.Y, _currentGridPoint.Y);

            SelectedGrids = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
            RequestInvalidate.Invoke();
        }
    }
}
