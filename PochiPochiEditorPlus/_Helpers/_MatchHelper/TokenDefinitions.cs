using System.Collections.Generic;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers._MatchHelper
{
    public enum TokenType
    {
        Exact,
        Pointer,
        Range,
        Wildcard
    }

    /// <summary>
    /// トークンが持つべき情報を定義する。
    /// </summary>
    public interface IToken
    {
        int Length { get; }
        bool IsValid(byte[] data, int offset);
    }

    /// <summary>
    /// 複数指定可能な特定の値を設定し、比較する。
    /// </summary>
    public sealed class ExactToken : IToken
    {
        public int Length { get; }
        public bool IsSigned { get; }

        // 検索用
        private HashSet<long> _exactValues;

        public ExactToken(
            int length, 
            bool isSigned, 
            params long[] exactValues)
        {
            Length = length;
            IsSigned = isSigned;
            _exactValues = new HashSet<long>(exactValues);
        }

        public bool IsValid(byte[] data, int offset)
        {
            long value = IoHelper.ReadBytesAsLong(
                data,
                offset,
                Length,
                isLittleEndian: true,
                isSigned: IsSigned);

            // 値が含まれているか判定
            return _exactValues.Contains(value);
        }
    }

    /// <summary>
    /// ポインタとして読み取れるかどうかを判定する。
    /// </summary>
    public sealed class PointerToken : IToken
    {
        public int Offset { get; private set; }
        public int Length { get; }

        public PointerToken()
        {
            Length = Constants.UIntSize;
        }

        public bool IsValid(byte[] data, int offset)
        {
            // ポインタとして読み取る
            var result = IoHelper.TryReadPtr(
                data, 
                offset, 
                out int value);

            // 結果を格納
            Offset = value;
            return result;
        }

        /// <summary>
        /// nullポインタかどうかを判定する。
        /// </summary>
        public bool IsNullPointer 
            => Offset == Constants.InvalidValue;
    }

    /// <summary>
    /// 最小値と最大値を設定し、判定する。
    /// </summary>
    public sealed class RangeToken : IToken
    {
        public long Min { get; }
        public long Max { get; }
        public int Length { get; }
        public bool IsSigned { get; }

        public RangeToken(
            long min, 
            long max, 
            int length, 
            bool isSigned = false)
        {
            Min = min;
            Max = max;
            Length = length;
            IsSigned = isSigned;
        }

        public bool IsValid(byte[] data, int offset)
        {
            // リトルエンディアンで読み取る
            long value = IoHelper.ReadBytesAsLong(
                data,
                offset,
                Length,
                isLittleEndian: true,
                isSigned: IsSigned);

            // 範囲チェック
            return Min <= value && value <= Max;
        }
    }

    /// <summary>
    /// 何でもtrueを返す。長さは任意指定可能。
    /// </summary>
    public sealed class WildcardToken : IToken
    {
        public int Length { get; }

        public WildcardToken(int length)
        {
            Length = length;
        }

        // 常にtrueを返す
        public bool IsValid(byte[] data, int offset) => true;
    }
}
