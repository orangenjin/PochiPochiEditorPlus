using System;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers
{
    public static class IoHelper
    {
        /// <summary>
        /// 1, 2, 4バイトのデータを読み取り、整数として返す。
        /// </summary>
        public static long ReadBytesAsInt(
            byte[] data,
            int offset,
            int length,
            bool isLittleEndian = true,
            bool isSigned = false)
        {
            long result;
            switch (length)
            {
                // 1バイト
                case Constants.ByteSize:
                    result = data[offset];
                    break;

                // 2バイト
                case Constants.UShortSize:
                    result = isLittleEndian
                        ? data[offset]
                            | (data[offset + 1] << Constants.BitsPerByte)
                        : (data[offset] << Constants.BitsPerByte)
                            | data[offset + 1];
                    break;

                // 4バイト
                case Constants.UIntSize:
                    result = isLittleEndian
                        ? (long)data[offset]
                            | ((long)data[offset + 1] << (Constants.BitsPerByte * 1))
                            | ((long)data[offset + 2] << (Constants.BitsPerByte * 2))
                            | ((long)data[offset + 3] << (Constants.BitsPerByte * 3))
                        : ((long)data[offset] << (Constants.BitsPerByte * 3))
                            | ((long)data[offset + 1] << (Constants.BitsPerByte * 2))
                            | ((long)data[offset + 2] << (Constants.BitsPerByte * 1))
                            | (long)data[offset + 3];
                    break;

                default:
                    throw new Exception();
            }

            // 符号付きの場合
            if (isSigned)
            {
                int shiftBits =
                    (Constants.UIntSize - length) * Constants.BitsPerByte;
                result = (result << shiftBits) >> shiftBits;
            }

            return result;
        }

        /// <summary>
        /// 整数を1, 2, 4バイトのデータとして書き込む。
        /// </summary>
        public static void WriteIntAsBytes(
            byte[] buffer,
            int offset,
            long value,
            int length,
            bool isLittleEndian = true)
        {
            switch (length)
            {
                // 1バイト
                case Constants.ByteSize:
                    buffer[offset] =
                        (byte)(value & Constants.ByteMask);
                    break;

                // 2バイト
                case Constants.UShortSize:
                    if (isLittleEndian)
                    {
                        buffer[offset] =
                            (byte)(value & Constants.ByteMask);
                        buffer[offset + 1] =
                            (byte)((value >> Constants.BitsPerByte)
                                & Constants.ByteMask);
                    }
                    else
                    {
                        buffer[offset] =
                            (byte)((value >> Constants.BitsPerByte)
                                & Constants.ByteMask);
                        buffer[offset + 1] =
                            (byte)(value & Constants.ByteMask);
                    }
                    break;

                // 4バイト
                case Constants.UIntSize:
                    if (isLittleEndian)
                    {
                        buffer[offset] =
                            (byte)(value & Constants.ByteMask);
                        buffer[offset + 1] =
                            (byte)((value >> (Constants.BitsPerByte * 1))
                                & Constants.ByteMask);
                        buffer[offset + 2] =
                            (byte)((value >> (Constants.BitsPerByte * 2))
                                & Constants.ByteMask);
                        buffer[offset + 3] =
                            (byte)((value >> (Constants.BitsPerByte * 3))
                                & Constants.ByteMask);
                    }
                    else
                    {
                        buffer[offset] =
                            (byte)((value >> (Constants.BitsPerByte * 3))
                                & Constants.ByteMask);
                        buffer[offset + 1] =
                            (byte)((value >> (Constants.BitsPerByte * 2))
                                & Constants.ByteMask);
                        buffer[offset + 2] =
                            (byte)((value >> (Constants.BitsPerByte * 1))
                                & Constants.ByteMask);
                        buffer[offset + 3] =
                            (byte)(value & Constants.ByteMask);
                    }
                    break;

                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// データからポインタを読み取る。
        /// [00 00 00 00]はnullポインタとして、trueとConstants.InvalidValueを返す。
        /// ポインタとして読み取れない場合は、falseとConstants.InvalidValueを返す。
        /// </summary>
        public static bool TryReadPtr(
            byte[] data,
            int ptrOffset,
            out int resultOffset)
        {
            long rawAddr = ReadBytesAsInt(
                data,
                ptrOffset,
                Constants.UIntSize);

            // nullポインタ
            if (rawAddr == 0)
            {
                resultOffset = Constants.InvalidValue;
                return true;
            }

            // 有効なアドレス範囲外
            if (rawAddr < Constants.BaseAddr ||
                rawAddr > Constants.EndAddr)
            {
                resultOffset = Constants.InvalidValue;
                return false;
            }

            resultOffset = (int)(rawAddr - Constants.BaseAddr);
            return true;
        }

        /// <summary>
        /// 4の倍数サイズでないバイト配列（画像など）を、アライメント調整して書き込む。
        /// </summary>
        public static void WriteBytesToData(
            byte[] data,
            int offset,
            byte[] bytes,
            byte alignPaddingByte = Constants.PaddingByte)
        {
            Array.Copy(bytes, 0, data, offset, bytes.Length);

            int endOffset = offset + bytes.Length;
            int paddingCount =
                (Constants.UIntSize - (endOffset % Constants.UIntSize))
                % Constants.UIntSize;

            for (int i = 0; i < paddingCount; i++)
            {
                data[endOffset + i] = alignPaddingByte;
            }
        }
    }
}
