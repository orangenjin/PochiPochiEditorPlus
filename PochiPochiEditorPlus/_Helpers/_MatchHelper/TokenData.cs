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

        public static TokenData Exact(int exactValue)
        {
            var tokenDef = new ExactToken(exactValue);
            return new TokenData(TokenType.Exact, tokenDef);
        }

        public static TokenData Pointer()
        {
            var tokenDef = new PointerToken();
            return new TokenData(TokenType.Pointer, tokenDef);
        }

        // 1, 2, 4バイトしか想定していない
        public static TokenData Range(byte min, byte max, int length)
        {
            var tokenDef = new RangeToken(min, max, length);
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
        public bool IsMatch() => Def.IsValid(Value);

        /// <summary>
        /// トークンの長さを取得する。
        /// </summary>
        public int GetLength() => Def.Length;
    }
}
