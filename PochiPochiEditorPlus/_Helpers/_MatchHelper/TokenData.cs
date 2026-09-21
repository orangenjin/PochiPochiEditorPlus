namespace PochiPochiEditorPlus._Helpers._MatchHelper
{
    public sealed class TokenData
    {
        public TokenType Type { get; }
        public IToken Def { get; }
        public byte[] Value { get; set; } // 後入れ

        private TokenData(
            TokenType type,
            IToken def)
        {
            Type = type;
            Def = def;
        }

        // 1, 2, 4バイトしか想定していない
        public static TokenData Exact(
            int length, 
            bool isSigned = false, 
            params long[] exactValues)
        {
            var tokenDef = 
                new ExactToken(length, isSigned, exactValues);
            return new TokenData(TokenType.Exact, tokenDef);
        }

        public static TokenData Pointer()
        {
            var tokenDef = new PointerToken();
            return new TokenData(TokenType.Pointer, tokenDef);
        }

        // 1, 2, 4バイトしか想定していない
        public static TokenData Range(
            long min,
            long max,
            int length,
            bool isSigned = false)
        {
            var tokenDef =
                new RangeToken(min, max, length, isSigned);
            return new TokenData(TokenType.Range, tokenDef);
        }

        public static TokenData Wildcard(int length)
        {
            var tokenDef = new WildcardToken(length);
            return new TokenData(TokenType.Wildcard, tokenDef);
        }

        /// <summary>
        /// トークンの設定値に合致するかどうかを判定する。
        /// </summary>
        public bool IsMatch(byte[] data, int offset) 
            => Def.IsValid(data, offset);

        /// <summary>
        /// トークンの長さを取得する。
        /// </summary>
        public int GetLength() => Def.Length;
    }
}
