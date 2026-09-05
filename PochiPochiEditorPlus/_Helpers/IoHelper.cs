using System;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers
{
    public static class IoHelper
    {
        /// <summary>
        /// 1, 2, 4バイトのデータを読み取り、整数として返す。
        /// </summary>
        public static int ReadBytesAsInt(
            byte[] data,
            int offset,
            int length,
            bool isLittleEndian = true,
            bool isSigned = false)
        {
            // 戻り値
            int result = 0;

            switch (length) 
            {
                case Constants.ByteSize:
                    result = data[offset];
                    break;

                case Constants.UShortSize:
                    result = isLittleEndian
                        ? (data[offset] | (data[offset + 1] << Constants.BitsPerByte))
                        : (data[offset] << Constants.BitsPerByte) | data[offset + 1];
                    break;

                case Constants.UIntSize:
                    result = isLittleEndian
                        ? data[offset] | 
                            (data[offset + 1] << (Constants.BitsPerByte * 1)) | 
                            (data[offset + 2] << (Constants.BitsPerByte * 2)) | 
                            (data[offset + 3] << (Constants.BitsPerByte * 3))
                        : (data[offset] << (Constants.BitsPerByte * 3)) | 
                            (data[offset + 1] << (Constants.BitsPerByte * 2)) | 
                            (data[offset + 2] << (Constants.BitsPerByte * 1)) |
                            (data[offset + 3]);
                    break;
            }

            // 符号付きの場合
            if (isSigned)
            {
                int shiftBits = (Constants.UIntSize - length) * Constants.BitsPerByte;
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
            int value,
            int length,
            bool isLittleEndian = true)
        {
            switch (length)
            {
                case Constants.ByteSize:
                    buffer[offset] = (byte)(value & Constants.ByteMask);
                    break;

                case Constants.UShortSize:
                    if (isLittleEndian)
                    {
                        buffer[offset] = (byte)(value & Constants.ByteMask);
                        buffer[offset + 1] = (byte)((value >> Constants.BitsPerByte) & Constants.ByteMask);
                    }
                    else
                    {
                        buffer[offset] = (byte)((value >> Constants.BitsPerByte) & Constants.ByteMask);
                        buffer[offset + 1] = (byte)(value & Constants.ByteMask);
                    }
                    break;

                case Constants.UIntSize:
                    if (isLittleEndian)
                    {
                        buffer[offset] = (byte)(value & Constants.ByteMask);
                        buffer[offset + 1] = (byte)((value >> (Constants.BitsPerByte * 1)) & Constants.ByteMask);
                        buffer[offset + 2] = (byte)((value >> (Constants.BitsPerByte * 2)) & Constants.ByteMask);
                        buffer[offset + 3] = (byte)((value >> (Constants.BitsPerByte * 3)) & Constants.ByteMask);
                    }
                    else
                    {
                        buffer[offset] = (byte)((value >> (Constants.BitsPerByte * 3)) & Constants.ByteMask);
                        buffer[offset + 1] = (byte)((value >> (Constants.BitsPerByte * 2)) & Constants.ByteMask);
                        buffer[offset + 2] = (byte)((value >> (Constants.BitsPerByte * 1)) & Constants.ByteMask);
                        buffer[offset + 3] = (byte)(value & Constants.ByteMask);
                    }
                    break;
            }
        }
    }
}
