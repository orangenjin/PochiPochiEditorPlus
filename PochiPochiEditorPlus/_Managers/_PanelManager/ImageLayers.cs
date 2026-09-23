using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class ImageLayers<TEnum> where TEnum : Enum
    {
        private Panel _panel = null;
        private Dictionary<TEnum, ImageLayer<TEnum>> _layers = null;

        // PanelScrollerと併用前提
        private int _scrollY = 0;

        public ImageLayers(Panel panel, EventBinder eventBinder)
        {
            _panel = panel;
            _layers = new Dictionary<TEnum, ImageLayer<TEnum>>();

            // イベントの登録
            eventBinder.BindCustom(
                () => _panel.Paint += Panel_Paint,
                () => _panel.Paint -= Panel_Paint);
        }

        public Bitmap GetImage(TEnum id)
        {
            return _layers[id].Image;
        }

        public void SetImage(TEnum id, Bitmap image)
        {
            // 画像を破棄
            if (_layers.TryGetValue(id, out var layer))
            {
                layer.Image?.Dispose();
            }
            _layers[id] = new ImageLayer<TEnum>(id);

            _layers[id].Image = image;
            _panel.Invalidate();
        }

        public void SetVisible(TEnum id, bool visible)
        {
            _layers[id].Visible = visible;
            _panel.Invalidate();
        }

        public void SetScrollY(int scrollY)
        {
            _scrollY = Math.Max(0, scrollY);
            _panel.Invalidate();
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, -_scrollY);

            foreach (var layer in _layers.Values)
            {
                // 無効化されている場合はスキップ
                if (!layer.Visible) continue;

                // 画像データがない場合はスキップ
                if (layer.Image == null) continue;

                e.Graphics.DrawImage(layer.Image, 0, 0);
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
        public Bitmap Image { get; set; }

        public ImageLayer(TEnum id)
        {
            Id = id;
            Visible = true;
        }
    }
}
