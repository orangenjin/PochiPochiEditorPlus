using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Helpers._MatchHelper;
using PochiPochiEditorPlus._Managers._FieldManager;
using PochiPochiEditorPlus._Managers._FormGroupManager;
using PochiPochiEditorPlus._Managers._LayerManager;
using PochiPochiEditorPlus._Managers._MapManager;
using PochiPochiEditorPlus._Managers._UndoManager;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Forms
{
    [FormGroup(FormGroup.OwMap, 0)]
    public partial class OwMapEditor0 : Form, IEditorRefresh
    {
        // 共有データ用
        private dynamic _sharedData = null;
        private dynamic _groupData = null;
        // 変更履歴用
        private CommandManager _commandManager = null;
        // イベント登録・解除用
        private EventBinder _eventBinder = null;
        // 各テーブル用
        private dynamic _mapNameEntry = null;
        private dynamic _mapHeaderEntry = null;
        private dynamic _mapFooterEntry = null;
        private dynamic _mapGridDataEntry = null;
        // パネル描画用
        private dynamic _mapLayerHolder = null;
        private LayerScroller _mapLayerScroller = null;
        // UI制御用
        private MapTreeNode _currentMapNode = null;
        private ClipboardData _clipboardData = null;

        // 定義情報を事前に計算するため
        private List<FieldMetaData> _mapFooterDef = null;
        // 基準インデックスの計算を省略するため
        private int _mapNameFirstIndex = 0;
        private Dictionary<int, string> _mapNameCache = null;

        // ノードからエントリーインデックスを取得するため
        public sealed class MapTreeNode : TreeNode
        {
            public int MapBankIndex { get; }
            public int MapNumberIndex { get; }

            public MapTreeNode(string text, int bank, int number) : base(text)
            {
                MapBankIndex = bank;
                MapNumberIndex = number;
            }
        }

        public enum MapEditorLayerNames0
        {
            MapLower,
            MapUpper,
            Grid,
            Paste
        }

        public OwMapEditor0(
            SharedData sharedData,
            CommandManager commandManager,
            FormGroupData groupData)
        {
            InitializeComponent();
            _sharedData = sharedData;
            _commandManager = commandManager;
            _groupData = groupData;
            _eventBinder = new EventBinder();

            // マップ画像パネル
            _mapLayerHolder = new LayerHolder<MapEditorLayerNames0>(pnlMapDraw, _eventBinder);
            _mapLayerScroller = new LayerScroller(
                pnlMapDraw,
                hsbMapDraw,
                vsbMapDraw,
                _mapLayerHolder.Data,
                _eventBinder);

            // 定義情報を事前に計算
            _mapFooterDef = FieldMetaDataReader.Create("MapFooterEntry");

            // グループデータを登録
            RegisterFormGroupData(_groupData);

            InitializeMapNameEntry(); // 先に処理
            InitializeControls();
            InitializeLayers();
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
            groupData.Register(this, () => _clipboardData);
            groupData.Register(this, () => _mapFooterEntry);
        }

        private void InitializeMapNameEntry()
        {
            // マップ名テーブルを作成
            int tableOffset = _sharedData.Config.MapNameTableOffset;

            // ポインタエントリー数を仮カウント（誤って含まれている可能性あり）
            var pointerPattern = new List<TokenData>() { TokenData.Pointer() };
            var pointerCount = PatternMatcher.TryCountByPattern(
                pointerPattern,
                _sharedData.RomData,
                tableOffset,
                allowNullPointer: false); // nullポインタを許容しない

            // マップ名ポインタのオフセットをすべて取得
            var mapNameCount = 0;
            var mapNameAllowedLength = _sharedData.Config.MapNameAllowedLength;
            for (int i = 0; i < pointerCount; i++)
            {
                IoHelper.TryReadPtr(
                    _sharedData.RomData,
                    tableOffset + i * Constants.UIntSize,
                    out int mapNameOffset);

                // 終端文字が１つ以上あるか判定
                var IsValid = PatternMatcher.TrySearch(
                    _sharedData.RomData,
                    new byte[] { Constants.StrTerminatorByte },
                    mapNameOffset,
                    mapNameAllowedLength);

                // 終端文字が無ければ終了
                if (!IsValid) break;

                mapNameCount++;
            }

            _mapNameEntry = 
                new EntryManager("MapNamePointerEntry", tableOffset, mapNameCount, _sharedData);
        }

        private void InitializeControls()
        {
            // ダブルバッファリングを有効化
            CtrlHelper.EnableDoubleBuffering(
                pnlMapDraw,
                pnlBorderDraw);

            // 各コンボボックスにアイテムを追加
            CtrlHelper.LoadComboBoxFromFile(
                (cmbMapType, "txt/map/MapType.txt"),
                (cmbMapWthr, "txt/map/MapWthr.txt"),
                (cmbMapSight, "txt/map/MapSight.txt"),
                (cmbMapBike, "txt/map/MapBike.txt"),
                (cmbMapSpBg, "txt/map/MapSpBg.txt"),
                (cmbMapNameType, "txt/map/MapNameType.txt"));
        }

        private void InitializeLayers()
        {
            // マップレイヤー
            _mapLayerHolder.AddLayer<BlockImageLayer>(MapEditorLayerNames0.MapLower);
            _mapLayerHolder.AddLayer<BlockImageLayer>(MapEditorLayerNames0.MapUpper);
            _mapLayerHolder.AddLayer<GridLayer>(MapEditorLayerNames0.Grid);
            _mapLayerHolder.AddLayer<PasteLayer>(MapEditorLayerNames0.Paste);
        }

        private void InitializeMapHeaderEntry()
        {
            // マップヘッダーエントリー格納先を先に作成
            _mapHeaderEntry = new List<Entry[]>();

            // エントリー数を仮カウント（誤って含まれている可能性あり）
            var pointerPattern = new List<TokenData>(){ TokenData.Pointer() };
            int tableOffset = _sharedData.Config.MapBankTableOffset;
            var bankEntryCount = PatternMatcher.TryCountByPattern(
                pointerPattern,
                _sharedData.RomData,
                tableOffset,
                allowNullPointer: false); // nullポインタを許容しない

            // マップバンクテーブルを仮作成
            dynamic mapBankEntry = 
                new EntryManager("MapBankPointerEntry", tableOffset, bankEntryCount, _sharedData);

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

            try
            {
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
            }
            finally
            {
                cmbMapNameIndex.EndUpdate();
            }
        }

        private void LoadMapNames()
        {
            _mapNameCache = new Dictionary<int, string>();

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
                () => CtrlHelper.AttachBorder(grpMapView, pnlMapDraw, pnlBorderDraw),
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
                ClearData();
                _groupData.RequestRefresh(this);
            }
        }

        private void tvwMapSelector_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is MapTreeNode mapNode)
            {
                _currentMapNode = mapNode;
                LoadDataToUI();
            }
            else
            {
                _currentMapNode = null;
                ClearData();
            }

            // 他のフォームの再描画
            _groupData.RequestRefresh(this);


            ReadMapGridData();
        }

        private void LoadDataToUI()
        {
            var entry = _mapHeaderEntry[_currentMapNode.MapBankIndex][_currentMapNode.MapNumberIndex];

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
                cmbMapType.SelectedValue =
                    entry.MapType
                    .GetData<byte>();
                nudMapRelLayer.Value =
                    entry.MapRelLayer
                    .GetData<int>();
                cmbMapWthr.SelectedValue =
                    entry.MapWthr
                    .GetData<byte>();
                cmbMapSight.SelectedValue =
                    entry.MapSight
                    .GetData<byte>();
                cmbMapBike.SelectedValue =
                    entry.MapBike
                    .GetData<byte>();
                cmbMapSpBg.SelectedValue =
                    entry.MapSpBg
                    .GetData<byte>();
                cmbMapNameIndex.SelectedValue =
                    entry.MapNameIndex
                    .GetData<int>();
                cmbMapNameType.SelectedValue =
                    entry.MapNameType
                    .GetData<byte>();
                nudBgmIndex.Value =
                    entry.BgmIndex
                    .GetData<int>();

                ReadMapFooter(MapFooterOffset);
            }
            else
            {
                ClearData();
            }
        }

        private void ChangeControlsState(bool state)
        {
            CtrlHelper.ResetControls(
                grpMapHeader,
                includeSelf: false);
            CtrlHelper.SetControlsEnabled(
                grpMapHeader,
                enabled: state,
                includeSelf: true);
            CtrlHelper.ResetControls(
                grpMapView,
                includeSelf: false);
            CtrlHelper.SetControlsEnabled(
                grpMapView,
                enabled: state,
                includeSelf: true);

            // タイル画像のレイヤー表示切り替え
            _mapLayerHolder.SetAllLayersVisibility(state);
            _mapLayerHolder.Panel.Invalidate();
        }

        private void ClearData()
        {
            ChangeControlsState(false);
            _mapFooterEntry = null;
        }

        private void ReadMapFooter(int offset)
        {
            if (offset != Constants.InvalidValue)
            {
                var entryFields = new List<FieldValueHolder>();
                for (int i = 0; i < _mapFooterDef.Count; i++)
                {
                    // FieldValueを生成
                    var fieldValue = new FieldValueHolder(
                        _mapFooterDef[i],
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

        private void ReadMapGridData()
        {
            if (_mapFooterEntry == null) return;

            // オフセットの確認
            int tableOffset = _mapFooterEntry.MapGridDataOffset.GetData<int>();
            if (tableOffset == Constants.InvalidValue) return;

            int entryCount = _mapFooterEntry.MapWidth.GetData<int>() * _mapFooterEntry.MapHeight.GetData<int>();
            _mapGridDataEntry = new EntryManager("MapGridDataEntry", tableOffset, entryCount, _sharedData);

            DrawEntireMap();
        }

        private void DrawEntireMap()
        {
            if (_mapGridDataEntry == null) return;

            // ブロック画像レイヤーを取得
            var sourceBlockLayerHolder = _groupData._blockLayerHolder;
            if (sourceBlockLayerHolder == null) return;

            var lowerLayer = sourceBlockLayerHolder.BlockLower as BlockImageLayer;
            var upperLayer = sourceBlockLayerHolder.BlockUpper as BlockImageLayer;
            if (lowerLayer == null || upperLayer == null) return;

            // マップのサイズ設定
            int blockSize = sourceBlockLayerHolder.Data.GridSize;
            int mapWidth = _mapFooterEntry.MapWidth.GetData<int>();
            int mapHeight = _mapFooterEntry.MapHeight.GetData<int>();
            int totalGrids = mapWidth * mapHeight;

            // レイヤーの初期設定
            _mapLayerHolder.Initialize(
                gridSize: blockSize,
                validItemCount: totalGrids);

            // マップの幅と高さを再設定する。
            _mapLayerHolder.Data.Columns = mapWidth;
            _mapLayerHolder.Data.Rows = mapHeight;

            // 配列の画像領域を再確保
            _mapLayerHolder.MapLower.Allocate();
            _mapLayerHolder.MapUpper.Allocate();

            int sourceColumns = sourceBlockLayerHolder.Data.Columns;

            for (int i = 0; i < totalGrids; i++)
            {
                // ブロックのインデックスを取得
                var field = _mapGridDataEntry.Entries[i].MapGridData;
                var mapGridData = MapDrawCalc.GetBlockTileData(field);
                int blockIndex = mapGridData.BlockIndex;

                // 参照元のマス座標を計算
                int sourceGridX = blockIndex % sourceColumns;
                int sourceGridY = blockIndex / sourceColumns;

                // 下位と上位の画像を取得
                var lowerImg = lowerLayer.GetBlockImage(sourceGridX, sourceGridY);
                var upperImg = upperLayer.GetBlockImage(sourceGridX, sourceGridY);

                // 書き込み先のマス座標を計算
                int destGridX = i % mapWidth;
                int destGridY = i / mapWidth;

                // 画像をセット
                _mapLayerHolder.MapLower.SetBlockImage(destGridX, destGridY, lowerImg);
                _mapLayerHolder.MapUpper.SetBlockImage(destGridX, destGridY, upperImg);
            }

            // スクロールバーの更新と再描画
            _mapLayerScroller.UpdateScrollRange();
            _mapLayerHolder.Panel.Invalidate();
        }

        /// <summary>
        /// FormGroupManagerからのUI再描画用の処理。
        /// </summary>
        public void RefreshUI()
        {
            LoadDataToUI();
        }
    }
}
