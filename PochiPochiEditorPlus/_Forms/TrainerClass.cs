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
        // イベント登録・解除用
        private EventBinder _eventBinder = new EventBinder();
        // 共有データ用
        private SharedData _sharedData = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // 各テーブル用
        // private EntryManager _classNameEntry = null;
        // private EntryManager _prizeMultiEntry = null;
        // private EntryManager _encMusicEntry = null;
        // private EntryManager _battleMusicEntry = null;
        // private EntryManager _pokeBallEntry = null;
        // private EntryManager _baseIvEntry = null;
        // 追加データ判定用
        private bool _isEncounterMusicEnabled = false;
        private bool _isBattleMusicEnabled = false;
        private bool _isPokeBallEnabled = false;
        private bool _isBaseIvEnabled = false;
        // UI制御用
        private int _currentClassIndex = default;

        public TrainerClass(SharedData sharedData, UndoManager undoManager)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _undoManager = undoManager;

            // InitializeEntries();
            // InitializeControls();
            // InitializeEventHandlers();

            // LoadDataToUI(_currentClassIndex);
        }
    }
}
