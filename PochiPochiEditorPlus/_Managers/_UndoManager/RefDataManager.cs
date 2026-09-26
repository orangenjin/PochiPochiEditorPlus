using System;
using System.Linq;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._CommandManager
{
    /// <summary>
    /// フィールド値でない可変長データを扱う。
    /// </summary>
    public sealed class RefDataManager
    {
        public Enum Name { get; }
        public int Offset { get; set; }
        public byte[] BinaryData { get; set; }

        // 共有データ用
        private SharedData _sharedData = null;

        public RefDataManager(
            Enum enumKey,
            int offset,
            byte[] binaryData,
            SharedData sharedData)
        {
            Name = enumKey;
            _sharedData = sharedData;

            SetData(offset, binaryData);
        }

        public void SetData(int offset, byte[] binaryData)
        {
            Offset = offset;
            BinaryData = binaryData;
        }

        public void WriteData(int offset, byte[] binaryData)
        {
            IoHelper.WriteBytesToData(
                _sharedData.RomData,
                offset,
                binaryData);
        }

        public void UpdateData(
            CommandManager commandManager,
            int newOffset,
            byte[] newBinaryData,
            string desc)
        {
            var command = CreateUpdateCommand(
                newOffset,
                newBinaryData,
                desc);

            if (command != null)
            {
                commandManager.PushCommand(command);
            }
        }

        /// <summary>
        /// コマンドを生成する。
        /// </summary>
        public ICommand CreateUpdateCommand(
            int newOffset,
            byte[] newBinaryData,
            string desc)
        {
            int oldOffset = Offset;
            byte[] oldBinaryData = (byte[])BinaryData.Clone();

            // 同一なら無視
            if (oldOffset == newOffset &&
                oldBinaryData.SequenceEqual(newBinaryData)) return null;

            // 新しい書き込み先の変更前データ
            byte[] oldTargetData = new byte[newBinaryData.Length];
            Array.Copy(
                _sharedData.RomData,
                newOffset,
                oldTargetData,
                0,
                newBinaryData.Length);

            // データを更新
            WriteData(newOffset, newBinaryData);
            SetData(newOffset, newBinaryData);

            return new RefDataChangeCommand(
                this,
                oldOffset,
                oldBinaryData,
                newOffset,
                newBinaryData,
                oldTargetData,
                desc);
        }
    }
}
