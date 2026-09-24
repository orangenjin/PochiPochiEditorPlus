using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class LayerScroller
    {
        public int ScrollX
        {
            get => _hsb?.Value ?? 0;
            set
            {
                if (_hsb != null)
                {
                    _hsb.Value = Math.Max(
                        _hsb.Minimum,
                        Math.Min(_hsb.Maximum - _hsb.LargeChange + 1, value));
                }
            }
        }

        public int ScrollY
        {
            get => _vsb?.Value ?? 0;
            set
            {
                if (_vsb != null)
                {
                    _vsb.Value = Math.Max(
                        _vsb.Minimum,
                        Math.Min(_vsb.Maximum - _vsb.LargeChange + 1, value));
                }
            }
        }

        private Panel _panel;
        private HScrollBar _hsb;
        private VScrollBar _vsb;
        private LayerData _layerData;

        public LayerScroller(
            Panel panel,
            HScrollBar hsb,
            VScrollBar vsb,
            LayerData layerData,
            EventBinder eventBinder)
        {
            _panel = panel;
            _hsb = hsb;
            _vsb = vsb;
            _layerData = layerData;

            // このインスタンスのスクロール位置情報を提供
            _layerData.ScrollOffsetProvider = () => new Point(ScrollX, ScrollY);

            if (_hsb != null)
            {
                // 水平方向のスクロールイベント
                eventBinder.BindCustom(
                    () => _hsb.ValueChanged += ScrollBar_ValueChanged,
                    () => _hsb.ValueChanged -= ScrollBar_ValueChanged);
            }

            if (_vsb != null)
            {
                // 垂直方向のスクロールイベント
                eventBinder.BindCustom(
                    () => _vsb.ValueChanged += ScrollBar_ValueChanged,
                    () => _vsb.ValueChanged -= ScrollBar_ValueChanged);
            }

            // マウスホイールのスクロールイベント
            eventBinder.BindCustom(
                () => _panel.MouseWheel += Panel_MouseWheel,
                () => _panel.MouseWheel -= Panel_MouseWheel);
        }

        /// <summary>
        /// スクロールバーの範囲を自動更新する。
        /// </summary>
        public void UpdateScrollRange()
        {
            // スクロール量を計算
            int contentWidth = _layerData.Columns * _layerData.ScaledGridSize;
            int contentHeight = _layerData.Rows * _layerData.ScaledGridSize;
            int smallChange = _layerData.ScaledGridSize;

            UpdateRangeX(contentWidth, smallChange);
            UpdateRangeY(contentHeight, smallChange);
        }

        private void UpdateRangeX(int contentWidth, int smallChange)
        {
            if (_hsb == null) return;

            int clientWidth = _panel.ClientSize.Width;

            if (contentWidth > clientWidth)
            {
                _hsb.Enabled = true;
                _hsb.LargeChange = clientWidth;
                _hsb.SmallChange = smallChange;
                _hsb.Minimum = 0;
                _hsb.Maximum = contentWidth - 1;
            }
            else
            {
                _hsb.Enabled = false;
                _hsb.Value = 0;
            }
        }

        private void UpdateRangeY(int contentHeight, int smallChange)
        {
            if (_vsb == null) return;

            int clientHeight = _panel.ClientSize.Height;

            if (contentHeight > clientHeight)
            {
                _vsb.Enabled = true;
                _vsb.LargeChange = clientHeight;
                _vsb.SmallChange = smallChange;
                _vsb.Minimum = 0;
                _vsb.Maximum = contentHeight - 1;
            }
            else
            {
                _vsb.Enabled = false;
                _vsb.Value = 0;
            }
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _panel.Invalidate();
        }

        private void Panel_MouseWheel(object sender, MouseEventArgs e)
        {
            // シフトキーで水平スクロール
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (_hsb != null && _hsb.Enabled)
                {
                    // 上下の仕様が逆なので、符号を反転させる
                    ScrollX += e.Delta > 0
                        ? -_hsb.SmallChange
                        : _hsb.SmallChange;
                }
            }
            else
            {
                if (_vsb != null && _vsb.Enabled)
                {
                    ScrollY += e.Delta > 0
                        ? -_vsb.SmallChange
                        : _vsb.SmallChange;
                }
            }
        }
    }
}
