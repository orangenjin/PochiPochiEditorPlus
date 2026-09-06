using System;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._EntryManager
{
    public static class FieldValueConv
    {
        /// <summary>
        /// byte[](FieldValue.BinaryData)を型Tとして変換する。
        /// </summary>
        public static T BytesToModelConv<T>(
            FieldValueHolder fieldValue,
            int argIndex,
            CharmapManager charmap)
        {
            int entryLength = fieldValue.Lengths.EntryLength;
            byte[] binaryData = fieldValue.BinaryData;

            // 文字列
            if (fieldValue.Lengths.AllowedLength > 0)
            {
                return (T)Convert.ChangeType
                    (charmap.BytesToString(binaryData, 0, entryLength),
                    typeof(T));
            }

            // StateValueから読み取り方法を取得
            bool isSigned = fieldValue.State == FieldValueHolder.StateValue.Signed;
            bool isPointer = fieldValue.State == FieldValueHolder.StateValue.Pointer;

            long rawValue;
            switch (entryLength)
            {
                case Constants.ByteSize:
                case Constants.UShortSize:
                case Constants.UIntSize:
                    rawValue = IoHelper.ReadBytesAsInt(
                        binaryData,
                        0,
                        entryLength,
                        isLittleEndian: true,
                        isSigned: isSigned);
                    break;

                default:
                    return default;
            }

            // 1バイトの特殊処理
            if (entryLength == Constants.ByteSize)
            {
                // ニブル
                if (fieldValue.ArgCount == AttrKind.NibbleAttr.GetAttrCount())
                {
                    int nibbleValue = argIndex == (int)NibbleAttrArgs.HighValueArg
                        ? (int)((rawValue >> Constants.NibbleShift) & Constants.NibbleMask)
                        : (int)(rawValue & Constants.NibbleMask);

                    return (T)Convert.ChangeType(nibbleValue, typeof(T));
                }
                // ビット
                else if (fieldValue.ArgCount == AttrKind.BitAttr.GetAttrCount())
                {
                    int bitValue = (int)((rawValue >> argIndex) & 1);
                    return (T)Convert.ChangeType(bitValue, typeof(T));
                }
            }
            // 4バイトの特殊処理
            else if (entryLength == Constants.UIntSize && isPointer)
            {
                return rawValue == 0
                    ? (T)Convert.ChangeType(Constants.InvalidValue, typeof(T))
                    : (T)Convert.ChangeType(rawValue - Constants.BaseAddr, typeof(T));
            }

            return (T)Convert.ChangeType(rawValue, typeof(T));
        }

        /// <summary>
        /// 型Tの値をbyte[](FieldValue.BinaryData)に変換する。
        /// </summary>
        public static byte[] ModelToBytesConv<T>(
           T value,
           FieldValueHolder fieldValue,
           int argIndex,
           CharmapManager charmap)
        {
            int entryLength = fieldValue.Lengths.EntryLength;
            bool isPointer = fieldValue.State == FieldValueHolder.StateValue.Pointer;

            // 戻り値・マージ用
            byte[] result = new byte[entryLength];
            Array.Copy(fieldValue.BinaryData, 0, result, 0, entryLength);

            // 文字列
            if (fieldValue.Lengths.AllowedLength > 0)
            {
                var text = Convert.ToString(value);

                // AllowedLength分
                byte[] bytes = charmap.StringToBytes(
                    text,
                    appendTerminator: true,
                    targetLength: fieldValue.Lengths.AllowedLength);

                // EntryLength分
                Array.Copy(bytes, 0, result, 0, Math.Min(bytes.Length, entryLength));

                return result;
            }

            // 1バイトの特殊処理
            if (entryLength == Constants.ByteSize)
            {
                int nibbleSize = AttrKind.NibbleAttr.GetAttrCount();
                int bitSize = AttrKind.BitAttr.GetAttrCount();
                byte rawByte = result[0];

                // ニブル（特定のニブルのみ更新）
                if (fieldValue.ArgCount == nibbleSize)
                {
                    byte nibbleValue = Convert.ToByte(value);

                    if (argIndex == (int)NibbleAttrArgs.HighValueArg)
                    {
                        // 下位ニブルを残し、上位ニブルに値をセット
                        rawByte = (byte)((rawByte & ~(Constants.NibbleMask << Constants.NibbleShift))
                                       | ((nibbleValue & Constants.NibbleMask) << Constants.NibbleShift));
                    }
                    else
                    {
                        // 上位ニブルを残し、下位ニブルに値をセット
                        rawByte = (byte)((rawByte & (Constants.NibbleMask << Constants.NibbleShift))
                                       | (nibbleValue & Constants.NibbleMask));
                    }

                    result[0] = rawByte;
                    return result;
                }
                // ビット（特定の1ビットのみ更新）
                else if (fieldValue.ArgCount == bitSize)
                {
                    byte bitValue = Convert.ToByte(value);

                    if ((bitValue & 1) == 1)
                    {
                        rawByte |= (byte)(1 << argIndex);
                    }
                    else
                    {
                        rawByte &= (byte)~(1 << argIndex);
                    }

                    result[0] = rawByte;
                    return result;
                }
            }

            long rawValue;
            // 4バイト特有の処理
            if (entryLength == Constants.UIntSize && isPointer)
            {
                long tempValue = Convert.ToInt64(value);
                rawValue = tempValue == Constants.InvalidValue
                    ? 0
                    : tempValue + Constants.BaseAddr;
            }
            else
            {
                rawValue = Convert.ToInt64(value);
            }

            IoHelper.WriteIntAsBytes(
                buffer: result,
                offset: 0,
                value: rawValue,
                length: entryLength);

            return result;
        }

    }
}
