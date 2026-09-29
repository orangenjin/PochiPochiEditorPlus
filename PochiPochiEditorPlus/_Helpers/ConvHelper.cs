using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
            // null, 空白である場合
            if (string.IsNullOrWhiteSpace(str))
            {
                return BinaryConstants.InvalidValue;
            }

            // 字詰め
            var trimStr = str.Replace(" ", string.Empty);

            // 変換テスト
            return int.TryParse(trimStr, NumberStyles.HexNumber, null, out int value)
                    ? value
                    : BinaryConstants.InvalidValue; // 変換失敗時
        }

        /// <summary>
        /// intから16進数stringへ変換する。
        /// </summary>
        public static string ParseIntToString(
            this int value,
            int digits = BinaryConstants.OffsetDigits)
        {
            return value != BinaryConstants.InvalidValue
                ? value.ToString($"X{digits}")
                : string.Empty;
        }

        /// <summary>
        /// Enumの数値をビット長として扱い、Enumの上からの順番で上位からビットを分割する。
        /// Enumは全範囲を規定している必要がある。
        /// </summary>
        public static Dictionary<TEnum, uint> BitExtract<TEnum>(uint value, params TEnum[] sequence) 
            where TEnum : Enum
        {
            var result = new Dictionary<TEnum, uint>(sequence.Length);
            int currentPos = GetTotalBits(sequence);

            foreach (var key in sequence)
            {
                // Enumの数値をビット長として取得
                int length = Convert.ToInt32(key);

                // 上位から取得するため減算
                currentPos -= length;

                // ビット長のマスクを作成
                uint mask = length == BinaryConstants.BitsPerByte * (int)DataSize.Byte
                    ? uint.MaxValue 
                    : (1U << length) - 1;

                // 対象部分をマスクして辞書に格納
                uint extractedValue = (value >> currentPos) & mask;
                result[key] = extractedValue;
            }

            return result;
        }

        /// <summary>
        /// 辞書の値とEnumの順番を元に、Enumの数値をビット長として扱い、上位からuint値に結合する。
        /// </summary>
        public static uint BitCombine<TEnum>(Dictionary<TEnum, uint> values, params TEnum[] sequence) 
            where TEnum : Enum
        {
            uint result = 0;
            int currentPos = GetTotalBits(sequence);

            foreach (var key in sequence)
            {
                int length = Convert.ToInt32(key);
                currentPos -= length;

                // ビット長のマスクを作成
                uint mask = length == BinaryConstants.BitsPerByte * (int)DataSize.Byte
                    ? uint.MaxValue
                    : (1U << length) - 1;

                // 値をマスクし、シフトして結合
                result |= (values[key] & mask) << currentPos;
            }

            return result;
        }

        /// <summary>
        /// 重複した数値の存在を考慮して、Enumの宣言順を取得する。
        /// </summary>
        private static TEnum[] GetEnumSequence<TEnum>() where TEnum : Enum
        {
            return typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (TEnum)f.GetValue(null))
                .ToArray();
        }

        /// <summary>
        /// sequence内の全ビット長の合計を取得する。
        /// </summary>
        private static int GetTotalBits<TEnum>(TEnum[] sequence) 
            where TEnum : Enum
        {
            int totalBits = 0;
            foreach (var key in sequence)
            {
                totalBits += Convert.ToInt32(key);
            }
            return totalBits;
        }
    }
}
