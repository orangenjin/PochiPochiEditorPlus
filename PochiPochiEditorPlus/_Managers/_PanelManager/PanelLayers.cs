using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelLayers<TEnum> where TEnum : Enum
    {
        // 対象のパネル
        private Panel _panel = null;
        // ImageLayerを基礎とする
        private Dictionary<TEnum, ImageLayer<TEnum>> _layers = null;

        private int _scale = Constants.DefaultScale;
        private int _scrollX = 0; // PanelScrollerと併用前提
        private int _scrollY = 0;

        public PanelLayers(Panel panel, EventBinder eventBinder)
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
            TEnum index, 
            Bitmap newImage)
        {
            // 新規ならインスタンスを生成
            if (!_layers.TryGetValue(index, out var layer))
            {
                layer = new ImageLayer<TEnum>();
                _layers[index] = layer;
            }

            // 画像を入れ替え
            var oldImage = layer.Image;
            layer.Image = newImage;

            // 画像を破棄
            if (oldImage != null && oldImage != newImage)
            {
                oldImage.Dispose();
            }

            _panel.Invalidate();
        }

        public void SetVisible(TEnum id, bool visible)
        {
            _layers[id].Visible = visible;
            _panel.Invalidate();
        }

        public void SetScrollX(int scrollX)
        {
            _scrollX = Math.Max(0, scrollX);
            _panel.Invalidate();
        }

        public void SetScrollY(int scrollY)
        {
            _scrollY = Math.Max(0, scrollY);
            _panel.Invalidate();
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, -_scrollX);
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
    /// 各レイヤーに配置する土台となるクラスを定義する。
    /// </summary>
    public sealed class ImageLayer<TEnum> where TEnum : Enum
    {
        public Bitmap Image { get; set; }
        public bool Visible { get; set; }

        public ImageLayer()
        {
            // 初期設定では表示する
            Visible = true;
        }
    }
}
