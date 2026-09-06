using System;
using System.Linq;
using PochiPochiEditorPlus._Managers._CommandManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._FieldManager
{
    public sealed class FieldValueHolder
    {
        public string FieldName { get; }
        public int Offset { get; set; }
        public FieldLength Lengths { get; }
        public StateValue State { get; }
        public int ArgCount { get; }
        public byte[] BinaryData
        {
            get
            {
                var length = Lengths.EntryLength;
                byte[] data = new byte[length];

                // RomDataから直接現在のデータを取得
                Array.Copy(_sharedData.RomData, Offset, data, 0, length);
                return data;
            }
            set
            {
                Array.Copy(value, 0, _sharedData.RomData, Offset, Lengths.EntryLength);
            }
        }

        /// <summary>
        /// エントリー長と入力可能な長さ(StringAttr用)の情報を保持する。
        /// </summary>
        public class FieldLength
        {
            public int EntryLength { get; set; }
            public int AllowedLength { get; set; }
        }

        /// <summary>
        /// 読み取り方法を定義する。
        /// </summary>
        public enum StateValue
        {
            Normal,
            Signed,
            Pointer
        }

        // 共有データを保持する
        private SharedData _sharedData = null;

        /// <summary>
        /// FieldMetaDataReaderで読み込んだ定義情報からコンテナを作成する。(ループ処理前提)
        /// </summary>
        public FieldValueHolder(FieldMetaData metaData, SharedData sharedData)
        {
            // 後で使用するので保持
            _sharedData = sharedData;

            // フィールド名を代入
            FieldName = metaData.Name;

            // 長さを仮置きする
            Lengths = new FieldLength();
            Lengths.EntryLength = metaData.Field.GetFieldSize();
            Lengths.AllowedLength = Constants.InvalidValue;

            // 属性の種類を確認
            foreach (var attr in metaData.Attrs)
            {
                // 高々1つと想定しているのでbreakする
                switch (attr.Kind)
                {
                    case AttrKind.StringAttr:
                        // 第2引数(AllowedLengthArg)がない場合がある
                        ArgCount = Math.Min(
                            attr.Args.Length,
                            AttrKind.StringAttr.GetAttrCount()); // 引数の最大値と比較
                        int[] lengths = new int[ArgCount];

                        dynamic config = _sharedData.Config;
                        for (int i = 0; i < ArgCount; i++)
                        {
                            // 属性引数名はiniで定義名と同じ
                            lengths[i] = (int)config[attr.Args[i]];
                        }

                        // AllowedLengthArgが存在しない場合、同値を入れる
                        Lengths.EntryLength = lengths[(int)StringAttrArgs.EntryLengthArg];
                        Lengths.AllowedLength = lengths.Length > 1
                            ? lengths[(int)StringAttrArgs.AllowedLengthArg]
                            : lengths[(int)StringAttrArgs.EntryLengthArg];
                        break;

                    case AttrKind.NibbleAttr:
                        ArgCount = AttrKind.NibbleAttr.GetAttrCount();
                        break;

                    case AttrKind.BitAttr:
                        ArgCount = AttrKind.BitAttr.GetAttrCount();
                        break;
                }
            }

            // 読み取り方法を取得
            var isSigned = metaData.Field.IsSigned();
            var isPointer = metaData.Field.IsPointer();
            State = isPointer
                ? StateValue.Pointer
                : (isSigned
                     ? StateValue.Signed
                     : StateValue.Normal);
        }

        /// <summary>
        /// BinaryDataから型Tの値を取得する。
        /// </summary>
        public T GetData<T>(
            int argIndex = 0,
            Func<FieldValueHolder, int, CharmapManager, T> converter = null)
        {
            // 通常の型Tで対応できない特殊処理があれば渡す
            return converter != null
                ? converter(this, argIndex, _sharedData.Charmap)
                : FieldValueConv.BytesToModelConv<T>(this, argIndex, _sharedData.Charmap);
        }

        /// <summary>
        /// 型Tの値をBinaryDataに適用する。
        /// </summary>
        public void SetData<T>(
            T rawData,
            int argIndex = 0,
            Func<T, FieldValueHolder, int, CharmapManager, byte[]> converter = null)
        {
            // 通常の型Tで対応できない特殊処理があれば渡す
            byte[] newBytes = converter != null
                    ? converter(rawData, this, argIndex, _sharedData.Charmap)
                    : FieldValueConv.ModelToBytesConv(rawData, this, argIndex, _sharedData.Charmap);

            // 新しいbyte[]を代入
            BinaryData = newBytes;
        }

        /// <summary>
        /// 簡易的に値(通常)を更新する。
        /// </summary>
        public void UpdateData<T>(
            UndoManager undoManager,
            T data,
            string desc,
            int argIndex = 0)
        {
            var command = CreateUpdateCommand(
                data,
                desc,
                argIndex);

            if (command != null)
            {
                undoManager.PushCommand(command);
            }
        }

        /// <summary>
        /// コマンドを生成する。
        /// </summary>
        public ICommand CreateUpdateCommand<T>(
            T data,
            string desc,
            int argIndex = 0)
        {
            // 変更前のバイナリデータ
            byte[] oldBinary = BinaryData;
            // データ更新
            SetData(data, argIndex);
            // 変更後のバイナリデータ
            byte[] newBinary = BinaryData;

            // 同じなら無視
            if (oldBinary.SequenceEqual(newBinary)) return null;

            return new FieldValueChangeCommand(
                this,
                oldBinary,
                newBinary,
                desc);
        }
    }
}
