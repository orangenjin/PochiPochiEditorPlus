using System;
using System.ComponentModel;
using System.Windows.Forms;
using PochiPochiEditorPlus._Managers;

namespace PochiPochiEditorPlus._Utilities._CustomCtrls
{
    public sealed class StrTextBox : TextBox
    {
        private int _allowedLength = 1;
        private bool _needTerminator = true;

        /// <summary>
        /// 事前にCharmapManagerのインスタンスを代入する必要あり。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CharmapManager CharmapManager { get; set; }

        /// <summary>
        /// 入力可能な最大の長さ(バイト数)を設定する。
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [DefaultValue(1)]
        [Description("文字として入力可能な最大バイト数(AllowedLength)を指定します。")]
        public int AllowedLength
        {
            get => _allowedLength;
            set
            {
                // 不正な値が設定されないようガード
                if (value < 1) value = 1;

                _allowedLength = value;
                ValidateTextLength();
            }
        }

        /// <summary>
        /// 終端文字を含めるかどうかを設定する。
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [DefaultValue(true)]
        [Description("文字列の調整時に終端文字を含めるかどうかを指定します。")]
        public bool NeedTerminator
        {
            get => _needTerminator;
            set
            {
                _needTerminator = value;
                ValidateTextLength();
            }
        }

        /// <summary>
        /// テキストが変更された時に実行する。
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            ValidateTextLength();
        }

        /// <summary>
        /// 現在のテキスト長の検証と切り詰めを行う。
        /// </summary>
        private void ValidateTextLength()
        {
            // 文字列の検証と切り詰め
            Text = CharmapManager.TextLengthValidate(Text, AllowedLength, NeedTerminator);

            // カーソルを末尾へ移動
            SelectionStart = Text.Length;
            SelectionLength = 0;
        }
    }
}
