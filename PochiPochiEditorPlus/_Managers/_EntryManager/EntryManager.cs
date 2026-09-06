using System.Collections.Generic;
using System.Linq;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._EntryManager
{
    /// <summary>
    /// 典型的なエントリーテーブルを作成する。
    /// </summary>
    public sealed class EntryManager
    {
        public List<Entry> Entries { get; set; }

        public EntryManager(
            string defFileName,
            int baseOffset,
            int entryCount,
            SharedData sharedData)
        {
            // 初期化
            Entries = new List<Entry>();

            // FieldMetaDataReaderrから定義情報を読み込む
            var fieldMetaDataList = FieldMetaDataReader.Create(defFileName);

            // 各エントリーに対して
            for (int i = 0; i < entryCount; i++)
            {
                //　単一エントリーに対するフィールドのリスト
                var entryFields = new List<FieldValueHolder>();

                for (int j = 0; j < fieldMetaDataList.Count; j++)
                {
                    // FieldValueを生成
                    var fieldValue = new FieldValueHolder(
                        fieldMetaDataList[j],
                        sharedData);

                    entryFields.Add(fieldValue);
                }

                Entries.Add(new Entry(baseOffset, i, entryFields));
            }
        }
    }

    /// <summary>
    /// 単一エントリー作成のために公開している。
    /// </summary>
    public sealed class Entry : DynamicAccessor<FieldValueHolder>
    {
        public int EntryIndex { get; }
        public int EntrySize { get; }
        public List<FieldValueHolder> Fields { get; }

        public Entry(
            int baseOffset, 
            int entryIndex, 
            List<FieldValueHolder> fields)
        {
            EntryIndex = entryIndex;
            EntrySize = fields.Sum(f => f.Lengths.EntryLength);
            Fields = fields;

            // 現在のエントリーのオフセットを計算
            int entryStartOffset = baseOffset + (EntryIndex * EntrySize);

            // 各フィールドのオフセットを代入して、辞書に登録
            int currentRelativeOffset = 0;
            foreach (var field in Fields)
            {
                field.Offset = entryStartOffset + currentRelativeOffset;
                Register(field.FieldName, field);

                currentRelativeOffset += field.Lengths.EntryLength;
            }
        }
    }
}
