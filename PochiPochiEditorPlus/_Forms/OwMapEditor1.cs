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
        private LayerHolder<LayerNames> _tileLayerHolder = null;
        private LayerScroller _tileLayerScroller = null;
        private LayerHolder<LayerNames> _blockLayerHolder = null;
        private LayerScroller _blockLayerScroller = null;
        private enum LayerNames { Tile, BlockLower, BlockUpper }
        // UI制御用
        private int _selectedBlockIndex = 0;
        private byte[] _combinedTilesetImageData = null;

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
                    var layer = _tileLayerHolder.GetLayer<ImageLayer>(LayerNames.Tile);
                    if (layer == null) return;

                    // パレットを更新
                    int palIndex = cmbTilePalette.SelectedIndex;
                    var palData = GetProperPaletteData(palIndex);
                    layer.ApplyPalette(palData);
                    pnlTileView.Invalidate();
                });

            // タイルインデックス数値
            _eventBinder.BindCtrl(
                h => nudBlockIndex.ValueChanged += h,
                h => nudBlockIndex.ValueChanged -= h,
                (_, __) =>
                {
                    // nudの数値とtxtの表示
                    _selectedBlockIndex = (int)nudBlockIndex.Value;
                    txtBlockIndex.Text =
                        _selectedBlockIndex.ParseIntToString(txtBlockIndex.Digits);

                    // 選択範囲の更新
                    var selectedIndexList = _blockLayerHolder.SelectorLayer.GetSelectedIndexList();
                    if (selectedIndexList.Count > 0 && selectedIndexList[0] == _selectedBlockIndex) return;
                    _blockLayerHolder.SelectorLayer.SelectSingleItem(_selectedBlockIndex);
                });
            _eventBinder.BindCustom(
                () => _blockLayerHolder.SelectorLayer.SelectChanged += UpdateSelectedBlockIndex,
                () => _blockLayerHolder.SelectorLayer.SelectChanged -= UpdateSelectedBlockIndex);

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
                scale: Constants.DefaultScale,
                validItemCount: totalTiles);

            // 画像の設定
            _tileLayerHolder.SetImageLayer(
                LayerNames.Tile,
                _combinedTilesetImageData,
                palData,
                width,
                height);
            _tileLayerHolder.SetLayerVisible(LayerNames.Tile, true);

            // グリッドの設定
            _tileLayerHolder.SetGridVisible(true);

            // 選択範囲の設定
            var maxLength = Constants.TilePerBlockSide;
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
                scale: Constants.DefaultScale,
                validItemCount: _blockDataList.Count);

            // 画像を破棄
            var oldLower = _blockLayerHolder.GetLayer<MapBlockLayer>(LayerNames.BlockLower);
            oldLower?.DisposeImages();
            var oldUpper = _blockLayerHolder.GetLayer<MapBlockLayer>(LayerNames.BlockUpper);
            oldUpper?.DisposeImages();

            var lowerBlockLayer = new MapBlockLayer(_blockLayerHolder.Data, blockSize);
            var upperBlockLayer = new MapBlockLayer(_blockLayerHolder.Data, blockSize);

            // 定数を事前に計算
            var tileLayer = _tileLayerHolder.GetLayer<ImageLayer>(LayerNames.Tile);
            int tilesPerRow = Constants.TilesetImageWidth / Constants.TileSize;

            for (int i = 0; i < _blockDataList.Count; i++)
            {
                var blockData = _blockDataList[i];

                // 1ブロック分の画像を用意
                Bitmap lowerBmp = new Bitmap(blockSize, blockSize);
                Bitmap upperBmp = new Bitmap(blockSize, blockSize);

                using (Graphics gLower = Graphics.FromImage(lowerBmp))
                using (Graphics gUpper = Graphics.FromImage(upperBmp))
                {
                    DrawBlockLayer(gLower, blockData.Lower, 0, 0, tileLayer, tilesPerRow, isLower: true);
                    DrawBlockLayer(gUpper, blockData.Upper, 0, 0, tileLayer, tilesPerRow, isLower: false);
                }

                // リストに追加
                lowerBlockLayer.BlockImages.Add(lowerBmp);
                upperBlockLayer.BlockImages.Add(upperBmp);
            }

            // 生成したカスタムレイヤーを登録
            _blockLayerHolder.AddCustomLayer(LayerNames.BlockLower, lowerBlockLayer);
            _blockLayerHolder.AddCustomLayer(LayerNames.BlockUpper, upperBlockLayer);

            // 画像レイヤーを表示する
            _blockLayerHolder.SetLayerVisible(LayerNames.BlockLower, true);
            _blockLayerHolder.SetLayerVisible(LayerNames.BlockUpper, true);

            // グリッドと選択範囲の設定
            _blockLayerHolder.SetGridVisible(true);
            _blockLayerHolder.SelectorLayer.MaxSelectSize = new Size(1, 1);
            _blockLayerHolder.SetSelectorVisible(true);

            // スクロールバーの設定
            _blockLayerScroller.UpdateScrollRange();
        }

        private void DrawBlockLayer(
            Graphics gfx,
            BlockLayer layer,
            int drawX,
            int drawY,
            ImageLayer tileLayer,
            int tilesPerRow,
            bool isLower)
        {
            // 4つのタイルを配置
            DrawTile(
                gfx, 
                layer.TopLeft, 
                drawX,
                drawY, 
                tileLayer, 
                tilesPerRow, 
                isLower);
            DrawTile(
                gfx, 
                layer.TopRight, 
                drawX + Constants.TileSize,
                drawY, 
                tileLayer, 
                tilesPerRow, 
                isLower);
            DrawTile(
                gfx, 
                layer.BottomLeft, 
                drawX, 
                drawY + Constants.TileSize, 
                tileLayer,
                tilesPerRow,
                isLower);
            DrawTile(
                gfx, 
                layer.BottomRight, 
                drawX + Constants.TileSize, 
                drawY + Constants.TileSize,
                tileLayer,
                tilesPerRow, 
                isLower);
        }

        private void DrawTile(
            Graphics gfx,
            BlockTileData tileData,
            int x,
            int y,
            ImageLayer tileLayer,
            int tilesPerRow,
            bool isLower)
        {
            // タイルを描画するマス座標(タイルの画像レイヤー)を計算
            int tileGridX = tileData.TileIndex % tilesPerRow;
            int tileGridY = tileData.TileIndex / tilesPerRow;

            // タイルの画像レイヤーから画像データを取得
            byte[] tileBytes = tileLayer.ExtractImageDataAtGrid(tileGridX, tileGridY);
            if (tileBytes == null || tileBytes.Length == 0) return;

            // パレットデータを取得
            byte[] palData = GetProperPaletteData(tileData.PaletteIndex);

            // 上位レイヤーは背景色を透過する
            using (Bitmap tileBmp = ImageHelper.CreateBitmap(
                tileBytes, palData, Constants.TileSize, Constants.TileSize, showBackColor: isLower))
            {
                if (tileBmp == null) return;

                // タイルの反転情報を適用
                RotateFlipType flipType = RotateFlipType.RotateNoneFlipNone;
                if (tileData.ReverseX && tileData.ReverseY)
                {
                    flipType = RotateFlipType.RotateNoneFlipXY;
                }
                else if (tileData.ReverseX)
                {
                    flipType = RotateFlipType.RotateNoneFlipX;
                }
                else if (tileData.ReverseY)
                {
                    flipType = RotateFlipType.RotateNoneFlipY;
                }

                // 反転が必要なら反転させる
                if (flipType != RotateFlipType.RotateNoneFlipNone)
                {
                    tileBmp.RotateFlip(flipType);
                }

                // キャンバスに描画
                gfx.DrawImage(tileBmp, x, y);
            }
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
                includeSelf: true);
            // タイル画像パネル
            if (!state)
            {
                _tileLayerHolder.SetLayerVisible(LayerNames.Tile, state);
                _tileLayerHolder.SetGridVisible(state);
                _tileLayerHolder.SelectorLayer.ClearSelect();
                _tileLayerHolder.SetSelectorVisible(state);
            }
            // ブロック画像パネル
            if (!state)
            {
                _blockLayerHolder.SetLayerVisible(LayerNames.BlockLower, state);
                _blockLayerHolder.SetLayerVisible(LayerNames.BlockUpper, state);
                _blockLayerHolder.SetGridVisible(state);
                _blockLayerHolder.SelectorLayer.ClearSelect();
                _blockLayerHolder.SetSelectorVisible(state);
            }
        }

        private byte[] GetProperPaletteData(int palIndex)
        {
            if (palIndex < 0) return Array.Empty<byte>();
            return palIndex >= (int)TilesetHeaderHolder.PaletteKind.Palette7to12
                ? _tileset2Manager.PaletteData[palIndex]
                : _tileset1Manager.PaletteData[palIndex];
        }

        private void UpdateSelectedBlockIndex()
        {
            var indexList = _blockLayerHolder.SelectorLayer.GetSelectedIndexList();
            if (indexList.Count == 0) return;
            nudBlockIndex.Value = (decimal)indexList[0];
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

    // マップのブロックを管理するためのカスタムレイヤー
    public sealed class MapBlockLayer : LayerBase
    {
        public List<Bitmap> BlockImages { get; set; } 
        public int BlockSize { get; set; }

        public MapBlockLayer(LayerData layerData, int blockSize)
        {
            _layerData = layerData;

            BlockImages = new List<Bitmap>();
            BlockSize = blockSize;
        }

        public override void Draw(Graphics gfx)
        {
            if (BlockImages == null || BlockImages.Count == 0) return;

            int cols = _layerData.Columns;
            int scaledBlockSize = BlockSize * _layerData.Scale;

            for (int i = 0; i < BlockImages.Count; i++)
            {
                var bmp = BlockImages[i];
                if (bmp == null) continue;

                int gridX = i % cols;
                int gridY = i / cols;
                int drawX = gridX * scaledBlockSize;
                int drawY = gridY * scaledBlockSize;

                gfx.DrawImage(bmp,
                    new Rectangle(drawX, drawY, scaledBlockSize, scaledBlockSize),
                    new Rectangle(0, 0, BlockSize, BlockSize),
                    GraphicsUnit.Pixel);
            }
        }

        // メモリリークを防ぐため
        public void DisposeImages()
        {
            foreach (var img in BlockImages)
            {
                img?.Dispose();
            }
            BlockImages.Clear();
        }
    }
}
