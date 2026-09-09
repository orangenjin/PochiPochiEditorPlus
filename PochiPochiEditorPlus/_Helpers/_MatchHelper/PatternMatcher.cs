using System;
using System.Collections.Generic;

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

            // Listの要素数に対して
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                // 占有するバイト数
                int length = token.GetLength();

                // 範囲内かどうかの判定
                if (currentPos + length > data.Length) return false;

                // 対象となるバイト配列を取得
                token.Value = new byte[length];
                Array.Copy(
                    data,
                    currentPos,
                    token.Value,
                    0,
                    length);

                // トークン判定、メソッドを抜ける
                if (!token.IsMatch()) return false;

                // nullポインタを許容せず、ポインタトークンの時
                if (!allowNullPointer && token.Def is PointerToken pToken)
                {
                    if (pToken.IsSus) return false;
                }

                // 次のトークンへ
                currentPos += length;
            }

            return true;
        }

        /// <summary>
        /// パターンマッチングが連続する個数を取得する。
        /// </summary>
        public static int TryCount(
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
        /// TokenDataの長さを計算する。
        /// </summary>
        private static int GetPatternLength(List<TokenData> tokens)
        {
            int length = 0;

            for (int i = 0; i < tokens.Count; i++)
            {
                length += tokens[i].GetLength();
            }

            return length;
        }
    }
}
