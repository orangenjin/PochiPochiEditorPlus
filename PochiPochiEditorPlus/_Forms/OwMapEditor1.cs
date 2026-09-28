using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Managers._LayerManager;
using PochiPochiEditorPlus._Managers._TilesetManager;
using PochiPochiEditorPlus._Managers._UndoManager;
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
        private CommandManager _commandManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各エントリーテーブル用
        private dynamic _tileset1Manager = null;
        private dynamic _tileset2Manager = null;
        private List<BlockData> _blockDataList = null;
        // パネル描画用
        private dynamic _tileLayerHolder = null;
        private LayerScroller _tileLayerScroller = null;
        private dynamic _blockLayerHolder = null;
        private LayerScroller _blockLayerScroller = null;
        private dynamic _dataLayerHolder = null;
        // UI制御用
        private int _selectedBlockIndex = 0;
        private byte[] _combinedTilesetImageData = null;

        public enum MapEditorLayerNames1
        { 
            Tile,
            BlockLower,
            BlockUpper,
            BlockFlat,
            Data,
            Grid,
            Select,
            Paste
        }

        public OwMapEditor1(
            SharedData sharedData,
            CommandManager commandManager,
            FormGroupData groupData)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _commandManager = commandManager;
            _groupData = groupData;
            _eventBinder = new EventBinder();
            _tileset1Manager = new TilesetHeaderHolder(_sharedData);
            _tileset2Manager = new TilesetHeaderHolder(_sharedData);

            // グループデータを登録
            RegisterFormGroupData(_groupData);

            // タイル画像パネル
            _tileLayerHolder = new LayerHolder<MapEditorLayerNames1>(pnlTileView, _eventBinder);
            _tileLayerScroller = new LayerScroller(
                pnlTileView,
                null,
                vsbTileView,
                _tileLayerHolder.Data,
                _eventBinder);
            // ブロック画像パネル
            _blockLayerHolder = new LayerHolder<MapEditorLayerNames1>(pnlBlockView, _eventBinder);
            _blockLayerScroller = new LayerScroller(
                pnlBlockView,
                null,
                vsrBlockView,
                _blockLayerHolder.Data,
                _eventBinder);
            // データ画像パネル
            _dataLayerHolder = new LayerHolder<MapEditorLayerNames1>(pnlBlockDataImage, _eventBinder);

            InitializeControls();
            InitializeLayers();
            InitializeEventHandlers();

            RefreshUI();
        }

        private void RegisterFormGroupData(FormGroupData groupData)
        {
            groupData.Register(this, () => _blockLayerHolder);
        }

        private void InitializeControls()
        {
            // ダブルバッファリングを有効化
            CtrlHelper.EnableDoubleBuffering(
                pnlTileView,
                pnlBlockView, 
                pnlBlockDataImage);

            // 各コンボボックスにアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbTilePalette, "txt/tileset/TilesetPaletteIndex.txt"),
                (cmbBlockAttrAction, "txt/tileset/TilesetBlockAttrAction.txt"),
                (cmbBlockAttrType, "txt/tileset/TilesetBlockAttrType.txt"),
                (cmbBlockAttrUnk, "txt/tileset/TilesetBlockAttrUnk.txt"),
                (cmbBlockAttrLayer, "txt/tileset/TilesetBlockAttrLayer.txt"));

            ChangeBlockTabState(false);
        }

        private void InitializeLayers()
        {
            // タイルレイヤー
            _tileLayerHolder.AddLayer<SingleImageLayer>(MapEditorLayerNames1.Tile);
            _tileLayerHolder.AddLayer<GridLayer>(MapEditorLayerNames1.Grid);
            _tileLayerHolder.AddLayer<SelectLayer>(MapEditorLayerNames1.Select);

            // ブロックレイヤー
            _blockLayerHolder.AddLayer<BlockImageLayer>(MapEditorLayerNames1.BlockLower);
            _blockLayerHolder.AddLayer<BlockImageLayer>(MapEditorLayerNames1.BlockUpper);
            _blockLayerHolder.AddLayer<GridLayer>(MapEditorLayerNames1.Grid);
            _blockLayerHolder.AddLayer<SelectLayer>(MapEditorLayerNames1.Select);

            // データレイヤー
            _dataLayerHolder.AddLayer<BlockImageLayer>(MapEditorLayerNames1.BlockFlat);
            _dataLayerHolder.AddLayer<GridLayer>(MapEditorLayerNames1.Grid);
            _dataLayerHolder.AddLayer<SelectLayer>(MapEditorLayerNames1.Select);
            _dataLayerHolder.AddLayer<PasteLayer>(MapEditorLayerNames1.Paste);
        }

        private void InitializeEventHandlers()
        {
            // 枠描画
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpBlockSelector, pnlBlockView),
                () => CtrlHelper.DetachBorder(grpBlockSelector));
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpTileSelector, picBoardTile, pnlTileView),
                () => CtrlHelper.DetachBorder(grpTileSelector));
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpBlockDataAndAttr, pnlBlockDataImage, pnlTripleLayerImage),
                () => CtrlHelper.DetachBorder(grpBlockDataAndAttr));

            // パレット切り替え
            _eventBinder.BindCtrl(
                h => cmbTilePalette.SelectedIndexChanged += h,
                h => cmbTilePalette.SelectedIndexChanged -= h,
                (_, __) =>
                {
                    // 画像レイヤーを取得
                    var layer = _tileLayerHolder.Tile;
                    if (layer == null) return;

                    // パレットを更新
                    int palIndex = cmbTilePalette.SelectedIndex;
                    var palData = GetProperPaletteData(palIndex);
                    layer.ChangePalette(palData);
                    _tileLayerHolder.Panel.Invalidate();
                });

            // ブロックインデックス数値
            _eventBinder.BindCtrl(
                h => nudBlockIndex.ValueChanged += h,
                h => nudBlockIndex.ValueChanged -= h,
                (_, __) => BlockIndexChanged());
            // dunamic型にイベント登録できないのでキャスト
            var selectLayer = (SelectLayer)_blockLayerHolder.Select;
            _eventBinder.BindCustom(
                () => selectLayer.SelectChanged += UpdateSelectedBlockIndex,
                () => selectLayer.SelectChanged -= UpdateSelectedBlockIndex);

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
            CombineTilesetImage();
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

        private void CombineTilesetImage()
        {
            // タイルセット1とタイルセット2の画像を連結
            byte[] imgData1 = _tileset1Manager.ImageData ?? Array.Empty<byte>();
            byte[] imgData2 = _tileset2Manager.ImageData ?? Array.Empty<byte>();
            _combinedTilesetImageData = new byte[imgData1.Length + imgData2.Length];
            Array.Copy(imgData1, 0, _combinedTilesetImageData, 0, imgData1.Length);
            Array.Copy(imgData2, 0, _combinedTilesetImageData, imgData1.Length, imgData2.Length);
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
            int tileRows = (_combinedTilesetImageData.Length + bytesPerTileRow - 1) / bytesPerTileRow;
            int height = tileRows * Constants.TileSize;

            // 有効なタイル数を計算
            int totalTiles = _tileset1Manager.GetTotalTileCount() + _tileset2Manager.GetTotalTileCount();

            // レイヤー初期設定
            _tileLayerHolder.Initialize(
                gridSize: Constants.TileSize,
                validItemCount: totalTiles,
                scale: Constants.DefaultScale);

            // 画像の設定
            _tileLayerHolder.Tile.SetImageData(
                _combinedTilesetImageData,
                palData,
                width,
                height);

            // 選択範囲の設定
            var maxLength = Constants.TilePerBlockSide;
            _tileLayerHolder.Select.MaxSelectSize = new Size(maxLength, maxLength);

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
                blockData = TilesetBlockCalc.GetBlockData(currentIndex, _tileset1Manager.BlockDataEntries[i]);
                _blockDataList.Add(blockData);
            }
            // 続きからタイルセット2のブロックを追加
            for (int i = 0; i < _tileset2Manager.BlockDataEntries.Count; i++)
            {
                blockData = TilesetBlockCalc.GetBlockData(currentIndex, _tileset2Manager.BlockDataEntries[i]);
                _blockDataList.Add(blockData);
            }
        }

        private void UpdateBlockView()
        {
            if (_blockDataList == null || _blockDataList.Count == 0) return;
            int blockSize = Constants.TileSize * Constants.TilePerBlockSide;

            // ブロック数に基づいてnudの上限を設定
            if (_blockDataList.Count > 0)
            {
                nudBlockIndex.Maximum = _blockDataList.Count - 1;
                nudBlockIndex.Minimum = 0;
                _selectedBlockIndex = Math.Min(_selectedBlockIndex, _blockDataList.Count - 1);
                nudBlockIndex.Value = _selectedBlockIndex;
                txtBlockIndex.Text =
                    _selectedBlockIndex.ParseIntToString(txtBlockIndex.Digits);
            }

            // レイヤーの初期設定
            _blockLayerHolder.Initialize(
                gridSize: blockSize,
                validItemCount: _blockDataList.Count,
                scale: Constants.DefaultScale);

            // 画像を破棄
            _blockLayerHolder.BlockLower.Allocate();
            _blockLayerHolder.BlockUpper.Allocate();

            // 定数を事前に計算
            int tilesPerRow = Constants.TilesetImageWidth / Constants.TileSize;
            int columns = _blockLayerHolder.Data.Columns;

            for (int i = 0; i < _blockDataList.Count; i++)
            {
                var blockData = _blockDataList[i];

                // 1ブロック分の画像を用意
                using (Bitmap lowerBmp = new Bitmap(blockSize, blockSize))
                using (Bitmap upperBmp = new Bitmap(blockSize, blockSize))
                {
                    using (Graphics gLower = Graphics.FromImage(lowerBmp))
                    using (Graphics gUpper = Graphics.FromImage(upperBmp))
                    {
                        DrawBlockLayer(gLower, blockData.Lower, 0, 0, tilesPerRow, isLower: true);
                        DrawBlockLayer(gUpper, blockData.Upper, 0, 0, tilesPerRow, isLower: false);
                    }

                    // マス座標を計算
                    int gridX = i % columns;
                    int gridY = i / columns;

                    // 画像をセットする
                    _blockLayerHolder.BlockLower.SetBlockImage(gridX, gridY, lowerBmp);
                    _blockLayerHolder.BlockUpper.SetBlockImage(gridX, gridY, upperBmp);
                }
            }

            // 選択範囲の設定
            _blockLayerHolder.Select.MaxSelectSize = new Size(1, 1);

            // スクロールバーの設定
            _blockLayerScroller.UpdateScrollRange();

            // 初期選択
            BlockIndexChanged();
            UpdateSelectedBlockIndex();
        }

        private void DrawBlockLayer(
            Graphics gfx,
            BlockLayer layer,
            int drawX,
            int drawY,
            int tilesPerRow,
            bool isLower)
        {
            int tileSize = Constants.TileSize;

            foreach (var (tile, offsetX, offsetY) in TilesetBlockCalc.GetTilesWithOffset(layer))
            {
                DrawTile(tile, drawX + (offsetX * tileSize), drawY + (offsetY * tileSize));
            }

            void DrawTile(BlockTileData tileData, int x, int y)
            {
                using (Bitmap tileBmp = GetTileBitmap(tileData, tilesPerRow, isLower))
                {
                    if (tileBmp != null)
                    {
                        gfx.DrawImage(tileBmp, x, y);
                    }
                }
            }
        }

        private Bitmap GetTileBitmap(BlockTileData tileData, int tilesPerRow, bool isLower)
        {
            var tileLayer = _tileLayerHolder.Tile;
            if (tileLayer == null) return null;

            int tileGridX = tileData.TileIndex % tilesPerRow;
            int tileGridY = tileData.TileIndex / tilesPerRow;

            byte[] tileBytes = tileLayer.ExtractImageDataAtGrid(tileGridX, tileGridY);
            byte[] palData = GetProperPaletteData(tileData.PaletteIndex);

            return TilesetBlockCalc.CreateTileImage(
                tileData,
                tileBytes,
                palData,
                Constants.TileSize,
                isLower);
        }

        private void BlockIndexChanged()
        {
            // nudの数値とtxtの表示
            _selectedBlockIndex = (int)nudBlockIndex.Value;
            txtBlockIndex.Text = _selectedBlockIndex.ParseIntToString(txtBlockIndex.Digits);

            // 選択範囲の更新
            var selectedIndexList = _blockLayerHolder.Select.GetSelectedIndexList();
            if (selectedIndexList.Count > 0 && selectedIndexList[0] == _selectedBlockIndex) return;
            _blockLayerHolder.Select.SelectSingleItem(_selectedBlockIndex);
        }

        private void UpdateSelectedBlockIndex()
        {
            var indexList = _blockLayerHolder.Select.GetSelectedIndexList();
            if (indexList.Count == 0) return;
            nudBlockIndex.Value = (decimal)indexList[0];

            // データ画像の更新
            UpdateBlockDataImages();

            // 属性の更新
            LoadBlockAttrs();
        }

        private void UpdateBlockDataImages()
        {
            if (_blockDataList == null || _blockDataList.Count == 0) return;

            // 選択中のブロック
            var blockData = _blockDataList[_selectedBlockIndex];

            // レイヤーの初期設定
            int tileSize = Constants.TileSize;
            int scale = pnlBlockDataImage.Height / (tileSize * 2);
            int validItemCount = Constants.TilePerBlockSide * Constants.TilePerBlockSide * 2;
            _dataLayerHolder.Initialize(
                gridSize: tileSize,
                validItemCount: validItemCount,
                scale: scale);

            // 画像を破棄して配列を確保
            _dataLayerHolder.BlockFlat.Allocate();
            int tilesPerRow = Constants.TilesetImageWidth / Constants.TileSize;

            // 下位レイヤー
            foreach (var (tile, offsetX, offsetY) in TilesetBlockCalc.GetTilesWithOffset(blockData.Lower))
            {
                SetTileImageToDataLayer(tile, offsetX, offsetY, isLower: true);
            }

            // 上位レイヤー
            foreach (var (tile, offsetX, offsetY) in TilesetBlockCalc.GetTilesWithOffset(blockData.Upper))
            {
                SetTileImageToDataLayer(tile, 2 + offsetX, offsetY, isLower: false);
            }

            // 選択範囲の設定
            _dataLayerHolder.Select.MaxSelectSize = new Size(1, 1);

            // 再描画要求
            _dataLayerHolder.Panel.Invalidate();

            void SetTileImageToDataLayer(
                BlockTileData tileData,
                int gridX,
                int gridY,
                bool isLower)
            {
                using (Bitmap tileBmp = GetTileBitmap(tileData, tilesPerRow, isLower))
                {
                    if (tileBmp != null)
                    {
                        _dataLayerHolder.BlockFlat.SetBlockImage(gridX, gridY, tileBmp);
                    }
                }
            }
        }

        private void LoadBlockAttrs()
        {
            // インデックスの閾値
            int threshold = _tileset1Manager.BlockAttrEntries.Count;
            var entry = _selectedBlockIndex <= threshold
                ? _tileset1Manager.BlockAttrEntries[_selectedBlockIndex]
                : _tileset2Manager.BlockAttrEntries[_selectedBlockIndex - threshold];

            // 各属性を反映
            cmbBlockAttrAction.SelectedValue = entry.ActionAttr.GetData<byte>();
            cmbBlockAttrType.SelectedValue = entry.TypeAttr.GetData<byte>();
            cmbBlockAttrUnk.SelectedValue = entry.UnkAttr.GetData<byte>();
            var layerAndWildEncAttr = TilesetBlockCalc.GetLayerAndWildEncAttr(entry.LayerAndWildEncAttr);
            cmbBlockAttrLayer.SelectedValue = layerAndWildEncAttr.Layer;
            chkBlockAttrWildEncGrass.Checked = layerAndWildEncAttr.WildEncGrass;
            chkBlockAttrWildEncWater.Checked = layerAndWildEncAttr.WildEncWater;
        }

        private byte[] GetProperPaletteData(int palIndex)
        {
            if (palIndex < 0) return Array.Empty<byte>();
            return palIndex >= (int)TilesetHeaderHolder.PaletteKind.Palette7to12
                ? _tileset2Manager.PaletteData[palIndex]
                : _tileset1Manager.PaletteData[palIndex];
        }

        private void ChangeBlockTabState(bool state)
        {
            // ブロックタブページ
            CtrlHelper.SetControlsEnabled(
                tbpBlock,
                enabled: state,
                includeSelf: true);
            CtrlHelper.ResetControls(
                tbpBlock,
                includeSelf: false);
            // タイル画像のレイヤー表示切り替え
            _tileLayerHolder.SetAllLayersVisibility(state);
            _tileLayerHolder.Panel.Invalidate();
            // ブロック画像のレイヤー表示切り替え
            _blockLayerHolder.SetAllLayersVisibility(state);
            _blockLayerHolder.Panel.Invalidate();
            // データ画像のレイヤー表示切り替え
            _dataLayerHolder.SetAllLayersVisibility(state);
            _dataLayerHolder.Panel.Invalidate();
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
