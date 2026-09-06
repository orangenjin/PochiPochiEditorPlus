using System.Globalization;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers
{
    public static class ConvHelper
    {
        /// <summary>
        /// 16進数stringからintへ変換する。
        /// </summary>
        public static int ParseStringToInt(this string str)
        {
            // null, 空白である場合スキップ
            if (string.IsNullOrWhiteSpace(str)) return Constants.InvalidValue;

            // 字詰め
            var trimStr = str.Replace(Constants.SpaceChar.ToString(), string.Empty);

            // 変換テスト
            return int.TryParse(trimStr, NumberStyles.HexNumber, null, out int value)
                    ? value
                    : Constants.InvalidValue; // 変換失敗時
        }

        /// <summary>
        /// intから16進数stringへ変換する。
        /// </summary>
        public static string ParseIntToString(
            this int val,
            int digits = Constants.OffsetDigits)
        {
            return val != Constants.InvalidValue
                ? val.ToString($"X{digits}")
                : string.Empty;
        }
    }
}
