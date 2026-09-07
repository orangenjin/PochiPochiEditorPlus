
namespace PochiPochiEditorPlus._Utilities._QuickInput
{
    partial class QuickInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickInputForm));
            this.grpInputData = new System.Windows.Forms.GroupBox();
            this.lblResultOffset = new System.Windows.Forms.Label();
            this.txtResultOffset = new PochiPochiEditorPlus._Utilities._CustomCtrls.HexTextBox();
            this.lblResultIndex = new System.Windows.Forms.Label();
            this.cmbResultIndex = new System.Windows.Forms.ComboBox();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.nudResultCount = new System.Windows.Forms.NumericUpDown();
            this.lblResultPath = new System.Windows.Forms.Label();
            this.txtResultPath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.grpInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudResultCount)).BeginInit();
            this.SuspendLayout();
            // 
            // grpInputData
            // 
            this.grpInputData.Controls.Add(this.btnSelectFile);
            this.grpInputData.Controls.Add(this.txtResultPath);
            this.grpInputData.Controls.Add(this.lblResultPath);
            this.grpInputData.Controls.Add(this.nudResultCount);
            this.grpInputData.Controls.Add(this.lblResultCount);
            this.grpInputData.Controls.Add(this.cmbResultIndex);
            this.grpInputData.Controls.Add(this.lblResultIndex);
            this.grpInputData.Controls.Add(this.txtResultOffset);
            this.grpInputData.Controls.Add(this.lblResultOffset);
            this.grpInputData.Location = new System.Drawing.Point(20, 16);
            this.grpInputData.Margin = new System.Windows.Forms.Padding(0);
            this.grpInputData.Name = "grpInputData";
            this.grpInputData.Padding = new System.Windows.Forms.Padding(0);
            this.grpInputData.Size = new System.Drawing.Size(420, 162);
            this.grpInputData.TabIndex = 0;
            this.grpInputData.TabStop = false;
            this.grpInputData.Text = "入力情報";
            // 
            // lblResultOffset
            // 
            this.lblResultOffset.AutoSize = true;
            this.lblResultOffset.Location = new System.Drawing.Point(20, 32);
            this.lblResultOffset.Margin = new System.Windows.Forms.Padding(0);
            this.lblResultOffset.Name = "lblResultOffset";
            this.lblResultOffset.Size = new System.Drawing.Size(104, 15);
            this.lblResultOffset.TabIndex = 0;
            this.lblResultOffset.Text = "書き込み先アドレス :";
            // 
            // txtResultOffset
            // 
            this.txtResultOffset.Location = new System.Drawing.Point(136, 28);
            this.txtResultOffset.Margin = new System.Windows.Forms.Padding(0);
            this.txtResultOffset.Name = "txtResultOffset";
            this.txtResultOffset.Size = new System.Drawing.Size(100, 23);
            this.txtResultOffset.TabIndex = 1;
            // 
            // lblResultIndex
            // 
            this.lblResultIndex.AutoSize = true;
            this.lblResultIndex.Location = new System.Drawing.Point(20, 62);
            this.lblResultIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblResultIndex.Name = "lblResultIndex";
            this.lblResultIndex.Size = new System.Drawing.Size(66, 15);
            this.lblResultIndex.TabIndex = 2;
            this.lblResultIndex.Text = "データタイプ :";
            // 
            // cmbResultIndex
            // 
            this.cmbResultIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResultIndex.FormattingEnabled = true;
            this.cmbResultIndex.Location = new System.Drawing.Point(136, 58);
            this.cmbResultIndex.Margin = new System.Windows.Forms.Padding(0);
            this.cmbResultIndex.Name = "cmbResultIndex";
            this.cmbResultIndex.Size = new System.Drawing.Size(152, 23);
            this.cmbResultIndex.TabIndex = 3;
            // 
            // lblResultCount
            // 
            this.lblResultCount.AutoSize = true;
            this.lblResultCount.Location = new System.Drawing.Point(20, 92);
            this.lblResultCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(67, 15);
            this.lblResultCount.TabIndex = 4;
            this.lblResultCount.Text = "エントリー数 :";
            // 
            // nudResultCount
            // 
            this.nudResultCount.Location = new System.Drawing.Point(136, 88);
            this.nudResultCount.Margin = new System.Windows.Forms.Padding(0);
            this.nudResultCount.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudResultCount.Name = "nudResultCount";
            this.nudResultCount.Size = new System.Drawing.Size(72, 23);
            this.nudResultCount.TabIndex = 5;
            // 
            // lblResultPath
            // 
            this.lblResultPath.AutoSize = true;
            this.lblResultPath.Location = new System.Drawing.Point(20, 122);
            this.lblResultPath.Margin = new System.Windows.Forms.Padding(0);
            this.lblResultPath.Name = "lblResultPath";
            this.lblResultPath.Size = new System.Drawing.Size(71, 15);
            this.lblResultPath.TabIndex = 6;
            this.lblResultPath.Text = "参照ファイル :";
            // 
            // txtResultPath
            // 
            this.txtResultPath.Location = new System.Drawing.Point(136, 118);
            this.txtResultPath.Margin = new System.Windows.Forms.Padding(0);
            this.txtResultPath.Name = "txtResultPath";
            this.txtResultPath.ReadOnly = true;
            this.txtResultPath.Size = new System.Drawing.Size(176, 23);
            this.txtResultPath.TabIndex = 7;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(322, 118);
            this.btnSelectFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(72, 23);
            this.btnSelectFile.TabIndex = 8;
            this.btnSelectFile.Text = "選択";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(20, 184);
            this.btnApply.Margin = new System.Windows.Forms.Padding(0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(420, 31);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "適用";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // QuickInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 235);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.grpInputData);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "QuickInputForm";
            this.Text = "入力画面";
            this.grpInputData.ResumeLayout(false);
            this.grpInputData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudResultCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpInputData;
        private System.Windows.Forms.Label lblResultOffset;
        private _CustomCtrls.HexTextBox txtResultOffset;
        private System.Windows.Forms.Label lblResultPath;
        private System.Windows.Forms.NumericUpDown nudResultCount;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.ComboBox cmbResultIndex;
        private System.Windows.Forms.Label lblResultIndex;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtResultPath;
        private System.Windows.Forms.Button btnApply;
    }
}