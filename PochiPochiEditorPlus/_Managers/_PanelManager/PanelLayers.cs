using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._PanelManager
{
    public sealed class PanelLayers<TEnum> where TEnum : Enum
    {
        public int Scale { get; set; }
        public Func<Point> ScrollOffsetProvider { get; set; }

        // 対象のパネル
        private Panel _panel = null;
        // ImageLayerを基礎とする
        private Dictionary<TEnum, ImageLayer<TEnum>> _layers = null;

        public PanelLayers(Panel panel, EventBinder eventBinder)
        {
            _panel = panel;
            _layers = new Dictionary<TEnum, ImageLayer<TEnum>>();

            // イベントの登録
            eventBinder.BindCustom(
                () => _panel.Paint += Panel_Paint,
                () => _panel.Paint -= Panel_Paint);
        }

        public Bitmap GetImage(TEnum index)
        {
            return _layers[index].Image;
        }

        public void SetImage(TEnum index, Bitmap newImage)
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

        public void SetVisible(TEnum index, bool visible)
        {
            if (_layers.TryGetValue(index, out var layer))
            {
                layer.Visible = visible;
                _panel.Invalidate();
            }
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            // X軸とY軸にスクロールオフセットを取得
            var offset = ScrollOffsetProvider?.Invoke() ?? Point.Empty;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            foreach (var kvp in _layers.OrderBy(x => x.Key))
            {
                var layer = kvp.Value;
                if (!layer.Visible || layer.Image == null) continue;

                // 拡大後の描画幅と高さを計算
                var scaledWidth = layer.Image.Width * Scale;
                var scaledHeight = layer.Image.Height * Scale;

                e.Graphics.DrawImage(
                    layer.Image,
                    new Rectangle(-offset.X, -offset.Y, scaledWidth, scaledHeight),
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
