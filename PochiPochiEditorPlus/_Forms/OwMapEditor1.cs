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
using PochiPochiEditorPlus._Managers._PanelManager;
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
        // 各エントリーテーブル用
        private dynamic _tileset1Manager = null;
        private dynamic _tileset2Manager = null;
        // UI制御用
        private PanelLayers<LayerNames> _panelLayers = null;

        private enum LayerNames
        {
            Base,
            Grid,
            Select
        }

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
            _tileset1Manager = new TilesetManager(_sharedData);
            _tileset2Manager = new TilesetManager(_sharedData);
            _panelLayers = new PanelLayers<LayerNames>(
                pnlTileView,
                _eventBinder);

            InitializeControls();
            InitializeEventHandlers();

            RefreshUI();
        }

        private void InitializeControls()
        {
            // 各コンボボックスにアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbPaletteType, "txt/tileset/TilesetPaletteype.txt"),
                (cmbTilePalette, "txt/tileset/TilesetPaletteIndex.txt"),
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
            // マップフッターが無効、またはタイルセット番号の計算に失敗した場合
            if (!TryGetTilesetNumbers(out int tileset1No, out int tileset2No))
            {
                ChangeBlockTabState(false);
                return;
            }

            // マップフッターが有効の場合
            ChangeBlockTabState(true);
            _tileset1Manager.ReadHeader(tileset1No, _sharedData);
            _tileset2Manager.ReadHeader(tileset2No, _sharedData);

            // タイルを描画
            cmbTilePalette.SelectedIndex = 0;
            UpdateTileView();

            // フッターとタイル番号の検証ヘルパー
            bool TryGetTilesetNumbers(out int no1, out int no2)
            {
                no1 = 0;
                no2 = 0;

                var footer = _groupData._mapFooterEntry;
                if (footer == null) return false;

                return _tileset1Manager.TryCalcTilesetNo(footer.Tileset1HeaderOffset.GetData<int>(), out no1) &&
                       _tileset2Manager.TryCalcTilesetNo(footer.Tileset2HeaderOffset.GetData<int>(), out no2);
            }
        }

        private void ChangeBlockTabState(bool value)
        {
            CtrlHelper.ResetControls(
                tbpBlock,
                includeSelf: false);

            CtrlHelper.SetControlsEnabled(
                tbpBlock,
                enabled: value,
                includeSelf: true);

            _panelLayers.SetImage(LayerNames.Base, null);
        }

        /// <summary>
        /// 画像とパレットからBitmapを生成して表示する
        /// </summary>
        private void UpdateTileView()
        {
            // 選択中のパレットを取得
            int palIndex = cmbTilePalette.SelectedIndex;
            if (palIndex < 0) return;
            byte[] palData = palIndex >= (int)TilesetManager.PaletteKind.Palette7to12
                ? _tileset2Manager.PaletteData[palIndex]
                : _tileset1Manager.PaletteData[palIndex];

            // タイルセット1とタイルセット2の画像を連結
            byte[] imgData1 = _tileset1Manager.ImageData ?? Array.Empty<byte>();
            byte[] imgData2 = _tileset2Manager.ImageData ?? Array.Empty<byte>();
            byte[] combinedImageData = new byte[imgData1.Length + imgData2.Length];
            Array.Copy(imgData1, 0, combinedImageData, 0, imgData1.Length);
            Array.Copy(imgData2, 0, combinedImageData, imgData1.Length, imgData2.Length);

            // 各値の計算
            int width = Constants.TilesetImageWidth;
            int bytesPerTileRow = (width * Constants.TileSize) / Constants.PixelsPerByte;
            int tileRows = (combinedImageData.Length + bytesPerTileRow - 1) / bytesPerTileRow;
            int height = tileRows * Constants.TileSize;

            try
            {
                Bitmap image = ImageHelper.CreateBitmap(
                    combinedImageData,
                    palData,
                    width,
                    height,
                    showBackColor: true);
                _panelLayers.SetImage(LayerNames.Base, image);
            }
            catch
            {
                _panelLayers.SetImage(LayerNames.Base, null);
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
