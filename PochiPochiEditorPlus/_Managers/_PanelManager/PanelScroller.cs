using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelScroller
    {
        public EventHandler ScrollChanged { get; set; }

        private Panel _panel = null;
        private VScrollBar _vsb = null;
        private int _imageHeight = 0;

        public int ScrollY => _vsb.Value;

        public PanelScroller(
            Panel panel,
            VScrollBar vsb,
            EventBinder eventBinder)
        {
            _panel = panel;
            _vsb = vsb;

            eventBinder.BindCustom(
                () => _vsb.ValueChanged += Vsb_ValueChanged,
                () => _vsb.ValueChanged -= Vsb_ValueChanged);
        }

        public void SetProperties(
            int height, 
            int largeChange, 
            int smallChange)
        {
            _imageHeight = height;
            _vsb.LargeChange = largeChange;
            _vsb.SmallChange = smallChange;
            UpdateScrollBar();
        }

        private void Vsb_ValueChanged(object sender, EventArgs e)
        {
            _panel.Invalidate();
            ScrollChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateScrollBar()
        {
            if (_imageHeight > _panel.ClientSize.Height)
            {
                _vsb.Enabled = true;
                _vsb.Minimum = 0;
                _vsb.Maximum = _imageHeight - _panel.ClientSize.Height + _vsb.LargeChange - 1;
                _vsb.Value = 0;
            }
            else
            {
                _vsb.Enabled = false;
                _vsb.Value = 0;
            }
        }
    }
}
