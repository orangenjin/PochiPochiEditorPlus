using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers
{
    public static class CtrlHelper
    {
        // AttachExternalBorderの対象コントロールを保持
        private static Dictionary<Control, List<Control>> _drawBorders = 
            new Dictionary<Control, List<Control>>();

        // AttachBtnsToNudのコントロール対応を保持
        private static List<NudNavigator> _nudNavigators = new List<NudNavigator>();
        private class NudNavigator
        {
            public NumericUpDown Nud { get; }
            public Button Prev { get; }
            public Button Next { get; }

            public NudNavigator(
                NumericUpDown nud,
                Button prev,
                Button next)
            {
                Nud = nud;
                Prev = prev;
                Next = next;
            }
        }

        // AttachRbToCtrlの対応を保持
        private static List<RbLink> _rbLinks = new List<RbLink>();
        private class RbLink
        {
            public RadioButton Rb { get; }
            public Control Ctrl { get; }

            public RbLink(
                RadioButton rb,
                Control ctrl)
            {
                Rb = rb;
                Ctrl = ctrl;
            }
        }

        /// <summary>
        /// コンテナを指定して、再帰的にコントロールを有効化/無効化する。
        /// </summary>
        public static void SetControlsEnabled(
            Control container,
            bool enabled,
            bool includeSelf = true,
            IEnumerable<string> excludeNames = null,
            IEnumerable<Type> excludeTypes = null)
        {
            foreach (var ctrl in GetTargetControls(container, includeSelf, excludeNames, excludeTypes))
            {
                ctrl.Enabled = enabled;
            }
        }

        /// <summary>
        /// コンテナを指定して、再帰的にコントロールを初期化する。
        /// </summary>
        public static void ResetControls(
            Control container,
            bool includeSelf = true,
            IEnumerable<string> excludeNames = null,
            IEnumerable<Type> excludeTypes = null)
        {
            foreach (var ctrl in GetTargetControls(container, includeSelf, excludeNames, excludeTypes))
            {
                // 種類が少ないから、直接記述している。
                switch (ctrl)
                {
                    case TextBox textBox:
                        textBox.Text = string.Empty;
                        break;
                    case NumericUpDown nud:
                        nud.Value = Math.Max(nud.Minimum, 0);
                        break;
                    case ComboBox comboBox:
                        comboBox.SelectedIndex = Constants.InvalidValue;
                        break;
                    case CheckBox checkBox:
                        checkBox.Checked = false;
                        break;
                    case RadioButton radioButton:
                        radioButton.Checked = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 条件に合致するコントロールを返す。
        /// </summary>
        private static List<Control> GetTargetControls(
            Control container,
            bool includeSelf,
            IEnumerable<string> excludeNames,
            IEnumerable<Type> excludeTypes)
        {
            var results = new List<Control>();

            // 例外設定
            var nameSet = excludeNames?.ToList();
            var typeSet = excludeTypes?.ToList();

            // 自身を含むかどうか
            if (includeSelf && !IsExcluded(container))
            {
                results.Add(container);
            }

            // 探索して終了
            SearchCtrl(container);
            return results;

            // 除外対象かどうかを判定するヘルパー
            bool IsExcluded(Control ctrl) =>
                (nameSet?.Contains(ctrl.Name) ?? false) ||
                (typeSet?.Contains(ctrl.GetType()) ?? false);

            // 再帰的に探すためにメソッド化
            void SearchCtrl(Control parent)
            {
                // コンテナであるか判定
                if (!ShouldRecurse(parent)) return;

                foreach (Control child in parent.Controls)
                {
                    if (!IsExcluded(child))
                    {
                        results.Add(child);
                    }

                    SearchCtrl(child);
                }
            }
        }

        /// <summary>
        /// 変な挙動をしないように、一応再帰すべきコンテナを指定する。
        /// </summary>
        public static bool ShouldRecurse(Control ctrl)
        {
            return ctrl is Form ||
                   ctrl is Panel ||
                   ctrl is GroupBox ||
                   ctrl is TabControl ||
                   ctrl is TabPage;
        }

        /// <summary>
        /// コントロールの外側に枠を描画する。
        /// </summary>
        public static void AttachBorder(Control parent, params Control[] targets)
        {
            // 対象コントロールを追加
            var targetCtrl = new List<Control>();
            foreach (var target in targets)
            {
                targetCtrl.Add(target);
            }
            _drawBorders[parent] = targetCtrl;

            parent.Paint += BorderPaint;
            parent.Invalidate();
        }

        /// <summary>
        /// 親コントロールに描画されたすべての枠を削除する。
        /// </summary>
        public static void DetachBorder(Control parent)
        {
            _drawBorders.Remove(parent);
            parent.Paint -= BorderPaint;
            parent.Invalidate();
        }

        /// <summary>
        /// 親コントロールの描画が頻発するなら要修正。
        /// </summary>
        private static void BorderPaint(object sender, PaintEventArgs e)
        {
            if (sender is Control parent && _drawBorders.TryGetValue(parent, out var targets))
            {
                using (var pen = new Pen(Color.Gray, 1))
                {
                    foreach (var target in targets)
                    {
                        var rect = new Rectangle(
                            target.Left - 1,
                            target.Top - 1,
                            target.Width + 1,
                            target.Height + 1);

                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
            }
        }

        /// <summary>
        /// nudの増減に対応するbtnを追加する。
        /// </summary>
        public static void AttachBtnsToNud(
            NumericUpDown nud,
            Button btnPrev,
            Button btnNext)
        {
            // 対応を保持する
            var navigator = new NudNavigator(nud, btnPrev, btnNext);
            _nudNavigators.Add(navigator);

            btnPrev.Click += BtnDecrease;
            btnNext.Click += BtnIncrease;
            nud.ValueChanged += UpdateBtnsToNud;

            // 念のため一度実行しておく
            UpdateBtnsToNud(nud, EventArgs.Empty);
        }

        /// <summary>
        /// nudの増減に対応するbtnの関連付けを解除する。
        /// </summary>
        public static void DetachBtnsToNud(
            NumericUpDown nud,
            Button btnPrev,
            Button btnNext)
        {
            btnPrev.Click -= BtnDecrease;
            btnNext.Click -= BtnIncrease;
            nud.ValueChanged -= UpdateBtnsToNud;

            // 辞書から削除しておく
            var navigator = _nudNavigators.First(x => x.Nud == nud);
            _nudNavigators.Remove(navigator);
        }

        private static void BtnDecrease(object sender, EventArgs e)
        {
            if (!(sender is Button btn)) return;

            // senderに対応するnudを取得する
            var navigator = _nudNavigators.First(x => x.Prev == btn);
            var nud = navigator.Nud;

            if (nud.Value > nud.Minimum)
            {
                nud.Value--;
            }
        }

        private static void BtnIncrease(object sender, EventArgs e)
        {
            if (!(sender is Button btn)) return;

            // senderに対応するnudを取得する
            var navigator = _nudNavigators.First(x => x.Next == btn);
            var nud = navigator.Nud;

            if (nud.Value < nud.Maximum)
            {
                nud.Value++;
            }
        }

        private static void UpdateBtnsToNud(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown nud)) return;

            var navigator = _nudNavigators.First(x => x.Nud == nud);

            // どこかに飛んでしまうフォーカスを制御する
            bool canGoPrev = nud.Value > nud.Minimum;
            if (!canGoPrev && navigator.Prev.Focused)
            {
                nud.Focus();
            }
            navigator.Prev.Enabled = canGoPrev;

            bool canGoNext = nud.Value < nud.Maximum;
            if (!canGoNext && navigator.Next.Focused)
            {
                nud.Focus();
            }
            navigator.Next.Enabled = canGoNext;
        }

        /// <summary>
        /// rbとctrlを連動させる。
        /// </summary>
        public static void AttachRbToCtrl(RadioButton rb, Control ctrl)
        {
            // 対応を保持
            var navigator = new RbLink(rb, ctrl);
            _rbLinks.Add(navigator);

            ctrl.Enter += CtrlEnter;
            rb.CheckedChanged += RbChecked;
        }

        /// <summary>
        /// rbとctrlの連動を解除する。
        /// </summary>
        public static void DetachRbToCtrl(RadioButton rb, Control ctrl)
        {
            ctrl.Enter -= CtrlEnter;
            rb.CheckedChanged -= RbChecked;

            var navigator = _rbLinks.First(x => x.Rb == rb && x.Ctrl == ctrl);
            _rbLinks.Remove(navigator);
        }

        private static void CtrlEnter(object sender, EventArgs e)
        {
            if (!(sender is Control ctrl)) return;

            var navigator = _rbLinks.First(x => x.Ctrl == ctrl);
            navigator.Rb.Checked = true;
        }

        private static void RbChecked(object sender, EventArgs e)
        {
            if (!(sender is RadioButton rb)) return;

            var navigator = _rbLinks.First(x => x.Rb == rb);
            // checkがtrueの場合を想定
            if (rb.Checked)
            {
                navigator.Ctrl.Focus();
            }
        }

        /// <summary>
        /// cmbにアイテムを追加する。初期選択指定可。
        /// </summary>
        public static void SetupCmbItems(
            ComboBox cmb,
            int defaultIndex = 0,
            params string[] items)
        {
            cmb.BeginUpdate();
            try
            {
                cmb.Items.Clear();
                cmb.Items.AddRange(items);
                cmb.SelectedIndex = defaultIndex;
            }
            finally
            {
                cmb.EndUpdate();
            }
        }

        /// <summary>
        /// 外部ファイルからcmbに格納する。各行の書式：[XX]ItemName（1バイト対応のみ）
        /// </summary>
        public static void LoadComboBoxFromFile(params (ComboBox cmb, string path)[] targets)
        {
            foreach (var target in targets)
            {
                var entries = new List<KeyValuePair<byte, string>>();

                // ファイルを読み込む
                foreach (string line in File.ReadLines(target.path))
                {
                    // 空行とコメント行をスキップ
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) continue;

                    // 行解析、 "["を除外
                    int closeBracketIndex = line.IndexOf(']');
                    var hex = line.Substring(1, closeBracketIndex - 1).ParseStringToInt();

                    var entry = new KeyValuePair<byte, string>((byte)hex, line);
                    entries.Add(entry);
                }

                // データをバインド
                target.cmb.DisplayMember = nameof(KeyValuePair<byte, string>.Value);
                target.cmb.ValueMember = nameof(KeyValuePair<byte, string>.Key);
                target.cmb.DataSource = entries;
            }
        }

        /// <summary>
        /// テキストボックスのカーソルを末尾に移動する。
        /// </summary>
        public static void MoveCursorToEnd(this TextBox textBox)
        {
            // 念のため
            textBox.Focus();

            // カーソルを末尾へ移動
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;

            // Multilineの場合
            textBox.ScrollToCaret();
        }
    }
}
