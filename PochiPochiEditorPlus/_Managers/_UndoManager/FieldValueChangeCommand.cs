using PochiPochiEditorPlus._Managers._FieldManager;

namespace PochiPochiEditorPlus._Managers._CommandManager
{
    public sealed class FieldValueChangeCommand : ICommand
    {
        private FieldValueHolder _target = null;
        private byte[] _oldData = null;
        private byte[] _newData = null;

        public string Desc { get; }

        public FieldValueChangeCommand
            (FieldValueHolder target,
            byte[] oldData,
            byte[] newData,
            string description)
        {
            _target = target;
            _oldData = (byte[])oldData.Clone(); // 参照を切るためにCloneする
            _newData = (byte[])newData.Clone();
            Desc = description;
        }

        public void Undo()
        {
            _target.BinaryData = _oldData;
        }

        public void Redo()
        {
            _target.BinaryData = _newData;
        }
    }
}
