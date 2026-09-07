namespace PochiPochiEditorPlus._Utilities._QuickInput
{
    public sealed class InputField
    {
        public string Label { get; set; }
        public InputType Type { get; set; }

        // オプション設定
        public int DefaultValue { get; set; }
        public string[] Options { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string FileFilter { get; set; }

        public InputField(
            string label, 
            InputType type,

            int defaultValue = 0,
            string[] options = null,
            int maxValue = 0,
            int minValue = byte.MaxValue,
            string fileFilter = null)
        {
            Label = label;
            Type = type;

            DefaultValue = defaultValue;
            Options = options;
            MinValue = maxValue;
            MinValue = minValue;
            FileFilter = fileFilter;
        }
    }

    public enum InputType
    {
        Text,
        Offset,
        Choice,
        Number,
        File
    }
}
