using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._TilesetManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._TilesetManager
{
    public sealed class TilesetBlockData
    {
        public int TileIndex { get; set; }
        public bool ReverseX { get; set; }
        public bool ReverseY { get; set; }
        public int PaletteIndex { get; set; }

        public TilesetBlockData(
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
}
