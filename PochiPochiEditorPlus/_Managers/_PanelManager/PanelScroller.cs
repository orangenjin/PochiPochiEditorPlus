using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelScroller
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
        public bool EnabledX => _hsb?.Enabled ?? false;
        public bool EnabledY => _vsb?.Enabled ?? false;

        private readonly Panel _panel;
        private readonly HScrollBar _hsb;
        private readonly VScrollBar _vsb;

        public PanelScroller(
            Panel panel, 
            HScrollBar hsb, 
            VScrollBar vsb, 
            EventBinder eventBinder)
        {
            _panel = panel;
            _hsb = hsb;
            _vsb = vsb;

            if (_hsb != null)
            {
                eventBinder.BindCustom(
                    () => _hsb.ValueChanged += ScrollBar_ValueChanged,
                    () => _hsb.ValueChanged -= ScrollBar_ValueChanged);
            }

            if (_vsb != null)
            {
                eventBinder.BindCustom(
                    () => _vsb.ValueChanged += ScrollBar_ValueChanged,
                    () => _vsb.ValueChanged -= ScrollBar_ValueChanged);
            }
        }

        public void UpdateRangeX(int contentWidth, int smallChange)
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
                _hsb.Value = 0;
            }
            else
            {
                _hsb.Enabled = false;
                _hsb.Value = 0;
            }
        }

        public void UpdateRangeY(int contentHeight, int smallChange)
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
                _vsb.Value = 0;
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
    }
}
