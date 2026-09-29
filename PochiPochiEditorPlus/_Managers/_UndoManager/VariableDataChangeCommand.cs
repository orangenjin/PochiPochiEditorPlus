namespace PochiPochiEditorPlus._Managers._UndoManager
{
    public sealed class VariableDataChangeCommand : ICommand
    {
        public string Desc { get; }

        private VariableDataManager _varData = null;

        // VariableDataの変更前
        private int _oldOffset = 0;
        private byte[] _oldData = null;

        // VariableDataの変更後
        private int _newOffset = 0;
        private byte[] _newData = null;

        // newOffsetに元々存在していたデータ
        private byte[] _oldTargetData = null;

        public VariableDataChangeCommand(
            VariableDataManager varData,
            int oldOffset,
            byte[] oldData,
            int newOffset,
            byte[] newData,
            byte[] oldTargetData,
            string desc)
        {
            _varData = varData;

            _oldOffset = oldOffset;
            _oldData = (byte[])oldData.Clone(); // 参照を切るためにCloneする

            _newOffset = newOffset;
            _newData = (byte[])newData.Clone();

            _oldTargetData = oldTargetData;

            Desc = desc;
        }

        public void Undo()
        {
            // 新しい書き込み先を元に戻す
            _varData.WriteData(_newOffset, _oldTargetData);

            // VariableDataを元に戻す
            _varData.SetData(_oldOffset, _oldData);
        }

        public void Redo()
        {
            // 新しいデータを書き戻す
            _varData.WriteData(_newOffset, _newData);

            // VariableDataを新しい状態にする
            _varData.SetData(_newOffset, _newData);
        }
    }
}
