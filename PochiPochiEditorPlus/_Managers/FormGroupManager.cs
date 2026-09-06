using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers
{
    public class FormGroupManager
    {
        private Form _ownerForm = null;
        private List<Form> _forms = null;

        // メイン画面のUI状態更新用
        public EventHandler Closed = null;

        public FormGroupManager(
            Form ownerForm,
            FormGroup group,
            SharedData sharedData,
            UndoManager undoManager)
        {
            _ownerForm = ownerForm;
            _forms = new List<Form>();

            // グループと順番判定
            var formTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Form).IsAssignableFrom(t))
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<FormGroupAttribute>()
                })
                .Where(x => x.Attribute?.Group == group)
                .OrderBy(x => x.Attribute.Order)
                .Select(x => x.Type);

            // フォーム作成
            foreach (var type in formTypes)
            {
                var form = (Form)Activator.CreateInstance(type, sharedData, undoManager);

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

            // 呼び出し元フォームを前に出す
            _ownerForm.BringToFront();
            Closed.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Undo, Redo時、各エディタのUI再描画を行う。
        /// </summary>
        public void RefreshForms()
        {
            foreach (var form in _forms)
            {
                // 念のため
                if (form.IsDisposed) continue;

                if (form is IEditorRefresh refreshable)
                {
                    refreshable.RefreshFromData();
                }
            }
        }
    }

    /// <summary>
    /// Undo, Redo時にUIを再描画するため。
    /// </summary>
    public interface IEditorRefresh
    {
        void RefreshFromData();
    }

    /// <summary>
    /// 属するフォームグループと、表示の順番を指定する。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FormGroupAttribute : Attribute
    {
        public FormGroup Group { get; }
        public int Order { get; }

        // 順番がどうでもいい時はデフォルト値
        public FormGroupAttribute(FormGroup group, int order = 0)
        {
            Group = group;
            Order = order;
        }
    }

    /// <summary>
    /// フォームグループの種類を定義する。
    /// </summary>
    public enum FormGroup
    {
        Map,
        Tileset,

        TrainerClass,
        TrainerSprite
    }
}
