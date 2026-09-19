using System;

namespace PochiPochiEditorPlus._Managers._FormGroupManager
{
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
        public FormGroupAttribute(FormGroup group, int order = -1)
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
