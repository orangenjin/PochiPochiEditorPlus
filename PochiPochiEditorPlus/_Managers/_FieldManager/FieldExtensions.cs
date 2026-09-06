using System;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._FieldManager
{
    public static class FieldExtensions
    {
        /// <summary>
        /// FieldKindに対応するサイズを取得する。
        /// </summary>
        public static int GetFieldSize(this FieldKind kind)
        {
            switch (kind)
            {
                case FieldKind.Byte:
                case FieldKind.SByte:
                    return Constants.ByteSize;

                case FieldKind.UInt16:
                case FieldKind.Int16:
                    return Constants.UShortSize;

                case FieldKind.UInt32:
                case FieldKind.Int32:
                case FieldKind.Pointer:
                    return Constants.UIntSize;

                // stringは動的長さ
                default:
                    return Constants.InvalidValue;
            }
        }

        /// <summary>
        /// フィールドの種類が符号付きかどうかを判定する。
        /// </summary>
        public static bool IsSigned(this FieldKind kind) =>
            kind is FieldKind.SByte ||
            kind is FieldKind.Int16 ||
            kind is FieldKind.Int32;

        /// <summary>
        /// フィールドの種類がポインタかどうかを判定する。
        /// </summary>
        public static bool IsPointer(this FieldKind kind) =>
            kind is FieldKind.Pointer;

        /// <summary>
        /// AttrKindに対応する引数の数を取得する。
        /// </summary>
        public static int GetAttrCount(this AttrKind kind)
        {
            // 戻り値用
            int count = default;

            switch (kind)
            {
                case AttrKind.StringAttr:
                    count = Enum.GetValues(typeof(StringAttrArgs)).Length;
                    break;

                case AttrKind.NibbleAttr:
                    count = Enum.GetValues(typeof(NibbleAttrArgs)).Length;
                    break;

                case AttrKind.BitAttr:
                    count = Enum.GetValues(typeof(BitAttrArgs)).Length;
                    break;
            }

            return count;
        }
    }

    public enum StringAttrArgs
    {
        EntryLengthArg,
        AllowedLengthArg
    }

    public enum NibbleAttrArgs
    {
        HighValueArg,
        LowValueArg
    }

    public enum BitAttrArgs
    {
        Bit0Arg,
        Bit1Arg,
        Bit2Arg,
        Bit3Arg,
        Bit4Arg,
        Bit5Arg,
        Bit6Arg,
        Bit7Arg
    }
}
