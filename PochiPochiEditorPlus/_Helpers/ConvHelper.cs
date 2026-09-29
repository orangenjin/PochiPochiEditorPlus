using System;
using System.Collections.Generic;
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
        /// Enumの数値をビット長として扱い、指定された順番で左側からビットを分割する。
        /// Enumは全範囲を規定している必要がある。
        /// </summary>
        public static Dictionary<TEnum, uint> BitExtract<TEnum>(uint value, params TEnum[] sequence)
        {
            // ビット長に変換し、全体のビット長を求める
            var fields = CreateBitFields(sequence, out int totalBits);

            var result = new Dictionary<TEnum, uint>();
            int currentPos = totalBits;

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
        /// 辞書の値とEnumの順番を元に、Enumの数値をビット長として扱い、uint値に結合する。
        /// </summary>
        public static uint BitCombine<TEnum>(Dictionary<TEnum, uint> values, params TEnum[] sequence)
        {
            // ビット長に変換し、全体のビット長を求める
            var fields = CreateBitFields(sequence, out int totalBits);

            uint result = 0;
            int currentPos = totalBits;

            foreach (var field in fields)
            {
                int length = field.Length;

                // 上位から格納するため減算
                currentPos -= length;

                // ビット長のマスクを作成
                uint mask = length == BinaryConstants.BitsPerByte * (int)DataSize.Byte
                    ? uint.MaxValue
                    : (1U << length) - 1;

                // 値をマスクし、シフトして結合
                result |= (values[field.Key] & mask) << currentPos;
            }

            return result;
        }

        private static (TEnum Key, int Length)[] CreateBitFields<TEnum>(TEnum[] sequence, out int totalBits)
        {
            var fields = new (TEnum Key, int Length)[sequence.Length];

            // Enumの数値を合計して、全体のビット長を求める
            totalBits = 0;

            for (int i = 0; i < sequence.Length; i++)
            {
                // Enumの数値をビット長として扱う
                int length = Convert.ToInt32(sequence[i]);

                fields[i] = (sequence[i], length);
                totalBits += length;
            }

            return fields;
        }
    }
}
