using System.Collections.Generic;
using System.IO;
using System.Text;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers
{
    public sealed class ConfigManager : DynamicAccessor<object>
    {
        // 設定名をキーとして、パスを保持
        public Dictionary<string, string> Configs { get; }

        /// <summary>
        /// 設定ファイル名とパスを格納する。
        /// </summary>
        public ConfigManager(string folderPath)
        {
            var searchExt = $"*.{ExtConstants.IniExt}";
            Configs = new Dictionary<string, string>();

            foreach (string filePath in Directory.EnumerateFiles(folderPath, searchExt))
            {
                string name = Path.GetFileNameWithoutExtension(filePath);
                Configs[name] = filePath;
            }
        }

        /// <summary>
        /// 選択された設定名の内容を解析し、_values辞書に登録する。
        /// </summary>
        public void LoadConfig(string configName, byte[] source)
        {
            // 初期化
            ClearDict();

            // ファイルパスを取得
            if (!Configs.TryGetValue(configName, out string filePath)) return;

            // 設定値を解析
            foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
            {
                // 空行とコメントをスキップして、分割
                if (!TryParseLine(line, out string key, out string rawValue)) continue;

                // bool値かどうか
                if (bool.TryParse(rawValue, out bool boolValue))
                {
                    Register(key, boolValue);
                    continue;
                }

                // ポインタかどうか
                if (rawValue.StartsWith("*"))
                {
                    if (TryParseNumber(rawValue.Substring(1), out int pointerOffset))
                    {
                        // ポインタとして読み取る
                        if (IoHelper.TryReadPointer(source, pointerOffset, out int resultOffset))
                        {
                            Register(key, resultOffset);
                            continue;
                        }
                    }
                }

                // 数字かどうか
                if (TryParseNumber(rawValue, out int numValue))
                {
                    Register(key, numValue);
                }

                // 該当しない場合は何も登録しない
            }
        }

        /// <summary>
        /// 空行を除外して、stringとして分割する。
        /// </summary>
        private bool TryParseLine(string line, out string key, out string rawValue)
        {
            key = string.Empty;
            rawValue = string.Empty;

            // 除外行を判定
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) return false;

            // イコールで分割
            string[] parts = line.Split('=');

            key = parts[(int)PartName.Key].Trim();
            rawValue = parts[(int)PartName.Value].Trim();
            return true;
        }

        /// <summary>
        /// 10進数か16進数(0x付き)を判定し、intに変換する。
        /// </summary>
        private bool TryParseNumber(string rawValue, out int parsedValue)
        {
            if (rawValue.StartsWith(PrefixConstants.HexPrefix)) // 0x
            {
                string hexPart = rawValue.Substring(PrefixConstants.HexPrefix.Length);
                parsedValue = ConvHelper.ParseStringToInt(hexPart);
                return true;
            }

            return int.TryParse(rawValue, out parsedValue); // 10進数
        }
    }
}
