
namespace PochiPochiEditorPlus._Forms
{
    partial class TilesetEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesetEditor));
            this.lblTilesetNo = new System.Windows.Forms.Label();
            this.nudTilesetNo = new System.Windows.Forms.NumericUpDown();
            this.btnLoadTileset = new System.Windows.Forms.Button();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tbpHeader = new System.Windows.Forms.TabPage();
            this.btnEditAnimEntry = new System.Windows.Forms.Button();
            this.btnImportPalette = new System.Windows.Forms.Button();
            this.btnImportImage = new System.Windows.Forms.Button();
            this.btnEditBlockCount = new System.Windows.Forms.Button();
            this.btnCreateNewHeader = new System.Windows.Forms.Button();
            this.txtAnimHeaderOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblAnimHeaderOffset = new System.Windows.Forms.Label();
            this.txtBlockAttrTableOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.txtBlockDataTableOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblBlockAttrTableOffset = new System.Windows.Forms.Label();
            this.lblBlockDataTableOffset = new System.Windows.Forms.Label();
            this.txtPaletteOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.txtImageOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblPaletteOffset = new System.Windows.Forms.Label();
            this.lblImageOffset = new System.Windows.Forms.Label();
            this.cmbImageCompType = new System.Windows.Forms.ComboBox();
            this.cmbPaletteType = new System.Windows.Forms.ComboBox();
            this.lblPaletteType = new System.Windows.Forms.Label();
            this.lblImageCompType = new System.Windows.Forms.Label();
            this.tbpAnim = new System.Windows.Forms.TabPage();
            this.grpView = new System.Windows.Forms.GroupBox();
            this.vsbViewImage = new System.Windows.Forms.VScrollBar();
            this.lblViewPalette = new System.Windows.Forms.Label();
            this.cmbViewPalette = new System.Windows.Forms.ComboBox();
            this.txtViewTileIndex = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.nudViewTileIndex = new System.Windows.Forms.NumericUpDown();
            this.lblViewTileIndex = new System.Windows.Forms.Label();
            this.pnlViewImage = new System.Windows.Forms.Panel();
            this.btnReloadTileset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudTilesetNo)).BeginInit();
            this.tbcMain.SuspendLayout();
            this.tbpHeader.SuspendLayout();
            this.grpView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudViewTileIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTilesetNo
            // 
            this.lblTilesetNo.AutoSize = true;
            this.lblTilesetNo.Location = new System.Drawing.Point(20, 24);
            this.lblTilesetNo.Margin = new System.Windows.Forms.Padding(0);
            this.lblTilesetNo.Name = "lblTilesetNo";
            this.lblTilesetNo.Size = new System.Drawing.Size(83, 15);
            this.lblTilesetNo.TabIndex = 0;
            this.lblTilesetNo.Text = "タイルセットNo :";
            // 
            // nudTilesetNo
            // 
            this.nudTilesetNo.Location = new System.Drawing.Point(112, 20);
            this.nudTilesetNo.Margin = new System.Windows.Forms.Padding(0);
            this.nudTilesetNo.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.nudTilesetNo.Name = "nudTilesetNo";
            this.nudTilesetNo.Size = new System.Drawing.Size(104, 23);
            this.nudTilesetNo.TabIndex = 1;
            // 
            // btnLoadTileset
            // 
            this.btnLoadTileset.Location = new System.Drawing.Point(228, 20);
            this.btnLoadTileset.Margin = new System.Windows.Forms.Padding(0);
            this.btnLoadTileset.Name = "btnLoadTileset";
            this.btnLoadTileset.Size = new System.Drawing.Size(96, 23);
            this.btnLoadTileset.TabIndex = 2;
            this.btnLoadTileset.Text = "読み込み";
            this.btnLoadTileset.UseVisualStyleBackColor = true;
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tbpHeader);
            this.tbcMain.Controls.Add(this.tbpAnim);
            this.tbcMain.Location = new System.Drawing.Point(368, 64);
            this.tbcMain.Margin = new System.Windows.Forms.Padding(0);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(468, 496);
            this.tbcMain.TabIndex = 3;
            // 
            // tbpHeader
            // 
            this.tbpHeader.Controls.Add(this.btnEditAnimEntry);
            this.tbpHeader.Controls.Add(this.btnImportPalette);
            this.tbpHeader.Controls.Add(this.btnImportImage);
            this.tbpHeader.Controls.Add(this.btnEditBlockCount);
            this.tbpHeader.Controls.Add(this.btnCreateNewHeader);
            this.tbpHeader.Controls.Add(this.txtAnimHeaderOffset);
            this.tbpHeader.Controls.Add(this.lblAnimHeaderOffset);
            this.tbpHeader.Controls.Add(this.txtBlockAttrTableOffset);
            this.tbpHeader.Controls.Add(this.txtBlockDataTableOffset);
            this.tbpHeader.Controls.Add(this.lblBlockAttrTableOffset);
            this.tbpHeader.Controls.Add(this.lblBlockDataTableOffset);
            this.tbpHeader.Controls.Add(this.txtPaletteOffset);
            this.tbpHeader.Controls.Add(this.txtImageOffset);
            this.tbpHeader.Controls.Add(this.lblPaletteOffset);
            this.tbpHeader.Controls.Add(this.lblImageOffset);
            this.tbpHeader.Controls.Add(this.cmbImageCompType);
            this.tbpHeader.Controls.Add(this.cmbPaletteType);
            this.tbpHeader.Controls.Add(this.lblPaletteType);
            this.tbpHeader.Controls.Add(this.lblImageCompType);
            this.tbpHeader.Location = new System.Drawing.Point(4, 24);
            this.tbpHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tbpHeader.Name = "tbpHeader";
            this.tbpHeader.Size = new System.Drawing.Size(460, 468);
            this.tbpHeader.TabIndex = 0;
            this.tbpHeader.Text = "ヘッダー";
            this.tbpHeader.UseVisualStyleBackColor = true;
            // 
            // btnEditAnimEntry
            // 
            this.btnEditAnimEntry.Location = new System.Drawing.Point(284, 200);
            this.btnEditAnimEntry.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditAnimEntry.Name = "btnEditAnimEntry";
            this.btnEditAnimEntry.Size = new System.Drawing.Size(112, 23);
            this.btnEditAnimEntry.TabIndex = 14;
            this.btnEditAnimEntry.Text = "エントリー数を変更";
            this.btnEditAnimEntry.UseVisualStyleBackColor = true;
            // 
            // btnImportPalette
            // 
            this.btnImportPalette.Location = new System.Drawing.Point(284, 110);
            this.btnImportPalette.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportPalette.Name = "btnImportPalette";
            this.btnImportPalette.Size = new System.Drawing.Size(112, 23);
            this.btnImportPalette.TabIndex = 13;
            this.btnImportPalette.Text = "パレットをインポート";
            this.btnImportPalette.UseVisualStyleBackColor = true;
            // 
            // btnImportImage
            // 
            this.btnImportImage.Location = new System.Drawing.Point(284, 80);
            this.btnImportImage.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportImage.Name = "btnImportImage";
            this.btnImportImage.Size = new System.Drawing.Size(112, 23);
            this.btnImportImage.TabIndex = 13;
            this.btnImportImage.Text = "画像をインポート";
            this.btnImportImage.UseVisualStyleBackColor = true;
            // 
            // btnEditBlockCount
            // 
            this.btnEditBlockCount.Location = new System.Drawing.Point(284, 140);
            this.btnEditBlockCount.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditBlockCount.Name = "btnEditBlockCount";
            this.btnEditBlockCount.Size = new System.Drawing.Size(112, 53);
            this.btnEditBlockCount.TabIndex = 12;
            this.btnEditBlockCount.Text = "ブロック数を変更";
            this.btnEditBlockCount.UseVisualStyleBackColor = true;
            // 
            // btnCreateNewHeader
            // 
            this.btnCreateNewHeader.Location = new System.Drawing.Point(20, 230);
            this.btnCreateNewHeader.Margin = new System.Windows.Forms.Padding(0);
            this.btnCreateNewHeader.Name = "btnCreateNewHeader";
            this.btnCreateNewHeader.Size = new System.Drawing.Size(252, 23);
            this.btnCreateNewHeader.TabIndex = 11;
            this.btnCreateNewHeader.Text = "新規ヘッダーを作成";
            this.btnCreateNewHeader.UseVisualStyleBackColor = true;
            // 
            // txtAnimHeaderOffset
            // 
            this.txtAnimHeaderOffset.Location = new System.Drawing.Point(152, 200);
            this.txtAnimHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtAnimHeaderOffset.Name = "txtAnimHeaderOffset";
            this.txtAnimHeaderOffset.Size = new System.Drawing.Size(120, 23);
            this.txtAnimHeaderOffset.TabIndex = 10;
            // 
            // lblAnimHeaderOffset
            // 
            this.lblAnimHeaderOffset.AutoSize = true;
            this.lblAnimHeaderOffset.Location = new System.Drawing.Point(20, 204);
            this.lblAnimHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblAnimHeaderOffset.Name = "lblAnimHeaderOffset";
            this.lblAnimHeaderOffset.Size = new System.Drawing.Size(109, 15);
            this.lblAnimHeaderOffset.TabIndex = 9;
            this.lblAnimHeaderOffset.Text = "アニメヘッダーアドレス :";
            // 
            // txtBlockAttrTableOffset
            // 
            this.txtBlockAttrTableOffset.Location = new System.Drawing.Point(152, 170);
            this.txtBlockAttrTableOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtBlockAttrTableOffset.Name = "txtBlockAttrTableOffset";
            this.txtBlockAttrTableOffset.Size = new System.Drawing.Size(120, 23);
            this.txtBlockAttrTableOffset.TabIndex = 7;
            // 
            // txtBlockDataTableOffset
            // 
            this.txtBlockDataTableOffset.Location = new System.Drawing.Point(152, 140);
            this.txtBlockDataTableOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtBlockDataTableOffset.Name = "txtBlockDataTableOffset";
            this.txtBlockDataTableOffset.Size = new System.Drawing.Size(120, 23);
            this.txtBlockDataTableOffset.TabIndex = 8;
            // 
            // lblBlockAttrTableOffset
            // 
            this.lblBlockAttrTableOffset.AutoSize = true;
            this.lblBlockAttrTableOffset.Location = new System.Drawing.Point(20, 174);
            this.lblBlockAttrTableOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockAttrTableOffset.Name = "lblBlockAttrTableOffset";
            this.lblBlockAttrTableOffset.Size = new System.Drawing.Size(108, 15);
            this.lblBlockAttrTableOffset.TabIndex = 5;
            this.lblBlockAttrTableOffset.Text = "ブロック属性テーブル :";
            // 
            // lblBlockDataTableOffset
            // 
            this.lblBlockDataTableOffset.AutoSize = true;
            this.lblBlockDataTableOffset.Location = new System.Drawing.Point(20, 144);
            this.lblBlockDataTableOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblBlockDataTableOffset.Name = "lblBlockDataTableOffset";
            this.lblBlockDataTableOffset.Size = new System.Drawing.Size(110, 15);
            this.lblBlockDataTableOffset.TabIndex = 6;
            this.lblBlockDataTableOffset.Text = "ブロックデータテーブル :";
            // 
            // txtPaletteOffset
            // 
            this.txtPaletteOffset.Location = new System.Drawing.Point(152, 110);
            this.txtPaletteOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtPaletteOffset.Name = "txtPaletteOffset";
            this.txtPaletteOffset.Size = new System.Drawing.Size(120, 23);
            this.txtPaletteOffset.TabIndex = 4;
            // 
            // txtImageOffset
            // 
            this.txtImageOffset.Location = new System.Drawing.Point(152, 80);
            this.txtImageOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtImageOffset.Name = "txtImageOffset";
            this.txtImageOffset.Size = new System.Drawing.Size(120, 23);
            this.txtImageOffset.TabIndex = 4;
            // 
            // lblPaletteOffset
            // 
            this.lblPaletteOffset.AutoSize = true;
            this.lblPaletteOffset.Location = new System.Drawing.Point(20, 114);
            this.lblPaletteOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblPaletteOffset.Name = "lblPaletteOffset";
            this.lblPaletteOffset.Size = new System.Drawing.Size(83, 15);
            this.lblPaletteOffset.TabIndex = 2;
            this.lblPaletteOffset.Text = "パレットアドレス :";
            // 
            // lblImageOffset
            // 
            this.lblImageOffset.AutoSize = true;
            this.lblImageOffset.Location = new System.Drawing.Point(20, 84);
            this.lblImageOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblImageOffset.Name = "lblImageOffset";
            this.lblImageOffset.Size = new System.Drawing.Size(72, 15);
            this.lblImageOffset.TabIndex = 3;
            this.lblImageOffset.Text = "画像アドレス :";
            // 
            // cmbImageCompType
            // 
            this.cmbImageCompType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImageCompType.Enabled = false;
            this.cmbImageCompType.FormattingEnabled = true;
            this.cmbImageCompType.Location = new System.Drawing.Point(152, 20);
            this.cmbImageCompType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbImageCompType.Name = "cmbImageCompType";
            this.cmbImageCompType.Size = new System.Drawing.Size(120, 23);
            this.cmbImageCompType.TabIndex = 1;
            // 
            // cmbPaletteType
            // 
            this.cmbPaletteType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaletteType.Enabled = false;
            this.cmbPaletteType.FormattingEnabled = true;
            this.cmbPaletteType.Location = new System.Drawing.Point(152, 50);
            this.cmbPaletteType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbPaletteType.Name = "cmbPaletteType";
            this.cmbPaletteType.Size = new System.Drawing.Size(120, 23);
            this.cmbPaletteType.TabIndex = 1;
            // 
            // lblPaletteType
            // 
            this.lblPaletteType.AutoSize = true;
            this.lblPaletteType.Location = new System.Drawing.Point(20, 54);
            this.lblPaletteType.Margin = new System.Windows.Forms.Padding(0);
            this.lblPaletteType.Name = "lblPaletteType";
            this.lblPaletteType.Size = new System.Drawing.Size(118, 15);
            this.lblPaletteType.TabIndex = 0;
            this.lblPaletteType.Text = "パレット読み込み設定 :";
            // 
            // lblImageCompType
            // 
            this.lblImageCompType.AutoSize = true;
            this.lblImageCompType.Location = new System.Drawing.Point(20, 24);
            this.lblImageCompType.Margin = new System.Windows.Forms.Padding(0);
            this.lblImageCompType.Name = "lblImageCompType";
            this.lblImageCompType.Size = new System.Drawing.Size(85, 15);
            this.lblImageCompType.TabIndex = 0;
            this.lblImageCompType.Text = "画像圧縮設定 :";
            // 
            // tbpAnim
            // 
            this.tbpAnim.Location = new System.Drawing.Point(4, 24);
            this.tbpAnim.Margin = new System.Windows.Forms.Padding(0);
            this.tbpAnim.Name = "tbpAnim";
            this.tbpAnim.Size = new System.Drawing.Size(460, 468);
            this.tbpAnim.TabIndex = 1;
            this.tbpAnim.Text = "タイルアニメ";
            this.tbpAnim.UseVisualStyleBackColor = true;
            // 
            // grpView
            // 
            this.grpView.Controls.Add(this.vsbViewImage);
            this.grpView.Controls.Add(this.lblViewPalette);
            this.grpView.Controls.Add(this.cmbViewPalette);
            this.grpView.Controls.Add(this.txtViewTileIndex);
            this.grpView.Controls.Add(this.nudViewTileIndex);
            this.grpView.Controls.Add(this.lblViewTileIndex);
            this.grpView.Controls.Add(this.pnlViewImage);
            this.grpView.Location = new System.Drawing.Point(20, 56);
            this.grpView.Margin = new System.Windows.Forms.Padding(0);
            this.grpView.Name = "grpView";
            this.grpView.Padding = new System.Windows.Forms.Padding(0);
            this.grpView.Size = new System.Drawing.Size(328, 504);
            this.grpView.TabIndex = 4;
            this.grpView.TabStop = false;
            this.grpView.Text = "閲覧用";
            // 
            // vsbViewImage
            // 
            this.vsbViewImage.Location = new System.Drawing.Point(288, 96);
            this.vsbViewImage.Name = "vsbViewImage";
            this.vsbViewImage.Size = new System.Drawing.Size(16, 384);
            this.vsbViewImage.TabIndex = 7;
            // 
            // lblViewPalette
            // 
            this.lblViewPalette.AutoSize = true;
            this.lblViewPalette.Location = new System.Drawing.Point(24, 62);
            this.lblViewPalette.Margin = new System.Windows.Forms.Padding(0);
            this.lblViewPalette.Name = "lblViewPalette";
            this.lblViewPalette.Size = new System.Drawing.Size(48, 15);
            this.lblViewPalette.TabIndex = 6;
            this.lblViewPalette.Text = "パレット :";
            // 
            // cmbViewPalette
            // 
            this.cmbViewPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViewPalette.FormattingEnabled = true;
            this.cmbViewPalette.Location = new System.Drawing.Point(96, 58);
            this.cmbViewPalette.Margin = new System.Windows.Forms.Padding(0);
            this.cmbViewPalette.Name = "cmbViewPalette";
            this.cmbViewPalette.Size = new System.Drawing.Size(152, 23);
            this.cmbViewPalette.TabIndex = 0;
            // 
            // txtViewTileIndex
            // 
            this.txtViewTileIndex.Digits = 4;
            this.txtViewTileIndex.Location = new System.Drawing.Point(176, 28);
            this.txtViewTileIndex.Margin = new System.Windows.Forms.Padding(0);
            this.txtViewTileIndex.Name = "txtViewTileIndex";
            this.txtViewTileIndex.ReadOnly = true;
            this.txtViewTileIndex.Size = new System.Drawing.Size(72, 23);
            this.txtViewTileIndex.TabIndex = 4;
            // 
            // nudViewTileIndex
            // 
            this.nudViewTileIndex.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudViewTileIndex.Location = new System.Drawing.Point(96, 28);
            this.nudViewTileIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudViewTileIndex.Maximum = new decimal(new int[] {
            639,
            0,
            0,
            0});
            this.nudViewTileIndex.Name = "nudViewTileIndex";
            this.nudViewTileIndex.ReadOnly = true;
            this.nudViewTileIndex.Size = new System.Drawing.Size(72, 23);
            this.nudViewTileIndex.TabIndex = 3;
            // 
            // lblViewTileIndex
            // 
            this.lblViewTileIndex.AutoSize = true;
            this.lblViewTileIndex.Location = new System.Drawing.Point(24, 32);
            this.lblViewTileIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblViewTileIndex.Name = "lblViewTileIndex";
            this.lblViewTileIndex.Size = new System.Drawing.Size(65, 15);
            this.lblViewTileIndex.TabIndex = 2;
            this.lblViewTileIndex.Text = "タイル番号 :";
            // 
            // pnlViewImage
            // 
            this.pnlViewImage.Location = new System.Drawing.Point(24, 96);
            this.pnlViewImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlViewImage.Name = "pnlViewImage";
            this.pnlViewImage.Size = new System.Drawing.Size(256, 384);
            this.pnlViewImage.TabIndex = 1;
            // 
            // btnReloadTileset
            // 
            this.btnReloadTileset.Location = new System.Drawing.Point(332, 20);
            this.btnReloadTileset.Margin = new System.Windows.Forms.Padding(0);
            this.btnReloadTileset.Name = "btnReloadTileset";
            this.btnReloadTileset.Size = new System.Drawing.Size(96, 23);
            this.btnReloadTileset.TabIndex = 5;
            this.btnReloadTileset.Text = "再読み込み";
            this.btnReloadTileset.UseVisualStyleBackColor = true;
            // 
            // TilesetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 581);
            this.Controls.Add(this.btnReloadTileset);
            this.Controls.Add(this.grpView);
            this.Controls.Add(this.tbcMain);
            this.Controls.Add(this.btnLoadTileset);
            this.Controls.Add(this.nudTilesetNo);
            this.Controls.Add(this.lblTilesetNo);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TilesetEditor";
            this.Text = "タイルセット";
            ((System.ComponentModel.ISupportInitialize)(this.nudTilesetNo)).EndInit();
            this.tbcMain.ResumeLayout(false);
            this.tbpHeader.ResumeLayout(false);
            this.tbpHeader.PerformLayout();
            this.grpView.ResumeLayout(false);
            this.grpView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudViewTileIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTilesetNo;
        private System.Windows.Forms.NumericUpDown nudTilesetNo;
        private System.Windows.Forms.Button btnLoadTileset;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tbpHeader;
        private System.Windows.Forms.TabPage tbpAnim;
        private System.Windows.Forms.Label lblImageCompType;
        private System.Windows.Forms.ComboBox cmbImageCompType;
        private System.Windows.Forms.ComboBox cmbPaletteType;
        private System.Windows.Forms.Label lblPaletteType;
        private _Utilities._CustomCtrls.HexTextBox txtPaletteOffset;
        private _Utilities._CustomCtrls.HexTextBox txtImageOffset;
        private System.Windows.Forms.Label lblPaletteOffset;
        private System.Windows.Forms.Label lblImageOffset;
        private System.Windows.Forms.GroupBox grpView;
        private _Utilities._CustomCtrls.HexTextBox txtAnimHeaderOffset;
        private System.Windows.Forms.Label lblAnimHeaderOffset;
        private _Utilities._CustomCtrls.HexTextBox txtBlockAttrTableOffset;
        private _Utilities._CustomCtrls.HexTextBox txtBlockDataTableOffset;
        private System.Windows.Forms.Label lblBlockAttrTableOffset;
        private System.Windows.Forms.Label lblBlockDataTableOffset;
        private System.Windows.Forms.Button btnCreateNewHeader;
        private System.Windows.Forms.Button btnImportPalette;
        private System.Windows.Forms.Button btnImportImage;
        private System.Windows.Forms.Button btnEditBlockCount;
        private System.Windows.Forms.Button btnEditAnimEntry;
        private System.Windows.Forms.ComboBox cmbViewPalette;
        private System.Windows.Forms.Panel pnlViewImage;
        private System.Windows.Forms.Button btnReloadTileset;
        private System.Windows.Forms.NumericUpDown nudViewTileIndex;
        private System.Windows.Forms.Label lblViewTileIndex;
        private _Utilities._CustomCtrls.HexTextBox txtViewTileIndex;
        private System.Windows.Forms.VScrollBar vsbViewImage;
        private System.Windows.Forms.Label lblViewPalette;
    }
}