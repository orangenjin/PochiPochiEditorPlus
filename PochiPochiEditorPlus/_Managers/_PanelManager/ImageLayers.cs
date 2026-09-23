using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class ImageLayers<TEnum> where TEnum : Enum
    {
        private EventBinder _eventBinder = null;
        private Panel _panel = null;
        private Dictionary<TEnum, ImageLayer<TEnum>> _layers = null;

        public ImageLayers(Panel panel)
        {
            _eventBinder = new EventBinder();
            _panel = panel;
            _layers = new Dictionary<TEnum, ImageLayer<TEnum>>();

            // イベントの登録
            _eventBinder.BindCustom(
                () => _panel.Paint += Panel_Paint,
                () => _panel.Paint -= Panel_Paint);
            _eventBinder.BindCtrl(
                h => _panel.Disposed += h,
                h => _panel.Disposed -= h);
        }

        public void AddLayer(TEnum id)
        {
            _layers[id] = new ImageLayer<TEnum>(id);
            _panel.Invalidate();
        }

        public ImageLayer<TEnum> GetLayer(TEnum id)
        {
            return _layers[id];
        }

        public void SetVisible(TEnum id, bool visible)
        {
            _layers[id].Visible = visible;
            _panel.Invalidate();
        }

        public void SetImage(TEnum id, Image image)
        {
            _layers[id].Image = image;
            _panel.Invalidate();
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var layer in _layers.Values)
            {
                // 無効化されている場合はスキップ
                if (!layer.Visible) continue;

                // 画像データがない場合はスキップ
                if (layer.Image == null) continue;

                e.Graphics.DrawImage(
                    layer.Image,
                    _panel.ClientRectangle);
            }
        }
    }

    /// <summary>
    /// 各レイヤーに配置するクラスを定義する。
    /// </summary>
    public sealed class ImageLayer<TEnum> where TEnum : Enum
    {
        public TEnum Id { get; }
        public bool Visible { get; set; }
        public Image Image { get; set; }

        public ImageLayer(TEnum id)
        {
            Id = id;
            Visible = true;
        }
    }
}
