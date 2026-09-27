using System.Windows.Forms;
using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class PasteLayer : LayerBase
    {
        private ClipboardData _clipBoardData = null;
        private BlockImageLayer _targetImageLayer = null;

        public PasteLayer(
            ClipboardData clipBoardData,
            BlockImageLayer targetImageLayer)
        {
            _clipBoardData = clipBoardData;
            _targetImageLayer = targetImageLayer;
        }

        public override void Draw(Graphics gfx)
        {
            // ペーストのプレビュー等を描画する
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            // 左クリックでペースト
            if (e.Button != MouseButtons.Left) return;
            if (_clipBoardData.Images == null) return;

            // マウス座標からクリック地点のマス座標を取得
            var startGridPoint = Data.GetGridPoint(e.Location);
            bool isPasted = false;

            for (int y = 0; y < _clipBoardData.Height; y++)
            {
                for (int x = 0; x < _clipBoardData.Width; x++)
                {
                    int targetX = startGridPoint.X + x;
                    int targetY = startGridPoint.Y + y;

                    // 有効な範囲内かどうかを判定
                    if (Data.IsValidGrid(targetX, targetY))
                    {
                        var imgToPaste = _clipBoardData.Images[x, y];
                        _targetImageLayer.SetBlockImage(targetX, targetY, imgToPaste);
                        isPasted = true;
                    }
                }
            }

            // 再描画
            if (isPasted)
            {
                RequestInvalidate.Invoke();
            }
        }
    }
}
