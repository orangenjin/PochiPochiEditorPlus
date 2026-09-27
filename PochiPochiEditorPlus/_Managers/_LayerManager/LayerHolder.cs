using System;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._LayerManager
{
    public sealed class LayerHolder<TEnum> : DynamicAccessor<LayerBase> where TEnum : Enum
    {
        // 対象のパネルコントロール
        public Panel Panel { get; }
        // レイヤーの基礎情報を各レイヤーに注入する
        public LayerData Data { get; }

        public LayerHolder(Panel panel, EventBinder eventBinder)
        {
            Data = new LayerData();
            Panel = panel;

            eventBinder.BindCustom(
                () => {
                    Panel.Paint += Panel_Paint;
                    Panel.MouseDown += Panel_MouseDown;
                    Panel.MouseMove += Panel_MouseMove;
                    Panel.MouseUp += Panel_MouseUp;
                },
                () => {
                    Panel.Paint -= Panel_Paint;
                    Panel.MouseDown -= Panel_MouseDown;
                    Panel.MouseMove -= Panel_MouseMove;
                    Panel.MouseUp -= Panel_MouseUp;
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
            Data.CalcLayout(Panel.ClientSize.Width);
        }

        /// <summary>
        /// LayerBaseを継承したレイヤーを登録する。
        /// </summary>
        public void AddLayer<TLayer>(TEnum key) where TLayer : LayerBase, new()
        {
            var layer = new TLayer();
            layer.Data = Data;
            layer.RequestInvalidate = () => Panel.Invalidate();
            Register(key.ToString(), layer);
        }

        /// <summary>
        /// すべてのレイヤーの表示/非表示を変更する。
        /// </summary>
        public void SetAllLayersVisibility(bool visible)
        {
            foreach (var layer in _values.Values)
            {
                layer.Visible = visible;
            }
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
            foreach (var key in Enum.GetValues(typeof(TEnum)))
            {
                string keyStr = key.ToString();
                if (_values.TryGetValue(keyStr, out var layer) && layer.Visible)
                {
                    layer.Draw(e.Graphics);
                }
            }

            e.Graphics.ResetTransform();
        }

        // マウスイベントを各レイヤーに渡す
        private void Panel_MouseDown(object sender, MouseEventArgs e)
            => _values.Values.Where(l => l.Visible).ToList().ForEach(l => l.OnMouseDown(e));
        private void Panel_MouseMove(object sender, MouseEventArgs e)
            => _values.Values.Where(l => l.Visible).ToList().ForEach(l => l.OnMouseMove(e));
        private void Panel_MouseUp(object sender, MouseEventArgs e)
            => _values.Values.Where(l => l.Visible).ToList().ForEach(l => l.OnMouseUp(e));
    }
}
