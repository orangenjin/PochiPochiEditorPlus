using System;
using System.Windows.Forms;
using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class PasteLayer : LayerBase
    {
        private ClipboardData _clipboard;
        private BlockImageLayer _targetImageLayer;
        private Action _requestInvalidate;

        public PasteLayer(
            LayerData layerData,
            ClipboardData clipboard,
            BlockImageLayer targetImageLayer,
            Action requestInvalidate)
        {
            _layerData = layerData;
            _clipboard = clipboard;
            _targetImageLayer = targetImageLayer;
            _requestInvalidate = requestInvalidate;
        }

        public override void Draw(Graphics gfx)
        {
            // ペーストのプレビュー等を描画する
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            // 左クリックでペースト
            if (e.Button != MouseButtons.Left) return;
            if (_clipboard.CopiedImages == null) return;

            // マウス座標からクリック地点のマス座標を取得
            var startGridPoint = _layerData.GetGridPoint(e.Location);
            bool isPasted = false;

            for (int y = 0; y < _clipboard.Height; y++)
            {
                for (int x = 0; x < _clipboard.Width; x++)
                {
                    int targetX = startGridPoint.X + x;
                    int targetY = startGridPoint.Y + y;

                    // 有効な範囲内かどうかを判定
                    if (_layerData.IsValidGrid(targetX, targetY))
                    {
                        var imgToPaste = _clipboard.CopiedImages[x, y];
                        _targetImageLayer.SetBlockImage(targetX, targetY, imgToPaste);
                        isPasted = true;
                    }
                }
            }

            // 再描画
            if (isPasted)
            {
                _requestInvalidate?.Invoke();
            }
        }
    }
}
