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
    /// 特定の値を設定し、比較する。
    /// </summary>
    public sealed class ExactToken : IToken
    {
        public int ExactValue { get; }
        public int Length { get; }

        public ExactToken(int exactValue)
        {
            ExactValue = exactValue;
            Length = Constants.ByteSize;
        }

        public bool IsValid(byte[] bytes)
        {
            // 1バイト分を抽出
            var hexValue = bytes[0];

            return ExactValue == hexValue;
        }
    }

    public sealed class PointerToken : IToken
    {
        // 再帰的マッチングのため
        public int Offset { get; set; }
        public int Length { get; }

        public PointerToken()
        {
            Length = Constants.UIntSize;
        }

        public bool IsValid(byte[] bytes)
        {
            // ポインタとして読み取る
            var result = IoHelper.TryReadPtr(bytes, 0, out int offset);

            // 結果を格納
            Offset = offset;

            return result;
        }

        public bool IsSus => Offset == Constants.InvalidValue;
    }

    /// <summary>
    /// Range型、最小値と最大値を設定する。
    /// </summary>
    public sealed class RangeToken : IToken
    {
        public byte Min { get; }
        public byte Max { get; }
        public int Length { get; }

        public RangeToken(byte min, byte max, int length)
        {
            Min = min;
            Max = max;
            Length = length;
        }

        public bool IsValid(byte[] bytes)
        {
            // リトルエンディアンで読み取る
            var hexValue = (byte)IoHelper.ReadBytesAsInt(bytes, 0, Length);

            // 範囲内かチェック
            return Min <= hexValue && hexValue <= Max;
        }
    }

    /// <summary>
    /// 何でもtrueを返す。
    /// </summary>
    public sealed class WildcardToken : IToken
    {
        public int Length { get; }

        public WildcardToken(int length)
        {
            Length = length;
        }

        public bool IsValid(byte[] bytes) => true;
    }


    public interface IToken
    {
        int Length { get; }
        bool IsValid(byte[] bytes);
    }
}
