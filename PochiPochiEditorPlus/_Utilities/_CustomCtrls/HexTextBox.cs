using System;
using System.ComponentModel;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;

namespace PochiPochiEditorPlus._Utilities._CustomCtrls
{
    public sealed class HexTextBox : TextBox
    {
        private int _digits = Constants.OffsetDigits;

        /// <summary>
        /// 整形後の16進数の桁数を設定する。
        /// </summary>
        [Browsable(true)]
        [Category("表示")]
        [DefaultValue(Constants.OffsetDigits)]
        [Description("フォーカス離脱時に整形する桁数を指定します。")]
        public int Digits
        {
            get => _digits;
            set
            {
                // 不正な値が設定されないようガード
                if (value < 1) value = 1;
                _digits = value;
            }
        }

        /// <summary>
        /// 16進数文字のキー入力の判定を行う。
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // 制御キーを許可
            if (char.IsControl(e.KeyChar)) return;

            // 入力文字判定
            if (!Uri.IsHexDigit(e.KeyChar))
            {
                e.Handled = true; // 入力をキャンセル
            }
        }

        /// <summary>
        /// フォーカスが外れた時、桁数整形を行う。
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            // 一度数値に変換
            int val = Text.ParseStringToInt();

            // 再度変換して、文字列を代入
            Text = val.ParseIntToString(Digits);
        }
    }
}
