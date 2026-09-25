namespace PochiPochiEditorPlus._Managers._TilesetManager
{
    public sealed class BlockTileData
    {
        public int TileIndex { get; set; }
        public bool ReverseX { get; set; }
        public bool ReverseY { get; set; }
        public int PaletteIndex { get; set; }

        public BlockTileData(
            int tileIndex, 
            int paletteIndex,
            bool reverseX = false,
            bool reverseY = false)
        {
            TileIndex = tileIndex;
            PaletteIndex = paletteIndex;

            // 初期設定では反転なし
            ReverseX = reverseX;
            ReverseY = reverseY;
        }
    }

    public sealed class BlockLayer
    {
        public BlockTileData TopLeft { get; set; }
        public BlockTileData TopRight { get; set; }
        public BlockTileData BottomLeft { get; set; }
        public BlockTileData BottomRight { get; set; }

        public BlockLayer(
            BlockTileData topLeft,
            BlockTileData topRight,
            BlockTileData bottomLeft,
            BlockTileData bottomRight)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }
    }

    public sealed class BlockData
    {
        public int BlockIndex { get; set; }

        public BlockLayer Lower { get; set; }
        public BlockLayer Upper { get; set; }

        public BlockData(
            int blockIndex,
            BlockLayer lower,
            BlockLayer upper)
        {
            BlockIndex = blockIndex;
            Lower = lower;
            Upper = upper;
        }
    }
}
