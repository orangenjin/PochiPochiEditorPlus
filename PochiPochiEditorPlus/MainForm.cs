using System;
using System.Collections.Generic;
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
        private ConfigManager _configManager = null;
        private CharmapManager _charmapManager = null;
        // 変更履歴管理用
        private UndoManager _undoManager = new UndoManager();

        // パス用
        private string _romPath = string.Empty;
        private string _iniFolder = Path.Combine(Application.StartupPath, Constants.IniExt);
        private string _tblPath = Path.Combine(Application.StartupPath, "charmap.tbl");

        // 保存形式識別用
        private enum SaveMode{ SaveOver, SaveAs }

        public MainForm()
        {
            InitializeComponent();

            // 共有データを初期化
            _configManager = new ConfigManager(_iniFolder);
            _charmapManager = new CharmapManager(_tblPath);
            _sharedData = new SharedData(_configManager, _charmapManager);

            // タグ付加
            tsmiSaveOver.Tag = SaveMode.SaveOver;
            tsmiSaveAs.Tag = SaveMode.SaveAs;

            // イベントハンドラの登録
            InitializeEventHandlers();

            // UI状態の更新
            MainFormUIUpdate();
        }

        private void InitializeEventHandlers()
        {
            // 読み込み関連
            _eventBinder.BindCtrl(
                h => tsmiLoadRom.Click += h,
                h => tsmiLoadRom.Click -= h,
                (_, __) =>
                {
                    using (var ofd = new OpenFileDialog())
                    {
                        // 設定名を保持
                        var configKeys = _configManager.Configs.Keys.ToList();

                        // フィルターを作成
                        ofd.Filter = string.Join(
                            "|",
                            configKeys.Select(key => $"{key}|*.{Constants.GbaExt}"));

                        // キャンセルを確認
                        if (ofd.ShowDialog() != DialogResult.OK) return;

                        // ROM読み込み
                        _romPath = ofd.FileName;
                        _sharedData.LoadRom(File.ReadAllBytes(_romPath));

                        // 選択された設定を取得
                        int selectedIndex = ofd.FilterIndex - 1;
                        string selectedKey = configKeys[selectedIndex];
                        _sharedData.Config.LoadConfig(selectedKey, _sharedData.RomData);

                        // UI更新
                        MainFormUIUpdate();
                    }
                });
            _eventBinder.BindCtrl(
                h => tsmiClearRom.Click += h,
                h => tsmiClearRom.Click -= h,
                (_, __) =>
                {
                    // Rom情報を更新
                    _romPath = string.Empty;
                    _sharedData.ClearRom();

                    // 変更履歴をクリア
                    _undoManager.Clear();

                    // UIの状態を更新
                    MainFormUIUpdate();
                });

            // 保存関連
            _eventBinder.BindCtrl(
                h => tsmiSaveOver.Click += h,
                h => tsmiSaveOver.Click -= h,
                SaveButton_Click);
            _eventBinder.BindCtrl(
                h => tsmiSaveAs.Click += h,
                h => tsmiSaveAs.Click -= h,
                SaveButton_Click);

            // 各エディタ用
            foreach (Button btn in grpEditors.Controls)
            {
                _eventBinder.BindCtrl(
                    h => btn.Click += h,
                    h => btn.Click -= h,
                    EditorButton_Click);
            }

            // 変更履歴関連
            _eventBinder.BindCtrl(
                h => _undoManager.StateChanged += h,
                h => _undoManager.StateChanged -= h,
                (_, __) =>
                {
                    MainFormUIUpdate();
                    UpdateHistoryList();
                    _formGroupManager?.RefreshForms();
                });
            _eventBinder.BindCtrl(
                h => tsmiUndo.Click += h,
                h => tsmiUndo.Click -= h,
                (_, __) =>
                {
                    _undoManager.Undo();
                });
            _eventBinder.BindCtrl(
                h => tsmiRedo.Click += h,
                h => tsmiRedo.Click -= h,
                (_, __) =>
                {
                    _undoManager.Redo();
                });
            _eventBinder.BindCustom(
                () => lstHistory.DrawItem += lstHistory_DrawItem,
                () => lstHistory.DrawItem -= lstHistory_DrawItem);
            _eventBinder.BindCtrl(
                h => lstHistory.Click += h,
                h => lstHistory.Click -= h,
                (_, __) =>
                {
                    int index = lstHistory.SelectedIndex;

                    if (index < 0) return;
                    _undoManager.MoveTo(index + 1);
                });

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void MainFormUIUpdate()
        {
            // 現在の状態を整理
            bool isRomLoaded = _sharedData.IsRomLoaded;
            bool isEditorOpen = _formGroupManager != null;

            // 読み込み前、エディタ起動前
            bool canLoadConfig = !isRomLoaded && !isEditorOpen;
            tsmiLoadRom.Enabled = canLoadConfig;

            // 読み込み後、エディタ起動前
            bool canOpenEditor = isRomLoaded && !isEditorOpen;
            tsmiClearRom.Enabled = canOpenEditor;
            CtrlHelper.SetControlsEnabled(grpEditors, canOpenEditor);
            tsmiSave.Enabled = canOpenEditor;

            // 読み込み後、エディタ起動後
            CtrlHelper.SetControlsEnabled(grpHistory, isRomLoaded);
            tsmiEdit.Enabled = canOpenEditor;
            tsmiTool.Enabled = canOpenEditor;
        }

        private void EditorButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button button)) return;

            // "btn" を外す
            string groupName = button.Name.Substring(Constants.ButtonPrefix.Length);

            // グループ名を取得
            if (!Enum.TryParse(groupName, out FormGroup group)) return;

            // フォーム生成
            _formGroupManager = new FormGroupManager(this, group, _sharedData, _undoManager);
            _formGroupManager.Closed += (_, __) =>
            {
                _formGroupManager = null;
                MainFormUIUpdate();
            };
            _formGroupManager.ShowFormGroup();

            MainFormUIUpdate();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!((sender as ToolStripMenuItem).Tag is SaveMode mode)) return;

            if (mode == SaveMode.SaveOver) // 上書き保存
            {
                SaveRom(_romPath);
            }
            else if (mode == SaveMode.SaveAs) // 名前を付けて保存
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = Constants.RomFileFilter;
                    saveFileDialog.FileName = Path.GetFileName(_romPath);

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        SaveRom(saveFileDialog.FileName);
                        _romPath = saveFileDialog.FileName;
                    }
                }
            }

            // 保存処理メソッド
            void SaveRom(string path)
            {
                try
                {
                    File.WriteAllBytes(path, _sharedData.RomData);
                    MessageBox.Show(
                        "保存に成功しました。",
                        "保存完了",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "保存エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateHistoryList()
        {
            try
            {
                lstHistory.BeginUpdate();
                lstHistory.Items.Clear();

                foreach (var command in _undoManager.History)
                {
                    lstHistory.Items.Add(command);
                }
            }
            finally
            {
                lstHistory.EndUpdate();
                lstHistory.Invalidate();
            }
        }

        private void lstHistory_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var command = _undoManager.History[e.Index];
            bool isFuture = e.Index >= _undoManager.CurrentIndex;

            e.DrawBackground();
            Color textColor = GetHistoryTextColor();

            using (var brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(
                    command.Desc,
                    e.Font,
                    brush,
                    e.Bounds);
            }

            e.DrawFocusRectangle();

            // 色処理ヘルパー
            Color GetHistoryTextColor()
            {
                if (!isFuture) return lstHistory.ForeColor;

                Color baseColor = lstHistory.ForeColor;
                Color backColor = lstHistory.BackColor;

                // 淡色化
                return Color.FromArgb(
                    (baseColor.R + backColor.R) / 2,
                    (baseColor.G + backColor.G) / 2,
                    (baseColor.B + backColor.B) / 2);
            }
        }
    }
}
