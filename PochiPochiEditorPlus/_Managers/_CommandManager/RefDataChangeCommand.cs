namespace PochiPochiEditorPlus._Managers._CommandManager
{
    public sealed class RefDataChangeCommand : ICommand
    {
        private RefDataManager _refData = null;

        // RefDataの変更前
        private int _oldOffset = default;
        private byte[] _oldData = null;

        // RefDataの変更後
        private int _newOffset = default;
        private byte[] _newData = null;

        // newOffsetに元々存在していたデータ
        private byte[] _oldTargetData = null;

        public string Desc { get; }

        public RefDataChangeCommand(
            RefDataManager refData,
            int oldOffset,
            byte[] oldData,
            int newOffset,
            byte[] newData,
            byte[] oldTargetData,
            string desc)
        {
            _refData = refData;

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
            _refData.WriteData(_newOffset, _oldTargetData);

            // RefDataを元に戻す
            _refData.SetData(_oldOffset, _oldData);
        }

        public void Redo()
        {
            // 新しいデータを書き戻す
            _refData.WriteData(_newOffset, _newData);

            // RefDataを新しい状態にする
            _refData.SetData(_newOffset, _newData);
        }
    }
}
