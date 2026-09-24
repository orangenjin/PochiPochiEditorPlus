using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Helpers._MatchHelper;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Managers._LayerManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.Tileset)]
    public partial class TilesetEditor : Form, IEditorRefresh
    {
        // 共有データ用
        private SharedData _sharedData = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各エントリーテーブル用
        private dynamic _tilesetManager = null;
        // パネル描画用
        private LayerHolder<LayerNames> _layerHolder = null;
        // UI制御用
        private int _currentTilesetNo = 0;
        private int _selectedTileIndex = 0;

        private enum LayerNames{ Tileset }

        public TilesetEditor(SharedData sharedData, UndoManager undoManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _undoManager = undoManager;
            _eventBinder = new EventBinder();
            _tilesetManager = new TilesetManager(_sharedData);
            _layerHolder = new LayerHolder<LayerNames>(pnlViewImage, _eventBinder);

            InitializeControls();
            InitializeEventHandlers();
        }

        private void InitializeControls()
        {
            // タイル画像パネルのダブルバッファリングを有効化
            typeof(Control).GetProperty(
                nameof(DoubleBuffered), 
                System.Reflection.BindingFlags.Instance 
                | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(pnlViewImage, true, null);

            // コンボボックスのアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbImageCompType, "txt/tileset/TilesetImageCompType.txt"),
                (cmbPaletteType, "txt/tileset/TilesetPaletteype.txt"),
                (cmbViewPalette, "txt/tileset/TilesetPaletteIndex.txt"));

            // 一時的にタブコントロールを無効化
            UpdateTabPageState(false);
        }

        private void InitializeEventHandlers()
        {
            // 枠描画
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpView, pnlViewImage),
                () => CtrlHelper.DetachBorder(grpView));

            // タイルセットの読み込み
            _eventBinder.BindCtrl(
                h => btnLoadTileset.Click += h,
                h => btnLoadTileset.Click -= h,
                (_, __) =>
                {
                    // 現在のタイルセット番号を更新
                    _currentTilesetNo = (int)nudTilesetNo.Value;

                    if (ValidateHeader(_currentTilesetNo))
                    {
                        // UIを有効化
                        UpdateTabPageState(true);
                        // マッチングに成功したら読み込む
                        LoadDataToUI(_currentTilesetNo);
                        // 読み込みUIを更新
                        UpdateLoadUIState(false);
                    }
                    else
                    {
                        // 失敗メッセージの表示
                        MessageBox.Show(
                            "ヘッダーの読み込みに失敗しました。",
                            "読み込みエラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        // 失敗したらタブを無効化・リセット
                        UpdateTabPageState(false);
                        // 読み込みUIを更新
                        UpdateLoadUIState(true);
                    }
                });
            _eventBinder.BindCtrl(
                h => btnReloadTileset.Click += h,
                h => btnReloadTileset.Click -= h,
                (_, __) =>
                {
                    UpdateTabPageState(false);
                    UpdateLoadUIState(true);
                });

            // 画像アドレス
            _eventBinder.BindCtrl(
                h => txtImageOffset.Validated += h,
                h => txtImageOffset.Validated -= h,
                (sender, e) => UpdateFromTextBox(
                    sender, _tilesetManager.HeaderEntry.ImageOffset, "画像アドレス"));
            // パレットアドレス
            _eventBinder.BindCtrl(
                h => txtPaletteOffset.Validated += h,
                h => txtPaletteOffset.Validated -= h,
                (sender, e) => UpdateFromTextBox(
                    sender, _tilesetManager.HeaderEntry.PaletteOffset, "パレットアドレス"));
            // ブロックデータテーブル
            _eventBinder.BindCtrl(
                h => txtBlockDataTableOffset.Validated += h,
                h => txtBlockDataTableOffset.Validated -= h,
                (sender, e) => UpdateFromTextBox(
                    sender, _tilesetManager.HeaderEntry.BlockDataTableOffset, "ブロックデータテーブル"));
            // ブロック属性テーブル
            _eventBinder.BindCtrl(
                h => txtBlockAttrTableOffset.Validated += h,
                h => txtBlockAttrTableOffset.Validated -= h,
                (sender, e) => UpdateFromTextBox(
                    sender, _tilesetManager.HeaderEntry.BlockAttrTableOffset, "ブロック属性テーブル"));
            // アニメヘッダーアドレス
            _eventBinder.BindCtrl(
                h => txtAnimHeaderOffset.Validated += h,
                h => txtAnimHeaderOffset.Validated -= h,
                (sender, e) => UpdateFromTextBox(
                    sender, _tilesetManager.HeaderEntry.AnimHeaderOffset, "アニメヘッダーアドレス"));
            // 上記の更新ヘルパー
            void UpdateFromTextBox(object sender, dynamic entry, string itemName)
            {
                var value = ((TextBox)sender).Text.ParseStringToInt();
                var desc = $"[{this.Text}]{itemName}(ID:{_currentTilesetNo:D8})";
                entry.UpdateData(_undoManager, value, desc);
            }

            // パレット切り替え
            _eventBinder.BindCtrl(
                h => cmbViewPalette.SelectedIndexChanged += h,
                h => cmbViewPalette.SelectedIndexChanged -= h,
                (_, __) =>
                {
                    int palIndex = cmbViewPalette.SelectedIndex;
                    if (palIndex < 0) return;

                    // 画像レイヤーを取得
                    var layer = _layerHolder.GetImageLayer(LayerNames.Tileset);
                    if (layer == null) return;

                    // パレットを更新
                    byte[] palData = _tilesetManager.PaletteData[palIndex];
                    layer.ApplyPalette(palData);
                    pnlViewImage.Invalidate();
                });
            // タイルインデックス数値
            _eventBinder.BindCtrl(
                h => nudViewTileIndex.ValueChanged += h,
                h => nudViewTileIndex.ValueChanged -= h,
                (_, __) =>
                {
                    _selectedTileIndex = (int)nudViewTileIndex.Value;
                    txtViewTileIndex.Text =
                        _selectedTileIndex.ParseIntToString(txtViewTileIndex.Digits);
                });

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void LoadDataToUI(int tilsetNo)
        {
            // ヘッダーを再読み込み
            _tilesetManager.ReadHeader(tilsetNo, _sharedData);

            // ヘッダーデータをUIに反映
            cmbImageCompType.SelectedValue =
                _tilesetManager.HeaderEntry.ImageCompType.GetData<byte>();
            cmbPaletteType.SelectedValue =
                _tilesetManager.HeaderEntry.PaletteType.GetData<byte>();
            txtImageOffset.Text =
                ConvHelper.ParseIntToString(
                    _tilesetManager.HeaderEntry.ImageOffset.GetData<int>());
            txtPaletteOffset.Text =
                ConvHelper.ParseIntToString(
                    _tilesetManager.HeaderEntry.PaletteOffset.GetData<int>());
            txtBlockDataTableOffset.Text =
                ConvHelper.ParseIntToString(
                    _tilesetManager.HeaderEntry.BlockDataTableOffset.GetData<int>());
            txtBlockAttrTableOffset.Text =
                ConvHelper.ParseIntToString(
                    _tilesetManager.HeaderEntry.BlockAttrTableOffset.GetData<int>());
            txtAnimHeaderOffset.Text =
                ConvHelper.ParseIntToString(
                    _tilesetManager.HeaderEntry.AnimHeaderOffset.GetData<int>());

            // パレットタイプに応じてパレット初期選択を変更
            cmbViewPalette.SelectedIndex =
                Convert.ToBoolean(_tilesetManager.HeaderEntry.PaletteType.GetData<int>())
                    ? (int)TilesetManager.PaletteKind.Palette7to12
                    : (int)TilesetManager.PaletteKind.Palette0to6;
            UpdateViewImage();
        }

        /// <summary>
        /// 画像とパレットからBitmapを生成して表示する
        /// </summary>
        private void UpdateViewImage()
        {
            // 選択中のパレットを取得
            int palIndex = cmbViewPalette.SelectedIndex;
            if (palIndex < 0) return;
            byte[] palData = _tilesetManager.PaletteData[palIndex];

            // 横幅は128固定
            var width = Constants.TilesetImageWidth;
            // 1行に対するバイト数
            var bytesPerTileRow = (width * Constants.TileSize) / Constants.PixelsPerByte;
            // 必要なタイル行数を計算（端数は切り上げ）
            var tileRows = (_tilesetManager.ImageData.Length + bytesPerTileRow - 1) / bytesPerTileRow;
            // 必要な高さを求める
            var height = tileRows * Constants.TileSize;

            // 有効なタイル数に基づいてnudの上限を設定
            int totalTiles = _tilesetManager.GetTotalTileCount();
            if (totalTiles > 0)
            {
                nudViewTileIndex.Maximum = totalTiles - 1;
                nudViewTileIndex.Minimum = 0;
                _selectedTileIndex = Math.Min(_selectedTileIndex, totalTiles - 1);
                nudViewTileIndex.Value = _selectedTileIndex;
                txtViewTileIndex.Text =
                    _selectedTileIndex.ParseIntToString(txtViewTileIndex.Digits);
            }

            // レイヤー初期設定
            _layerHolder.Initialize(
                gridSize: Constants.TileSize,
                scale: Constants.DefaultScale,
                validItemCount: totalTiles);

            // 画像の設定
            _layerHolder.SetImageLayer(
                LayerNames.Tileset,
                _tilesetManager.ImageData,
                palData,
                width,
                height);
            _layerHolder.SetLayerVisible(LayerNames.Tileset, true);

            // グリッドの設定
            _layerHolder.SetGridVisible(true);

            // 選択範囲の設定
            var maxLength = Constants.TilesetImageWidth / Constants.TileSize;
            _layerHolder.SelectorLayer.MaxSelectSize = new Size(maxLength, maxLength);
            _layerHolder.SetSelectorVisible(true);


            /*
            // スクロールバーの設定
            int contentHeight = image.Height * _panelLayers.Scale;
            _panelScroller.UpdateRangeY(
                contentHeight, 
                Constants.TileSize * Constants.DefaultScale);
            */



        }

        private void UpdateTabPageState(bool state)
        {
            // 閲覧用グループ
            CtrlHelper.SetControlsEnabled(grpView, state);
            CtrlHelper.ResetControls(grpView);

            // タイル画像パネル
            if (!state)
            {
                _layerHolder.SetLayerVisible(LayerNames.Tileset, false);
                _layerHolder.SetGridVisible(false);
                _layerHolder.SelectorLayer.ClearSelect();
                pnlViewImage.Invalidate();
            }

            // タブページ
            CtrlHelper.SetControlsEnabled(
                tbcMain, 
                state,
                includeSelf: true,
                new[] { nameof(cmbImageCompType), nameof(cmbPaletteType) },
                null);
            CtrlHelper.ResetControls(tbcMain);

            // 再読み込みボタン
            btnReloadTileset.Enabled = state;
        }

        private void UpdateLoadUIState(bool state)
        {
            btnLoadTileset.Enabled = state;
            lblTilesetNo.Enabled = state;
            nudTilesetNo.ReadOnly = !state;
            nudTilesetNo.Increment = Convert.ToInt32(state);
        }

        private bool ValidateHeader(int tilesetNo)
        {
            // マッチングパターン
            var headerPattern = new List<TokenData>()
            {
                TokenData.Range(byte.MinValue, (byte)cmbImageCompType.Items.Count, Constants.ByteSize),
                TokenData.Range(byte.MinValue, (byte)cmbPaletteType.Items.Count, Constants.ByteSize),
                TokenData.Exact(Constants.ByteSize, exactValues: 0x0),
                TokenData.Exact(Constants.ByteSize, exactValues: 0x0),
                TokenData.Pointer(),
                TokenData.Pointer(),
                TokenData.Pointer(),
                TokenData.Pointer(),
                TokenData.Pointer()
            };

            return PatternMatcher.TryMatch(
                headerPattern,
                _sharedData.RomData,
                _tilesetManager.CalcOffset(tilesetNo),
                allowNullPointer: true); // nullポインタを許容する
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshUI()
        {
            LoadDataToUI(_currentTilesetNo);
        }
    }
}
