using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelScroller
    {
        public EventHandler Scrolled { get; set; }
        public int ScrollY => _vsb.Value;

        private Panel _panel = null;
        private VScrollBar _vsb = null;

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

        public void SetHeight(
            int height, 
            int largeChange, 
            int smallChange)
        {
            _vsb.LargeChange = largeChange;
            _vsb.SmallChange = smallChange;

            if (height > _panel.ClientSize.Height)
            {
                _vsb.Enabled = true;
                _vsb.Minimum = 0;
                _vsb.Maximum = height - _panel.ClientSize.Height + _vsb.LargeChange - 1;
            }
            else
            {
                _vsb.Enabled = false;
                _vsb.Value = 0;
            }
        }

        private void Vsb_ValueChanged(object sender, EventArgs e)
        {
            _panel.Invalidate();
            Scrolled?.Invoke(this, EventArgs.Empty);
        }
    }
}
