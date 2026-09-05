using System;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;

namespace PochiPochiEditorPlus._CustomControls
{
    public class HexTextBox : TextBox
    {
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
                e.Handled = true;
            }
        }

        /// <summary>
        /// フォーカスが外れた時、桁数整形を行う。
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            // まず数値に変換
            int val = Text.ParseStringToInt();

            // 再度変換して、文字列を代入
            Text = val.ParseIntToString();
        }
    }
}
