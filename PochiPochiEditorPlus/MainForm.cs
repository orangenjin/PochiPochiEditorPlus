using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus
{
    public partial class MainForm : Form
    {
        // フォーム同時起動用
        private FormGroupManager _formGroupManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = new EventBinder();
        // 共有データ用
        private SharedData _sharedData = null;
        // 変更履歴管理用
        private UndoManager _undoManager = new UndoManager();

        public MainForm()
        {
            InitializeComponent();

            // 設定名のコンボボックスの初期化が必要
            var config = new IniManager(_iniFolder, cmbConfig);
            // 現在言語変更できない
            var charmap = new TblManager(_tblPath);
            _sharedData = new SharedData(config, charmap);

            InitializeControls();
            InitializeEventHandlers();

            // UI状態の更新
            MainFormUIUpdate();
        }
    }
}
