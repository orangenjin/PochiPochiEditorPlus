using System.Collections.Generic;

namespace PochiPochiEditorPlus._Managers._FieldManager
{
    /// <summary>
    /// defファイルの各行において、コロンで区切られる順番を定義する。
    /// </summary>
    public enum DefName
    {
        FieldName,
        KindName,
        AttrName
    }

    /// <summary>
    /// defファイルの各行が持つ情報を定義する。
    /// </summary>
    public sealed class FieldMetaData
    {
        public string Name { get; }
        public FieldKind Field { get; }
        public List<FieldAttribute> Attrs { get; }

        public FieldMetaData(
            string name,
            FieldKind kind,
            List<FieldAttribute> attrs)
        {
            Name = name;
            Field = kind;
            Attrs = attrs; // nullを想定していない
        }
    }

    /// <summary>
    /// defファイルのフィールドの型の種類を定義する。
    /// </summary>

    public enum FieldKind
    {
        Byte,
        SByte,
        UInt16,
        Int16,
        UInt32,
        Int32,
        Pointer,
        String
    }

    /// <summary>
    /// 記述任意の属性を定義する。
    /// </summary>
    public sealed class FieldAttribute
    {
        public AttrKind Kind { get; }
        public string[] Args { get; }

        public FieldAttribute(
            AttrKind attrKind,
            params string[] args)
        {
            Kind = attrKind;
            Args = args;
        }
    }

    /// <summary>
    /// defファイルの属性の種類を定義する。
    /// </summary>
    public enum AttrKind
    {
        StringAttr,

        // byte想定
        NibbleAttr,
        BitAttr
    }
}
