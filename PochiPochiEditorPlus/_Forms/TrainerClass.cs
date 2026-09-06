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
    public partial class TrainerClass : Form
    {
        // 共有データ用
        private SharedData _sharedData = null;
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
            _undoManager = undoManager;

            InitializeEntries();
            InitializeControls();
            // InitializeEventHandlers();

            // LoadDataToUI(_currentClassIndex);
        }

        private void InitializeEntries()
        {
            dynamic config = _sharedData.Config;

            // 肩書名テーブルを作成
            int tableOffset = config.TrainerClassNameTableOffset;
            int entrycount = config.TrainerClassNameCount;
            _classNameEntry = new EntryManager("TrainerClassNameEntry", tableOffset, entrycount, _sharedData);

            // 賞金倍率テーブルを作成
            tableOffset = config.TrainerClassPrizeMultiTableOffset;
            entrycount = config.TrainerClassPrizeMultiCount;
            _prizeMultiEntry = new EntryManager("TrainerClassPrizeMultiEntry", tableOffset, entrycount, _sharedData);

            nudClassPrizeMultiValue.Value = _prizeMultiEntry.Entries[3].PrizeMultiValue.GetData<int>();

            txtClassNameStr.Text = _classNameEntry.Entries[0].ClassNameStr.GetData<string>();
        }







        private void InitializeControls()
        {
            // 肩書き名のテキストボックス設定
            // txtClassNameStr.CharmapManager = _sharedData.Charmap;
            // txtClassNameStr.AllowedLength = 8;
            // txtClassNameStr.NeedTerminator = false;
        }

    }
}
