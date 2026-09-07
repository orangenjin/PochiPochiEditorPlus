namespace PochiPochiEditorPlus._Utilities._QuickInput
{
    /// <summary>
    /// フォームを表示するための設定をまとめる。
    /// </summary>
    public sealed class QuickInputConfig
    {
        public int? DefaultOffset { get; set; }
        public string[] CmbItems { get; set; }
        public decimal? NudMin { get; set; }
        public decimal? NudMax { get; set; }
        public string FileFilter { get; set; }

        public bool HasOffset => DefaultOffset.HasValue;
        public bool HasCombo => CmbItems != null && CmbItems.Length > 0;
        public bool HasNumeric => NudMin.HasValue && NudMax.HasValue;
        public bool HasFile => !string.IsNullOrEmpty(FileFilter);
    }

    /// <summary>
    /// フォームが返す結果をまとめる。
    /// </summary>
    public sealed class QuickInputResult
    {
        public int Offset { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public string Path { get; set; }
    }
}
