
namespace PochiPochiEditorPlus._Forms._Tools
{
    partial class TilesetNoCalc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesetNoCalc));
            this.lblTilesetNo = new System.Windows.Forms.Label();
            this.nudTilesetNo = new System.Windows.Forms.NumericUpDown();
            this.btnConvNoToHeaderOffset = new System.Windows.Forms.Button();
            this.btnConvHeaderOffsetToNo = new System.Windows.Forms.Button();
            this.lblHeaderOffset = new System.Windows.Forms.Label();
            this.txtHeaderOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.grpConvResult = new System.Windows.Forms.GroupBox();
            this.lblConvResult = new System.Windows.Forms.Label();
            this.lblRecPair = new System.Windows.Forms.Label();
            this.nudRecTilesetNo = new System.Windows.Forms.NumericUpDown();
            this.txtRecHeaderOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudTilesetNo)).BeginInit();
            this.grpConvResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecTilesetNo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTilesetNo
            // 
            this.lblTilesetNo.AutoSize = true;
            this.lblTilesetNo.Location = new System.Drawing.Point(22, 26);
            this.lblTilesetNo.Margin = new System.Windows.Forms.Padding(0);
            this.lblTilesetNo.Name = "lblTilesetNo";
            this.lblTilesetNo.Size = new System.Drawing.Size(86, 15);
            this.lblTilesetNo.TabIndex = 0;
            this.lblTilesetNo.Text = "タイルセットNo. :";
            // 
            // nudTilesetNo
            // 
            this.nudTilesetNo.Location = new System.Drawing.Point(118, 22);
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
            // btnConvNoToHeaderOffset
            // 
            this.btnConvNoToHeaderOffset.Location = new System.Drawing.Point(25, 52);
            this.btnConvNoToHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.btnConvNoToHeaderOffset.Name = "btnConvNoToHeaderOffset";
            this.btnConvNoToHeaderOffset.Size = new System.Drawing.Size(104, 31);
            this.btnConvNoToHeaderOffset.TabIndex = 2;
            this.btnConvNoToHeaderOffset.Text = "▼アドレスに変換";
            this.btnConvNoToHeaderOffset.UseVisualStyleBackColor = true;
            // 
            // btnConvHeaderOffsetToNo
            // 
            this.btnConvHeaderOffsetToNo.Location = new System.Drawing.Point(140, 52);
            this.btnConvHeaderOffsetToNo.Margin = new System.Windows.Forms.Padding(0);
            this.btnConvHeaderOffsetToNo.Name = "btnConvHeaderOffsetToNo";
            this.btnConvHeaderOffsetToNo.Size = new System.Drawing.Size(104, 31);
            this.btnConvHeaderOffsetToNo.TabIndex = 3;
            this.btnConvHeaderOffsetToNo.Text = "▲番号に変換";
            this.btnConvHeaderOffsetToNo.UseVisualStyleBackColor = true;
            // 
            // lblHeaderOffset
            // 
            this.lblHeaderOffset.AutoSize = true;
            this.lblHeaderOffset.Location = new System.Drawing.Point(22, 94);
            this.lblHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeaderOffset.Name = "lblHeaderOffset";
            this.lblHeaderOffset.Size = new System.Drawing.Size(83, 15);
            this.lblHeaderOffset.TabIndex = 4;
            this.lblHeaderOffset.Text = "ヘッダーアドレス :";
            // 
            // txtHeaderOffset
            // 
            this.txtHeaderOffset.Location = new System.Drawing.Point(118, 90);
            this.txtHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtHeaderOffset.Name = "txtHeaderOffset";
            this.txtHeaderOffset.Size = new System.Drawing.Size(104, 23);
            this.txtHeaderOffset.TabIndex = 5;
            // 
            // grpConvResult
            // 
            this.grpConvResult.Controls.Add(this.txtRecHeaderOffset);
            this.grpConvResult.Controls.Add(this.nudRecTilesetNo);
            this.grpConvResult.Controls.Add(this.lblRecPair);
            this.grpConvResult.Controls.Add(this.lblConvResult);
            this.grpConvResult.Location = new System.Drawing.Point(262, 14);
            this.grpConvResult.Margin = new System.Windows.Forms.Padding(0);
            this.grpConvResult.Name = "grpConvResult";
            this.grpConvResult.Padding = new System.Windows.Forms.Padding(0);
            this.grpConvResult.Size = new System.Drawing.Size(152, 160);
            this.grpConvResult.TabIndex = 6;
            this.grpConvResult.TabStop = false;
            this.grpConvResult.Text = "変換結果";
            // 
            // lblConvResult
            // 
            this.lblConvResult.AutoSize = true;
            this.lblConvResult.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.lblConvResult.Location = new System.Drawing.Point(24, 32);
            this.lblConvResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblConvResult.Name = "lblConvResult";
            this.lblConvResult.Size = new System.Drawing.Size(37, 19);
            this.lblConvResult.TabIndex = 0;
            this.lblConvResult.Text = "結果";
            // 
            // lblRecPair
            // 
            this.lblRecPair.AutoSize = true;
            this.lblRecPair.Location = new System.Drawing.Point(24, 64);
            this.lblRecPair.Margin = new System.Windows.Forms.Padding(0);
            this.lblRecPair.Name = "lblRecPair";
            this.lblRecPair.Size = new System.Drawing.Size(102, 15);
            this.lblRecPair.TabIndex = 1;
            this.lblRecPair.Text = "代替の組み合わせ :";
            // 
            // nudRecTilesetNo
            // 
            this.nudRecTilesetNo.Location = new System.Drawing.Point(24, 88);
            this.nudRecTilesetNo.Margin = new System.Windows.Forms.Padding(0);
            this.nudRecTilesetNo.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.nudRecTilesetNo.Name = "nudRecTilesetNo";
            this.nudRecTilesetNo.ReadOnly = true;
            this.nudRecTilesetNo.Size = new System.Drawing.Size(104, 23);
            this.nudRecTilesetNo.TabIndex = 2;
            // 
            // txtRecHeaderOffset
            // 
            this.txtRecHeaderOffset.Location = new System.Drawing.Point(24, 118);
            this.txtRecHeaderOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtRecHeaderOffset.Name = "txtRecHeaderOffset";
            this.txtRecHeaderOffset.ReadOnly = true;
            this.txtRecHeaderOffset.Size = new System.Drawing.Size(104, 23);
            this.txtRecHeaderOffset.TabIndex = 6;
            // 
            // TilesetCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 193);
            this.Controls.Add(this.grpConvResult);
            this.Controls.Add(this.txtHeaderOffset);
            this.Controls.Add(this.lblHeaderOffset);
            this.Controls.Add(this.btnConvHeaderOffsetToNo);
            this.Controls.Add(this.btnConvNoToHeaderOffset);
            this.Controls.Add(this.nudTilesetNo);
            this.Controls.Add(this.lblTilesetNo);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TilesetCalc";
            this.Text = "タイルセット番号計算";
            ((System.ComponentModel.ISupportInitialize)(this.nudTilesetNo)).EndInit();
            this.grpConvResult.ResumeLayout(false);
            this.grpConvResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecTilesetNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTilesetNo;
        private System.Windows.Forms.NumericUpDown nudTilesetNo;
        private System.Windows.Forms.Button btnConvNoToHeaderOffset;
        private System.Windows.Forms.Button btnConvHeaderOffsetToNo;
        private System.Windows.Forms.Label lblHeaderOffset;
        private _Utilities._CustomCtrls.HexTextBox txtHeaderOffset;
        private System.Windows.Forms.GroupBox grpConvResult;
        private _Utilities._CustomCtrls.HexTextBox txtRecHeaderOffset;
        private System.Windows.Forms.NumericUpDown nudRecTilesetNo;
        private System.Windows.Forms.Label lblRecPair;
        private System.Windows.Forms.Label lblConvResult;
    }
}