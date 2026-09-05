using System;
using System.Globalization;
using System.Windows.Forms;

namespace PochiPochiEditorPlus.CustomControls
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

            // 空白をスキップ
            if (string.IsNullOrWhiteSpace(Text)) return;

            // 16進数に変換できるかチェック
            if (int.TryParse(Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int value))
            {
                Text = value.ToString("X8");
            }
            else
            {
                Text = string.Empty;
            }
        }
    }
}
