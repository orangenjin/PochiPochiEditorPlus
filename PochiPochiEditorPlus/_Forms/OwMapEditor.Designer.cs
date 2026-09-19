
namespace PochiPochiEditorPlus._Forms
{
    partial class OwMapEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OwMapEditor));
            this.grpMapSelector = new System.Windows.Forms.GroupBox();
            this.rbOrderByAsc = new System.Windows.Forms.RadioButton();
            this.rbOrderByName = new System.Windows.Forms.RadioButton();
            this.chkOrderByTerrain = new System.Windows.Forms.CheckBox();
            this.tvwMapSelector = new System.Windows.Forms.TreeView();
            this.grpMapHeader = new System.Windows.Forms.GroupBox();
            this.lblMapFooterOffset = new System.Windows.Forms.Label();
            this.btnEditMapFooter = new System.Windows.Forms.Button();
            this.btnEditEventScriptHeader = new System.Windows.Forms.Button();
            this.lblEventScriptHeaderOffset = new System.Windows.Forms.Label();
            this.btnEditConnHeader = new System.Windows.Forms.Button();
            this.lblConnHeaderOffset = new System.Windows.Forms.Label();
            this.btnEditLevelScript = new System.Windows.Forms.Button();
            this.lblLevelScriptOffset = new System.Windows.Forms.Label();
            this.nudMapTerrainIndex = new System.Windows.Forms.NumericUpDown();
            this.cmbMapType = new System.Windows.Forms.ComboBox();
            this.lblMapType = new System.Windows.Forms.Label();
            this.lblMapTerrainIndex = new System.Windows.Forms.Label();
            this.lblMapRelLayer = new System.Windows.Forms.Label();
            this.nudMapRelLayer = new System.Windows.Forms.NumericUpDown();
            this.lblMapWthr = new System.Windows.Forms.Label();
            this.cmbMapWthr = new System.Windows.Forms.ComboBox();
            this.lblMapSight = new System.Windows.Forms.Label();
            this.cmbMapSight = new System.Windows.Forms.ComboBox();
            this.lblMapSpBg = new System.Windows.Forms.Label();
            this.cmbMapSpBg = new System.Windows.Forms.ComboBox();
            this.lblMapBike = new System.Windows.Forms.Label();
            this.cmbMapBike = new System.Windows.Forms.ComboBox();
            this.lblMapNameType = new System.Windows.Forms.Label();
            this.cmbMapNameType = new System.Windows.Forms.ComboBox();
            this.lblMapNameIndex = new System.Windows.Forms.Label();
            this.cmbMapNameIndex = new System.Windows.Forms.ComboBox();
            this.lblBgmIndex = new System.Windows.Forms.Label();
            this.nudBgmIndex = new System.Windows.Forms.NumericUpDown();
            this.grpMapView = new System.Windows.Forms.GroupBox();
            this.pnlMapDraw = new System.Windows.Forms.Panel();
            this.txtConnHeaderOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.txtLevelScriptOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.txtEventScriptHeaderOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.txtMapFooterOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.grpMapSelector.SuspendLayout();
            this.grpMapHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapTerrainIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapRelLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBgmIndex)).BeginInit();
            this.grpMapView.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMapSelector
            // 
            this.grpMapSelector.Controls.Add(this.tvwMapSelector);
            this.grpMapSelector.Controls.Add(this.chkOrderByTerrain);
            this.grpMapSelector.Controls.Add(this.rbOrderByName);
            this.grpMapSelector.Controls.Add(this.rbOrderByAsc);
            this.grpMapSelector.Location = new System.Drawing.Point(20, 16);
            this.grpMapSelector.Margin = new System.Windows.Forms.Padding(0);
            this.grpMapSelector.Name = "grpMapSelector";
            this.grpMapSelector.Padding = new System.Windows.Forms.Padding(0);
            this.grpMapSelector.Size = new System.Drawing.Size(326, 264);
            this.grpMapSelector.TabIndex = 0;
            this.grpMapSelector.TabStop = false;
            this.grpMapSelector.Text = "マップを選択";
            // 
            // rbOrderByAsc
            // 
            this.rbOrderByAsc.AutoSize = true;
            this.rbOrderByAsc.Location = new System.Drawing.Point(20, 28);
            this.rbOrderByAsc.Margin = new System.Windows.Forms.Padding(0);
            this.rbOrderByAsc.Name = "rbOrderByAsc";
            this.rbOrderByAsc.Size = new System.Drawing.Size(61, 19);
            this.rbOrderByAsc.TabIndex = 0;
            this.rbOrderByAsc.TabStop = true;
            this.rbOrderByAsc.Text = "番号順";
            this.rbOrderByAsc.UseVisualStyleBackColor = true;
            // 
            // rbOrderByName
            // 
            this.rbOrderByName.AutoSize = true;
            this.rbOrderByName.Location = new System.Drawing.Point(88, 28);
            this.rbOrderByName.Margin = new System.Windows.Forms.Padding(0);
            this.rbOrderByName.Name = "rbOrderByName";
            this.rbOrderByName.Size = new System.Drawing.Size(75, 19);
            this.rbOrderByName.TabIndex = 0;
            this.rbOrderByName.TabStop = true;
            this.rbOrderByName.Text = "マップ名順";
            this.rbOrderByName.UseVisualStyleBackColor = true;
            // 
            // chkOrderByTerrain
            // 
            this.chkOrderByTerrain.AutoSize = true;
            this.chkOrderByTerrain.Location = new System.Drawing.Point(171, 29);
            this.chkOrderByTerrain.Margin = new System.Windows.Forms.Padding(0);
            this.chkOrderByTerrain.Name = "chkOrderByTerrain";
            this.chkOrderByTerrain.Size = new System.Drawing.Size(99, 19);
            this.chkOrderByTerrain.TabIndex = 1;
            this.chkOrderByTerrain.Text = "マップ地形ID順";
            this.chkOrderByTerrain.UseVisualStyleBackColor = true;
            // 
            // tvwMapSelector
            // 
            this.tvwMapSelector.Location = new System.Drawing.Point(20, 56);
            this.tvwMapSelector.Margin = new System.Windows.Forms.Padding(0);
            this.tvwMapSelector.Name = "tvwMapSelector";
            this.tvwMapSelector.Size = new System.Drawing.Size(284, 186);
            this.tvwMapSelector.TabIndex = 2;
            // 
            // grpMapHeader
            // 
            this.grpMapHeader.Controls.Add(this.lblBgmIndex);
            this.grpMapHeader.Controls.Add(this.nudBgmIndex);
            this.grpMapHeader.Controls.Add(this.lblMapNameType);
            this.grpMapHeader.Controls.Add(this.cmbMapNameType);
            this.grpMapHeader.Controls.Add(this.lblMapNameIndex);
            this.grpMapHeader.Controls.Add(this.cmbMapNameIndex);
            this.grpMapHeader.Controls.Add(this.lblMapSpBg);
            this.grpMapHeader.Controls.Add(this.cmbMapSpBg);
            this.grpMapHeader.Controls.Add(this.lblMapBike);
            this.grpMapHeader.Controls.Add(this.cmbMapBike);
            this.grpMapHeader.Controls.Add(this.lblMapSight);
            this.grpMapHeader.Controls.Add(this.cmbMapSight);
            this.grpMapHeader.Controls.Add(this.lblMapWthr);
            this.grpMapHeader.Controls.Add(this.cmbMapWthr);
            this.grpMapHeader.Controls.Add(this.lblMapRelLayer);
            this.grpMapHeader.Controls.Add(this.nudMapRelLayer);
            this.grpMapHeader.Controls.Add(this.lblMapType);
            this.grpMapHeader.Controls.Add(this.lblMapTerrainIndex);
            this.grpMapHeader.Controls.Add(this.cmbMapType);
            this.grpMapHeader.Controls.Add(this.nudMapTerrainIndex);
            this.grpMapHeader.Controls.Add(this.btnEditConnHeader);
            this.grpMapHeader.Controls.Add(this.txtConnHeaderOffset);
            this.grpMapHeader.Controls.Add(this.lblConnHeaderOffset);
            this.grpMapHeader.Controls.Add(this.btnEditLevelScript);
            this.grpMapHeader.Controls.Add(this.txtLevelScriptOffset);
            this.grpMapHeader.Controls.Add(this.lblLevelScriptOffset);
            this.grpMapHeader.Controls.Add(this.btnEditEventScriptHeader);
            this.grpMapHeader.Controls.Add(this.txtEventScriptHeaderOffset);
            this.grpMapHeader.Controls.Add(this.lblEventScriptHeaderOffset);
            this.grpMapHeader.Controls.Add(this.btnEditMapFooter);
            this.grpMapHeader.Controls.Add(this.txtMapFooterOffset);
            this.grpMapHeader.Controls.Add(this.lblMapFooterOffset);
            this.grpMapHeader.Location = new System.Drawing.Point(20, 292);
            this.grpMapHeader.Margin = new System.Windows.Forms.Padding(0);
            this.grpMapHeader.Name = "grpMapHeader";
            this.grpMapHeader.Padding = new System.Windows.Forms.Padding(0);
            this.grpMapHeader.Size = new System.Drawing.Size(326, 458);
            this.grpMapHeader.TabIndex = 1;
            this.grpMapHeader.TabStop = false;
            this.grpMapHeader.Text = "マップヘッダー";
            // 
            // lblMapFooterOffset
            // 
            this.lblMapFooterOffset.AutoSize = true;
            this.lblMapFooterOffset.Location = new System.Drawing.Point(20, 32);
            this.lblMapFooterOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapFooterOffset.Name = "lblMapFooterOffset";
            this.lblMapFooterOffset.Size = new System.Drawing.Size(107, 15);
            this.lblMapFooterOffset.TabIndex = 0;
            this.lblMapFooterOffset.Text = "マップフッターアドレス :";
            // 
            // btnEditMapFooter
            // 
            this.btnEditMapFooter.Location = new System.Drawing.Point(246, 28);
            this.btnEditMapFooter.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditMapFooter.Name = "btnEditMapFooter";
            this.btnEditMapFooter.Size = new System.Drawing.Size(58, 23);
            this.btnEditMapFooter.TabIndex = 2;
            this.btnEditMapFooter.Text = "編集";
            this.btnEditMapFooter.UseVisualStyleBackColor = true;
            // 
            // btnEditEventScriptHeader
            // 
            this.btnEditEventScriptHeader.Location = new System.Drawing.Point(246, 58);
            this.btnEditEventScriptHeader.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditEventScriptHeader.Name = "btnEditEventScriptHeader";
            this.btnEditEventScriptHeader.Size = new System.Drawing.Size(58, 23);
            this.btnEditEventScriptHeader.TabIndex = 5;
            this.btnEditEventScriptHeader.Text = "編集";
            this.btnEditEventScriptHeader.UseVisualStyleBackColor = true;
            // 
            // lblEventScriptHeaderOffset
            // 
            this.lblEventScriptHeaderOffset.AutoSize = true;
            this.lblEventScriptHeaderOffset.Location = new System.Drawing.Point(20, 62);
            this.lblEventScriptHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblEventScriptHeaderOffset.Name = "lblEventScriptHeaderOffset";
            this.lblEventScriptHeaderOffset.Size = new System.Drawing.Size(119, 15);
            this.lblEventScriptHeaderOffset.TabIndex = 3;
            this.lblEventScriptHeaderOffset.Text = "イベントヘッダーアドレス :";
            // 
            // btnEditConnHeader
            // 
            this.btnEditConnHeader.Location = new System.Drawing.Point(246, 118);
            this.btnEditConnHeader.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditConnHeader.Name = "btnEditConnHeader";
            this.btnEditConnHeader.Size = new System.Drawing.Size(58, 23);
            this.btnEditConnHeader.TabIndex = 11;
            this.btnEditConnHeader.Text = "編集";
            this.btnEditConnHeader.UseVisualStyleBackColor = true;
            // 
            // lblConnHeaderOffset
            // 
            this.lblConnHeaderOffset.AutoSize = true;
            this.lblConnHeaderOffset.Location = new System.Drawing.Point(20, 122);
            this.lblConnHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblConnHeaderOffset.Name = "lblConnHeaderOffset";
            this.lblConnHeaderOffset.Size = new System.Drawing.Size(107, 15);
            this.lblConnHeaderOffset.TabIndex = 9;
            this.lblConnHeaderOffset.Text = "接続ヘッダーアドレス :";
            // 
            // btnEditLevelScript
            // 
            this.btnEditLevelScript.Location = new System.Drawing.Point(246, 88);
            this.btnEditLevelScript.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditLevelScript.Name = "btnEditLevelScript";
            this.btnEditLevelScript.Size = new System.Drawing.Size(58, 23);
            this.btnEditLevelScript.TabIndex = 8;
            this.btnEditLevelScript.Text = "編集";
            this.btnEditLevelScript.UseVisualStyleBackColor = true;
            // 
            // lblLevelScriptOffset
            // 
            this.lblLevelScriptOffset.AutoSize = true;
            this.lblLevelScriptOffset.Location = new System.Drawing.Point(20, 92);
            this.lblLevelScriptOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblLevelScriptOffset.Name = "lblLevelScriptOffset";
            this.lblLevelScriptOffset.Size = new System.Drawing.Size(117, 15);
            this.lblLevelScriptOffset.TabIndex = 6;
            this.lblLevelScriptOffset.Text = "マップスクリプトアドレス :";
            // 
            // nudMapTerrainIndex
            // 
            this.nudMapTerrainIndex.Location = new System.Drawing.Point(136, 148);
            this.nudMapTerrainIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudMapTerrainIndex.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudMapTerrainIndex.Name = "nudMapTerrainIndex";
            this.nudMapTerrainIndex.Size = new System.Drawing.Size(100, 23);
            this.nudMapTerrainIndex.TabIndex = 12;
            // 
            // cmbMapType
            // 
            this.cmbMapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapType.FormattingEnabled = true;
            this.cmbMapType.Location = new System.Drawing.Point(136, 178);
            this.cmbMapType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapType.Name = "cmbMapType";
            this.cmbMapType.Size = new System.Drawing.Size(168, 23);
            this.cmbMapType.TabIndex = 13;
            // 
            // lblMapType
            // 
            this.lblMapType.AutoSize = true;
            this.lblMapType.Location = new System.Drawing.Point(20, 182);
            this.lblMapType.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapType.Name = "lblMapType";
            this.lblMapType.Size = new System.Drawing.Size(66, 15);
            this.lblMapType.TabIndex = 15;
            this.lblMapType.Text = "マップタイプ :";
            // 
            // lblMapTerrainIndex
            // 
            this.lblMapTerrainIndex.AutoSize = true;
            this.lblMapTerrainIndex.Location = new System.Drawing.Point(20, 152);
            this.lblMapTerrainIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapTerrainIndex.Name = "lblMapTerrainIndex";
            this.lblMapTerrainIndex.Size = new System.Drawing.Size(74, 15);
            this.lblMapTerrainIndex.TabIndex = 14;
            this.lblMapTerrainIndex.Text = "マップ地形ID :";
            // 
            // lblMapRelLayer
            // 
            this.lblMapRelLayer.AutoSize = true;
            this.lblMapRelLayer.Location = new System.Drawing.Point(20, 212);
            this.lblMapRelLayer.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapRelLayer.Name = "lblMapRelLayer";
            this.lblMapRelLayer.Size = new System.Drawing.Size(61, 15);
            this.lblMapRelLayer.TabIndex = 17;
            this.lblMapRelLayer.Text = "相対階層 :";
            // 
            // nudMapRelLayer
            // 
            this.nudMapRelLayer.Location = new System.Drawing.Point(136, 208);
            this.nudMapRelLayer.Margin = new System.Windows.Forms.Padding(0);
            this.nudMapRelLayer.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nudMapRelLayer.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.nudMapRelLayer.Name = "nudMapRelLayer";
            this.nudMapRelLayer.Size = new System.Drawing.Size(100, 23);
            this.nudMapRelLayer.TabIndex = 16;
            // 
            // lblMapWthr
            // 
            this.lblMapWthr.AutoSize = true;
            this.lblMapWthr.Location = new System.Drawing.Point(20, 242);
            this.lblMapWthr.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapWthr.Name = "lblMapWthr";
            this.lblMapWthr.Size = new System.Drawing.Size(37, 15);
            this.lblMapWthr.TabIndex = 19;
            this.lblMapWthr.Text = "天候 :";
            // 
            // cmbMapWthr
            // 
            this.cmbMapWthr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapWthr.FormattingEnabled = true;
            this.cmbMapWthr.Location = new System.Drawing.Point(136, 238);
            this.cmbMapWthr.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapWthr.Name = "cmbMapWthr";
            this.cmbMapWthr.Size = new System.Drawing.Size(168, 23);
            this.cmbMapWthr.TabIndex = 18;
            // 
            // lblMapSight
            // 
            this.lblMapSight.AutoSize = true;
            this.lblMapSight.Location = new System.Drawing.Point(20, 272);
            this.lblMapSight.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapSight.Name = "lblMapSight";
            this.lblMapSight.Size = new System.Drawing.Size(61, 15);
            this.lblMapSight.TabIndex = 21;
            this.lblMapSight.Text = "視界状況 :";
            // 
            // cmbMapSight
            // 
            this.cmbMapSight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapSight.FormattingEnabled = true;
            this.cmbMapSight.Location = new System.Drawing.Point(136, 268);
            this.cmbMapSight.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapSight.Name = "cmbMapSight";
            this.cmbMapSight.Size = new System.Drawing.Size(168, 23);
            this.cmbMapSight.TabIndex = 20;
            // 
            // lblMapSpBg
            // 
            this.lblMapSpBg.AutoSize = true;
            this.lblMapSpBg.Location = new System.Drawing.Point(20, 332);
            this.lblMapSpBg.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapSpBg.Name = "lblMapSpBg";
            this.lblMapSpBg.Size = new System.Drawing.Size(93, 15);
            this.lblMapSpBg.TabIndex = 25;
            this.lblMapSpBg.Text = "戦闘背景(特殊) :";
            // 
            // cmbMapSpBg
            // 
            this.cmbMapSpBg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapSpBg.FormattingEnabled = true;
            this.cmbMapSpBg.Location = new System.Drawing.Point(136, 328);
            this.cmbMapSpBg.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapSpBg.Name = "cmbMapSpBg";
            this.cmbMapSpBg.Size = new System.Drawing.Size(168, 23);
            this.cmbMapSpBg.TabIndex = 24;
            // 
            // lblMapBike
            // 
            this.lblMapBike.AutoSize = true;
            this.lblMapBike.Location = new System.Drawing.Point(20, 302);
            this.lblMapBike.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapBike.Name = "lblMapBike";
            this.lblMapBike.Size = new System.Drawing.Size(73, 15);
            this.lblMapBike.TabIndex = 23;
            this.lblMapBike.Text = "自転車可否 :";
            // 
            // cmbMapBike
            // 
            this.cmbMapBike.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapBike.FormattingEnabled = true;
            this.cmbMapBike.Location = new System.Drawing.Point(136, 298);
            this.cmbMapBike.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapBike.Name = "cmbMapBike";
            this.cmbMapBike.Size = new System.Drawing.Size(168, 23);
            this.cmbMapBike.TabIndex = 22;
            // 
            // lblMapNameType
            // 
            this.lblMapNameType.AutoSize = true;
            this.lblMapNameType.Location = new System.Drawing.Point(20, 392);
            this.lblMapNameType.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapNameType.Name = "lblMapNameType";
            this.lblMapNameType.Size = new System.Drawing.Size(99, 15);
            this.lblMapNameType.TabIndex = 29;
            this.lblMapNameType.Text = "マップ名表示設定 :";
            // 
            // cmbMapNameType
            // 
            this.cmbMapNameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapNameType.FormattingEnabled = true;
            this.cmbMapNameType.Location = new System.Drawing.Point(136, 388);
            this.cmbMapNameType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapNameType.Name = "cmbMapNameType";
            this.cmbMapNameType.Size = new System.Drawing.Size(168, 23);
            this.cmbMapNameType.TabIndex = 28;
            // 
            // lblMapNameIndex
            // 
            this.lblMapNameIndex.AutoSize = true;
            this.lblMapNameIndex.Location = new System.Drawing.Point(20, 362);
            this.lblMapNameIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblMapNameIndex.Name = "lblMapNameIndex";
            this.lblMapNameIndex.Size = new System.Drawing.Size(62, 15);
            this.lblMapNameIndex.TabIndex = 27;
            this.lblMapNameIndex.Text = "マップ名ID :";
            // 
            // cmbMapNameIndex
            // 
            this.cmbMapNameIndex.FormattingEnabled = true;
            this.cmbMapNameIndex.Location = new System.Drawing.Point(136, 358);
            this.cmbMapNameIndex.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMapNameIndex.Name = "cmbMapNameIndex";
            this.cmbMapNameIndex.Size = new System.Drawing.Size(168, 23);
            this.cmbMapNameIndex.TabIndex = 26;
            // 
            // lblBgmIndex
            // 
            this.lblBgmIndex.AutoSize = true;
            this.lblBgmIndex.Location = new System.Drawing.Point(20, 422);
            this.lblBgmIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblBgmIndex.Name = "lblBgmIndex";
            this.lblBgmIndex.Size = new System.Drawing.Size(61, 15);
            this.lblBgmIndex.TabIndex = 31;
            this.lblBgmIndex.Text = "BGM No. :";
            // 
            // nudBgmIndex
            // 
            this.nudBgmIndex.Location = new System.Drawing.Point(136, 418);
            this.nudBgmIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudBgmIndex.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBgmIndex.Name = "nudBgmIndex";
            this.nudBgmIndex.Size = new System.Drawing.Size(100, 23);
            this.nudBgmIndex.TabIndex = 30;
            // 
            // grpMapView
            // 
            this.grpMapView.Controls.Add(this.pnlMapDraw);
            this.grpMapView.Location = new System.Drawing.Point(366, 16);
            this.grpMapView.Margin = new System.Windows.Forms.Padding(0);
            this.grpMapView.Name = "grpMapView";
            this.grpMapView.Padding = new System.Windows.Forms.Padding(0);
            this.grpMapView.Size = new System.Drawing.Size(711, 567);
            this.grpMapView.TabIndex = 2;
            this.grpMapView.TabStop = false;
            this.grpMapView.Text = "マップを編集";
            // 
            // pnlMapDraw
            // 
            this.pnlMapDraw.Location = new System.Drawing.Point(52, 67);
            this.pnlMapDraw.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMapDraw.Name = "pnlMapDraw";
            this.pnlMapDraw.Size = new System.Drawing.Size(608, 448);
            this.pnlMapDraw.TabIndex = 0;
            // 
            // txtConnHeaderOffset
            // 
            this.txtConnHeaderOffset.Location = new System.Drawing.Point(156, 118);
            this.txtConnHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtConnHeaderOffset.Name = "txtConnHeaderOffset";
            this.txtConnHeaderOffset.Size = new System.Drawing.Size(80, 23);
            this.txtConnHeaderOffset.TabIndex = 10;
            // 
            // txtLevelScriptOffset
            // 
            this.txtLevelScriptOffset.Location = new System.Drawing.Point(156, 88);
            this.txtLevelScriptOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtLevelScriptOffset.Name = "txtLevelScriptOffset";
            this.txtLevelScriptOffset.Size = new System.Drawing.Size(80, 23);
            this.txtLevelScriptOffset.TabIndex = 7;
            // 
            // txtEventScriptHeaderOffset
            // 
            this.txtEventScriptHeaderOffset.Location = new System.Drawing.Point(156, 58);
            this.txtEventScriptHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtEventScriptHeaderOffset.Name = "txtEventScriptHeaderOffset";
            this.txtEventScriptHeaderOffset.Size = new System.Drawing.Size(80, 23);
            this.txtEventScriptHeaderOffset.TabIndex = 4;
            // 
            // txtMapFooterOffset
            // 
            this.txtMapFooterOffset.Location = new System.Drawing.Point(156, 28);
            this.txtMapFooterOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtMapFooterOffset.Name = "txtMapFooterOffset";
            this.txtMapFooterOffset.Size = new System.Drawing.Size(80, 23);
            this.txtMapFooterOffset.TabIndex = 1;
            // 
            // OwMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 771);
            this.Controls.Add(this.grpMapView);
            this.Controls.Add(this.grpMapHeader);
            this.Controls.Add(this.grpMapSelector);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OwMapEditor";
            this.Text = "マップ";
            this.grpMapSelector.ResumeLayout(false);
            this.grpMapSelector.PerformLayout();
            this.grpMapHeader.ResumeLayout(false);
            this.grpMapHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapTerrainIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapRelLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBgmIndex)).EndInit();
            this.grpMapView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMapSelector;
        private System.Windows.Forms.TreeView tvwMapSelector;
        private System.Windows.Forms.CheckBox chkOrderByTerrain;
        private System.Windows.Forms.RadioButton rbOrderByName;
        private System.Windows.Forms.RadioButton rbOrderByAsc;
        private System.Windows.Forms.GroupBox grpMapHeader;
        private System.Windows.Forms.Button btnEditConnHeader;
        private _Utilities._CustomCtrls.HexTextBox txtConnHeaderOffset;
        private System.Windows.Forms.Label lblConnHeaderOffset;
        private System.Windows.Forms.Button btnEditLevelScript;
        private _Utilities._CustomCtrls.HexTextBox txtLevelScriptOffset;
        private System.Windows.Forms.Label lblLevelScriptOffset;
        private System.Windows.Forms.Button btnEditEventScriptHeader;
        private _Utilities._CustomCtrls.HexTextBox txtEventScriptHeaderOffset;
        private System.Windows.Forms.Label lblEventScriptHeaderOffset;
        private System.Windows.Forms.Button btnEditMapFooter;
        private _Utilities._CustomCtrls.HexTextBox txtMapFooterOffset;
        private System.Windows.Forms.Label lblMapFooterOffset;
        private System.Windows.Forms.NumericUpDown nudMapTerrainIndex;
        private System.Windows.Forms.ComboBox cmbMapType;
        private System.Windows.Forms.Label lblMapType;
        private System.Windows.Forms.Label lblMapTerrainIndex;
        private System.Windows.Forms.Label lblMapRelLayer;
        private System.Windows.Forms.NumericUpDown nudMapRelLayer;
        private System.Windows.Forms.Label lblMapNameType;
        private System.Windows.Forms.ComboBox cmbMapNameType;
        private System.Windows.Forms.Label lblMapNameIndex;
        private System.Windows.Forms.ComboBox cmbMapNameIndex;
        private System.Windows.Forms.Label lblMapSpBg;
        private System.Windows.Forms.ComboBox cmbMapSpBg;
        private System.Windows.Forms.Label lblMapBike;
        private System.Windows.Forms.ComboBox cmbMapBike;
        private System.Windows.Forms.Label lblMapSight;
        private System.Windows.Forms.ComboBox cmbMapSight;
        private System.Windows.Forms.Label lblMapWthr;
        private System.Windows.Forms.ComboBox cmbMapWthr;
        private System.Windows.Forms.Label lblBgmIndex;
        private System.Windows.Forms.NumericUpDown nudBgmIndex;
        private System.Windows.Forms.GroupBox grpMapView;
        private System.Windows.Forms.Panel pnlMapDraw;
    }
}