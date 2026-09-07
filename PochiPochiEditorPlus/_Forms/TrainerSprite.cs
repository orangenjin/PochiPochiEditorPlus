using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Managers._CommandManager;
using PochiPochiEditorPlus._Utilities;
using PochiPochiEditorPlus._Utilities._QuickInput;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.TrainerSprite)]
    public partial class TrainerSprite : Form, IEditorRefresh
    {
        // 共有データ用
        private SharedData _sharedData = null;
        private dynamic _dynamicConfig = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各テーブル用
        private dynamic _tileEntry = null;
        private dynamic _paletteEntry = null;
        private dynamic _yPosEntry = null;
        private dynamic _animPointerEntry = null;
        // 可変長データ管理用
        private RefDataManager _tileData = null;
        private RefDataManager _paletteData = null;
        // UI制御用
        private int _currentSpriteIndex = default;
        // データ識別タグ用
        private enum SpriteData { Tile, Palette }

        public TrainerSprite(SharedData sharedData, UndoManager undoManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _dynamicConfig = _sharedData.Config;
            _undoManager = undoManager;
            _eventBinder = new EventBinder();

            InitializeEntries();
            InitializeControls();
            InitializeEventHandlers();

            LoadDataToUI(_currentSpriteIndex);
        }

        private void InitializeEntries()
        {
            // 画像テーブルを作成
            int tableOffset = _dynamicConfig.TrainerSpriteImageTableOffset;
            int entrycount = _dynamicConfig.TrainerSpriteCount;
            _tileEntry = 
                new EntryManager("TrainerSpriteImageEntry", tableOffset, entrycount, _sharedData);

            // パレットテーブルを作成
            tableOffset = _dynamicConfig.TrainerSpritePaletteTableOffset;
            _paletteEntry = 
                new EntryManager("TrainerSpritePaletteEntry", tableOffset, entrycount, _sharedData);

            // Y座標位置テーブルを作成
            tableOffset = _dynamicConfig.TrainerSpriteYPosTableOffset;
            _yPosEntry = 
                new EntryManager("TrainerSpriteYPosEntry", tableOffset, entrycount, _sharedData);

            // アニメポインタテーブルを作成
            tableOffset = _dynamicConfig.TrainerSpriteAnimPointerTableOffset;
            _animPointerEntry = 
                new EntryManager("TrainerSpriteAnimationPointerEntry", tableOffset, entrycount, _sharedData);
        }

        private void InitializeControls()
        {
            // nudSpriteIndexの最大値
            int spriteCount = _dynamicConfig.TrainerSpriteCount;
            nudSpriteIndex.Maximum = spriteCount - 1;

            // タグ設定
            btnImportSpriteTile.Tag = SpriteData.Tile;
            btnImportSpritePalette.Tag = SpriteData.Palette;
        }

        private void InitializeEventHandlers()
        {
            // 枠描画
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(this, picSpriteBmp),
                () => CtrlHelper.DetachBorder(this));
            // nudにbtnを対応付ける
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBtnsToNud(
                    nudSpriteIndex,
                    btnSpriteIndexPrev,
                    btnSpriteIndexNext),
                () => CtrlHelper.DetachBtnsToNud(
                    nudSpriteIndex,
                    btnSpriteIndexPrev,
                    btnSpriteIndexNext));

            // 画像アドレス
            _eventBinder.BindCtrl(
                h => txtSpriteTileOffset.Validated += h,
                h => txtSpriteTileOffset.Validated -= h,
                (sender, e) =>
                {
                    // 入力されたアドレスを取得
                    var ctrl = (TextBox)sender;
                    var text = ctrl.Text;

                    // データを更新
                    var offsetValue = ConvHelper.ParseStringToInt(text);
                    var desc = $"[{this.Text}]画像アドレス(ID:{_currentSpriteIndex:D4})";

                    _tileEntry.Entries[_currentSpriteIndex].SpriteTileOffset
                        .UpdateData(_undoManager, offsetValue, desc);
                });
            // パレットアドレス
            _eventBinder.BindCtrl(
                h => txtSpritePaletteOffset.Validated += h,
                h => txtSpritePaletteOffset.Validated -= h,
                (sender, e) =>
                {
                    // 入力されたアドレスを取得
                    var ctrl = (TextBox)sender;
                    var text = ctrl.Text;

                    // データを更新
                    var offsetValue = ConvHelper.ParseStringToInt(text);
                    var desc = $"[{this.Text}]パレットアドレス(ID:{_currentSpriteIndex:D4})";

                    _paletteEntry.Entries[_currentSpriteIndex].SpritePaletteOffset
                        .UpdateData(_undoManager, offsetValue, desc);
                });
            // Y座標位置
            _eventBinder.BindCtrl(
                h => nudSpriteYPosValue.ValueChanged += h,
                h => nudSpriteYPosValue.ValueChanged -= h,
                (sender, e) =>
                {
                    // 入力値を取得
                    var ctrl = (NumericUpDown)sender;
                    var value = ctrl.Value;

                    // データを更新
                    var desc = $"[{this.Text}]Y座標位置(ID:{_currentSpriteIndex:D4})";
                    _yPosEntry.Entries[_currentSpriteIndex].SpriteYPosValue
                        .UpdateData(_undoManager, value, desc);
                });

            // 画像インデックスnud
            _eventBinder.BindCtrl(
                h => nudSpriteIndex.ValueChanged += h,
                h => nudSpriteIndex.ValueChanged -= h,
                (_, __) =>
                {
                    int newIndex = (int)nudSpriteIndex.Value;
                    LoadDataToUI(newIndex);
                });

            // エクスポート
            _eventBinder.BindCtrl(
                h => btnExportSprite.Click += h,
                h => btnExportSprite.Click -= h,
                (_, __) =>
                {
                    // 正規かどうかの判定
                    if (picSpriteBmp.Image == null) return;

                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Filter = Constants.SpriteExportFilter;
                        sfd.FileName = $"trainer_sprite_{_currentSpriteIndex:D4}";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            // RefDataから生成
                            var imageData = ImageHelper.DecompressLZ77(
                                _tileData.BinaryData);
                            var paletteData = ImageHelper.DecompressPalette(
                                _paletteData.BinaryData);
                            var sprite = ImageHelper.CreateBitmap(
                                imageData,
                                paletteData,
                                Constants.SpriteSize,
                                Constants.SpriteSize,
                                showBackColor: true);

                            ImageHelper.ExportIndexedImage(
                                sprite,
                                sfd.FileName);
                        }
                    }
                });
            // インポート
            _eventBinder.BindCtrl(
                h => btnImportSpriteTile.Click += h,
                h => btnImportSpriteTile.Click -= h,
                SpriteImport_Click);
            _eventBinder.BindCtrl(
                h => btnImportSpritePalette.Click += h,
                h => btnImportSpritePalette.Click -= h,
                SpriteImport_Click);

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void LoadDataToUI(int index)
        {
            _currentSpriteIndex = index;

            // 画像アドレス
            txtSpriteTileOffset.Text =
                ConvHelper.ParseIntToString(
                    _tileEntry.Entries[index].SpriteTileOffset.GetData<int>());
            // パレットアドレス
            txtSpritePaletteOffset.Text =
                ConvHelper.ParseIntToString(
                    _paletteEntry.Entries[index].SpritePaletteOffset.GetData<int>());
            // Y座標位置
            nudSpriteYPosValue.Value =
                _yPosEntry.Entries[index].SpriteYPosValue.GetData<int>();

            // アニメーションポインタアドレス
            txtSpriteAnimPointerOffset.Text =
                ConvHelper.ParseIntToString(
                    _animPointerEntry.Entries[index].SpriteAnimPointerOffset.GetData<int>());
            // アニメーションデータアドレス
            int targetOffset =
                _animPointerEntry.Entries[index].SpriteAnimPointerOffset.GetData<int>();
            txtSpriteAnimDataOffset.Text = IoHelper.TryReadPtr(
                _sharedData.RomData,
                targetOffset,
                out int resultOffset)
                && resultOffset != Constants.InvalidValue
                    ? resultOffset.ParseIntToString()
                    : string.Empty;

            // 画像の再描画
            DisplayTrainerSprite();
        }

        private void DisplayTrainerSprite()
        {
            var imageOffsetStr = txtSpriteTileOffset.Text;
            var paletteOffsetStr = txtSpritePaletteOffset.Text;
            var isImageInvalid = string.IsNullOrWhiteSpace(imageOffsetStr);
            var isPaletteInValid = string.IsNullOrWhiteSpace(paletteOffsetStr);

            // 無効なアドレスの場合は何も描画しない
            if (isImageInvalid || isPaletteInValid)
            {
                picSpriteBmp.Image?.Dispose();
                picSpriteBmp.Image = null;
                return;
            }

            try
            {
                // 画像アドレスを取得
                var imageOffsetValue = imageOffsetStr.ParseStringToInt();
                var imageData = ImageHelper.DecompressLZ77(
                    _sharedData.RomData,
                    imageOffsetValue);
                // RefDataとして保持する
                var imageDataLz77 = ImageHelper.CompressLZ77(imageData);
                _tileData = new RefDataManager(
                    SpriteData.Tile,
                    imageOffsetValue,
                    imageDataLz77,
                    _sharedData);

                // パレットアドレスを取得
                var paletteOffsetValue = paletteOffsetStr.ParseStringToInt();
                var paletteData = ImageHelper.DecompressPalette(
                    _sharedData.RomData,
                    paletteOffsetValue);
                // RefDataとして保持する
                var paletteDataLz77 = ImageHelper.CompressPalette(paletteData);
                _paletteData = new RefDataManager(
                    SpriteData.Palette,
                    paletteOffsetValue,
                    paletteDataLz77,
                    _sharedData);

                var sprite = ImageHelper.CreateBitmap(
                    imageData,
                    paletteData,
                    Constants.SpriteSize,
                    Constants.SpriteSize,
                    showBackColor: true);
                var scaled = ImageHelper.ScaleBitmap(sprite);

                picSpriteBmp.Image?.Dispose();
                picSpriteBmp.Image = scaled;
                picSpriteBmp.Refresh();
            }
            catch
            {
                picSpriteBmp.Image?.Dispose();
                picSpriteBmp.Image = null;
            }
        }

        private void SpriteImport_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is SpriteData importKind)) return;

            var inputs = new List<InputField>
            {
                new InputField("書き込み先オフセット", InputType.Offset),
                new InputField("ファイルパス", InputType.File, fileFilter: Constants.SpriteImportFilter)
            };

            using (var popup = new QuickInputForm(inputs))
            {
                if (popup.ShowDialog() != DialogResult.OK) return;

                // 入力値
                int newOffset = 0;
                string filePath = default;

                using (Bitmap bmp = new Bitmap(filePath))
                {
                    // バイト配列を抽出
                    if (!ImageHelper.ExtractTileAndPalette(
                        bmp,
                        Constants.SpriteSize,
                        Constants.SpriteSize,
                        out byte[] imageData,
                        out byte[] paletteData)) return;

                    if (importKind == SpriteData.Tile)
                    {
                        // LZ77圧縮を適用
                        var compressedData = ImageHelper.CompressLZ77(imageData);
                        // コマンド表示名
                        string desc = $"[{this.Text}]画像インポート(ID:{_currentSpriteIndex:D4})";

                        // コマンドを統合するため準備
                        var combine = new CombineCommands(desc);

                        // FieldValueの変更コマンド
                        combine.Add(
                            _tileEntry.Entries[_currentSpriteIndex].SpriteTileOffset
                                .CreateUpdateCommand(newOffset, desc));
                        // RefDataの変更コマンド
                        combine.Add(
                            _tileData.CreateUpdateCommand(
                                newOffset,
                                compressedData,
                                desc));
                        // 要素数が0より大きければ
                        if (combine.HasCommands)
                        {
                            _undoManager.PushCommand(combine);
                        }
                    }
                    else
                    {
                        // LZ77圧縮を適用
                        var compressedData = ImageHelper.CompressPalette(paletteData);
                        // コマンド表示名
                        var desc = $"[{this.Text}]パレットインポート(ID:{_currentSpriteIndex:D4})";

                        // コマンドを統合するため準備
                        var combine = new CombineCommands(desc);

                        // FieldValueの変更コマンド
                        combine.Add(
                            _paletteEntry.Entries[_currentSpriteIndex].SpritePaletteOffset
                                .CreateUpdateCommand(newOffset, desc));
                        // RefDataの変更コマンド
                        combine.Add(
                            _paletteData.CreateUpdateCommand(
                                newOffset,
                                compressedData,
                                desc));
                        // 要素数が0より大きければ
                        if (combine.HasCommands)
                        {
                            _undoManager.PushCommand(combine);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshFromData()
        {
            // 現在のインデックスを再読み込み
            LoadDataToUI(_currentSpriteIndex);
        }
    }
}
