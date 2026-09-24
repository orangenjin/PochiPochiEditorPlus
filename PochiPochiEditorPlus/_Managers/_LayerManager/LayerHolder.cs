using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class LayerHolder<TEnum> where TEnum : Enum
    {
        // レイヤーの基礎情報を保持
        public LayerData Data { get; }
        // 各レイヤーについて
        public Dictionary<TEnum, ImageLayer> ImageLayers { get; }
        public GridLayer GridLayer { get; }
        public SelectorLayer SelectorLayer { get; }

        // 対象のパネルコントロール
        private Panel _panel;

        public LayerHolder(Panel panel, EventBinder eventBinder)
        {
            Data = new LayerData();
            ImageLayers = new Dictionary<TEnum, ImageLayer>();
            GridLayer = new GridLayer();
            SelectorLayer = new SelectorLayer(() => _panel.Invalidate());
            _panel = panel;

            eventBinder.BindCustom(
                () => {
                    _panel.Paint += Panel_Paint;
                    _panel.MouseDown += Panel_MouseDown;
                    _panel.MouseMove += Panel_MouseMove;
                    _panel.MouseUp += Panel_MouseUp;
                },
                () => {
                    _panel.Paint -= Panel_Paint;
                    _panel.MouseDown -= Panel_MouseDown;
                    _panel.MouseMove -= Panel_MouseMove;
                    _panel.MouseUp -= Panel_MouseUp;
                });
        }

        /// <summary>
        /// 全レイヤーに共通する初期設定を行う。
        /// </summary>
        public void Initialize(
            int gridSize,
            int scale,
            int validItemCount)
        {
            Data.GridSize = gridSize;
            Data.Scale = scale;
            Data.ValidItemCount = validItemCount;
            Data.CalcLayout(_panel.ClientSize.Width);
        }

        /// <summary>
        /// 特定のレイヤーを取得する。
        /// </summary>
        public ImageLayer GetImageLayer(TEnum key)
        {
            ImageLayers.TryGetValue(key, out var layer);
            return layer;
        }

        /// <summary>
        /// バイト配列形式で画像データを登録する。
        /// </summary>
        public void SetImageLayer(
            TEnum key, 
            byte[] imageData, 
            byte[] paletteData, 
            int width, 
            int height, 
            bool showBackColor = true)
        {
            // 新規ならインスタンスを生成
            if (!ImageLayers.TryGetValue(key, out var layer))
            {
                layer = new ImageLayer();
                ImageLayers[key] = layer;
            }

            layer.SetImageData(
                imageData, 
                paletteData, 
                width, 
                height, 
                showBackColor);
            _panel.Invalidate();
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            // 描画方法の設定
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            // スクロール位置を適用
            var offset = Data.ScrollOffset;
            e.Graphics.TranslateTransform(-offset.X, -offset.Y);

            // 各レイヤーの描画
            foreach (var kvp in ImageLayers.OrderBy(x => x.Key))
            {
                if (kvp.Value.Visible) kvp.Value.Draw(e.Graphics, Data);
            }
            if (GridLayer.Visible) GridLayer.Draw(e.Graphics, Data);
            if (SelectorLayer.Visible) SelectorLayer.Draw(e.Graphics, Data);

            e.Graphics.ResetTransform();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e) 
            => SelectorLayer.OnMouseDown(e, Data);
        private void Panel_MouseMove(object sender, MouseEventArgs e) 
            => SelectorLayer.OnMouseMove(e, Data);
        private void Panel_MouseUp(object sender, MouseEventArgs e)
            => SelectorLayer.OnMouseUp(e, Data);

        /// <summary>
        /// 画像レイヤーの表示を設定する。
        /// </summary>
        public void SetLayerVisible(TEnum key, bool visible)
        {
            if (ImageLayers.TryGetValue(key, out var layer))
            {
                layer.Visible = visible;
                _panel.Invalidate();
            }
        }

        /// <summary>
        /// グリッドレイヤーの表示を設定する。
        /// </summary>
        public void SetGridVisible(bool visible)
        {
            GridLayer.Visible = visible;
            _panel.Invalidate();
        }

        /// <summary>
        /// 選択範囲レイヤーの表示を設定する。
        /// </summary>
        public void SetSelectorVisible(bool visible)
        {
            SelectorLayer.Visible = visible;
            _panel.Invalidate();
        }

        /// <summary>
        /// 選択されているアイテムのインデックスを取得する。
        /// </summary>
        public List<int> GetSelectedIndex()
        {
            return SelectorLayer.GetSelectedIndex(Data);
        }

        /// <summary>
        /// 指定したインデックスのアイテムを単一選択する。
        /// </summary>
        public void SelectSingleItem(int index)
        {
            SelectorLayer.SelectSingleItem(index, Data);
        }
    }
}
