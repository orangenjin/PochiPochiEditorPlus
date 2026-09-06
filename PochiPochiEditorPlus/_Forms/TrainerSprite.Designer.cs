
namespace PochiPochiEditorPlus._Forms
{
    partial class TrainerSprite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainerSprite));
            this.picSpriteBmp = new System.Windows.Forms.PictureBox();
            this.btnSpriteIndexPrev = new System.Windows.Forms.Button();
            this.btnSpriteIndexNext = new System.Windows.Forms.Button();
            this.nudSpriteIndex = new System.Windows.Forms.NumericUpDown();
            this.btnExportSprite = new System.Windows.Forms.Button();
            this.lblSpriteTileOffset = new System.Windows.Forms.Label();
            this.txtSpriteTileOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.txtSpritePaletteOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblSpritePaletteOffset = new System.Windows.Forms.Label();
            this.lblSpriteYPosValue = new System.Windows.Forms.Label();
            this.nudSpriteYPosValue = new System.Windows.Forms.NumericUpDown();
            this.grpAnimData = new System.Windows.Forms.GroupBox();
            this.txtSpriteAnimDataOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblSpriteAnimDataOffset = new System.Windows.Forms.Label();
            this.txtSpriteAnimPointerOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblSpriteAnimPointerOffset = new System.Windows.Forms.Label();
            this.btnImportSpriteTile = new System.Windows.Forms.Button();
            this.btnImportSpritePalette = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picSpriteBmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteYPosValue)).BeginInit();
            this.grpAnimData.SuspendLayout();
            this.SuspendLayout();
            // 
            // picSpriteBmp
            // 
            this.picSpriteBmp.Location = new System.Drawing.Point(20, 20);
            this.picSpriteBmp.Margin = new System.Windows.Forms.Padding(0);
            this.picSpriteBmp.Name = "picSpriteBmp";
            this.picSpriteBmp.Size = new System.Drawing.Size(128, 128);
            this.picSpriteBmp.TabIndex = 0;
            this.picSpriteBmp.TabStop = false;
            // 
            // btnSpriteIndexPrev
            // 
            this.btnSpriteIndexPrev.Location = new System.Drawing.Point(20, 160);
            this.btnSpriteIndexPrev.Margin = new System.Windows.Forms.Padding(0);
            this.btnSpriteIndexPrev.Name = "btnSpriteIndexPrev";
            this.btnSpriteIndexPrev.Size = new System.Drawing.Size(28, 23);
            this.btnSpriteIndexPrev.TabIndex = 1;
            this.btnSpriteIndexPrev.Text = "<";
            this.btnSpriteIndexPrev.UseVisualStyleBackColor = true;
            // 
            // btnSpriteIndexNext
            // 
            this.btnSpriteIndexNext.Location = new System.Drawing.Point(120, 160);
            this.btnSpriteIndexNext.Margin = new System.Windows.Forms.Padding(0);
            this.btnSpriteIndexNext.Name = "btnSpriteIndexNext";
            this.btnSpriteIndexNext.Size = new System.Drawing.Size(28, 23);
            this.btnSpriteIndexNext.TabIndex = 1;
            this.btnSpriteIndexNext.Text = ">";
            this.btnSpriteIndexNext.UseVisualStyleBackColor = true;
            // 
            // nudSpriteIndex
            // 
            this.nudSpriteIndex.Location = new System.Drawing.Point(56, 160);
            this.nudSpriteIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudSpriteIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSpriteIndex.Name = "nudSpriteIndex";
            this.nudSpriteIndex.Size = new System.Drawing.Size(56, 23);
            this.nudSpriteIndex.TabIndex = 2;
            // 
            // btnExportSprite
            // 
            this.btnExportSprite.Location = new System.Drawing.Point(20, 192);
            this.btnExportSprite.Margin = new System.Windows.Forms.Padding(0);
            this.btnExportSprite.Name = "btnExportSprite";
            this.btnExportSprite.Size = new System.Drawing.Size(128, 23);
            this.btnExportSprite.TabIndex = 3;
            this.btnExportSprite.Text = "画像をエクスポート";
            this.btnExportSprite.UseVisualStyleBackColor = true;
            // 
            // lblSpriteTileOffset
            // 
            this.lblSpriteTileOffset.AutoSize = true;
            this.lblSpriteTileOffset.Location = new System.Drawing.Point(172, 24);
            this.lblSpriteTileOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpriteTileOffset.Name = "lblSpriteTileOffset";
            this.lblSpriteTileOffset.Size = new System.Drawing.Size(72, 15);
            this.lblSpriteTileOffset.TabIndex = 4;
            this.lblSpriteTileOffset.Text = "画像アドレス :";
            // 
            // txtSpriteTileOffset
            // 
            this.txtSpriteTileOffset.Location = new System.Drawing.Point(268, 20);
            this.txtSpriteTileOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtSpriteTileOffset.Name = "txtSpriteTileOffset";
            this.txtSpriteTileOffset.Size = new System.Drawing.Size(80, 23);
            this.txtSpriteTileOffset.TabIndex = 5;
            // 
            // txtSpritePaletteOffset
            // 
            this.txtSpritePaletteOffset.Location = new System.Drawing.Point(268, 50);
            this.txtSpritePaletteOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtSpritePaletteOffset.Name = "txtSpritePaletteOffset";
            this.txtSpritePaletteOffset.Size = new System.Drawing.Size(80, 23);
            this.txtSpritePaletteOffset.TabIndex = 7;
            // 
            // lblSpritePaletteOffset
            // 
            this.lblSpritePaletteOffset.AutoSize = true;
            this.lblSpritePaletteOffset.Location = new System.Drawing.Point(172, 54);
            this.lblSpritePaletteOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpritePaletteOffset.Name = "lblSpritePaletteOffset";
            this.lblSpritePaletteOffset.Size = new System.Drawing.Size(83, 15);
            this.lblSpritePaletteOffset.TabIndex = 6;
            this.lblSpritePaletteOffset.Text = "パレットアドレス :";
            // 
            // lblSpriteYPosValue
            // 
            this.lblSpriteYPosValue.AutoSize = true;
            this.lblSpriteYPosValue.Location = new System.Drawing.Point(172, 84);
            this.lblSpriteYPosValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpriteYPosValue.Name = "lblSpriteYPosValue";
            this.lblSpriteYPosValue.Size = new System.Drawing.Size(68, 15);
            this.lblSpriteYPosValue.TabIndex = 8;
            this.lblSpriteYPosValue.Text = "Y座標位置 :";
            // 
            // nudSpriteYPosValue
            // 
            this.nudSpriteYPosValue.Location = new System.Drawing.Point(268, 80);
            this.nudSpriteYPosValue.Margin = new System.Windows.Forms.Padding(0);
            this.nudSpriteYPosValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSpriteYPosValue.Name = "nudSpriteYPosValue";
            this.nudSpriteYPosValue.Size = new System.Drawing.Size(80, 23);
            this.nudSpriteYPosValue.TabIndex = 9;
            // 
            // grpAnimData
            // 
            this.grpAnimData.Controls.Add(this.txtSpriteAnimDataOffset);
            this.grpAnimData.Controls.Add(this.lblSpriteAnimDataOffset);
            this.grpAnimData.Controls.Add(this.txtSpriteAnimPointerOffset);
            this.grpAnimData.Controls.Add(this.lblSpriteAnimPointerOffset);
            this.grpAnimData.Location = new System.Drawing.Point(172, 116);
            this.grpAnimData.Margin = new System.Windows.Forms.Padding(0);
            this.grpAnimData.Name = "grpAnimData";
            this.grpAnimData.Padding = new System.Windows.Forms.Padding(0);
            this.grpAnimData.Size = new System.Drawing.Size(224, 100);
            this.grpAnimData.TabIndex = 10;
            this.grpAnimData.TabStop = false;
            this.grpAnimData.Text = "アニメーション?";
            // 
            // txtSpriteAnimDataOffset
            // 
            this.txtSpriteAnimDataOffset.Location = new System.Drawing.Point(116, 58);
            this.txtSpriteAnimDataOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtSpriteAnimDataOffset.Name = "txtSpriteAnimDataOffset";
            this.txtSpriteAnimDataOffset.ReadOnly = true;
            this.txtSpriteAnimDataOffset.Size = new System.Drawing.Size(80, 23);
            this.txtSpriteAnimDataOffset.TabIndex = 11;
            // 
            // lblSpriteAnimDataOffset
            // 
            this.lblSpriteAnimDataOffset.AutoSize = true;
            this.lblSpriteAnimDataOffset.Location = new System.Drawing.Point(20, 62);
            this.lblSpriteAnimDataOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpriteAnimDataOffset.Name = "lblSpriteAnimDataOffset";
            this.lblSpriteAnimDataOffset.Size = new System.Drawing.Size(74, 15);
            this.lblSpriteAnimDataOffset.TabIndex = 10;
            this.lblSpriteAnimDataOffset.Text = "データアドレス :";
            // 
            // txtSpriteAnimPointerOffset
            // 
            this.txtSpriteAnimPointerOffset.Location = new System.Drawing.Point(116, 28);
            this.txtSpriteAnimPointerOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtSpriteAnimPointerOffset.Name = "txtSpriteAnimPointerOffset";
            this.txtSpriteAnimPointerOffset.ReadOnly = true;
            this.txtSpriteAnimPointerOffset.Size = new System.Drawing.Size(80, 23);
            this.txtSpriteAnimPointerOffset.TabIndex = 9;
            // 
            // lblSpriteAnimPointerOffset
            // 
            this.lblSpriteAnimPointerOffset.AutoSize = true;
            this.lblSpriteAnimPointerOffset.Location = new System.Drawing.Point(20, 32);
            this.lblSpriteAnimPointerOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpriteAnimPointerOffset.Name = "lblSpriteAnimPointerOffset";
            this.lblSpriteAnimPointerOffset.Size = new System.Drawing.Size(85, 15);
            this.lblSpriteAnimPointerOffset.TabIndex = 8;
            this.lblSpriteAnimPointerOffset.Text = "ポインタアドレス :";
            // 
            // btnImportSpriteTile
            // 
            this.btnImportSpriteTile.Location = new System.Drawing.Point(362, 20);
            this.btnImportSpriteTile.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportSpriteTile.Name = "btnImportSpriteTile";
            this.btnImportSpriteTile.Size = new System.Drawing.Size(128, 23);
            this.btnImportSpriteTile.TabIndex = 11;
            this.btnImportSpriteTile.Text = "画像をインポート";
            this.btnImportSpriteTile.UseVisualStyleBackColor = true;
            // 
            // btnImportSpritePalette
            // 
            this.btnImportSpritePalette.Location = new System.Drawing.Point(362, 50);
            this.btnImportSpritePalette.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportSpritePalette.Name = "btnImportSpritePalette";
            this.btnImportSpritePalette.Size = new System.Drawing.Size(128, 23);
            this.btnImportSpritePalette.TabIndex = 12;
            this.btnImportSpritePalette.Text = "パレットをインポート";
            this.btnImportSpritePalette.UseVisualStyleBackColor = true;
            // 
            // TrainerSprite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 235);
            this.Controls.Add(this.btnImportSpritePalette);
            this.Controls.Add(this.btnImportSpriteTile);
            this.Controls.Add(this.grpAnimData);
            this.Controls.Add(this.nudSpriteYPosValue);
            this.Controls.Add(this.lblSpriteYPosValue);
            this.Controls.Add(this.txtSpritePaletteOffset);
            this.Controls.Add(this.lblSpritePaletteOffset);
            this.Controls.Add(this.txtSpriteTileOffset);
            this.Controls.Add(this.lblSpriteTileOffset);
            this.Controls.Add(this.btnExportSprite);
            this.Controls.Add(this.nudSpriteIndex);
            this.Controls.Add(this.btnSpriteIndexNext);
            this.Controls.Add(this.btnSpriteIndexPrev);
            this.Controls.Add(this.picSpriteBmp);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TrainerSprite";
            this.Text = "トレーナー画像";
            ((System.ComponentModel.ISupportInitialize)(this.picSpriteBmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteYPosValue)).EndInit();
            this.grpAnimData.ResumeLayout(false);
            this.grpAnimData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picSpriteBmp;
        private System.Windows.Forms.Button btnSpriteIndexPrev;
        private System.Windows.Forms.Button btnSpriteIndexNext;
        private System.Windows.Forms.NumericUpDown nudSpriteIndex;
        private System.Windows.Forms.Button btnExportSprite;
        private System.Windows.Forms.Label lblSpriteTileOffset;
        private _Utilities._CustomCtrls.HexTextBox txtSpriteTileOffset;
        private _Utilities._CustomCtrls.HexTextBox txtSpritePaletteOffset;
        private System.Windows.Forms.Label lblSpritePaletteOffset;
        private System.Windows.Forms.Label lblSpriteYPosValue;
        private System.Windows.Forms.NumericUpDown nudSpriteYPosValue;
        private System.Windows.Forms.GroupBox grpAnimData;
        private _Utilities._CustomCtrls.HexTextBox txtSpriteAnimDataOffset;
        private System.Windows.Forms.Label lblSpriteAnimDataOffset;
        private _Utilities._CustomCtrls.HexTextBox txtSpriteAnimPointerOffset;
        private System.Windows.Forms.Label lblSpriteAnimPointerOffset;
        private System.Windows.Forms.Button btnImportSpriteTile;
        private System.Windows.Forms.Button btnImportSpritePalette;
    }
}