using System;
using System.Collections.Generic;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers._MatchHelper
{
    public static class PatternMatcher
    {
        /// <summary>
        /// シンプルに1回のパターンマッチングを試みる。
        /// </summary>
        public static bool TryMatch(
            List<TokenData> tokens,
            byte[] data,
            int offset = 0,
            bool allowNullPointer = false)
        {
            // カーソル用
            int currentPos = offset;

            // Listの各要素に対して
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                // 占有するバイト数
                int length = token.GetLength();

                // 範囲内かどうかの判定
                if (currentPos + length > data.Length) return false;

                // トークン判定
                if (!token.IsMatch(data, currentPos)) return false;

                // nullポインタを許容せず、ポインタトークンの時
                if (!allowNullPointer && token.Def is PointerToken pToken)
                {
                    if (pToken.IsNullPointer) return false;
                }

                // 次のトークンへ
                currentPos += length;
            }

            // 判定成功時の場合
            currentPos = offset;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                int length = token.GetLength();

                token.Value = new byte[length];
                Array.Copy(
                    data,
                    currentPos,
                    token.Value,
                    0,
                    length);

                currentPos += length;
            }

            return true;
        }

        /// <summary>
        /// パターンマッチングが連続する個数を取得する。
        /// </summary>
        public static int TryCountByPattern(
            List<TokenData> tokens,
            byte[] data,
            int baseOffset = 0,
            bool allowNullPointer = false)
        {
            // 単一パターンの長さを取得
            int patternLength = GetPatternLength(tokens);

            int count = 0;
            int currentPos = baseOffset;

            while (currentPos + patternLength <= data.Length)
            {
                // falseが戻るまで続ける
                if (!TryMatch(tokens, data, currentPos, allowNullPointer)) break;

                count++;
                currentPos += patternLength;
            }

            return count;
        }

        /// <summary>
        /// 終端文字とパディング文字があるかどうかを判定する。
        /// </summary>
        public static bool TryCheck(
            byte[] data,
            int entryLength,
            byte terminatorByte = Constants.StrTerminatorByte,
            byte paddingByte = Constants.PaddingByte,
            int offset = 0)
        {
            // 探索開始位置を計算
            int endPos = offset + entryLength - 1;
            int currentPos = endPos;

            while (offset <= currentPos && data[currentPos] == paddingByte)
            {
                currentPos--;
            }

            // 現在の位置が有効且つ終端文字であるかを判定
            if (currentPos < offset || data[currentPos] != terminatorByte) return false;

            // 終端文字の位置が探索範囲の先頭である場合成功
            if (currentPos == offset) return true;

            // 終端文字の一つ前が特定の文字である場合失敗
            if (InvalidBytes.Contains(data[currentPos - 1])) return false;

            return true;
        }

        /// <summary>
        /// 終端文字とパディング文字で終わるエントリが連続する個数を取得する。
        /// </summary>
        public static int TryCheckByTerminator(
            byte[] data,
            int entryLength,
            int baseOffset = 0,
            byte terminatorByte = Constants.StrTerminatorByte,
            byte paddingByte = Constants.PaddingByte)
        {
            int count = 0;
            int currentPos = baseOffset;

            while (currentPos + entryLength <= data.Length)
            {
                // falseが戻るまで続ける
                if (!TryCheck(data, entryLength, terminatorByte, paddingByte, currentPos)) break;

                count++;
                currentPos += entryLength;
            }

            return count;
        }

        /// <summary>
        /// 特定のバイト配列が指定した個数分存在するかを検証する。
        /// </summary>
        public static bool TrySearch(
            byte[] data,
            byte[] pattern,
            int offset,
            int length,
            int expectedCount)
        {
            // 探索の終了位置を計算
            int endPos = offset + length;

            int count = 0;
            int currentPos = offset;

            // 探索開始
            while (currentPos + pattern.Length <= endPos)
            {
                bool isMatch = true;

                // 特定のバイト配列と一致するか検証
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (data[currentPos + i] != pattern[i])
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    // カウントを増加
                    count++;
                    // 範囲が被らないように位置を更新
                    currentPos += pattern.Length;
                }
                else
                {
                    currentPos++;
                }
            }

            // カウントが引数以上かを判定
            return count >= expectedCount;
        }

        /// <summary>
        /// TokenDataの長さを計算する。
        /// </summary>
        public static int GetPatternLength(List<TokenData> tokens)
        {
            int length = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                length += tokens[i].GetLength();
            }

            return length;
        }

        /// <summary>
        /// 無効なバイト文字を定義する。
        /// </summary>
        private static HashSet<byte> InvalidBytes = new HashSet<byte>()
        {
            Constants.StrTerminatorByte,
            Constants.PaddingByte,
            Constants.StrNewlineByte
        };
    }
}
