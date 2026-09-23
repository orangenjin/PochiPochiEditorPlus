using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class ImageLayers<TEnum> where TEnum : Enum
    {
        private Panel _panel = null;
        private Dictionary<TEnum, ImageLayer<TEnum>> _layers = null;

        private int _scrollY = 0; // PanelScrollerと併用前提
        private int _scale = 0;

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

        public void SetImage(
            TEnum id, 
            Bitmap newImage, 
            int scale = Constants.DefaultScale)
        {
            // 新規ならインスタンスを生成
            if (!_layers.TryGetValue(id, out var layer))
            {
                layer = new ImageLayer<TEnum>(id);
                _layers[id] = layer;
            }

            // 画像を入れ替え
            var oldImage = layer.Image;
            layer.Image = newImage;

            // 画像を破棄
            if (oldImage != null && oldImage != newImage)
            {
                oldImage.Dispose();
            }

            // 拡大設定
            _scale = scale;

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
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            foreach (var layer in _layers.Values)
            {
                // 無効化されている場合はスキップ
                if (!layer.Visible) continue;

                // 画像データがない場合はスキップ
                if (layer.Image == null) continue;

                // 拡大後の描画幅と高さを計算
                var scaledWidth = layer.Image.Width * _scale;
                var scaledHeight = layer.Image.Height * _scale;

                e.Graphics.DrawImage(
                    layer.Image,
                    new Rectangle(0, 0, scaledWidth, scaledHeight),
                    new Rectangle(0, 0, layer.Image.Width, layer.Image.Height),
                    GraphicsUnit.Pixel);
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
