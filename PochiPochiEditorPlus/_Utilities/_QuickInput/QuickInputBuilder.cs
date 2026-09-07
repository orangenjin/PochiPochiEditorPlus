using System.Windows.Forms;

namespace PochiPochiEditorPlus._Utilities._QuickInput
{
    public sealed class QuickInputBuilder
    {
        private QuickInputConfig _config = new QuickInputConfig();

        public QuickInputBuilder WithOffset(int defaultOffset)
        {
            _config.DefaultOffset = defaultOffset;
            return this;
        }

        public QuickInputBuilder WithCombo(string[] items)
        {
            _config.CmbItems = items;
            return this;
        }

        public QuickInputBuilder WithNumeric(decimal min, decimal max)
        {
            _config.NudMin = min;
            _config.NudMax = max;
            return this;
        }

        public QuickInputBuilder WithFile(string filter)
        {
            _config.FileFilter = filter;
            return this;
        }

        /// <summary>
        ///  フォームを表示して結果を返す。
        /// </summary>
        public QuickInputResult ShowDialog()
        {
            using (var form = new QuickInputForm(_config))
            {
                return form.ShowDialog() == DialogResult.OK
                    ? form.Result
                    : null;
            }
        }
    }
}
