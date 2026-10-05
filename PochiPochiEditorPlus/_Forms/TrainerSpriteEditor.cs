using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Helpers._MatchHelper;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Managers._UndoManager;
using PochiPochiEditorPlus._Utilities;
using PochiPochiEditorPlus._Utilities._QuickInput;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.TrainerSprite)]
    public partial class TrainerSpriteEditor : Form, IEditorRefresh
    {
        // 共有データ用
        private dynamic _sharedData = null;
        // 変更履歴用
        private CommandManager _commandManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各テーブル用
        private dynamic _imageEntry = null;
        private dynamic _paletteEntry = null;
        private dynamic _yPosEntry = null;
        private dynamic _animPointerEntry = null;
        // 可変長データ管理用
        private RefDataManager _imageData = null;
        private RefDataManager _paletteData = null;
        // UI制御用
        private int _currentSpriteIndex = 0;
        private int _entryCount = 0;
        // データ識別タグ用
        private enum SpriteData { Image, Palette }

        public TrainerSpriteEditor(SharedData sharedData, CommandManager commandManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _commandManager = commandManager;
            _eventBinder = new EventBinder();

            InitializeEntries();
            InitializeControls();
            InitializeEventHandlers();

            RefreshUI();
        }

        private void InitializeEntries()
        {
            // 画像テーブルを作成
            int tableOffset = _sharedData.Config.TrainerSpriteImageTableOffset;
            // エントリー数を計算
            var pattern = new List<TokenData>()
            {
                TokenData.Pointer(),
                TokenData.Exact(Constants.UShortSize, exactValues: new long[]{ 0x800, 0x1000 }),
                TokenData.Wildcard(1),
                TokenData.Exact(Constants.ByteSize, exactValues: 0x0)
            };
            _entryCount = PatternMatcher.TryCountByPattern(
                pattern,
                _sharedData.RomData,
                tableOffset);
            _imageEntry = 
                new EntryManager("TrainerSpriteImageEntry", tableOffset, _entryCount, _sharedData);

            // パレットテーブルを作成
            tableOffset = _sharedData.Config.TrainerSpritePaletteTableOffset;
            _paletteEntry = 
                new EntryManager("TrainerSpritePaletteEntry", tableOffset, _entryCount, _sharedData);

            // Y座標位置テーブルを作成
            tableOffset = _sharedData.Config.TrainerSpriteYPosTableOffset;
            _yPosEntry = 
                new EntryManager("TrainerSpriteYPosEntry", tableOffset, _entryCount, _sharedData);

            // アニメポインタテーブルを作成
            tableOffset = _sharedData.Config.TrainerSpriteAnimPointerTableOffset;
            _animPointerEntry = 
                new EntryManager("TrainerSpriteAnimationPointerEntry", tableOffset, _entryCount, _sharedData);
        }

        private void InitializeControls()
        {
            // nudSpriteIndexの最大値
            nudSpriteIndex.Maximum = _entryCount - 1;

            // タグ設定
            btnImportSpriteImage.Tag = SpriteData.Image;
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
                h => txtSpriteImageOffset.Validated += h,
                h => txtSpriteImageOffset.Validated -= h,
                (sender, e) =>
                {
                    // 入力されたアドレスを取得
                    var ctrl = (TextBox)sender;
                    var text = ctrl.Text;

                    // データを更新
                    var offsetValue = ConvHelper.ParseStringToInt(text);
                    var desc = $"[{this.Text}]画像アドレス(ID:{_currentSpriteIndex:D4})";

                    _imageEntry.Entries[_currentSpriteIndex].SpriteImageOffset
                        .UpdateData(_commandManager, offsetValue, desc);
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
                        .UpdateData(_commandManager, offsetValue, desc);
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
                        .UpdateData(_commandManager, value, desc);
                });

            // 画像インデックスnud
            _eventBinder.BindCtrl(
                h => nudSpriteIndex.ValueChanged += h,
                h => nudSpriteIndex.ValueChanged -= h,
                (_, __) =>
                {
                    var newIndex = (int)nudSpriteIndex.Value;
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
                                _imageData.BinaryData);
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
                h => btnImportSpriteImage.Click += h,
                h => btnImportSpriteImage.Click -= h,
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
            txtSpriteImageOffset.Text =
                ConvHelper.ParseIntToString(
                    _imageEntry.Entries[index].SpriteImageOffset.GetData<int>());
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
            var targetOffset =
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
            try
            {
                // 画像アドレスを取得
                var imageOffsetValue = txtSpriteImageOffset.Text.ParseStringToInt();
                var imageData = ImageHelper.DecompressLZ77(
                    _sharedData.RomData,
                    imageOffsetValue);
                // RefDataとして保持する
                var imageDataLz77 = ImageHelper.CompressLZ77(imageData);
                _imageData = new RefDataManager(
                    SpriteData.Image,
                    imageOffsetValue,
                    imageDataLz77,
                    _sharedData);

                // パレットアドレスを取得
                var paletteOffsetValue = txtSpritePaletteOffset.Text.ParseStringToInt();
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

                // 画像を生成
                var sprite = ImageHelper.CreateBitmap(
                    imageData,
                    paletteData,
                    Constants.SpriteSize,
                    Constants.SpriteSize,
                    showBackColor: true);
                // 2倍に拡大
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

            var inputResult = new QuickInputBuilder()
                .WithOffset(0x0)
                .WithFile(Constants.SpriteImportFilter)
                .ShowDialog();
            if (inputResult == null) return;

            // 入力値
            int newOffset = inputResult.Offset;
            string filePath = inputResult.Path;

            using (Bitmap bmp = new Bitmap(filePath))
            {
                // バイト配列を抽出
                if (!ImageHelper.ExtractImageAndPalette(
                    bmp,
                    Constants.SpriteSize,
                    Constants.SpriteSize,
                    out byte[] imageData,
                    out byte[] paletteData)) return;

                // タグを識別して、表示stringを設定
                bool isImage = importKind == SpriteData.Image;
                string targetName = isImage
                    ? "画像" 
                    : "パレット";
                string desc = 
                    $"[{this.Text}]{targetName}インポート(ID:{_currentSpriteIndex:D4})";

                // データを圧縮
                byte[] compressedData = isImage
                    ? ImageHelper.CompressLZ77(imageData)
                    : ImageHelper.CompressPalette(paletteData);

                // 更新対象を特定
                var targetEntry = isImage
                    ? _imageEntry.Entries[_currentSpriteIndex].SpriteImageOffset
                    : _paletteEntry.Entries[_currentSpriteIndex].SpritePaletteOffset;
                var targetData = isImage
                    ? _imageData 
                    : _paletteData;

                // コマンドの作成と登録
                var combine = new CombineCommands(desc);
                combine.Add(targetEntry.CreateUpdateCommand(newOffset, desc));
                combine.Add(targetData.CreateUpdateCommand(newOffset, compressedData, desc));

                if (combine.HasCommands)
                {
                    _commandManager.PushCommand(combine);
                }
            }
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshUI()
        {
            // 現在のインデックスを再読み込み
            LoadDataToUI(_currentSpriteIndex);
        }
    }
}
