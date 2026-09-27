using System.Windows.Forms;
using System.Drawing;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class PasteLayer : LayerBase
    {
        public ClipboardData Clipboard { get; set; }
        public BlockImageLayer TargetImageLayer { get; set; }

        public override void OnMouseDown(MouseEventArgs e)
        {
            // 左クリックでペースト
            if (e.Button != MouseButtons.Left) return;
            if (Clipboard.Images == null || TargetImageLayer == null) return;

            // マウス座標からクリック地点のマス座標を取得
            var startGridPoint = Data.GetGridPoint(e.Location);
            bool isPasted = false;

            for (int y = 0; y < Clipboard.Height; y++)
            {
                for (int x = 0; x < Clipboard.Width; x++)
                {
                    int targetX = startGridPoint.X + x;
                    int targetY = startGridPoint.Y + y;

                    // 有効なマス範囲内かどうかを判定
                    if (Data.IsValidGrid(targetX, targetY))
                    {
                        var imgToPaste = Clipboard.Images[x, y];

                        // 画像が存在する場合のみ反映
                        if (imgToPaste != null)
                        {
                            TargetImageLayer.SetBlockImage(
                                targetX,
                                targetY, 
                                (Bitmap)imgToPaste.Clone());
                            isPasted = true;
                        }
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
