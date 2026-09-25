using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Dictionary<TEnum, LayerBase> Layers { get; }
        public GridLayer GridLayer { get; }
        public SelectorLayer SelectorLayer { get; }

        // 対象のパネルコントロール
        private Panel _panel;

        public LayerHolder(Panel panel, EventBinder eventBinder)
        {
            Data = new LayerData();
            Layers = new Dictionary<TEnum, LayerBase>();
            GridLayer = new GridLayer(Data);
            SelectorLayer = new SelectorLayer(Data, () => _panel.Invalidate());
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
        public T GetLayer<T>(TEnum key) where T : LayerBase
        {
            if (Layers.TryGetValue(key, out var layer) 
                && layer is T typedLayer)
            {
                return typedLayer;
            }
            return null;
        }

        /// <summary>
        /// 特殊レイヤーを登録する。
        /// </summary>
        public void AddCustomLayer(TEnum key, LayerBase layer)
        {
            Layers[key] = layer;
            _panel.Invalidate();
        }

        /// <summary>
        /// 画像レイヤーを登録する。
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
            if (!Layers.TryGetValue(key, out var layer) 
                || !(layer is ImageLayer))
            {
                layer = new ImageLayer(Data);
                Layers[key] = layer;
            }

            ((ImageLayer)layer).SetImageData(
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
            foreach (var kvp in Layers.OrderBy(x => x.Key))
            {
                if (kvp.Value.Visible)
                {
                    kvp.Value.Draw(e.Graphics);
                }
            }
            if (GridLayer.Visible)
            {
                GridLayer.Draw(e.Graphics);
            }
            if (SelectorLayer.Visible)
            {
                SelectorLayer.Draw(e.Graphics);
            }

            e.Graphics.ResetTransform();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e) 
            => SelectorLayer.OnMouseDown(e);
        private void Panel_MouseMove(object sender, MouseEventArgs e) 
            => SelectorLayer.OnMouseMove(e);
        private void Panel_MouseUp(object sender, MouseEventArgs e)
            => SelectorLayer.OnMouseUp(e);

        /// <summary>
        /// レイヤーの表示を設定する。
        /// </summary>
        public void SetLayerVisible(TEnum key, bool visible)
        {
            if (Layers.TryGetValue(key, out var layer))
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
    }
}
