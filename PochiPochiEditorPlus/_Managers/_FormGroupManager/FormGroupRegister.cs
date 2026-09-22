using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._FormGroupManager
{
    public sealed class FormGroupRegister
    {
        // フォームグループ内でのデータのやり取り用
        public FormGroupData GroupData { get; set; }
        // メイン画面のUI状態更新用
        public EventHandler Closed { get; set; }

        private Form _ownerForm = null;
        private List<Form> _forms = null;

        public FormGroupRegister(
            Form ownerForm,
            FormGroup group,
            SharedData sharedData,
            UndoManager undoManager)
        {
            _ownerForm = ownerForm;
            _forms = new List<Form>();

            // グループと順番を判定
            var formInfos = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Form).IsAssignableFrom(t))
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<FormGroupAttribute>()
                })
                .Where(x => x.Attribute?.Group == group)
                .OrderBy(x => x.Attribute.Order)
                .ToList();

            // グループ内に複数のフォームがある場合
            if (formInfos.Any(x => x.Attribute.Order >= 0))
            {
                GroupData = new FormGroupData();
                GroupData.RefreshRequested += RefreshForms;
            }

            // フォーム作成
            foreach (var info in formInfos)
            {
                var form = GroupData != null
                    ? (Form)Activator.CreateInstance(info.Type, sharedData, undoManager, GroupData)
                    : (Form)Activator.CreateInstance(info.Type, sharedData, undoManager);

                form.FormClosed += SingleForm_FormClosed;
                _forms.Add(form);
            }
        }

        public void ShowFormGroup()
        {
            foreach (var form in _forms)
            {
                form.Show(_ownerForm);
            }
        }

        /// <summary>
        /// 同じフォームグループを閉じるようにする。
        /// </summary>
        private void SingleForm_FormClosed(object sender, EventArgs e)
        {
            foreach (var form in _forms)
            {
                if (ReferenceEquals(form, sender)) continue; // 既に閉じている

                if (!form.IsDisposed)
                {
                    form.FormClosed -= SingleForm_FormClosed;
                    form.Close();
                }
            }
            _forms.Clear();
            _forms = null;
            GroupData?.ClearDict();
            GroupData = null;

            // 呼び出し元フォームを前に出す
            _ownerForm.BringToFront();
            Closed.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 各エディタのUI再描画を行う。
        /// 引数で特定のフォームを除外可能。
        /// </summary>
        public void RefreshForms(object sender = null, Form excludeForm = null)
        {
            foreach (var form in _forms)
            {
                // 念のため
                if (form.IsDisposed) continue;

                // 除外対象をスキップ、指定しない場合ループするかも
                if (excludeForm != null && ReferenceEquals(form, excludeForm)) continue;

                if (form is IEditorRefresh refreshable)
                {
                    refreshable.RefreshUI();
                }
            }
        }
    }
}
