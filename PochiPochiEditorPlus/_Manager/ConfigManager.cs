using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Manager
{
    public sealed class ConfigManager : DynamicAccessor<object>
    {
        // 設定名からパスを取得
        private Dictionary<string, string> _configs = new Dictionary<string, string>();

        /// <summary>
        /// 設定ファイル名とパスを格納する。
        /// </summary>
        public ConfigManager(string folderPath, ComboBox targetCmb)
        {
            if (!Directory.Exists(folderPath)) return;

            var ext = Path.ChangeExtension(Constants.AsteriskChar.ToString(), Constants.IniExt);

            foreach (string filePath in Directory.EnumerateFiles(folderPath, ext))
            {
                string name = Path.GetFileNameWithoutExtension(filePath);

                // 辞書へ
                _configs[name] = filePath;
                // コンボボックスへ
                targetCmb.Items.Add(name);
            }

            // 初期選択
            if (targetCmb.Items.Count > 0)
            {
                targetCmb.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 選択された設定名の内容を解析し、格納する。
        /// </summary>
        public void LoadConfig(string configName, byte[] data)
        {
            // 初期化
            ClearDict();

            // ファイルパスを取得
            if (!_configs.TryGetValue(configName, out string filePath)) return;

            // 設定値を解析
            foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
            {
                // 空行とコメントをスキップして、分割
                if (!TryParseLine(line, out string key, out string rawString)) continue;

                // boolかどうか
                if (bool.TryParse(rawString, out bool boolValue))
                {
                    Register(key, boolValue);
                    continue;
                }

                // ポインタかどうか
                var asteStr = Constants.AsteriskChar.ToString();
                if (rawString.StartsWith(asteStr))
                {
                    if (TryParseNumber(rawString.Substring(asteStr.Length), out int ptrOffset))
                    {
                        // ポインタとして読み取る
                        if (IoHelper.TryReadPtr(data, ptrOffset, out int resultOffset))
                        {
                            Register(key, resultOffset);
                            continue;
                        }
                    }
                }

                // 数字かどうか
                if (TryParseNumber(rawString, out int numValue))
                {
                    Register(key, numValue);
                }
            }
        }

        /// <summary>
        /// 空行を除外して、stringとして分割する。
        /// </summary>
        private bool TryParseLine(string line, out string key, out string rawValue)
        {
            key = string.Empty;
            rawValue = string.Empty;

            // 除外行チェック
            if (string.IsNullOrWhiteSpace(line) 
                || line.StartsWith(Constants.CommentChar.ToString())) return false;

            // イコールで分割
            string[] parts = line.Split(Constants.EqualChar);

            key = parts[0].Trim();
            rawValue = parts[1].Trim();
            return true;
        }

        /// <summary>
        /// 10進数か16進数（0x付き）を判定し、intに変換する。
        /// </summary>
        private bool TryParseNumber(string rawString, out int parsedValue)
        {
            if (rawString.StartsWith(Constants.HexPrefix)) // 0x
            {
                string hexPart = rawString.Substring(Constants.HexPrefix.Length);
                parsedValue = ConvHelper.ParseStringToInt(hexPart);
                return true;
            }

            return int.TryParse(rawString, out parsedValue); // 10進数
        }
    }
}
