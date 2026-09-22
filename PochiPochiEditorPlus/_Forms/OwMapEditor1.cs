using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Helpers._MatchHelper;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Managers._CommandManager;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Utilities;
using PochiPochiEditorPlus._Utilities._QuickInput;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.OwMap, 1)]
    public partial class OwMapEditor1 : Form, IEditorRefresh
    {
        // 共有データ用
        private dynamic _sharedData = null;
        private dynamic _groupData = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;

        public OwMapEditor1(
            SharedData sharedData,
            UndoManager undoManager,
            FormGroupData groupData)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _undoManager = undoManager;
            _groupData = groupData;
            _eventBinder = new EventBinder();

            InitializeControls();
            InitializeEventHandlers();

            RefreshUI();
        }

        private void InitializeControls()
        {
            // 各コンボボックスにアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbPaletteType, "txt/tileset/TilesetPaletteype.txt"),
                (cmbSelectTilePalette, "txt/tileset/TilesetPaletteIndex.txt"),
                (cmbBlockAttrAction, "txt/tileset/TilesetBlockAttrAction.txt"),
                (cmbBlockAttrType, "txt/tileset/TilesetBlockAttrType.txt"),
                (cmbBlockAttrUnk, "txt/tileset/TilesetBlockAttrUnk.txt"),
                (cmbBlockAttrLayer, "txt/tileset/TilesetBlockAttrLayer.txt"));
        }

        private void InitializeEventHandlers()
        {
            // 枠描画
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpBlockSelector, pnlBlockView),
                () => CtrlHelper.DetachBorder(grpBlockSelector));
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpTileSelector, picSelectTile, pnlTileView),
                () => CtrlHelper.DetachBorder(grpTileSelector));
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpBlockDataAndAttr, picBlockDataImage),
                () => CtrlHelper.DetachBorder(grpBlockDataAndAttr));

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void LoadBlockTabPage()
        {
            if (_groupData._mapFooterEntry != null)
            {

            }
            else
            {

            }
        }

        private void LoadCollTabPage()
        {

        }

        private void LoadEventTabPage()
        {

        }



        private void test()
        {
            // var entry = _groupData._mapHeaderEntry[3][0];
            //textBox1.Text = entry.MapFooterOffset.GetData<int>().ToString("X8");
        }





        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshUI()
        {
            LoadBlockTabPage();
            LoadCollTabPage();
            LoadEventTabPage();
        }
    }
}
