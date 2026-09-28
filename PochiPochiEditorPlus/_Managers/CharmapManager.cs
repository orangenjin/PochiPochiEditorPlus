using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers
{
    public sealed class CharmapManager
    {
        // 対応探索用
        private ByteTrieNode _byteTrieRoot = null;
        private StringTrieNode _stringTrieRoot = null;

        /// <summary>
        /// ファイルパスを指定して、charmapを作成する。
        /// </summary>
        public CharmapManager(string filePath)
        {
            // 初期化
            _byteTrieRoot = new ByteTrieNode();
            _stringTrieRoot = new StringTrieNode();

            foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
            {
                // 空行とコメント行をスキップ
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) continue;

                // イコールで分割
                string[] parts = line.Split('=');

                // 2バイト以上の場合を想定
                string hexKey = parts[(int)PartName.Key].Replace(" ", string.Empty);
                string value = parts[(int)PartName.Value]; // 文字部分

                // キーをstringからbyteへ
                int byteLen = hexKey.Length / BinaryConstants.HexCharsPerByte;
                byte[] bytes = new byte[byteLen];
                for (int i = 0; i < byteLen; i++)
                {
                    string targetStr = hexKey.Substring(
                        i * BinaryConstants.HexCharsPerByte,
                        BinaryConstants.HexCharsPerByte);
                    bytes[i] = Convert.ToByte(targetStr, BinaryConstants.HexBase);
                }

                // バイト -> 文字
                ByteTrieNode currentByteNode = _byteTrieRoot; // 先頭
                foreach (byte b in bytes)
                {
                    if (!currentByteNode.Children.TryGetValue(b, out ByteTrieNode next))
                    {
                        next = new ByteTrieNode();
                        currentByteNode.Children[b] = next;
                    }
                    currentByteNode = next;
                }
                currentByteNode.Value = value; // 取得したい対象
                currentByteNode.IsTerminal = true;

                // 文字 -> バイト
                StringTrieNode currentStrNode = _stringTrieRoot;
                foreach (char c in value)
                {
                    if (!currentStrNode.Children.TryGetValue(c, out StringTrieNode next))
                    {
                        next = new StringTrieNode();
                        currentStrNode.Children[c] = next;
                    }
                    currentStrNode = next;
                }
                currentStrNode.Value = bytes;
                currentStrNode.IsTerminal = true;
            }
        }

        /// <summary>
        /// 通常は StrTerminatorByte = 0xFF 手前まで読み取る。
        /// </summary>
        public string BytesToString(
            byte[] buffer,
            int offset = 0,
            int? maxLength = null)
        {
            if (buffer == null) return string.Empty; // 空文字を返す
            StringBuilder result = new StringBuilder(); // 戻り値

            // 最大範囲を定める
            int calcLength = buffer.Length - offset;
            int length = maxLength.HasValue
                ? Math.Min(calcLength, maxLength.Value)
                : calcLength;

            int i = 0; // bufferに対するオフセット
            while (i < length)
            {
                int currentIndex = offset + i;
                byte currentByte = buffer[currentIndex];

                // 終端
                if (currentByte == Constants.StrTerminatorByte)
                {
                    break;
                }

                // 改行
                if (currentByte == Constants.StrNewlineByte)
                {
                    result.Append(Environment.NewLine);
                    i++;
                    continue;
                }

                // 一致検証開始
                int matchLength = 0;
                string matchedString = null;
                ByteTrieNode currentNode = _byteTrieRoot; // 先頭

                for (int j = 0; j < length - i; j++) // currentIndexに対するオフセット
                {
                    byte b = buffer[currentIndex + j];

                    if (currentNode.Children.TryGetValue(b, out ByteTrieNode next))
                    {
                        // ノードを進める
                        currentNode = next;

                        // 一旦文字を取り出す
                        if (currentNode.IsTerminal)
                        {
                            matchLength = j + 1; // バイト配列の長さ
                            matchedString = currentNode.Value;

                            // 最長一致を取得するためにbreakしない
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (matchLength > 0 && matchedString != null)
                {
                    result.Append(matchedString);
                    i += matchLength;
                }
                else
                {
                    // 振り出しに戻る
                    i++;
                }
            }

            return result.ToString();
        }


        /// <summary>
        /// targetLengthを指定すると、その長さまでpaddingByteを追加する。
        /// </summary>
        public byte[] StringToBytes(
            string text,
            bool appendTerminator = true,
            int targetLength = -1,
            byte paddingByte = Constants.PaddingByte)
        {
            text = text ?? string.Empty; // 空文字を入れる
            List<byte> result = new List<byte>(); // 戻り値

            int i = 0; // textに対するオフセット
            while (i < text.Length)
            {
                // 改行
                if (text[i] == '\r' && text[i + 1] == '\n')
                {
                    result.Add(Constants.StrNewlineByte);
                    i += 2;
                    continue;
                }

                int matchLength = 0;
                byte[] matchedBytes = null;
                StringTrieNode currentNode = _stringTrieRoot; // 先頭

                for (int j = 0; j < text.Length - i; j++) // iに対するオフセット
                {
                    char c = text[i + j];

                    if (currentNode.Children.TryGetValue(c, out StringTrieNode next))
                    {
                        // ノードを進める
                        currentNode = next;

                        // 一旦文字を取り出す
                        if (currentNode.IsTerminal)
                        {
                            matchLength = j + 1; // テキストの長さ
                            matchedBytes = currentNode.Value;

                            // 最長一致を取得するためにbreakしない
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (matchLength > 0 && matchedBytes != null)
                {
                    result.AddRange(matchedBytes);
                    i += matchLength;
                }
                else
                {
                    // 振り出しに戻る
                    i++;
                }
            }

            // 終端を追加するかどうか
            if (appendTerminator)
            {
                result.Add(Constants.StrTerminatorByte);
            }

            // 埋める必要があるかどうか
            if (targetLength > 0)
            {
                while (result.Count < targetLength)
                {
                    result.Add(paddingByte);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 文字列の長さを任意の長さ(バイト数)まで削る。
        /// </summary>
        public string TextLengthValidate(
            string text,
            int byteLength,
            bool needTerminator = true)
        {
            // 空白ならそのまま返す
            if (string.IsNullOrEmpty(text)) return text;

            // 最大のバイト数を取得
            int maxBytes = needTerminator
                ? byteLength - 1
                : byteLength;

            // 現在の長さを取得(終端文字なし)
            byte[] currentBytes = StringToBytes(text, false);

            // 範囲内ならそのまま返す
            if (currentBytes.Length <= maxBytes) return text;

            // StringInfoで分割
            StringInfo stringInfo = new StringInfo(text);
            int count = stringInfo.LengthInTextElements;

            // 一文字ずつ削る
            string currentText = text;
            while (count > 0)
            {
                // 末尾一文字を削った文字列
                count--;
                currentText = stringInfo.SubstringByTextElements(0, count);

                // バイト数をチェック
                byte[] bytes = StringToBytes(currentText, false);
                if (bytes.Length <= maxBytes)
                {
                    break;
                }
            }

            return currentText;
        }
    }

    public sealed class ByteTrieNode
    {
        public Dictionary<byte, ByteTrieNode> Children { get; set; }
        public string Value { get; set; }
        public bool IsTerminal { get; set; }

        public ByteTrieNode(string value = null, bool isTerminal = false)
        {
            Children = new Dictionary<byte, ByteTrieNode>();
            Value = value;
            IsTerminal = isTerminal;
        }
    }

    public sealed class StringTrieNode
    {
        public Dictionary<char, StringTrieNode> Children { get; set; }
        public byte[] Value { get; set; }
        public bool IsTerminal { get; set; }

        public StringTrieNode(byte[] value = null, bool isTerminal = false)
        {
            Children = new Dictionary<char, StringTrieNode>();
            Value = value;
            IsTerminal = isTerminal;
        }
    }
}
