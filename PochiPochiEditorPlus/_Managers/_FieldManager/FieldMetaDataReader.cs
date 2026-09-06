using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._FieldManager
{
    public static class FieldMetaDataReader
    {
        /// <summary>
        /// defファイルをパースして、FieldMetaDataリストとして返す。
        /// </summary>
        public static List<FieldMetaData> Create(string fileName)
        {
            var splitLines = ReadFile(fileName);
            return ReadFields(splitLines);
        }

        /// <summary>
        /// ファイルから各行を読み込み、要素をコロンで分割する。
        /// </summary>
        private static List<string[]> ReadFile(string fileName)
        {
            // 戻り値用
            List<string[]> splitLines = new List<string[]>();

            // defフォルダ階層下から指定したファイルを探す
            var foundFiles = Directory.GetFiles(
                Path.Combine(Application.StartupPath, Constants.DefExt),
                Path.ChangeExtension(fileName, Constants.DefExt),
                SearchOption.AllDirectories);

            // 最初にヒットしたもの
            var allLines = File.ReadAllLines(foundFiles[0]);

            foreach (var line in allLines)
            {
                // 空行をスキップ
                if (string.IsNullOrWhiteSpace(line)) continue;

                // コロン分割
                var parts = line
                    .Split(':')
                    .Select(p => p.Trim())
                    .ToArray();

                splitLines.Add(parts);
            }

            return splitLines;
        }

        /// <summary>
        /// 各行の内容をパースして、定義を読み取る。
        /// </summary>
        private static List<FieldMetaData> ReadFields(List<string[]> splitLines)
        {
            // 戻り値用
            var metaDataList = new List<FieldMetaData>();

            foreach (var line in splitLines)
            {
                // フィールド名を読み取る
                var name = line[(int)DefName.FieldName];

                // 型を読み取る
                var kindStr = line[(int)DefName.KindName];
                var kindEnum = (FieldKind)Enum.Parse(typeof(FieldKind), kindStr);

                // 属性読み取る
                var attrs = new List<FieldAttribute>();
                // 属性が付加されていない場合スキップされる
                for (int i = (int)DefName.AttrName; i < line.Length; i++)
                {
                    // 解析対象を取り出す
                    var attrText = line[i];
                    AttrKind attrKind;

                    // "("の位置を探す
                    var openParenIndex = attrText.IndexOf('(');

                    // 属性名のみの場合
                    if (openParenIndex == Constants.InvalidValue)
                    {
                        // 属性名を取得
                        attrKind = (AttrKind)Enum.Parse(typeof(AttrKind), attrText);

                        // 属性引数なし（要素数0を格納）
                        attrs.Add(new FieldAttribute(attrKind, Array.Empty<string>()));
                        continue;
                    }

                    //　")"の位置を探す
                    var closeParenIndex = attrText.LastIndexOf(')');

                    // 属性名を取得
                    var attrStr = attrText.Substring(0, openParenIndex);
                    attrKind = (AttrKind)Enum.Parse(typeof(AttrKind), attrStr);

                    // 属性引数を取得
                    var argsStr = attrText.Substring(
                        openParenIndex + 1,
                        closeParenIndex - openParenIndex - 1);
                    // カンマで分割
                    var argParts = argsStr
                        .Split(',')
                        .Select(p => p.Trim())
                        .ToArray();

                    // 属性名と引数を格納
                    attrs.Add(new FieldAttribute(attrKind, argParts));
                }

                // フィールド定義を格納
                metaDataList.Add(new FieldMetaData(name, kindEnum, attrs));
            }

            return metaDataList;
        }
    }
}
