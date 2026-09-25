
namespace PochiPochiEditorPlus._Forms
{
    partial class OwMapEditor1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OwMapEditor1));
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tbpBlock = new System.Windows.Forms.TabPage();
            this.grpBlockDataAndAttr = new System.Windows.Forms.GroupBox();
            this.chkBlockAttrWildEncWater = new System.Windows.Forms.CheckBox();
            this.chkBlockAttrWildEncGrass = new System.Windows.Forms.CheckBox();
            this.lblBlockAttrWildEnc = new System.Windows.Forms.Label();
            this.cmbBlockAttrLayer = new System.Windows.Forms.ComboBox();
            this.lblBlockAttrLayer = new System.Windows.Forms.Label();
            this.cmbBlockAttrUnk = new System.Windows.Forms.ComboBox();
            this.lblBlockAttrUnk = new System.Windows.Forms.Label();
            this.cmbBlockAttrType = new System.Windows.Forms.ComboBox();
            this.lblBlockAttrType = new System.Windows.Forms.Label();
            this.cmbBlockAttrAction = new System.Windows.Forms.ComboBox();
            this.lblBlockAttrAction = new System.Windows.Forms.Label();
            this.lblBlockDataImage = new System.Windows.Forms.Label();
            this.picBlockDataImage = new System.Windows.Forms.PictureBox();
            this.grpTileSelector = new System.Windows.Forms.GroupBox();
            this.lblSelectTile = new System.Windows.Forms.Label();
            this.cmbTilePalette = new System.Windows.Forms.ComboBox();
            this.chkSelectTileReverseV = new System.Windows.Forms.CheckBox();
            this.chkSelectTileReverseH = new System.Windows.Forms.CheckBox();
            this.picSelectTile = new System.Windows.Forms.PictureBox();
            this.vsbTileView = new System.Windows.Forms.VScrollBar();
            this.pnlTileView = new System.Windows.Forms.Panel();
            this.grpBlockSelector = new System.Windows.Forms.GroupBox();
            this.vsrBlockView = new System.Windows.Forms.VScrollBar();
            this.lblPaletteType = new System.Windows.Forms.Label();
            this.lblBlockIndex = new System.Windows.Forms.Label();
            this.pnlBlockView = new System.Windows.Forms.Panel();
            this.nudBlockIndex = new System.Windows.Forms.NumericUpDown();
            this.cmbPaletteType = new System.Windows.Forms.ComboBox();
            this.tbpColl = new System.Windows.Forms.TabPage();
            this.tbpEvent = new System.Windows.Forms.TabPage();
            this.txtBlockIndex = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.tbcMain.SuspendLayout();
            this.tbpBlock.SuspendLayout();
            this.grpBlockDataAndAttr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBlockDataImage)).BeginInit();
            this.grpTileSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectTile)).BeginInit();
            this.grpBlockSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlockIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tbpBlock);
            this.tbcMain.Controls.Add(this.tbpColl);
            this.tbcMain.Controls.Add(this.tbpEvent);
            this.tbcMain.Location = new System.Drawing.Point(20, 20);
            this.tbcMain.Margin = new System.Windows.Forms.Padding(0);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(980, 498);
            this.tbcMain.TabIndex = 0;
            // 
            // tbpBlock
            // 
            this.tbpBlock.Controls.Add(this.grpBlockDataAndAttr);
            this.tbpBlock.Controls.Add(this.grpTileSelector);
            this.tbpBlock.Controls.Add(this.grpBlockSelector);
            this.tbpBlock.Location = new System.Drawing.Point(4, 24);
            this.tbpBlock.Margin = new System.Windows.Forms.Padding(0);
            this.tbpBlock.Name = "tbpBlock";
            this.tbpBlock.Size = new System.Drawing.Size(972, 470);
            this.tbpBlock.TabIndex = 0;
            this.tbpBlock.Text = "ブロック";
            this.tbpBlock.UseVisualStyleBackColor = true;
            // 
            // grpBlockDataAndAttr
            // 
            this.grpBlockDataAndAttr.Controls.Add(this.chkBlockAttrWildEncWater);
            this.grpBlockDataAndAttr.Controls.Add(this.chkBlockAttrWildEncGrass);
            this.grpBlockDataAndAttr.Controls.Add(this.lblBlockAttrWildEnc);
            this.grpBlockDataAndAttr.Controls.Add(this.cmbBlockAttrLayer);
            this.grpBlockDataAndAttr.Controls.Add(this.lblBlockAttrLayer);
            this.grpBlockDataAndAttr.Controls.Add(this.cmbBlockAttrUnk);
            this.grpBlockDataAndAttr.Controls.Add(this.lblBlockAttrUnk);
            this.grpBlockDataAndAttr.Controls.Add(this.cmbBlockAttrType);
            this.grpBlockDataAndAttr.Controls.Add(this.lblBlockAttrType);
            this.grpBlockDataAndAttr.Controls.Add(this.cmbBlockAttrAction);
            this.grpBlockDataAndAttr.Controls.Add(this.lblBlockAttrAction);
            this.grpBlockDataAndAttr.Controls.Add(this.lblBlockDataImage);
            this.grpBlockDataAndAttr.Controls.Add(this.picBlockDataImage);
            this.grpBlockDataAndAttr.Location = new System.Drawing.Point(692, 16);
            this.grpBlockDataAndAttr.Margin = new System.Windows.Forms.Padding(0);
            this.grpBlockDataAndAttr.Name = "grpBlockDataAndAttr";
            this.grpBlockDataAndAttr.Padding = new System.Windows.Forms.Padding(0);
            this.grpBlockDataAndAttr.Size = new System.Drawing.Size(258, 434);
            this.grpBlockDataAndAttr.TabIndex = 2;
            this.grpBlockDataAndAttr.TabStop = false;
            this.grpBlockDataAndAttr.Text = "データと属性";
            // 
            // chkBlockAttrWildEncWater
            // 
            this.chkBlockAttrWildEncWater.AutoSize = true;
            this.chkBlockAttrWildEncWater.Location = new System.Drawing.Point(84, 370);
            this.chkBlockAttrWildEncWater.Margin = new System.Windows.Forms.Padding(0);
            this.chkBlockAttrWildEncWater.Name = "chkBlockAttrWildEncWater";
            this.chkBlockAttrWildEncWater.Size = new System.Drawing.Size(50, 19);
            this.chkBlockAttrWildEncWater.TabIndex = 22;
            this.chkBlockAttrWildEncWater.Text = "水上";
            this.chkBlockAttrWildEncWater.UseVisualStyleBackColor = true;
            // 
            // chkBlockAttrWildEncGrass
            // 
            this.chkBlockAttrWildEncGrass.AutoSize = true;
            this.chkBlockAttrWildEncGrass.Location = new System.Drawing.Point(22, 370);
            this.chkBlockAttrWildEncGrass.Margin = new System.Windows.Forms.Padding(0);
            this.chkBlockAttrWildEncGrass.Name = "chkBlockAttrWildEncGrass";
            this.chkBlockAttrWildEncGrass.Size = new System.Drawing.Size(57, 19);
            this.chkBlockAttrWildEncGrass.TabIndex = 21;
            this.chkBlockAttrWildEncGrass.Text = "草むら";
            this.chkBlockAttrWildEncGrass.UseVisualStyleBackColor = true;
            // 
            // lblBlockAttrWildEnc
            // 
            this.lblBlockAttrWildEnc.AutoSize = true;
            this.lblBlockAttrWildEnc.Location = new System.Drawing.Point(20, 348);
            this.lblBlockAttrWildEnc.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockAttrWildEnc.Name = "lblBlockAttrWildEnc";
            this.lblBlockAttrWildEnc.Size = new System.Drawing.Size(61, 15);
            this.lblBlockAttrWildEnc.TabIndex = 20;
            this.lblBlockAttrWildEnc.Text = "野生出現 :";
            // 
            // cmbBlockAttrLayer
            // 
            this.cmbBlockAttrLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlockAttrLayer.FormattingEnabled = true;
            this.cmbBlockAttrLayer.Location = new System.Drawing.Point(20, 316);
            this.cmbBlockAttrLayer.Margin = new System.Windows.Forms.Padding(0);
            this.cmbBlockAttrLayer.Name = "cmbBlockAttrLayer";
            this.cmbBlockAttrLayer.Size = new System.Drawing.Size(216, 23);
            this.cmbBlockAttrLayer.TabIndex = 19;
            // 
            // lblBlockAttrLayer
            // 
            this.lblBlockAttrLayer.AutoSize = true;
            this.lblBlockAttrLayer.Location = new System.Drawing.Point(20, 294);
            this.lblBlockAttrLayer.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockAttrLayer.Name = "lblBlockAttrLayer";
            this.lblBlockAttrLayer.Size = new System.Drawing.Size(73, 15);
            this.lblBlockAttrLayer.TabIndex = 18;
            this.lblBlockAttrLayer.Text = "描画レイヤー :";
            // 
            // cmbBlockAttrUnk
            // 
            this.cmbBlockAttrUnk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlockAttrUnk.FormattingEnabled = true;
            this.cmbBlockAttrUnk.Location = new System.Drawing.Point(20, 262);
            this.cmbBlockAttrUnk.Margin = new System.Windows.Forms.Padding(0);
            this.cmbBlockAttrUnk.Name = "cmbBlockAttrUnk";
            this.cmbBlockAttrUnk.Size = new System.Drawing.Size(216, 23);
            this.cmbBlockAttrUnk.TabIndex = 17;
            // 
            // lblBlockAttrUnk
            // 
            this.lblBlockAttrUnk.AutoSize = true;
            this.lblBlockAttrUnk.Location = new System.Drawing.Point(20, 240);
            this.lblBlockAttrUnk.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockAttrUnk.Name = "lblBlockAttrUnk";
            this.lblBlockAttrUnk.Size = new System.Drawing.Size(49, 15);
            this.lblBlockAttrUnk.TabIndex = 16;
            this.lblBlockAttrUnk.Text = "不明値 :";
            // 
            // cmbBlockAttrType
            // 
            this.cmbBlockAttrType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlockAttrType.FormattingEnabled = true;
            this.cmbBlockAttrType.Location = new System.Drawing.Point(20, 208);
            this.cmbBlockAttrType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbBlockAttrType.Name = "cmbBlockAttrType";
            this.cmbBlockAttrType.Size = new System.Drawing.Size(216, 23);
            this.cmbBlockAttrType.TabIndex = 15;
            // 
            // lblBlockAttrType
            // 
            this.lblBlockAttrType.AutoSize = true;
            this.lblBlockAttrType.Location = new System.Drawing.Point(20, 186);
            this.lblBlockAttrType.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockAttrType.Name = "lblBlockAttrType";
            this.lblBlockAttrType.Size = new System.Drawing.Size(40, 15);
            this.lblBlockAttrType.TabIndex = 14;
            this.lblBlockAttrType.Text = "タイプ :";
            // 
            // cmbBlockAttrAction
            // 
            this.cmbBlockAttrAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlockAttrAction.FormattingEnabled = true;
            this.cmbBlockAttrAction.Location = new System.Drawing.Point(20, 154);
            this.cmbBlockAttrAction.Margin = new System.Windows.Forms.Padding(0);
            this.cmbBlockAttrAction.Name = "cmbBlockAttrAction";
            this.cmbBlockAttrAction.Size = new System.Drawing.Size(216, 23);
            this.cmbBlockAttrAction.TabIndex = 13;
            // 
            // lblBlockAttrAction
            // 
            this.lblBlockAttrAction.AutoSize = true;
            this.lblBlockAttrAction.Location = new System.Drawing.Point(20, 132);
            this.lblBlockAttrAction.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockAttrAction.Name = "lblBlockAttrAction";
            this.lblBlockAttrAction.Size = new System.Drawing.Size(85, 15);
            this.lblBlockAttrAction.TabIndex = 12;
            this.lblBlockAttrAction.Text = "タイルアクション :";
            // 
            // lblBlockDataImage
            // 
            this.lblBlockDataImage.AutoSize = true;
            this.lblBlockDataImage.Location = new System.Drawing.Point(20, 28);
            this.lblBlockDataImage.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockDataImage.Name = "lblBlockDataImage";
            this.lblBlockDataImage.Size = new System.Drawing.Size(66, 15);
            this.lblBlockDataImage.TabIndex = 11;
            this.lblBlockDataImage.Text = "下位 / 上位";
            // 
            // picBlockDataImage
            // 
            this.picBlockDataImage.Location = new System.Drawing.Point(20, 50);
            this.picBlockDataImage.Margin = new System.Windows.Forms.Padding(0);
            this.picBlockDataImage.Name = "picBlockDataImage";
            this.picBlockDataImage.Size = new System.Drawing.Size(128, 64);
            this.picBlockDataImage.TabIndex = 10;
            this.picBlockDataImage.TabStop = false;
            // 
            // grpTileSelector
            // 
            this.grpTileSelector.Controls.Add(this.lblSelectTile);
            this.grpTileSelector.Controls.Add(this.cmbTilePalette);
            this.grpTileSelector.Controls.Add(this.chkSelectTileReverseV);
            this.grpTileSelector.Controls.Add(this.chkSelectTileReverseH);
            this.grpTileSelector.Controls.Add(this.picSelectTile);
            this.grpTileSelector.Controls.Add(this.vsbTileView);
            this.grpTileSelector.Controls.Add(this.pnlTileView);
            this.grpTileSelector.Location = new System.Drawing.Point(356, 16);
            this.grpTileSelector.Margin = new System.Windows.Forms.Padding(0);
            this.grpTileSelector.Name = "grpTileSelector";
            this.grpTileSelector.Padding = new System.Windows.Forms.Padding(0);
            this.grpTileSelector.Size = new System.Drawing.Size(318, 434);
            this.grpTileSelector.TabIndex = 1;
            this.grpTileSelector.TabStop = false;
            this.grpTileSelector.Text = "タイルを選択";
            // 
            // lblSelectTile
            // 
            this.lblSelectTile.AutoSize = true;
            this.lblSelectTile.Location = new System.Drawing.Point(20, 28);
            this.lblSelectTile.Margin = new System.Windows.Forms.Padding(0);
            this.lblSelectTile.Name = "lblSelectTile";
            this.lblSelectTile.Size = new System.Drawing.Size(87, 15);
            this.lblSelectTile.TabIndex = 12;
            this.lblSelectTile.Text = "選択中のタイル :";
            // 
            // cmbTilePalette
            // 
            this.cmbTilePalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTilePalette.FormattingEnabled = true;
            this.cmbTilePalette.Location = new System.Drawing.Point(162, 92);
            this.cmbTilePalette.Margin = new System.Windows.Forms.Padding(0);
            this.cmbTilePalette.Name = "cmbTilePalette";
            this.cmbTilePalette.Size = new System.Drawing.Size(114, 23);
            this.cmbTilePalette.TabIndex = 11;
            // 
            // chkSelectTileReverseV
            // 
            this.chkSelectTileReverseV.AutoSize = true;
            this.chkSelectTileReverseV.Location = new System.Drawing.Point(162, 66);
            this.chkSelectTileReverseV.Margin = new System.Windows.Forms.Padding(0);
            this.chkSelectTileReverseV.Name = "chkSelectTileReverseV";
            this.chkSelectTileReverseV.Size = new System.Drawing.Size(74, 19);
            this.chkSelectTileReverseV.TabIndex = 10;
            this.chkSelectTileReverseV.Text = "上下反転";
            this.chkSelectTileReverseV.UseVisualStyleBackColor = true;
            // 
            // chkSelectTileReverseH
            // 
            this.chkSelectTileReverseH.AutoSize = true;
            this.chkSelectTileReverseH.Location = new System.Drawing.Point(162, 42);
            this.chkSelectTileReverseH.Margin = new System.Windows.Forms.Padding(0);
            this.chkSelectTileReverseH.Name = "chkSelectTileReverseH";
            this.chkSelectTileReverseH.Size = new System.Drawing.Size(74, 19);
            this.chkSelectTileReverseH.TabIndex = 10;
            this.chkSelectTileReverseH.Text = "左右反転";
            this.chkSelectTileReverseH.UseVisualStyleBackColor = true;
            // 
            // picSelectTile
            // 
            this.picSelectTile.Location = new System.Drawing.Point(20, 50);
            this.picSelectTile.Margin = new System.Windows.Forms.Padding(0);
            this.picSelectTile.Name = "picSelectTile";
            this.picSelectTile.Size = new System.Drawing.Size(128, 64);
            this.picSelectTile.TabIndex = 9;
            this.picSelectTile.TabStop = false;
            // 
            // vsbTileView
            // 
            this.vsbTileView.Location = new System.Drawing.Point(282, 132);
            this.vsbTileView.Name = "vsbTileView";
            this.vsbTileView.Size = new System.Drawing.Size(16, 280);
            this.vsbTileView.TabIndex = 8;
            // 
            // pnlTileView
            // 
            this.pnlTileView.Location = new System.Drawing.Point(20, 132);
            this.pnlTileView.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTileView.Name = "pnlTileView";
            this.pnlTileView.Size = new System.Drawing.Size(256, 280);
            this.pnlTileView.TabIndex = 7;
            // 
            // grpBlockSelector
            // 
            this.grpBlockSelector.Controls.Add(this.vsrBlockView);
            this.grpBlockSelector.Controls.Add(this.lblPaletteType);
            this.grpBlockSelector.Controls.Add(this.lblBlockIndex);
            this.grpBlockSelector.Controls.Add(this.pnlBlockView);
            this.grpBlockSelector.Controls.Add(this.txtBlockIndex);
            this.grpBlockSelector.Controls.Add(this.nudBlockIndex);
            this.grpBlockSelector.Controls.Add(this.cmbPaletteType);
            this.grpBlockSelector.Location = new System.Drawing.Point(20, 16);
            this.grpBlockSelector.Margin = new System.Windows.Forms.Padding(0);
            this.grpBlockSelector.Name = "grpBlockSelector";
            this.grpBlockSelector.Padding = new System.Windows.Forms.Padding(0);
            this.grpBlockSelector.Size = new System.Drawing.Size(318, 434);
            this.grpBlockSelector.TabIndex = 0;
            this.grpBlockSelector.TabStop = false;
            this.grpBlockSelector.Text = "ブロックを選択";
            // 
            // vsrBlockView
            // 
            this.vsrBlockView.Location = new System.Drawing.Point(282, 92);
            this.vsrBlockView.Name = "vsrBlockView";
            this.vsrBlockView.Size = new System.Drawing.Size(16, 320);
            this.vsrBlockView.TabIndex = 6;
            // 
            // lblPaletteType
            // 
            this.lblPaletteType.AutoSize = true;
            this.lblPaletteType.Location = new System.Drawing.Point(20, 32);
            this.lblPaletteType.Margin = new System.Windows.Forms.Padding(0);
            this.lblPaletteType.Name = "lblPaletteType";
            this.lblPaletteType.Size = new System.Drawing.Size(118, 15);
            this.lblPaletteType.TabIndex = 5;
            this.lblPaletteType.Text = "パレット読み込み設定 :";
            // 
            // lblBlockIndex
            // 
            this.lblBlockIndex.AutoSize = true;
            this.lblBlockIndex.Location = new System.Drawing.Point(20, 62);
            this.lblBlockIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockIndex.Name = "lblBlockIndex";
            this.lblBlockIndex.Size = new System.Drawing.Size(72, 15);
            this.lblBlockIndex.TabIndex = 4;
            this.lblBlockIndex.Text = "ブロック番号 :";
            // 
            // pnlBlockView
            // 
            this.pnlBlockView.Location = new System.Drawing.Point(20, 92);
            this.pnlBlockView.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBlockView.Name = "pnlBlockView";
            this.pnlBlockView.Size = new System.Drawing.Size(256, 320);
            this.pnlBlockView.TabIndex = 3;
            // 
            // nudBlockIndex
            // 
            this.nudBlockIndex.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudBlockIndex.Location = new System.Drawing.Point(122, 58);
            this.nudBlockIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudBlockIndex.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.nudBlockIndex.Name = "nudBlockIndex";
            this.nudBlockIndex.Size = new System.Drawing.Size(72, 23);
            this.nudBlockIndex.TabIndex = 1;
            // 
            // cmbPaletteType
            // 
            this.cmbPaletteType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaletteType.Enabled = false;
            this.cmbPaletteType.FormattingEnabled = true;
            this.cmbPaletteType.Location = new System.Drawing.Point(156, 28);
            this.cmbPaletteType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbPaletteType.Name = "cmbPaletteType";
            this.cmbPaletteType.Size = new System.Drawing.Size(120, 23);
            this.cmbPaletteType.TabIndex = 0;
            // 
            // tbpColl
            // 
            this.tbpColl.Location = new System.Drawing.Point(4, 24);
            this.tbpColl.Margin = new System.Windows.Forms.Padding(0);
            this.tbpColl.Name = "tbpColl";
            this.tbpColl.Size = new System.Drawing.Size(972, 470);
            this.tbpColl.TabIndex = 1;
            this.tbpColl.Text = "移動エリア";
            this.tbpColl.UseVisualStyleBackColor = true;
            // 
            // tbpEvent
            // 
            this.tbpEvent.Location = new System.Drawing.Point(4, 24);
            this.tbpEvent.Margin = new System.Windows.Forms.Padding(0);
            this.tbpEvent.Name = "tbpEvent";
            this.tbpEvent.Size = new System.Drawing.Size(972, 470);
            this.tbpEvent.TabIndex = 2;
            this.tbpEvent.Text = "イベント";
            this.tbpEvent.UseVisualStyleBackColor = true;
            // 
            // txtBlockIndex
            // 
            this.txtBlockIndex.Digits = 4;
            this.txtBlockIndex.Location = new System.Drawing.Point(204, 58);
            this.txtBlockIndex.Margin = new System.Windows.Forms.Padding(0);
            this.txtBlockIndex.Name = "txtBlockIndex";
            this.txtBlockIndex.ReadOnly = true;
            this.txtBlockIndex.Size = new System.Drawing.Size(72, 23);
            this.txtBlockIndex.TabIndex = 2;
            // 
            // OwMapEditor1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 535);
            this.Controls.Add(this.tbcMain);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OwMapEditor1";
            this.Text = "マップ";
            this.tbcMain.ResumeLayout(false);
            this.tbpBlock.ResumeLayout(false);
            this.grpBlockDataAndAttr.ResumeLayout(false);
            this.grpBlockDataAndAttr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBlockDataImage)).EndInit();
            this.grpTileSelector.ResumeLayout(false);
            this.grpTileSelector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectTile)).EndInit();
            this.grpBlockSelector.ResumeLayout(false);
            this.grpBlockSelector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlockIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tbpBlock;
        private System.Windows.Forms.TabPage tbpColl;
        private System.Windows.Forms.TabPage tbpEvent;
        private System.Windows.Forms.GroupBox grpBlockSelector;
        private System.Windows.Forms.ComboBox cmbPaletteType;
        private System.Windows.Forms.NumericUpDown nudBlockIndex;
        private _Utilities._CustomCtrls.HexTextBox txtBlockIndex;
        private System.Windows.Forms.Label lblBlockIndex;
        private System.Windows.Forms.Panel pnlBlockView;
        private System.Windows.Forms.VScrollBar vsrBlockView;
        private System.Windows.Forms.Label lblPaletteType;
        private System.Windows.Forms.GroupBox grpTileSelector;
        private System.Windows.Forms.VScrollBar vsbTileView;
        private System.Windows.Forms.Panel pnlTileView;
        private System.Windows.Forms.CheckBox chkSelectTileReverseV;
        private System.Windows.Forms.CheckBox chkSelectTileReverseH;
        private System.Windows.Forms.PictureBox picSelectTile;
        private System.Windows.Forms.ComboBox cmbTilePalette;
        private System.Windows.Forms.GroupBox grpBlockDataAndAttr;
        private System.Windows.Forms.Label lblBlockDataImage;
        private System.Windows.Forms.PictureBox picBlockDataImage;
        private System.Windows.Forms.Label lblSelectTile;
        private System.Windows.Forms.Label lblBlockAttrAction;
        private System.Windows.Forms.ComboBox cmbBlockAttrAction;
        private System.Windows.Forms.ComboBox cmbBlockAttrType;
        private System.Windows.Forms.Label lblBlockAttrType;
        private System.Windows.Forms.ComboBox cmbBlockAttrUnk;
        private System.Windows.Forms.Label lblBlockAttrUnk;
        private System.Windows.Forms.ComboBox cmbBlockAttrLayer;
        private System.Windows.Forms.Label lblBlockAttrLayer;
        private System.Windows.Forms.Label lblBlockAttrWildEnc;
        private System.Windows.Forms.CheckBox chkBlockAttrWildEncWater;
        private System.Windows.Forms.CheckBox chkBlockAttrWildEncGrass;
    }
}