using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Helpers._MatchHelper;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.Tileset)]
    public partial class TilesetEditor : Form, IEditorRefresh
    {
        // 共有データ用
        private SharedData _sharedData = null;
        private dynamic _dynamicConfig = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各エントリーテーブル用
        private dynamic _tilesetManager = null;
        // UI制御用
        private int _currentTilesetNo = 0;
        private Bitmap _viewBmpOriginal = null;
        private Bitmap _viewBmpScale = null;

        public TilesetEditor(SharedData sharedData, UndoManager undoManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _dynamicConfig = _sharedData.Config;
            _undoManager = undoManager;
            _eventBinder = new EventBinder();
            _tilesetManager = new TilesetManager(_sharedData);

            InitializeControls();
            InitializeEventHandlers();
        }

        private void InitializeControls()
        {
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

            // タイルセット番号
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
                        // btnLoadTilesetを更新
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

                        // 失敗したらUIを無効化・リセット
                        UpdateTabPageState(false);
                        // btnLoadTilesetを更新
                        UpdateLoadUIState(true);
                    }
                });
            // タイルセット番号
            _eventBinder.BindCtrl(
                h => btnReloadTileset.Click += h,
                h => btnReloadTileset.Click -= h,
                (_, __) =>
                {
                    UpdateTabPageState(false);
                    UpdateLoadUIState(true);
                });

            // 画像圧縮設定
            _eventBinder.BindCtrl(
                h => cmbImageCompType.SelectionChangeCommitted += h,
                h => cmbImageCompType.SelectionChangeCommitted -= h,
                (sender, e) => UpdateFromComboBox(
                    sender, _tilesetManager.HeaderEntry.ImageCompType, "画像圧縮設定"));
            // パレット読み込み設定
            _eventBinder.BindCtrl(
                h => cmbPaletteType.SelectionChangeCommitted += h,
                h => cmbPaletteType.SelectionChangeCommitted -= h,
                (sender, e) => UpdateFromComboBox(
                    sender, _tilesetManager.HeaderEntry.PaletteType, "パレット読み込み設定"));
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
            // コンボボックス更新ヘルパー
            void UpdateFromComboBox(object sender, dynamic entry, string itemName)
            {
                var value = ((ComboBox)sender).SelectedValue;
                var desc = $"[{this.Text}]{itemName}(ID:{_currentTilesetNo:D8})";
                entry.UpdateData(_undoManager, value, desc);
            }
            // テキストボックス更新ヘルパー
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
                    if (_viewBmpOriginal == null || _viewBmpScale == null) return;

                    int palIndex = cmbViewPalette.SelectedIndex;
                    if (palIndex < 0) return;
                    byte[] palData = _tilesetManager.PaletteData[palIndex];

                    // パレットのみを書き換えて再描画
                    ImageHelper.ApplyPalette(_viewBmpOriginal, palData, showBackColor: true);
                    using (Graphics g = Graphics.FromImage(_viewBmpScale))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                        g.DrawImage(_viewBmpOriginal, new Rectangle(0, 0, _viewBmpScale.Width, _viewBmpScale.Height));
                    }
                    pnlViewImage.Invalidate();
                });
            // スクロールバー操作時の再描画
            _eventBinder.BindCtrl(
                h => vsbViewImage.ValueChanged += h,
                h => vsbViewImage.ValueChanged -= h,
                (_, __) => pnlViewImage.Invalidate());
            // マウスホイールでのスクロール
            _eventBinder.BindCustom(
                () => pnlViewImage.MouseWheel += pnlViewImage_MouseWheel,
                () => pnlViewImage.MouseWheel -= pnlViewImage_MouseWheel);
            // pnlViewImageの描画処理
            _eventBinder.BindCustom(
                () => pnlViewImage.Paint += pnlViewImage_Paint,
                () => pnlViewImage.Paint -= pnlViewImage_Paint);

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

            // grpView
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
            if (_tilesetManager.ImageData == null || _tilesetManager.ImageData.Length == 0) return;
            if (_tilesetManager.PaletteData == null || _tilesetManager.PaletteData.Count == 0) return;

            // 選択中のパレットを取得
            int palIndex = cmbViewPalette.SelectedIndex;
            if (palIndex < 0) return;
            byte[] palData = _tilesetManager.PaletteData[palIndex];

            // 横幅は128固定
            int width = Constants.TilesetImageWidth;

            // 1行に対するバイト数
            int bytesPerTileRow = (width * Constants.TileSize) / Constants.PixelsPerByte4Bpp;
            // 必要なタイル行数を計算（端数は切り上げ）
            int tileRows = (_tilesetManager.ImageData.Length + bytesPerTileRow - 1) / bytesPerTileRow;
            // 必要な高さを求める
            int height = tileRows * Constants.TileSize;

            // Bitmapを生成
            _viewBmpOriginal = ImageHelper.CreateBitmap(
                _tilesetManager.ImageData,
                palData,
                width,
                height,
                showBackColor: true);
            // 2倍にスケール
            _viewBmpScale = ImageHelper.ScaleBitmap(_viewBmpOriginal);

            // スクロールバーの設定
            if (_viewBmpScale.Height > pnlViewImage.Height)
            {
                vsbViewImage.Enabled = true;
                vsbViewImage.Minimum = 0;
                vsbViewImage.LargeChange = Constants.TileSize * 4;
                vsbViewImage.SmallChange = Constants.TileSize * 2;
                vsbViewImage.Maximum = (_viewBmpScale.Height - pnlViewImage.Height) + vsbViewImage.LargeChange - 1;
                vsbViewImage.Value = 0;
            }
            else
            {
                vsbViewImage.Enabled = false;
                vsbViewImage.Value = 0;
            }

            // パネルの再描画
            pnlViewImage.Invalidate();
        }

        private void UpdateTabPageState(bool state)
        {
            // grpView
            CtrlHelper.SetControlsEnabled(grpView, state);
            CtrlHelper.ResetControls(grpView);

            // pnlViewImage
            if (!state)
            {
                _viewBmpOriginal?.Dispose();
                _viewBmpOriginal = null;
                _viewBmpScale?.Dispose();
                _viewBmpScale = null;
                pnlViewImage.Invalidate();
            }

            // tbcMain
            CtrlHelper.SetControlsEnabled(tbcMain, state);
            CtrlHelper.ResetControls(tbcMain);

            // btnReloadTileset
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
                TokenData.Exact(0x0),
                TokenData.Exact(0x0),
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
        /// パネルでのマウスホイールでスクロールさせる。
        /// </summary>
        private void pnlViewImage_MouseWheel(object sender, MouseEventArgs e)
        {
            if (vsbViewImage.Enabled)
            {
                int change = e.Delta > 0 
                    ? -vsbViewImage.LargeChange 
                    : vsbViewImage.LargeChange;
                int newValue = vsbViewImage.Value + change;
                vsbViewImage.Value = Math.Max(vsbViewImage.Minimum,
                    Math.Min(newValue, vsbViewImage.Maximum - vsbViewImage.LargeChange + 1));
            }
        }

        /// <summary>
        /// パネルの描画を更新する。
        /// </summary>
        private void pnlViewImage_Paint(object sender, PaintEventArgs e)
        {
            if (_viewBmpScale != null)
            {
                // スクロールバーの値をマイナスのY座標にして描画することでスクロールを表現
                e.Graphics.DrawImage(_viewBmpScale, 0, -vsbViewImage.Value);
            }
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshFromData()
        {
            LoadDataToUI(_currentTilesetNo);
        }
    }
}
