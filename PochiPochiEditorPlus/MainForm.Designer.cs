
namespace PochiPochiEditorPlus
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.mnsItemLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemTool = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemLoadRom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemReload = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemSaveOver = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemFsf = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemTilesetIndexCalc = new System.Windows.Forms.ToolStripMenuItem();
            this.grpEditors = new System.Windows.Forms.GroupBox();
            this.btnPokeData = new System.Windows.Forms.Button();
            this.btnDexRegional = new System.Windows.Forms.Button();
            this.btnDexNational = new System.Windows.Forms.Button();
            this.btnDexSearch = new System.Windows.Forms.Button();
            this.btnDexHabitat = new System.Windows.Forms.Button();
            this.btnEggMove = new System.Windows.Forms.Button();
            this.btnTmHmTutor = new System.Windows.Forms.Button();
            this.btnRegionMap = new System.Windows.Forms.Button();
            this.btnOwSprite = new System.Windows.Forms.Button();
            this.btnTileset = new System.Windows.Forms.Button();
            this.btnOwMap = new System.Windows.Forms.Button();
            this.btnRoaming = new System.Windows.Forms.Button();
            this.btnSwarm = new System.Windows.Forms.Button();
            this.btnWildEnc = new System.Windows.Forms.Button();
            this.btnBattleBg = new System.Windows.Forms.Button();
            this.btnInGameTrade = new System.Windows.Forms.Button();
            this.btnTrainerList = new System.Windows.Forms.Button();
            this.btnTrainerSprite = new System.Windows.Forms.Button();
            this.btnTrainerClass = new System.Windows.Forms.Button();
            this.btnMailData = new System.Windows.Forms.Button();
            this.btnItemData = new System.Windows.Forms.Button();
            this.grpHistory = new System.Windows.Forms.GroupBox();
            this.mnsItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.lstHistory = new System.Windows.Forms.ListBox();
            this.mnsMain.SuspendLayout();
            this.grpEditors.SuspendLayout();
            this.grpHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsMain
            // 
            this.mnsMain.GripMargin = new System.Windows.Forms.Padding(0);
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnsItemLoad,
            this.mnsItemSave,
            this.mnsItemEdit,
            this.mnsItemTool});
            this.mnsMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Padding = new System.Windows.Forms.Padding(0);
            this.mnsMain.Size = new System.Drawing.Size(492, 24);
            this.mnsMain.TabIndex = 0;
            this.mnsMain.Text = "メニューバー";
            // 
            // mnsItemLoad
            // 
            this.mnsItemLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnsItemLoadRom,
            this.mnsItemReload});
            this.mnsItemLoad.Name = "mnsItemLoad";
            this.mnsItemLoad.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemLoad.Size = new System.Drawing.Size(57, 24);
            this.mnsItemLoad.Text = "読み込み";
            // 
            // mnsItemSave
            // 
            this.mnsItemSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnsItemSaveOver,
            this.mnsItemSaveAs});
            this.mnsItemSave.Name = "mnsItemSave";
            this.mnsItemSave.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemSave.Size = new System.Drawing.Size(35, 24);
            this.mnsItemSave.Text = "保存";
            // 
            // mnsItemTool
            // 
            this.mnsItemTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnsItemFsf,
            this.mnsItemTilesetIndexCalc});
            this.mnsItemTool.Name = "mnsItemTool";
            this.mnsItemTool.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemTool.Size = new System.Drawing.Size(38, 24);
            this.mnsItemTool.Text = "ツール";
            // 
            // mnsItemLoadRom
            // 
            this.mnsItemLoadRom.Name = "mnsItemLoadRom";
            this.mnsItemLoadRom.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemLoadRom.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnsItemLoadRom.Size = new System.Drawing.Size(208, 20);
            this.mnsItemLoadRom.Text = "ROMを読み込み";
            // 
            // mnsItemReload
            // 
            this.mnsItemReload.Name = "mnsItemReload";
            this.mnsItemReload.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnsItemReload.Size = new System.Drawing.Size(208, 20);
            this.mnsItemReload.Text = "ROMを再読み込み";
            // 
            // mnsItemSaveOver
            // 
            this.mnsItemSaveOver.Name = "mnsItemSaveOver";
            this.mnsItemSaveOver.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemSaveOver.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnsItemSaveOver.Size = new System.Drawing.Size(232, 20);
            this.mnsItemSaveOver.Text = "上書き保存";
            // 
            // mnsItemSaveAs
            // 
            this.mnsItemSaveAs.Name = "mnsItemSaveAs";
            this.mnsItemSaveAs.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.mnsItemSaveAs.Size = new System.Drawing.Size(232, 20);
            this.mnsItemSaveAs.Text = "名前を付けて保存";
            // 
            // mnsItemFsf
            // 
            this.mnsItemFsf.Name = "mnsItemFsf";
            this.mnsItemFsf.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemFsf.Size = new System.Drawing.Size(180, 20);
            this.mnsItemFsf.Text = "空き領域検索";
            // 
            // mnsItemTilesetIndexCalc
            // 
            this.mnsItemTilesetIndexCalc.Name = "mnsItemTilesetIndexCalc";
            this.mnsItemTilesetIndexCalc.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemTilesetIndexCalc.Size = new System.Drawing.Size(180, 20);
            this.mnsItemTilesetIndexCalc.Text = "タイルセット番号計算";
            // 
            // grpEditors
            // 
            this.grpEditors.Controls.Add(this.btnBattleBg);
            this.grpEditors.Controls.Add(this.btnInGameTrade);
            this.grpEditors.Controls.Add(this.btnTrainerList);
            this.grpEditors.Controls.Add(this.btnTrainerSprite);
            this.grpEditors.Controls.Add(this.btnTrainerClass);
            this.grpEditors.Controls.Add(this.btnMailData);
            this.grpEditors.Controls.Add(this.btnItemData);
            this.grpEditors.Controls.Add(this.btnRegionMap);
            this.grpEditors.Controls.Add(this.btnOwSprite);
            this.grpEditors.Controls.Add(this.btnTileset);
            this.grpEditors.Controls.Add(this.btnOwMap);
            this.grpEditors.Controls.Add(this.btnRoaming);
            this.grpEditors.Controls.Add(this.btnSwarm);
            this.grpEditors.Controls.Add(this.btnWildEnc);
            this.grpEditors.Controls.Add(this.btnEggMove);
            this.grpEditors.Controls.Add(this.btnTmHmTutor);
            this.grpEditors.Controls.Add(this.btnDexSearch);
            this.grpEditors.Controls.Add(this.btnDexHabitat);
            this.grpEditors.Controls.Add(this.btnDexNational);
            this.grpEditors.Controls.Add(this.btnDexRegional);
            this.grpEditors.Controls.Add(this.btnPokeData);
            this.grpEditors.Location = new System.Drawing.Point(20, 34);
            this.grpEditors.Margin = new System.Windows.Forms.Padding(0);
            this.grpEditors.Name = "grpEditors";
            this.grpEditors.Padding = new System.Windows.Forms.Padding(0);
            this.grpEditors.Size = new System.Drawing.Size(450, 282);
            this.grpEditors.TabIndex = 1;
            this.grpEditors.TabStop = false;
            this.grpEditors.Text = "編集項目";
            // 
            // btnPokeData
            // 
            this.btnPokeData.Location = new System.Drawing.Point(20, 28);
            this.btnPokeData.Margin = new System.Windows.Forms.Padding(0);
            this.btnPokeData.Name = "btnPokeData";
            this.btnPokeData.Size = new System.Drawing.Size(128, 23);
            this.btnPokeData.TabIndex = 0;
            this.btnPokeData.Text = "ポケモン";
            this.btnPokeData.UseVisualStyleBackColor = true;
            // 
            // btnDexRegional
            // 
            this.btnDexRegional.Location = new System.Drawing.Point(20, 58);
            this.btnDexRegional.Margin = new System.Windows.Forms.Padding(0);
            this.btnDexRegional.Name = "btnDexRegional";
            this.btnDexRegional.Size = new System.Drawing.Size(128, 23);
            this.btnDexRegional.TabIndex = 0;
            this.btnDexRegional.Text = "図鑑番号(地方)";
            this.btnDexRegional.UseVisualStyleBackColor = true;
            // 
            // btnDexNational
            // 
            this.btnDexNational.Location = new System.Drawing.Point(20, 88);
            this.btnDexNational.Margin = new System.Windows.Forms.Padding(0);
            this.btnDexNational.Name = "btnDexNational";
            this.btnDexNational.Size = new System.Drawing.Size(128, 23);
            this.btnDexNational.TabIndex = 0;
            this.btnDexNational.Text = "図鑑番号(全国)";
            this.btnDexNational.UseVisualStyleBackColor = true;
            // 
            // btnDexSearch
            // 
            this.btnDexSearch.Location = new System.Drawing.Point(20, 148);
            this.btnDexSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnDexSearch.Name = "btnDexSearch";
            this.btnDexSearch.Size = new System.Drawing.Size(128, 23);
            this.btnDexSearch.TabIndex = 1;
            this.btnDexSearch.Text = "図鑑索引";
            this.btnDexSearch.UseVisualStyleBackColor = true;
            // 
            // btnDexHabitat
            // 
            this.btnDexHabitat.Location = new System.Drawing.Point(20, 118);
            this.btnDexHabitat.Margin = new System.Windows.Forms.Padding(0);
            this.btnDexHabitat.Name = "btnDexHabitat";
            this.btnDexHabitat.Size = new System.Drawing.Size(128, 23);
            this.btnDexHabitat.TabIndex = 2;
            this.btnDexHabitat.Text = "図鑑生息地";
            this.btnDexHabitat.UseVisualStyleBackColor = true;
            // 
            // btnEggMove
            // 
            this.btnEggMove.Location = new System.Drawing.Point(20, 238);
            this.btnEggMove.Margin = new System.Windows.Forms.Padding(0);
            this.btnEggMove.Name = "btnEggMove";
            this.btnEggMove.Size = new System.Drawing.Size(128, 23);
            this.btnEggMove.TabIndex = 4;
            this.btnEggMove.Text = "タマゴ技";
            this.btnEggMove.UseVisualStyleBackColor = true;
            // 
            // btnTmHmTutor
            // 
            this.btnTmHmTutor.Location = new System.Drawing.Point(20, 208);
            this.btnTmHmTutor.Margin = new System.Windows.Forms.Padding(0);
            this.btnTmHmTutor.Name = "btnTmHmTutor";
            this.btnTmHmTutor.Size = new System.Drawing.Size(128, 23);
            this.btnTmHmTutor.TabIndex = 5;
            this.btnTmHmTutor.Text = "技マシン / 教え技";
            this.btnTmHmTutor.UseVisualStyleBackColor = true;
            // 
            // btnRegionMap
            // 
            this.btnRegionMap.Location = new System.Drawing.Point(160, 238);
            this.btnRegionMap.Margin = new System.Windows.Forms.Padding(0);
            this.btnRegionMap.Name = "btnRegionMap";
            this.btnRegionMap.Size = new System.Drawing.Size(128, 23);
            this.btnRegionMap.TabIndex = 11;
            this.btnRegionMap.Text = "タウンマップ";
            this.btnRegionMap.UseVisualStyleBackColor = true;
            // 
            // btnOwSprite
            // 
            this.btnOwSprite.Location = new System.Drawing.Point(160, 208);
            this.btnOwSprite.Margin = new System.Windows.Forms.Padding(0);
            this.btnOwSprite.Name = "btnOwSprite";
            this.btnOwSprite.Size = new System.Drawing.Size(128, 23);
            this.btnOwSprite.TabIndex = 12;
            this.btnOwSprite.Text = "歩行グラフィック";
            this.btnOwSprite.UseVisualStyleBackColor = true;
            // 
            // btnTileset
            // 
            this.btnTileset.Location = new System.Drawing.Point(160, 178);
            this.btnTileset.Margin = new System.Windows.Forms.Padding(0);
            this.btnTileset.Name = "btnTileset";
            this.btnTileset.Size = new System.Drawing.Size(128, 23);
            this.btnTileset.TabIndex = 9;
            this.btnTileset.Text = "タイルセット";
            this.btnTileset.UseVisualStyleBackColor = true;
            // 
            // btnOwMap
            // 
            this.btnOwMap.Location = new System.Drawing.Point(160, 148);
            this.btnOwMap.Margin = new System.Windows.Forms.Padding(0);
            this.btnOwMap.Name = "btnOwMap";
            this.btnOwMap.Size = new System.Drawing.Size(128, 23);
            this.btnOwMap.TabIndex = 10;
            this.btnOwMap.Text = "マップ";
            this.btnOwMap.UseVisualStyleBackColor = true;
            // 
            // btnRoaming
            // 
            this.btnRoaming.Location = new System.Drawing.Point(160, 88);
            this.btnRoaming.Margin = new System.Windows.Forms.Padding(0);
            this.btnRoaming.Name = "btnRoaming";
            this.btnRoaming.Size = new System.Drawing.Size(128, 23);
            this.btnRoaming.TabIndex = 6;
            this.btnRoaming.Text = "徘徊位置";
            this.btnRoaming.UseVisualStyleBackColor = true;
            // 
            // btnSwarm
            // 
            this.btnSwarm.Location = new System.Drawing.Point(160, 58);
            this.btnSwarm.Margin = new System.Windows.Forms.Padding(0);
            this.btnSwarm.Name = "btnSwarm";
            this.btnSwarm.Size = new System.Drawing.Size(128, 23);
            this.btnSwarm.TabIndex = 7;
            this.btnSwarm.Text = "大量発生";
            this.btnSwarm.UseVisualStyleBackColor = true;
            // 
            // btnWildEnc
            // 
            this.btnWildEnc.Location = new System.Drawing.Point(160, 28);
            this.btnWildEnc.Margin = new System.Windows.Forms.Padding(0);
            this.btnWildEnc.Name = "btnWildEnc";
            this.btnWildEnc.Size = new System.Drawing.Size(128, 23);
            this.btnWildEnc.TabIndex = 8;
            this.btnWildEnc.Text = "野生設定";
            this.btnWildEnc.UseVisualStyleBackColor = true;
            // 
            // btnBattleBg
            // 
            this.btnBattleBg.Location = new System.Drawing.Point(300, 238);
            this.btnBattleBg.Margin = new System.Windows.Forms.Padding(0);
            this.btnBattleBg.Name = "btnBattleBg";
            this.btnBattleBg.Size = new System.Drawing.Size(128, 23);
            this.btnBattleBg.TabIndex = 18;
            this.btnBattleBg.Text = "戦闘背景";
            this.btnBattleBg.UseVisualStyleBackColor = true;
            // 
            // btnInGameTrade
            // 
            this.btnInGameTrade.Location = new System.Drawing.Point(300, 208);
            this.btnInGameTrade.Margin = new System.Windows.Forms.Padding(0);
            this.btnInGameTrade.Name = "btnInGameTrade";
            this.btnInGameTrade.Size = new System.Drawing.Size(128, 23);
            this.btnInGameTrade.TabIndex = 19;
            this.btnInGameTrade.Text = "ゲーム内交換";
            this.btnInGameTrade.UseVisualStyleBackColor = true;
            // 
            // btnTrainerList
            // 
            this.btnTrainerList.Location = new System.Drawing.Point(300, 178);
            this.btnTrainerList.Margin = new System.Windows.Forms.Padding(0);
            this.btnTrainerList.Name = "btnTrainerList";
            this.btnTrainerList.Size = new System.Drawing.Size(128, 23);
            this.btnTrainerList.TabIndex = 16;
            this.btnTrainerList.Text = "トレーナーデータ";
            this.btnTrainerList.UseVisualStyleBackColor = true;
            // 
            // btnTrainerSprite
            // 
            this.btnTrainerSprite.Location = new System.Drawing.Point(300, 148);
            this.btnTrainerSprite.Margin = new System.Windows.Forms.Padding(0);
            this.btnTrainerSprite.Name = "btnTrainerSprite";
            this.btnTrainerSprite.Size = new System.Drawing.Size(128, 23);
            this.btnTrainerSprite.TabIndex = 17;
            this.btnTrainerSprite.Text = "トレーナー画像";
            this.btnTrainerSprite.UseVisualStyleBackColor = true;
            // 
            // btnTrainerClass
            // 
            this.btnTrainerClass.Location = new System.Drawing.Point(300, 118);
            this.btnTrainerClass.Margin = new System.Windows.Forms.Padding(0);
            this.btnTrainerClass.Name = "btnTrainerClass";
            this.btnTrainerClass.Size = new System.Drawing.Size(128, 23);
            this.btnTrainerClass.TabIndex = 13;
            this.btnTrainerClass.Text = "トレーナー肩書き";
            this.btnTrainerClass.UseVisualStyleBackColor = true;
            // 
            // btnMailData
            // 
            this.btnMailData.Location = new System.Drawing.Point(300, 58);
            this.btnMailData.Margin = new System.Windows.Forms.Padding(0);
            this.btnMailData.Name = "btnMailData";
            this.btnMailData.Size = new System.Drawing.Size(128, 23);
            this.btnMailData.TabIndex = 14;
            this.btnMailData.Text = "メール内容";
            this.btnMailData.UseVisualStyleBackColor = true;
            // 
            // btnItemData
            // 
            this.btnItemData.Location = new System.Drawing.Point(300, 28);
            this.btnItemData.Margin = new System.Windows.Forms.Padding(0);
            this.btnItemData.Name = "btnItemData";
            this.btnItemData.Size = new System.Drawing.Size(128, 23);
            this.btnItemData.TabIndex = 15;
            this.btnItemData.Text = "アイテム";
            this.btnItemData.UseVisualStyleBackColor = true;
            // 
            // grpHistory
            // 
            this.grpHistory.Controls.Add(this.lstHistory);
            this.grpHistory.Location = new System.Drawing.Point(20, 326);
            this.grpHistory.Margin = new System.Windows.Forms.Padding(0);
            this.grpHistory.Name = "grpHistory";
            this.grpHistory.Padding = new System.Windows.Forms.Padding(0);
            this.grpHistory.Size = new System.Drawing.Size(450, 174);
            this.grpHistory.TabIndex = 2;
            this.grpHistory.TabStop = false;
            this.grpHistory.Text = "変更履歴";
            // 
            // mnsItemEdit
            // 
            this.mnsItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnsItemUndo,
            this.mnsItemRedo});
            this.mnsItemEdit.Name = "mnsItemEdit";
            this.mnsItemEdit.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemEdit.Size = new System.Drawing.Size(35, 24);
            this.mnsItemEdit.Text = "編集";
            // 
            // mnsItemUndo
            // 
            this.mnsItemUndo.Name = "mnsItemUndo";
            this.mnsItemUndo.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnsItemUndo.Size = new System.Drawing.Size(180, 20);
            this.mnsItemUndo.Text = "元に戻す";
            // 
            // mnsItemRedo
            // 
            this.mnsItemRedo.Name = "mnsItemRedo";
            this.mnsItemRedo.Padding = new System.Windows.Forms.Padding(0);
            this.mnsItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.mnsItemRedo.Size = new System.Drawing.Size(180, 20);
            this.mnsItemRedo.Text = "やり直す";
            // 
            // lstHistory
            // 
            this.lstHistory.FormattingEnabled = true;
            this.lstHistory.ItemHeight = 15;
            this.lstHistory.Location = new System.Drawing.Point(20, 28);
            this.lstHistory.Margin = new System.Windows.Forms.Padding(0);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(408, 124);
            this.lstHistory.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 519);
            this.Controls.Add(this.grpHistory);
            this.Controls.Add(this.grpEditors);
            this.Controls.Add(this.mnsMain);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnsMain;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "メイン画面";
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.grpEditors.ResumeLayout(false);
            this.grpHistory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem mnsItemLoad;
        private System.Windows.Forms.ToolStripMenuItem mnsItemSave;
        private System.Windows.Forms.ToolStripMenuItem mnsItemTool;
        private System.Windows.Forms.ToolStripMenuItem mnsItemLoadRom;
        private System.Windows.Forms.ToolStripMenuItem mnsItemReload;
        private System.Windows.Forms.ToolStripMenuItem mnsItemSaveOver;
        private System.Windows.Forms.ToolStripMenuItem mnsItemSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnsItemFsf;
        private System.Windows.Forms.ToolStripMenuItem mnsItemTilesetIndexCalc;
        private System.Windows.Forms.GroupBox grpEditors;
        private System.Windows.Forms.Button btnPokeData;
        private System.Windows.Forms.Button btnDexNational;
        private System.Windows.Forms.Button btnDexRegional;
        private System.Windows.Forms.Button btnEggMove;
        private System.Windows.Forms.Button btnTmHmTutor;
        private System.Windows.Forms.Button btnDexSearch;
        private System.Windows.Forms.Button btnDexHabitat;
        private System.Windows.Forms.Button btnRegionMap;
        private System.Windows.Forms.Button btnOwSprite;
        private System.Windows.Forms.Button btnTileset;
        private System.Windows.Forms.Button btnOwMap;
        private System.Windows.Forms.Button btnRoaming;
        private System.Windows.Forms.Button btnSwarm;
        private System.Windows.Forms.Button btnWildEnc;
        private System.Windows.Forms.Button btnBattleBg;
        private System.Windows.Forms.Button btnInGameTrade;
        private System.Windows.Forms.Button btnTrainerList;
        private System.Windows.Forms.Button btnTrainerSprite;
        private System.Windows.Forms.Button btnTrainerClass;
        private System.Windows.Forms.Button btnMailData;
        private System.Windows.Forms.Button btnItemData;
        private System.Windows.Forms.GroupBox grpHistory;
        private System.Windows.Forms.ToolStripMenuItem mnsItemEdit;
        private System.Windows.Forms.ToolStripMenuItem mnsItemUndo;
        private System.Windows.Forms.ToolStripMenuItem mnsItemRedo;
        private System.Windows.Forms.ListBox lstHistory;
    }
}

