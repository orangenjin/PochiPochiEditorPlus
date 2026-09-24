using System;
using System.Drawing;
using System.Windows.Forms;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class LayerData
    {
        // この最小単位(1マス分、拡大前)で管理する
        public int GridSize { get; set; }
        // 拡大率を定義する。
        public int Scale { get; set; }
        // 画面上の1マスのサイズ
        public int ScaledGridSize => GridSize * Scale;
        // 有効なアイテム総数
        public int ValidItemCount { get; set; }
        // パネルのサイズに基づいて計算される列・行・合計マス
        public int Columns { get; set; }
        public int Rows { get; set; }
        // 描画領域として確保される合計マス数
        public int TotalGridCount => Columns * Rows;

        // スクロールによるオフセットを補正する
        public Func<Point> ScrollOffsetProvider { get; set; }
        public Point ScrollOffset => ScrollOffsetProvider?.Invoke() ?? Point.Empty;

        /// <summary>
        /// パネルの幅に基づいて、列数と行数を再計算する。
        /// </summary>
        public void CalcLayout(int panelWidth)
        {
            if (ScaledGridSize <= 0)
            {
                Columns = 0;
                Rows = 0;
                return;
            }

            Columns = panelWidth / ScaledGridSize;
            Rows = (ValidItemCount + Columns - 1) / Columns;
        }

        /// <summary>
        /// 指定されたマス座標が有効なアイテムであるかを判定する。
        /// </summary>
        public bool IsValidGrid(int gridX, int gridY)
        {
            if (gridX < 0 || gridX >= Columns || gridY < 0 || gridY >= Rows) return false;
            int index = gridY * Columns + gridX;
            return index < ValidItemCount; // 有効アイテム範囲内かどうか
        }

        /// <summary>
        /// マウスによる座標を、スクロールを考慮したマス座標に変換する。
        /// </summary>
        public Point GetGridPoint(Point mouseLoc)
        {
            var offset = ScrollOffset;
            int rawX = mouseLoc.X + offset.X;
            int rawY = mouseLoc.Y + offset.Y;

            // 負の座標を丸める
            int x = rawX < 0 
                ? (rawX - ScaledGridSize + 1) / ScaledGridSize 
                : rawX / ScaledGridSize;
            int y = rawY < 0 
                ? (rawY - ScaledGridSize + 1) / ScaledGridSize 
                : rawY / ScaledGridSize;

            return new Point(x, y);
        }
    }

    public abstract class LayerBase
    {
        public bool Visible { get; set; }

        public abstract void Draw(Graphics gfx, LayerData data);

        public virtual void OnMouseDown(MouseEventArgs e, LayerData data) { }
        public virtual void OnMouseMove(MouseEventArgs e, LayerData data) { }
        public virtual void OnMouseUp(MouseEventArgs e, LayerData data) { }
    }
}
