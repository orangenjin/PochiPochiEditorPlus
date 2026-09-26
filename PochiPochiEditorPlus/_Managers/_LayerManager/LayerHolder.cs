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
        // レイヤーの基礎情報を各レイヤーに注入する
        public LayerData Data { get; }
        // 各レイヤーを格納する
        public Dictionary<TEnum, LayerBase> Layers { get; }

        // 対象のパネルコントロール
        private Panel _panel;

        public LayerHolder(Panel panel, EventBinder eventBinder)
        {
            Data = new LayerData();
            Layers = new Dictionary<TEnum, LayerBase>();
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
        /// LayerBaseを継承したレイヤーを登録する。
        /// </summary>
        public void AddLayer(TEnum key, LayerBase layer)
        {
            if (Layers.TryGetValue(key, out var oldLayer))
            {
                oldLayer?.Dispose();
            }
            Layers[key] = layer;
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

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            // 描画方法の設定
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            // スクロール位置を適用
            var offset = Data.ScrollOffset;
            e.Graphics.TranslateTransform(-offset.X, -offset.Y);

            // 各レイヤーをEnum順に描画
            var sortedKeys = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().OrderBy(k => k);
            foreach (var key in sortedKeys)
            {
                if (Layers.TryGetValue(key, out var layer) && layer.Visible)
                {
                    layer.Draw(e.Graphics);
                }
            }

            e.Graphics.ResetTransform();
        }

        // マウスイベントを各レイヤーに渡す
        private void Panel_MouseDown(object sender, MouseEventArgs e)
            => Layers.Values.Where(l => l.Visible).ToList().ForEach(l => l.OnMouseDown(e));
        private void Panel_MouseMove(object sender, MouseEventArgs e)
            => Layers.Values.Where(l => l.Visible).ToList().ForEach(l => l.OnMouseMove(e));
        private void Panel_MouseUp(object sender, MouseEventArgs e)
            => Layers.Values.Where(l => l.Visible).ToList().ForEach(l => l.OnMouseUp(e));
    }
}
