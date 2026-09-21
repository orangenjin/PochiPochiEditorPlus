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
        /// 終端文字とパディング文字があるかどうかを判定する。
        /// </summary>
        public static bool TryCheckTerminator(
            byte[] data,
            int entryLength,
            byte terminatorByte,
            byte paddingByte,
            int offset = 0)
        {
            // 探索開始位置を計算
            int endPos = offset + entryLength - 1;
            int currentPos = endPos;

            while (offset <= currentPos && data[currentPos] == paddingByte)
            {
                currentPos--;
            }

            // それがterminatorByteであれば成功
            return currentPos >= offset && data[currentPos] == terminatorByte;
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
    }
}
