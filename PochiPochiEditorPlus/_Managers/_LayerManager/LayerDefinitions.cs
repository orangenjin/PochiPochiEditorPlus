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
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int TotalGridCount => Columns * Rows; // 描画領域として確保される合計マス数

        // スクロールによるオフセットを補正する
        public Func<Point> ScrollOffsetProvider { get; set; }
        public Point ScrollOffset => ScrollOffsetProvider?.Invoke() ?? Point.Empty;

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
        /// マウスによる座標を、スクロールを考慮したマス座標を取得する。
        /// </summary>
        public Point GetGridPoint(Point mouseLoc)
        {
            var offset = ScrollOffset;
            int x = (mouseLoc.X + offset.X) / ScaledGridSize;
            int y = (mouseLoc.Y + offset.Y) / ScaledGridSize;
            return new Point(x, y);
        }
    }

    public abstract class LayerBase
    {
        public bool Visible { get; set; }

        public abstract void Draw(Graphics gfx, Rectangle rect, LayerData data);

        public virtual void OnMouseDown(MouseEventArgs e, LayerData data) { }
        public virtual void OnMouseMove(MouseEventArgs e, LayerData data) { }
        public virtual void OnMouseUp(MouseEventArgs e, LayerData data) { }
    }
}
