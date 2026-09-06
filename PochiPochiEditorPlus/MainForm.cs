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
        // private UndoManager _undoManager = new UndoManager();

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
