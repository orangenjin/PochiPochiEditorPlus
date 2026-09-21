using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Helpers._MatchHelper;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Managers._CommandManager;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Utilities;
using PochiPochiEditorPlus._Utilities._QuickInput;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.OwMap, 0)]
    public partial class OwMapEditor0 : Form, IEditorRefresh
    {
        // 共有データ用
        private dynamic _sharedData = null;
        private dynamic _groupData = null;
        // 変更履歴用
        private UndoManager _undoManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各テーブル用
        private dynamic _mapNameEntry = null;
        private dynamic _mapHeaderEntry = null;
        private dynamic _mapFooterEntry = null;

        // 基準インデックスの計算を省略するため
        private int _mapNameFirstIndex = default;
        private Dictionary<int, string> _mapNameCache = new Dictionary<int, string>();

        // ノードからエントリーインデックスを取得するため
        public sealed class MapTreeNode : TreeNode, IMapNode
        {
            public int MapBankIndex { get; }
            public int MapNumberIndex { get; }

            public MapTreeNode(string text, int bank, int number) : base(text)
            {
                MapBankIndex = bank;
                MapNumberIndex = number;
            }
        }
        public interface IMapNode
        {
            int MapBankIndex { get; }
            int MapNumberIndex { get; }
        }

        public OwMapEditor0(
            SharedData sharedData,
            UndoManager undoManager,
            FormGroupData groupData)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _undoManager = undoManager;
            _groupData = groupData;
            _eventBinder = new EventBinder();

            // グループデータを登録
            RegisterFormGroupData(_groupData);

            InitializeMapNameEntry(); // 先に処理
            InitializeControls();
            InitializeMapHeaderEntry();

            UpdateMapNameComboBox();
            UpdateMapSelector();

            // イベントハンドラーの実装
            InitializeEventHandlers();

            // 初期選択
            rbOrderByAsc.Checked = true;
        }

        private void RegisterFormGroupData(FormGroupData groupData)
        {
            groupData.Register(this, () => _mapHeaderEntry);
        }

        private void InitializeMapNameEntry()
        {
            // マップ名テーブルを作成
            int tableOffset = _sharedData.Config.MapNameTableOffset;
            int entrycount = _sharedData.Config.MapNameCount;
            _mapNameEntry = 
                new EntryManager("MapNamePointerEntry", tableOffset, entrycount, _sharedData);
        }

        private void InitializeControls()
        {
            // 各コンボボックスにアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbMapType, "txt/Map/MapType.txt"),
                (cmbMapWthr, "txt/Map/MapWthr.txt"),
                (cmbMapSight, "txt/Map/MapSight.txt"),
                (cmbMapBike, "txt/Map/MapBike.txt"),
                (cmbMapSpBg, "txt/Map/MapSpBg.txt"),
                (cmbMapNameType, "txt/Map/MapNameType.txt"));
        }

        private void InitializeMapHeaderEntry()
        {
            // マップヘッダーエントリー格納先を先に作成
            _mapHeaderEntry = new List<Entry[]>();

            // エントリー数を仮カウント（誤って含まれている可能性あり）
            var pointerPattern = new List<TokenData>(){ TokenData.Pointer() };
            int tableOffset = _sharedData.Config.MapBankTableOffset;
            var bankEntrycount = PatternMatcher.TryCountByPattern(
                pointerPattern,
                _sharedData.RomData,
                tableOffset,
                allowNullPointer: false); // nullポインタを許容しない

            // マップバンクテーブルを仮作成
            dynamic mapBankEntry = 
                new EntryManager("MapBankPointerEntry", tableOffset, bankEntrycount, _sharedData);

            // マップナンバーテーブルの先頭オフセットをすべて取得
            var mapNumberTableOffsets = new List<int>();
            for (int i = 0; i < mapBankEntry.Entries.Count; i++)
            {
                var mapNumberTableOffset =
                    mapBankEntry.Entries[i].MapHeaderPointerOffset.GetData<int>();

                // 正しいテーブルオフセットの検証は、ポインタ判定が限界
                var IsValid = PatternMatcher.TryMatch(
                    pointerPattern,
                    _sharedData.RomData,
                    mapNumberTableOffset,
                    allowNullPointer: true); // nullポインタを許容する

                // 無効なオフセットだったら、そこで中断
                if (!IsValid) break;

                mapNumberTableOffsets.Add(mapNumberTableOffset);
            }

            // 後の計算に必要な定数を事前に計算
            var entryLength = TokenData.Pointer().GetLength();
            var headerPattern = new List<TokenData>()
            {
                TokenData.Pointer(),
                TokenData.Pointer(),
                TokenData.Pointer(),
                TokenData.Pointer(),
                TokenData.Wildcard(4),
                TokenData.Range((byte)_mapNameFirstIndex, byte.MaxValue, Constants.ByteSize),
                TokenData.Range(byte.MinValue, (byte)cmbMapSight.Items.Count, Constants.ByteSize),
                TokenData.Range(byte.MinValue, (byte)cmbMapWthr.Items.Count, Constants.ByteSize),
                TokenData.Range(byte.MinValue, (byte)cmbMapType.Items.Count, Constants.ByteSize),
                TokenData.Range(byte.MinValue, (byte)cmbMapBike.Items.Count, Constants.ByteSize),
                TokenData.Range(byte.MinValue, (byte)cmbMapNameType.Items.Count, Constants.ByteSize),
                TokenData.Wildcard(1),
                TokenData.Range(byte.MinValue, (byte)cmbMapSpBg.Items.Count, Constants.ByteSize),
            };

            // マップエントリーテーブルを検証
            for (int i = 0; i < mapBankEntry.Entries.Count; i++)
            {
                // そのテーブルのエントリー数を仮カウント
                var numberEntrycount = PatternMatcher.TryCountByPattern(
                    pointerPattern, // ポインタパターン
                    _sharedData.RomData,
                    mapNumberTableOffsets[i],
                    allowNullPointer: true); // nullポインタを許容する

                // そのテーブルの正しいエントリー数を求める
                int validEntryCount = numberEntrycount; // 仮カウント数を仮代入
                for (int j = 0; j < numberEntrycount; j++)
                {
                    var pointerOffset = mapNumberTableOffsets[i] + j * entryLength;

                    // 各マップナンバーテーブルのオフセットと比較して検証
                    bool hasOffset = mapNumberTableOffsets.Contains(pointerOffset);

                    // 別のマップナンバーテーブルオフセットだった場合
                    if (j > 0 && hasOffset)
                    {
                        validEntryCount = j;
                        break;
                    }

                    // ポインタ先が正規のマップヘッダーかどうかを検証
                    if (IoHelper.TryReadPtr(_sharedData.RomData, pointerOffset, out int entryOffset))
                    {
                        // nullポインタならスキップ
                        if (entryOffset == Constants.InvalidValue) continue;

                        var IsValid = PatternMatcher.TryMatch(
                            headerPattern,
                            _sharedData.RomData,
                            entryOffset,
                            allowNullPointer: true); // nullポインタを許容する

                        // 正規のマップヘッダーでない場合
                        if (!IsValid)
                        {
                            validEntryCount = j;
                            break;
                        }
                    }
                }

                // マップナンバーエントリーテーブルを作成
                dynamic mapNumberEntry = 
                    new EntryManager("MapNumberPointerEntry", mapNumberTableOffsets[i], validEntryCount, _sharedData);

                // マップヘッダーの定義情報を読み込む
                var mapHeaderMetaDataList = FieldMetaDataReader.Create("MapHeaderEntry");

                // エントリーを作成
                var entryArray = new Entry[validEntryCount];
                for (int j = 0; j < validEntryCount; j++)
                {
                    var entryFields = new List<FieldValueHolder>();
                    for (int k = 0; k < mapHeaderMetaDataList.Count; k++)
                    {
                        // FieldValueを生成
                        var fieldValue = new FieldValueHolder(
                            mapHeaderMetaDataList[k],
                            _sharedData);

                        entryFields.Add(fieldValue);
                    }
                    var entry = new Entry(
                        mapNumberEntry.Entries[j].MapHeaderOffset.GetData<int>(),
                        0,
                        entryFields);

                    entryArray[j] = entry;
                }

                // マップバンクに対してEntry[]を格納
                _mapHeaderEntry.Add(entryArray);
            }
        }

        private void UpdateMapNameComboBox()
        {
            // キャッシュを最新化
            LoadMapNames();

            cmbMapNameIndex.BeginUpdate();
            var entries = new List<KeyValuePair<int, string>>();

            // キャッシュから
            foreach (var kvp in _mapNameCache)
            {
                entries.Add(new KeyValuePair<int, string>(kvp.Key, $"[{kvp.Key:X2}]{kvp.Value}"));
            }

            cmbMapNameIndex.DisplayMember = nameof(KeyValuePair<int, string>.Value);
            cmbMapNameIndex.ValueMember = nameof(KeyValuePair<int, string>.Key);
            cmbMapNameIndex.DataSource = entries;

            cmbMapNameIndex.EndUpdate();
        }

        private void LoadMapNames()
        {
            _mapNameCache.Clear();

            // 基準となるンデックス
            _mapNameFirstIndex = _sharedData.Config.MapNameFirstIndex;

            // 順次格納していく
            for (int i = 0; i < _mapNameEntry.Entries.Count; i++)
            {
                var offset = _mapNameEntry.Entries[i].MapNamePointerOffset.GetData<int>();
                var mapName = _sharedData.Charmap.BytesToString(_sharedData.RomData, offset);

                int nameIndex = _mapNameFirstIndex + i;
                _mapNameCache[nameIndex] = mapName;
            }
        }

        private void UpdateMapSelector()
        {
            tvwMapSelector.BeginUpdate();
            tvwMapSelector.Nodes.Clear();

            // 番号順
            if (rbOrderByAsc.Checked)
            {
                for (int i = 0; i < _mapHeaderEntry.Count; i++)
                {
                    var bankNode = new TreeNode($"バンク{i}");

                    for (int j = 0; j < _mapHeaderEntry[i].Length; j++)
                    {
                        int nameIndex = _mapHeaderEntry[i][j].MapNameIndex.GetData<int>();
                        string mapName = _mapNameCache[nameIndex];

                        var mapNode = new MapTreeNode($"({i}, {j}) {mapName}", i, j);
                        bankNode.Nodes.Add(mapNode);
                    }
                    tvwMapSelector.Nodes.Add(bankNode);
                }
            }
            // マップ名順
            else if (rbOrderByName.Checked)
            {
                var nameGroupNodes = new Dictionary<int, TreeNode>();

                for (int i = 0; i < _mapHeaderEntry.Count; i++)
                {
                    for (int j = 0; j < _mapHeaderEntry[i].Length; j++)
                    {
                        int nameIndex = _mapHeaderEntry[i][j].MapNameIndex.GetData<int>();

                        // ルートノードが存在しない場合は新規作成
                        if (!nameGroupNodes.ContainsKey(nameIndex))
                        {
                            string rootName = $"[{nameIndex:X2}]{_mapNameCache[nameIndex]}";
                            nameGroupNodes[nameIndex] = new TreeNode(rootName);
                        }

                        string mapName = _mapNameCache[nameIndex];
                        var mapNode = new MapTreeNode($"({i}, {j}) {mapName}", i, j);
                        nameGroupNodes[nameIndex].Nodes.Add(mapNode);
                    }
                }

                // マップ名IDの昇順
                foreach (var key in nameGroupNodes.Keys.OrderBy(k => k))
                {
                    tvwMapSelector.Nodes.Add(nameGroupNodes[key]);
                }
            }

            tvwMapSelector.EndUpdate();
        }

        private void InitializeEventHandlers()
        {
            // 枠描画
            _eventBinder.BindCustom(
                () => CtrlHelper.AttachBorder(grpMapView, pnlMapDraw),
                () => CtrlHelper.DetachBorder(grpMapView));

            // マップ選択のラジオボタン
            _eventBinder.BindCtrl(
                h => rbOrderByAsc.CheckedChanged += h,
                h => rbOrderByAsc.CheckedChanged -= h,
                OrderRadioButton_CheckedChanged);
            _eventBinder.BindCtrl(
                h => rbOrderByName.CheckedChanged += h,
                h => rbOrderByName.CheckedChanged -= h,
                OrderRadioButton_CheckedChanged);

            // マップ選択
            _eventBinder.BindCustom(
                () => tvwMapSelector.AfterSelect += tvwMapSelector_AfterSelect,
                () => tvwMapSelector.AfterSelect -= tvwMapSelector_AfterSelect);

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void OrderRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton rb)) return;

            if (rb.Checked)
            {
                UpdateMapSelector();
                ChangeControlsState(false);
            }
        }

        private void tvwMapSelector_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is IMapNode mapNode)
            {
                var entry = _mapHeaderEntry[mapNode.MapBankIndex][mapNode.MapNumberIndex];
                LoadDataToUI(entry);
            }
            else
            {
                ChangeControlsState(false);
            }
        }

        private void LoadDataToUI(dynamic entry)
        {
            if (entry.Fields[0].Offset != Constants.InvalidValue)
            {
                // まずコントロールを有効化
                ChangeControlsState(true);

                // マップヘッダー
                var MapFooterOffset = 
                    entry.MapFooterOffset.GetData<int>();
                txtMapFooterOffset.Text =
                    ConvHelper.ParseIntToString(MapFooterOffset);
                txtEventScriptHeaderOffset.Text =
                    ConvHelper.ParseIntToString(
                        entry.EventScriptHeaderOffset.GetData<int>());
                txtLevelScriptOffset.Text =
                    ConvHelper.ParseIntToString(
                        entry.LevelScriptOffset.GetData<int>());
                txtConnHeaderOffset.Text =
                    ConvHelper.ParseIntToString(
                        entry.ConnHeaderOffset.GetData<int>());
                nudMapTerrainIndex.Value =
                    entry.MapTerrainIndex
                    .GetData<int>();
                cmbMapType.SelectedIndex =
                    entry.MapType
                    .GetData<int>();
                nudMapRelLayer.Value =
                    entry.MapRelLayer
                    .GetData<int>();
                cmbMapWthr.SelectedIndex =
                    entry.MapWthr
                    .GetData<int>();
                cmbMapSight.SelectedIndex =
                    entry.MapSight
                    .GetData<int>();
                cmbMapBike.SelectedIndex =
                    entry.MapBike
                    .GetData<int>();
                cmbMapSpBg.SelectedIndex =
                    entry.MapSpBg
                    .GetData<int>();
                cmbMapNameIndex.SelectedValue =
                    entry.MapNameIndex
                    .GetData<int>();
                cmbMapNameType.SelectedIndex =
                    entry.MapNameType
                    .GetData<int>();
                nudBgmIndex.Value =
                    entry.BgmIndex
                    .GetData<int>();

                ReadMapFooter(MapFooterOffset);
            }
            else
            {
                ChangeControlsState(false);
            }
        }

        private void ChangeControlsState(bool value)
        {
            // 値はリセットされる
            CtrlHelper.ResetControls(
                grpMapHeader,
                includeSelf: true);

            CtrlHelper.SetControlsEnabled(
                grpMapHeader,
                enabled: value,
                includeSelf: false);

            // 他のクリアコントロールも追加
        }

        private void ReadMapFooter(int offset)
        {
            if (offset != Constants.InvalidValue)
            {
                // マップフッターの定義情報を読み込む
                var mapFooterDef = FieldMetaDataReader.Create("MapFooterEntry");

                var entryFields = new List<FieldValueHolder>();
                for (int i = 0; i < mapFooterDef.Count; i++)
                {
                    // FieldValueを生成
                    var fieldValue = new FieldValueHolder(
                        mapFooterDef[i],
                        _sharedData);

                    entryFields.Add(fieldValue);
                }
                _mapFooterEntry = new Entry(
                    offset,
                    0,
                    entryFields);
            }
            else
            {
                _mapFooterEntry = null;
            }
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshFromData()
        {

        }
    }
}
