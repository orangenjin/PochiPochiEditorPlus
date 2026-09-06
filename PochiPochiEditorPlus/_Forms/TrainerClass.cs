using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.TrainerClass)]
    public partial class TrainerClass : Form, IEditorRefresh
    {
        // 共有データ用
        private SharedData _sharedData = null;
        private dynamic _dynamicConfig = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各エントリーテーブル用
        private dynamic _classNameEntry = null;
        private dynamic _prizeMultiEntry = null;
        private dynamic _encMusicEntry = null;
        private dynamic _battleMusicEntry = null;
        private dynamic _pokeBallEntry = null;
        private dynamic _baseIvEntry = null;
        // UI制御用
        private int _currentClassIndex = 0;
        // 追加データ判定用
        private bool _isEncounterMusicEnabled = false;
        private bool _isBattleMusicEnabled = false;
        private bool _isPokeBallEnabled = false;
        private bool _isBaseIvEnabled = false;

        public TrainerClass(SharedData sharedData, UndoManager undoManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _dynamicConfig = _sharedData.Config;
            _undoManager = undoManager;
            _eventBinder = new EventBinder();

            InitializeEntries();
            InitializeControls();
            InitializeEventHandlers();

            LoadDataToUI(_currentClassIndex);
        }

        private void InitializeEntries()
        {
            // 肩書名テーブルを作成
            int tableOffset = (int)_dynamicConfig.TrainerClassNameTableOffset;
            int entrycount = (int)_dynamicConfig.TrainerClassNameCount;
            _classNameEntry = 
                new EntryManager("TrainerClassNameEntry", tableOffset, entrycount, _sharedData);

            // 追加データのbool判定
            _isEncounterMusicEnabled = (bool)_dynamicConfig.EnableTrainerClassEncMusic;
            _isBattleMusicEnabled = (bool)_dynamicConfig.EnableTrainerClassBattleMusic;
            _isPokeBallEnabled = (bool)_dynamicConfig.EnableTrainerClassPokeBall;
            _isBaseIvEnabled = (bool)_dynamicConfig.EnableTrainerClassBaseIV;

            if (_isEncounterMusicEnabled)
            {
                // 戦闘前BGMテーブルを作成
                tableOffset = (int)_dynamicConfig.TrainerClassEncMusicTableOffset;
                _encMusicEntry = 
                    new EntryManager("TrainerClassEncMusicEntry", tableOffset, entrycount, _sharedData);
            }

            if (_isBattleMusicEnabled)
            {
                // 戦闘中BGMテーブルを作成
                tableOffset = (int)_dynamicConfig.TrainerClassBattleMusicTableOffset;
                _battleMusicEntry = 
                    new EntryManager("TrainerClassBattleMusicEntry", tableOffset, entrycount, _sharedData);
            }

            if (_isPokeBallEnabled)
            {
                // 使用ボールIDテーブルを作成
                tableOffset = (int)_dynamicConfig.TrainerClassPokeBallTableOffset;
                _pokeBallEntry = 
                    new EntryManager("TrainerClassPokeBallEntry", tableOffset, entrycount, _sharedData);
            }

            if (_isBaseIvEnabled)
            {
                // 基礎個体値テーブルを作成
                tableOffset = (int)_dynamicConfig.TrainerClassBaseIVTableOffset;
                _baseIvEntry = 
                    new EntryManager("TrainerClassBaseIvEntry", tableOffset, entrycount, _sharedData);
            }

            // 賞金倍率テーブルを作成
            tableOffset = (int)_dynamicConfig.TrainerClassPrizeMultiTableOffset;
            entrycount = (int)_dynamicConfig.TrainerClassPrizeMultiCount;
            _prizeMultiEntry = 
                new EntryManager("TrainerClassPrizeMultiEntry", tableOffset, entrycount, _sharedData);
        }

        private void InitializeControls()
        {
            UpdateClassNameComboBox();

            // 追加データのctrlの無効化
            if (!_isEncounterMusicEnabled)
            {
                lblEncMusicIndex.Enabled = false;
                nudEncMusicIndex.Enabled = false;
            }

            if (!_isBattleMusicEnabled)
            {
                lblBattleMusicIndex.Enabled = false;
                nudBattleMusicIndex.Enabled = false;
            }

            if (!_isPokeBallEnabled)
            {
                lblPokeBallIndex.Enabled = false;
                nudPokeBallIndex.Enabled = false;
            }

            if (!_isBaseIvEnabled)
            {
                lblBaseIvValue.Enabled = false;
                nudBaseIvValue.Enabled = false;
            }

            // txtClassNameStr
            txtClassNameStr.CharmapManager = _sharedData.Charmap;
            txtClassNameStr.AllowedLength = (int)_dynamicConfig.TrainerClassNameEntryLength;
            txtClassNameStr.NeedTerminator = true;
        }

        private void UpdateClassNameComboBox()
        {
            try
            {
                cmbClassNameIndex.BeginUpdate();
                cmbClassNameIndex.Items.Clear();

                foreach (var entry in _classNameEntry.Entries)
                {
                    string className = entry.ClassNameStr.GetData<string>();
                    cmbClassNameIndex.Items.Add(className);
                }
            }
            finally
            {
                cmbClassNameIndex.EndUpdate();
            }

            // 再選択
            if (cmbClassNameIndex.Items.Count > 0)
            {
                cmbClassNameIndex.SelectedIndex = _currentClassIndex;
            }
        }

        private void InitializeEventHandlers()
        {
            // 肩書き名コンボボックス
            _eventBinder.BindCtrl(
                h => cmbClassNameIndex.SelectedIndexChanged += h,
                h => cmbClassNameIndex.SelectedIndexChanged -= h,
                (_, __) =>
                {
                    int newIndex = cmbClassNameIndex.SelectedIndex;
                    LoadDataToUI(newIndex);
                });

            // 肩書き名テキストボックス
            _eventBinder.BindCtrl(
                h => txtClassNameStr.Validated += h,
                h => txtClassNameStr.Validated -= h,
                (sender, e) =>
                {
                    // 入力されたテキストを取得
                    var ctrl = (TextBox)sender;
                    var text = ctrl.Text;

                    // データを更新
                    var desc = $"[{this.Text}]肩書き名(ID:{_currentClassIndex:D4})";
                    _classNameEntry.Entries[_currentClassIndex].ClassNameStr
                        .UpdateData(_undoManager, text, desc);
                });

            // 賞金倍率
            _eventBinder.BindCtrl(
                h => nudClassPrizeMultiValue.ValueChanged += h,
                h => nudClassPrizeMultiValue.ValueChanged -= h,
                (sender, e) =>
                {
                    // 入力値を取得
                    var ctrl = (NumericUpDown)sender;
                    var value = ctrl.Value;

                    // 対象のインデックスを計算
                    var calcIndex = CalcPrizeMultiIndex(_currentClassIndex);

                    // データを更新
                    var desc = $"[{this.Text}]賞金倍率(ID:{_currentClassIndex:D4})";
                    _prizeMultiEntry.Entries[calcIndex].ClassPrizeMultiValue
                        .UpdateData(_undoManager, value, desc);
                });

            // 追加データ関連
            if (_isEncounterMusicEnabled)
            {
                _eventBinder.BindCtrl(
                    h => nudEncMusicIndex.ValueChanged += h,
                    h => nudEncMusicIndex.ValueChanged -= h,
                    (sender, e) =>
                    {
                        // 入力値を取得
                        var ctrl = (NumericUpDown)sender;
                        var value = ctrl.Value;

                        // データを更新
                        var desc = $"[{this.Text}]戦闘前BGM(ID:{_currentClassIndex:D4})";
                        _encMusicEntry.Entries[_currentClassIndex].EncounterMusicIndex
                            .UpdateData(_undoManager, value, desc);
                    });
            }

            if (_isBattleMusicEnabled)
            {
                _eventBinder.BindCtrl(
                    h => nudBattleMusicIndex.ValueChanged += h,
                    h => nudBattleMusicIndex.ValueChanged -= h,
                    (sender, e) =>
                    {
                        // 入力値を取得
                        var ctrl = (NumericUpDown)sender;
                        var value = ctrl.Value;

                        // データを更新
                        var desc = $"[{this.Text}]戦闘中BGM(ID:{_currentClassIndex:D4})";
                        _battleMusicEntry.Entries[_currentClassIndex].BattleMusicIndex
                            .UpdateData(_undoManager, value, desc);
                    });
            }

            if (_isPokeBallEnabled)
            {
                _eventBinder.BindCtrl(
                    h => nudPokeBallIndex.ValueChanged += h,
                    h => nudPokeBallIndex.ValueChanged -= h,
                    (sender, e) =>
                    {
                        // 入力値を取得
                        var ctrl = (NumericUpDown)sender;
                        var value = ctrl.Value;

                        // データを更新
                        var desc = $"[{this.Text}]使用ボールID(ID:{_currentClassIndex:D4})";
                        _pokeBallEntry.Entries[_currentClassIndex].PokeBallIndex
                            .UpdateData(_undoManager, value, desc);
                    });
            }

            if (_isBaseIvEnabled)
            {
                _eventBinder.BindCtrl(
                    h => nudBaseIvValue.ValueChanged += h,
                    h => nudBaseIvValue.ValueChanged -= h,
                    (sender, e) =>
                    {
                        // 入力値を取得
                        var ctrl = (NumericUpDown)sender;
                        var value = ctrl.Value;

                        // データを更新
                        var desc = $"[{this.Text}]基礎個体値(ID:{_currentClassIndex:D4})";
                        _baseIvEntry.Entries[_currentClassIndex].BaseIvValue
                            .UpdateData(_undoManager, value, desc);
                    });
            }

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void LoadDataToUI(int index)
        {
            // インデックス更新
            _currentClassIndex = index;
            cmbClassNameIndex.SelectedIndex = index;
            nudClassNameIndex.Value = (decimal)index;

            // クラス名
            txtClassNameStr.Text =
                _classNameEntry.Entries[index].ClassNameStr.GetData<string>();

            // 賞金倍率、インデックス計算あり
            int calcIndex = CalcPrizeMultiIndex(index);
            nudClassPrizeMultiValue.Value =
                (decimal)_prizeMultiEntry.Entries[calcIndex].ClassPrizeMultiValue.GetData<int>();

            // 追加データ
            if (_isEncounterMusicEnabled)
            {
                nudEncMusicIndex.Value =
                    (decimal)_encMusicEntry.Entries[index].EncounterMusicIndex.GetData<int>();
            }

            if (_isBattleMusicEnabled)
            {
                nudBattleMusicIndex.Value =
                    (decimal)_battleMusicEntry.Entries[index].BattleMusicIndex.GetData<int>();
            }

            if (_isPokeBallEnabled)
            {
                nudPokeBallIndex.Value =
                    (decimal)_pokeBallEntry.Entries[index].PokeBallIndex.GetData<int>();
            }

            if (_isBaseIvEnabled)
            {
                nudBaseIvValue.Value =
                    (decimal)_baseIvEntry.Entries[index].BaseIvValue.GetData<int>();
            }
        }

        /// <summary>
        /// 賞金倍率のインデックスを計算する。
        /// </summary>
        private int CalcPrizeMultiIndex(int index)
        {
            int fallbackIndex = 0;
            var entries = _prizeMultiEntry.Entries;

            for (int i = 0; i < entries.Count; i++)
            {
                int currentClassNameIndex = entries[i].ClassNameIndex.GetData<int>();

                // 存在する場合
                if (currentClassNameIndex == index)
                {
                    return i;
                }

                // 0xFFである場合インデックスを保持
                if (currentClassNameIndex == 0xFF)
                {
                    fallbackIndex = i;
                }
            }

            return fallbackIndex;
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshFromData()
        {
            // 現在のインデックスを再読み込み
            LoadDataToUI(_currentClassIndex);

            UpdateClassNameComboBox();
        }
    }
}
