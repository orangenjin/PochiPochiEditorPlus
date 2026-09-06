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
        // 各テーブル用
        // private EntryManager _classNameEntry = null;
        // private EntryManager _prizeMultiEntry = null;
        // private EntryManager _encMusicEntry = null;
        // private EntryManager _battleMusicEntry = null;
        // private EntryManager _pokeBallEntry = null;
        // private EntryManager _baseIvEntry = null;
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

            // InitializeEntries();
            InitializeControls();
            // InitializeEventHandlers();

            // LoadDataToUI(_currentClassIndex);
        }









        private void InitializeControls()
        {
            // 肩書き名のテキストボックス設定
            txtClassName.CharmapManager = _sharedData.Charmap;
            txtClassName.AllowedLength = 8;
            txtClassName.NeedTerminator = false;
        }

    }
}
