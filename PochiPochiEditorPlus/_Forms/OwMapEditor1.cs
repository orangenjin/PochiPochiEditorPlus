using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Managers._LayerManager;
using PochiPochiEditorPlus._Managers._TilesetManager;
using PochiPochiEditorPlus._Utilities;

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
        private List<BlockData> _blockDataList = null;
        // パネル描画用
        private LayerHolder<LayerNames> _tileLayerHolder = null;
        private LayerScroller _tileLayerScroller = null;
        private LayerHolder<LayerNames> _blockLayerHolder = null;
        private LayerScroller _blockLayerScroller = null;
        private enum LayerNames { Tile, Block }
        // UI制御用
        private byte[] _combinedImageData = null;

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
            _tileset1Manager = new TilesetHeaderHolder(_sharedData);
            _tileset2Manager = new TilesetHeaderHolder(_sharedData);

            // タイル画像パネル
            _tileLayerHolder = new LayerHolder<LayerNames>(pnlTileView, _eventBinder);
            _tileLayerScroller = new LayerScroller(
                pnlTileView,
                null,
                vsbTileView,
                _tileLayerHolder.Data,
                _eventBinder);
            // ブロック画像パネル
            _blockLayerHolder = new LayerHolder<LayerNames>(pnlBlockView, _eventBinder);
            _blockLayerScroller = new LayerScroller(
                pnlBlockView,
                null,
                vsrBlockView,
                _blockLayerHolder.Data,
                _eventBinder);

            InitializeControls();
            InitializeEventHandlers();

            RefreshUI();
        }

        private void InitializeControls()
        {
            // タイル画像パネルのダブルバッファリングを有効化
            typeof(Control).GetProperty(
                nameof(DoubleBuffered),
                System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(pnlTileView, true, null);

            // ブロック画像パネルのダブルバッファリングを有効化
            typeof(Control).GetProperty(
                nameof(DoubleBuffered),
                System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(pnlBlockView, true, null);

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

            // パレット切り替え
            _eventBinder.BindCtrl(
                h => cmbTilePalette.SelectedIndexChanged += h,
                h => cmbTilePalette.SelectedIndexChanged -= h,
                (_, __) =>
                {
                    // 画像レイヤーを取得
                    var layer = _tileLayerHolder.GetImageLayer(LayerNames.Tile);
                    if (layer == null) return;

                    // パレットを更新
                    int palIndex = cmbTilePalette.SelectedIndex;
                    var palData = GetProperPaletteData(palIndex);
                    layer.ApplyPalette(palData);
                    pnlTileView.Invalidate();
                });

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

            // まず画像を連結させる
            CombineTilesetImage();

            // タイルを描画
            cmbTilePalette.SelectedIndex = 0;
            UpdateTileView();

            // ブロックを描画
            SetBlockData();
            UpdateBlockView();

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

        /// <summary>
        /// 画像とパレットからBitmapを生成して表示する
        /// </summary>
        private void UpdateTileView()
        {
            // 選択中のパレットを取得
            int palIndex = cmbTilePalette.SelectedIndex;
            var palData = GetProperPaletteData(palIndex);

            // 幅と高さの計算
            int width = Constants.TilesetImageWidth;
            int bytesPerTileRow = (width * Constants.TileSize) / Constants.PixelsPerByte;
            int tileRows = (_combinedImageData.Length + bytesPerTileRow - 1) / bytesPerTileRow;
            int height = tileRows * Constants.TileSize;

            // 有効なタイル数を計算
            int totalTiles = _tileset1Manager.GetTotalTileCount() + _tileset2Manager.GetTotalTileCount();

            // レイヤー初期設定
            _tileLayerHolder.Initialize(
                gridSize: Constants.TileSize,
                scale: Constants.DefaultScale,
                validItemCount: totalTiles);

            // 画像の設定
            _tileLayerHolder.SetImageLayer(
                LayerNames.Tile,
                _combinedImageData,
                palData,
                width,
                height);
            _tileLayerHolder.SetLayerVisible(LayerNames.Tile, true);

            // グリッドの設定
            _tileLayerHolder.SetGridVisible(true);

            // 選択範囲の設定
            var maxLength = Constants.DefaultScale;
            _tileLayerHolder.SelectorLayer.MaxSelectSize = new Size(maxLength, maxLength);
            _tileLayerHolder.SetSelectorVisible(true);

            // スクロールバーの設定
            _tileLayerScroller.UpdateScrollRange();
        }

        private void SetBlockData()
        {
            _blockDataList = new List<BlockData>();
            BlockData blockData;
            int currentIndex = _blockDataList.Count;

            // タイルセット1のブロックを追加
            for (int i = 0; i < _tileset1Manager.BlockDataEntries.Count; i++)
            {
                blockData = TilesetDataCalc.GetBlockData(currentIndex, _tileset1Manager.BlockDataEntries[i]);
                _blockDataList.Add(blockData);
            }
            // 続きからタイルセット2のブロックを追加
            for (int i = 0; i < _tileset2Manager.BlockDataEntries.Count; i++)
            {
                blockData = TilesetDataCalc.GetBlockData(currentIndex, _tileset2Manager.BlockDataEntries[i]);
                _blockDataList.Add(blockData);
            }
        }

        private void UpdateBlockView()
        {

        }

        private void ChangeBlockTabState(bool state)
        {
            // ブロックタブページ
            CtrlHelper.ResetControls(
                tbpBlock,
                includeSelf: false);
            CtrlHelper.SetControlsEnabled(
                tbpBlock,
                enabled: state,
                includeSelf: true,
                excludeNames: new string[] { nameof(cmbPaletteType) });

            // タイル画像パネル
            if (!state)
            {
                _tileLayerHolder.SetLayerVisible(LayerNames.Tile, false);
                _tileLayerHolder.SetGridVisible(false);
                _tileLayerHolder.SelectorLayer.ClearSelect();
                _tileLayerHolder.SetSelectorVisible(false);
            }
        }

        private void CombineTilesetImage()
        {
            // タイルセット1とタイルセット2の画像を連結
            byte[] imgData1 = _tileset1Manager.ImageData ?? Array.Empty<byte>();
            byte[] imgData2 = _tileset2Manager.ImageData ?? Array.Empty<byte>();
            _combinedImageData = new byte[imgData1.Length + imgData2.Length];
            Array.Copy(imgData1, 0, _combinedImageData, 0, imgData1.Length);
            Array.Copy(imgData2, 0, _combinedImageData, imgData1.Length, imgData2.Length);
        }

        private byte[] GetProperPaletteData(int palIndex)
        {
            if (palIndex < 0) return Array.Empty<byte>();
            return palIndex >= (int)TilesetHeaderHolder.PaletteKind.Palette7to12
                ? _tileset2Manager.PaletteData[palIndex]
                : _tileset1Manager.PaletteData[palIndex];
        }

        private void LoadCollTabPage()
        {

        }

        private void LoadEventTabPage()
        {

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
