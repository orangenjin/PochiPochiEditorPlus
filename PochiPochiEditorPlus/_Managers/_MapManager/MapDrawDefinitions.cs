namespace PochiPochiEditorPlus._Managers._MapManager
{
    public sealed class MapGridData
    {
        public int CollIndex { get; set; }
        public int BlockIndex { get; set; }

        public MapGridData(int collIndex, int blockIndex)
        {
            CollIndex = collIndex;
            BlockIndex = blockIndex;
        }
    }
}
