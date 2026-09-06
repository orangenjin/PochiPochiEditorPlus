using System.Collections.Generic;
using System.IO;
using System.Text;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers
{
    public sealed class ConfigManager : DynamicAccessor<object>
    {
        // 設定名からパスを取得
        public Dictionary<string, string> Configs => _configs;
        private Dictionary<string, string> _configs = new Dictionary<string, string>();

        /// <summary>
        /// 設定ファイル名とパスを格納する。
        /// </summary>
        public ConfigManager(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;

            var searchPattern = $"*.{Constants.IniExt}";

            foreach (string filePath in Directory.EnumerateFiles(folderPath, searchPattern))
            {
                string name = Path.GetFileNameWithoutExtension(filePath);
                _configs[name] = filePath;
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
                if (rawString.StartsWith("*"))
                {
                    if (TryParseNumber(rawString.Substring(1), out int ptrOffset))
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
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) return false;

            // イコールで分割
            string[] parts = line.Split('=');

            key = parts[(int)Constants.PartName.Key].Trim();
            rawValue = parts[(int)Constants.PartName.Value].Trim();
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
