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
            _layerData.ScrollOffsetProvider = 
                () => new Point(
                    ScrollX * _layerData.ScaledGridSize,
                    ScrollY * _layerData.ScaledGridSize);

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
        /// スクロールバーの範囲を更新する。
        /// </summary>
        public void UpdateScrollRange()
        {
            UpdateRangeX();
            UpdateRangeY();
        }

        private void UpdateRangeX()
        {
            if (_hsb == null) return;

            int clientWidth = _panel.ClientSize.Width;
            int scaledSize = _layerData.ScaledGridSize;
            int totalColumns = _layerData.Columns;

            // 表示できる列数
            int visibleColumns = clientWidth / scaledSize;

            if (totalColumns > visibleColumns)
            {
                _hsb.Enabled = true;
                _hsb.Minimum = 0;
                _hsb.SmallChange = 1;
                _hsb.LargeChange = Math.Max(1, visibleColumns);

                int maxScroll = totalColumns - visibleColumns;
                _hsb.Maximum = maxScroll + _hsb.LargeChange - 1;
            }
            else
            {
                _hsb.Enabled = false;
                _hsb.Value = 0;
            }
        }

        private void UpdateRangeY()
        {
            if (_vsb == null) return;

            int clientHeight = _panel.ClientSize.Height;
            int scaledSize = _layerData.ScaledGridSize;
            int totalRows = _layerData.Rows;

            // 表示できる行数
            int visibleRows = clientHeight / scaledSize;

            if (totalRows > visibleRows)
            {
                _vsb.Enabled = true;
                _vsb.Minimum = 0;
                _vsb.SmallChange = 1; 
                _vsb.LargeChange = Math.Max(1, visibleRows);

                int maxScroll = totalRows - visibleRows;
                _vsb.Maximum = maxScroll + _vsb.LargeChange - 1;
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
