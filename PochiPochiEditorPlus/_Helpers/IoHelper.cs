using System;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Helpers
{
    public static class IoHelper
    {
        /// <summary>
        /// 1, 2, 4バイトのデータを読み取り、整数として返す。
        /// 符号付きにも対応するために、long型を採用している。
        /// </summary>
        public static long ReadBytesAsLong(
            byte[] buffer,
            int offset,
            DataSize size,
            bool isLittleEndian = true,
            bool isSigned = false)
        {
            // 戻り値
            long result;

            switch (size)
            {
                // 1バイト
                case DataSize.Byte:
                    result = (long)buffer[offset];
                    break;

                // 2バイト
                case DataSize.UShort:
                    result = isLittleEndian
                        ? (long)buffer[offset]
                            | ((long)buffer[offset + 1] << BinaryConstants.BitsPerByte)
                        : ((long)buffer[offset] << BinaryConstants.BitsPerByte)
                            | (long)buffer[offset + 1];
                    break;

                // 4バイト
                case DataSize.UInt:
                    result = isLittleEndian
                        ? (long)buffer[offset]
                            | ((long)buffer[offset + 1] << (BinaryConstants.BitsPerByte * 1))
                            | ((long)buffer[offset + 2] << (BinaryConstants.BitsPerByte * 2))
                            | ((long)buffer[offset + 3] << (BinaryConstants.BitsPerByte * 3))
                        : ((long)buffer[offset] << (BinaryConstants.BitsPerByte * 3))
                            | ((long)buffer[offset + 1] << (BinaryConstants.BitsPerByte * 2))
                            | ((long)buffer[offset + 2] << (BinaryConstants.BitsPerByte * 1))
                            | (long)buffer[offset + 3];
                    break;
                
                // その他
                default:
                    throw new Exception();
            }

            // 符号付きの場合
            if (isSigned)
            {
                // 実データ範囲
                int bits = (int)size * BinaryConstants.BitsPerByte;
                long valueMask = (1L << bits) - 1;

                // 符号を表すビット位置
                long signBit = 1L << (bits - 1);

                // マイナス値であるならば
                if ((result & signBit) != 0)
                {
                    // 符号を表すビット位置より上位すべてを1にする
                    result |= ~valueMask;
                }
            }

            return result;
        }

        /// <summary>
        /// long型整数を1, 2, 4バイトのデータとして書き込む。
        /// 内部表現をそのまま書き込むため、符号判定は必要ない。
        /// </summary>
        public static void WriteLongAsBytes(
            byte[] buffer,
            int offset,
            long value,
            DataSize size,
            bool isLittleEndian = true)
        {
            switch (size)
            {
                // 1バイト
                case DataSize.Byte:
                    buffer[offset] =
                        (byte)(value & BinaryConstants.ByteMask);
                    break;

                // 2バイト
                case DataSize.UShort:
                    if (isLittleEndian)
                    {
                        buffer[offset] =
                            (byte)(value & BinaryConstants.ByteMask);
                        buffer[offset + 1] =
                            (byte)((value >> BinaryConstants.BitsPerByte)
                                & BinaryConstants.ByteMask);
                    }
                    else
                    {
                        buffer[offset] =
                            (byte)((value >> BinaryConstants.BitsPerByte)
                                & BinaryConstants.ByteMask);
                        buffer[offset + 1] =
                            (byte)(value & BinaryConstants.ByteMask);
                    }
                    break;

                // 4バイト
                case DataSize.UInt:
                    if (isLittleEndian)
                    {
                        buffer[offset] =
                            (byte)(value & BinaryConstants.ByteMask);
                        buffer[offset + 1] =
                            (byte)((value >> (BinaryConstants.BitsPerByte * 1))
                                & BinaryConstants.ByteMask);
                        buffer[offset + 2] =
                            (byte)((value >> (BinaryConstants.BitsPerByte * 2))
                                & BinaryConstants.ByteMask);
                        buffer[offset + 3] =
                            (byte)((value >> (BinaryConstants.BitsPerByte * 3))
                                & BinaryConstants.ByteMask);
                    }
                    else
                    {
                        buffer[offset] =
                            (byte)((value >> (BinaryConstants.BitsPerByte * 3))
                                & BinaryConstants.ByteMask);
                        buffer[offset + 1] =
                            (byte)((value >> (BinaryConstants.BitsPerByte * 2))
                                & BinaryConstants.ByteMask);
                        buffer[offset + 2] =
                            (byte)((value >> (BinaryConstants.BitsPerByte * 1))
                                & BinaryConstants.ByteMask);
                        buffer[offset + 3] =
                            (byte)(value & BinaryConstants.ByteMask);
                    }
                    break;

                // その他
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// データからポインタ先のオフセット(int)を読み取る。
        /// [00 00 00 00]はnullポインタとして、trueとInvalidValueを返す。
        /// ポインタとして読み取れない場合は、falseとInvalidValueを返す。
        /// </summary>
        public static bool TryReadPointer(
            byte[] source,
            int pointerOffset,
            out int resultOffset)
        {
            uint rawAddr = (uint)ReadBytesAsLong(
                source,
                pointerOffset,
                DataSize.UInt);

            // nullポインタ
            if (rawAddr == 0U)
            {
                resultOffset = BinaryConstants.InvalidValue;
                return true;
            }

            // 有効なアドレス範囲外
            if (rawAddr < BinaryConstants.BaseAddr
                || rawAddr > BinaryConstants.EndAddr)
            {
                resultOffset = BinaryConstants.InvalidValue;
                return false;
            }

            resultOffset = (int)(rawAddr - BinaryConstants.BaseAddr);
            return true;
        }

        /// <summary>
        /// 4の倍数サイズでないバイト配列を、アライメント調整して書き込む。
        /// </summary>
        public static void WriteBytesToData(
            byte[] buffer,
            int offset,
            byte[] value,
            byte alignPaddingByte = BinaryConstants.PaddingByte)
        {
            // データを書き込み
            Array.Copy(value, 0, buffer, offset, value.Length);

            int endOffset = offset + value.Length;
            int alignment = sizeof(uint);

            // 穴埋めに必要な個数を求める
            int paddingCount = (alignment - (endOffset % alignment)) % alignment;

            for (int i = 0; i < paddingCount; i++)
            {
                buffer[endOffset + i] = alignPaddingByte;
            }
        }
    }
}
