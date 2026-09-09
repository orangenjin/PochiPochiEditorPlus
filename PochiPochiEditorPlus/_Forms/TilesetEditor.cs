using System;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
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

        public TilesetEditor(SharedData sharedData, UndoManager undoManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _dynamicConfig = _sharedData.Config;
            _undoManager = undoManager;
            _eventBinder = new EventBinder();

            InitializeControls();
            InitializeEventHandlers();

            _tilesetManager = new TilesetManager(_sharedData);
            LoadDataToUI(_currentTilesetNo);
        }

        private void InitializeControls()
        {
            // コンボボックスのアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbImageCompType, "txt/tileset/TilesetImageCompType.txt"),
                (cmbPaletteType, "txt/tileset/TilesetPaletteype.txt"),
                (cmbTilesetViewPalette, "txt/tileset/TilesetPaletteIndex.txt"));

            // 一時的に全コントロールを無効化
            UpdateTabPageState(false);
        }

        private void UpdateTabPageState(bool state)
        {
            // pnlのクリア処理が必要
            //

            // tbcMain
            CtrlHelper.SetControlsEnabled(tbcMain, state);
            CtrlHelper.ResetControls(tbcMain);
        }

        private void InitializeEventHandlers()
        {
            // 枠描画
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpTilesetView, pnlTilesetViewImage),
                () => CtrlHelper.DetachBorder(grpTilesetView));

            // タイルセット番号
            _eventBinder.BindCtrl(
                h => btnLoadTileset.Click += h,
                h => btnLoadTileset.Click -= h,
                (_, __) =>
                {
                    _currentTilesetNo = (int)nudTilesetNo.Value;
                    LoadDataToUI(_currentTilesetNo);
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

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void LoadDataToUI(int tilsetNo)
        {
            // ヘッダーを再読み込み
            _tilesetManager.ReadHeader(tilsetNo, _sharedData);

            // UIを有効化
            UpdateTabPageState(true);

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
