
namespace PochiPochiEditorPlus._Forms
{
    partial class TrainerClassEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainerClassEditor));
            this.nudClassNameIndex = new System.Windows.Forms.NumericUpDown();
            this.cmbClassNameIndex = new System.Windows.Forms.ComboBox();
            this.grpClassData = new System.Windows.Forms.GroupBox();
            this.nudClassPrizeMultiValue = new System.Windows.Forms.NumericUpDown();
            this.lblClassPrizeMultiValue = new System.Windows.Forms.Label();
            this.lblClassNameStr = new System.Windows.Forms.Label();
            this.lblClassNameIndex = new System.Windows.Forms.Label();
            this.grpExtraData = new System.Windows.Forms.GroupBox();
            this.nudBaseIvValue = new System.Windows.Forms.NumericUpDown();
            this.nudPokeBallIndex = new System.Windows.Forms.NumericUpDown();
            this.lblBaseIvValue = new System.Windows.Forms.Label();
            this.lblPokeBallIndex = new System.Windows.Forms.Label();
            this.nudBattleMusicIndex = new System.Windows.Forms.NumericUpDown();
            this.nudEncMusicIndex = new System.Windows.Forms.NumericUpDown();
            this.lblBattleMusicIndex = new System.Windows.Forms.Label();
            this.lblEncMusicIndex = new System.Windows.Forms.Label();
            this.txtClassNameStr = new PochiPochiEditorPlus._Utilities._CustomCtrls.StrTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassNameIndex)).BeginInit();
            this.grpClassData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPrizeMultiValue)).BeginInit();
            this.grpExtraData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseIvValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPokeBallIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBattleMusicIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEncMusicIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // nudClassNameIndex
            // 
            this.nudClassNameIndex.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudClassNameIndex.Location = new System.Drawing.Point(56, 20);
            this.nudClassNameIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudClassNameIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassNameIndex.Name = "nudClassNameIndex";
            this.nudClassNameIndex.ReadOnly = true;
            this.nudClassNameIndex.Size = new System.Drawing.Size(56, 23);
            this.nudClassNameIndex.TabIndex = 0;
            // 
            // cmbClassNameIndex
            // 
            this.cmbClassNameIndex.FormattingEnabled = true;
            this.cmbClassNameIndex.Location = new System.Drawing.Point(124, 20);
            this.cmbClassNameIndex.Margin = new System.Windows.Forms.Padding(0);
            this.cmbClassNameIndex.Name = "cmbClassNameIndex";
            this.cmbClassNameIndex.Size = new System.Drawing.Size(144, 23);
            this.cmbClassNameIndex.TabIndex = 1;
            // 
            // grpClassData
            // 
            this.grpClassData.Controls.Add(this.txtClassNameStr);
            this.grpClassData.Controls.Add(this.nudClassPrizeMultiValue);
            this.grpClassData.Controls.Add(this.lblClassPrizeMultiValue);
            this.grpClassData.Controls.Add(this.lblClassNameStr);
            this.grpClassData.Location = new System.Drawing.Point(20, 56);
            this.grpClassData.Margin = new System.Windows.Forms.Padding(0);
            this.grpClassData.Name = "grpClassData";
            this.grpClassData.Padding = new System.Windows.Forms.Padding(0);
            this.grpClassData.Size = new System.Drawing.Size(270, 100);
            this.grpClassData.TabIndex = 2;
            this.grpClassData.TabStop = false;
            this.grpClassData.Text = "肩書きデータ";
            // 
            // nudClassPrizeMultiValue
            // 
            this.nudClassPrizeMultiValue.Location = new System.Drawing.Point(104, 58);
            this.nudClassPrizeMultiValue.Margin = new System.Windows.Forms.Padding(0);
            this.nudClassPrizeMultiValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassPrizeMultiValue.Name = "nudClassPrizeMultiValue";
            this.nudClassPrizeMultiValue.Size = new System.Drawing.Size(56, 23);
            this.nudClassPrizeMultiValue.TabIndex = 1;
            // 
            // lblClassPrizeMultiValue
            // 
            this.lblClassPrizeMultiValue.AutoSize = true;
            this.lblClassPrizeMultiValue.Location = new System.Drawing.Point(20, 62);
            this.lblClassPrizeMultiValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblClassPrizeMultiValue.Name = "lblClassPrizeMultiValue";
            this.lblClassPrizeMultiValue.Size = new System.Drawing.Size(61, 15);
            this.lblClassPrizeMultiValue.TabIndex = 0;
            this.lblClassPrizeMultiValue.Text = "賞金倍率 :";
            // 
            // lblClassNameStr
            // 
            this.lblClassNameStr.AutoSize = true;
            this.lblClassNameStr.Location = new System.Drawing.Point(20, 32);
            this.lblClassNameStr.Margin = new System.Windows.Forms.Padding(0);
            this.lblClassNameStr.Name = "lblClassNameStr";
            this.lblClassNameStr.Size = new System.Drawing.Size(58, 15);
            this.lblClassNameStr.TabIndex = 0;
            this.lblClassNameStr.Text = "肩書き名 :";
            // 
            // lblClassNameIndex
            // 
            this.lblClassNameIndex.AutoSize = true;
            this.lblClassNameIndex.Location = new System.Drawing.Point(20, 24);
            this.lblClassNameIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblClassNameIndex.Name = "lblClassNameIndex";
            this.lblClassNameIndex.Size = new System.Drawing.Size(26, 15);
            this.lblClassNameIndex.TabIndex = 3;
            this.lblClassNameIndex.Text = "No.";
            // 
            // grpExtraData
            // 
            this.grpExtraData.Controls.Add(this.nudBaseIvValue);
            this.grpExtraData.Controls.Add(this.nudPokeBallIndex);
            this.grpExtraData.Controls.Add(this.lblBaseIvValue);
            this.grpExtraData.Controls.Add(this.lblPokeBallIndex);
            this.grpExtraData.Controls.Add(this.nudBattleMusicIndex);
            this.grpExtraData.Controls.Add(this.nudEncMusicIndex);
            this.grpExtraData.Controls.Add(this.lblBattleMusicIndex);
            this.grpExtraData.Controls.Add(this.lblEncMusicIndex);
            this.grpExtraData.Location = new System.Drawing.Point(20, 168);
            this.grpExtraData.Margin = new System.Windows.Forms.Padding(0);
            this.grpExtraData.Name = "grpExtraData";
            this.grpExtraData.Padding = new System.Windows.Forms.Padding(0);
            this.grpExtraData.Size = new System.Drawing.Size(200, 160);
            this.grpExtraData.TabIndex = 4;
            this.grpExtraData.TabStop = false;
            this.grpExtraData.Text = "追加データ";
            // 
            // nudBaseIvValue
            // 
            this.nudBaseIvValue.Location = new System.Drawing.Point(104, 118);
            this.nudBaseIvValue.Margin = new System.Windows.Forms.Padding(0);
            this.nudBaseIvValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudBaseIvValue.Name = "nudBaseIvValue";
            this.nudBaseIvValue.Size = new System.Drawing.Size(56, 23);
            this.nudBaseIvValue.TabIndex = 6;
            // 
            // nudPokeBallIndex
            // 
            this.nudPokeBallIndex.Location = new System.Drawing.Point(104, 88);
            this.nudPokeBallIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudPokeBallIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudPokeBallIndex.Name = "nudPokeBallIndex";
            this.nudPokeBallIndex.Size = new System.Drawing.Size(56, 23);
            this.nudPokeBallIndex.TabIndex = 7;
            // 
            // lblBaseIvValue
            // 
            this.lblBaseIvValue.AutoSize = true;
            this.lblBaseIvValue.Location = new System.Drawing.Point(20, 122);
            this.lblBaseIvValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblBaseIvValue.Name = "lblBaseIvValue";
            this.lblBaseIvValue.Size = new System.Drawing.Size(73, 15);
            this.lblBaseIvValue.TabIndex = 4;
            this.lblBaseIvValue.Text = "基礎個体値 :";
            // 
            // lblPokeBallIndex
            // 
            this.lblPokeBallIndex.AutoSize = true;
            this.lblPokeBallIndex.Location = new System.Drawing.Point(20, 92);
            this.lblPokeBallIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblPokeBallIndex.Name = "lblPokeBallIndex";
            this.lblPokeBallIndex.Size = new System.Drawing.Size(76, 15);
            this.lblPokeBallIndex.TabIndex = 5;
            this.lblPokeBallIndex.Text = "使用ボールID :";
            // 
            // nudBattleMusicIndex
            // 
            this.nudBattleMusicIndex.Location = new System.Drawing.Point(104, 58);
            this.nudBattleMusicIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudBattleMusicIndex.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBattleMusicIndex.Name = "nudBattleMusicIndex";
            this.nudBattleMusicIndex.Size = new System.Drawing.Size(72, 23);
            this.nudBattleMusicIndex.TabIndex = 3;
            // 
            // nudEncMusicIndex
            // 
            this.nudEncMusicIndex.Location = new System.Drawing.Point(104, 28);
            this.nudEncMusicIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nudEncMusicIndex.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudEncMusicIndex.Name = "nudEncMusicIndex";
            this.nudEncMusicIndex.Size = new System.Drawing.Size(72, 23);
            this.nudEncMusicIndex.TabIndex = 3;
            // 
            // lblBattleMusicIndex
            // 
            this.lblBattleMusicIndex.AutoSize = true;
            this.lblBattleMusicIndex.Location = new System.Drawing.Point(20, 62);
            this.lblBattleMusicIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblBattleMusicIndex.Name = "lblBattleMusicIndex";
            this.lblBattleMusicIndex.Size = new System.Drawing.Size(75, 15);
            this.lblBattleMusicIndex.TabIndex = 2;
            this.lblBattleMusicIndex.Text = "戦闘中BGM :";
            // 
            // lblEncMusicIndex
            // 
            this.lblEncMusicIndex.AutoSize = true;
            this.lblEncMusicIndex.Location = new System.Drawing.Point(20, 32);
            this.lblEncMusicIndex.Margin = new System.Windows.Forms.Padding(0);
            this.lblEncMusicIndex.Name = "lblEncMusicIndex";
            this.lblEncMusicIndex.Size = new System.Drawing.Size(75, 15);
            this.lblEncMusicIndex.TabIndex = 2;
            this.lblEncMusicIndex.Text = "戦闘前BGM :";
            // 
            // txtClassNameStr
            // 
            this.txtClassNameStr.Location = new System.Drawing.Point(104, 28);
            this.txtClassNameStr.Margin = new System.Windows.Forms.Padding(0);
            this.txtClassNameStr.Name = "txtClassNameStr";
            this.txtClassNameStr.Size = new System.Drawing.Size(144, 23);
            this.txtClassNameStr.TabIndex = 2;
            // 
            // TrainerClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 345);
            this.Controls.Add(this.grpExtraData);
            this.Controls.Add(this.lblClassNameIndex);
            this.Controls.Add(this.grpClassData);
            this.Controls.Add(this.cmbClassNameIndex);
            this.Controls.Add(this.nudClassNameIndex);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TrainerClass";
            this.Text = "トレーナー肩書き";
            ((System.ComponentModel.ISupportInitialize)(this.nudClassNameIndex)).EndInit();
            this.grpClassData.ResumeLayout(false);
            this.grpClassData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPrizeMultiValue)).EndInit();
            this.grpExtraData.ResumeLayout(false);
            this.grpExtraData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseIvValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPokeBallIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBattleMusicIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEncMusicIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudClassNameIndex;
        private System.Windows.Forms.ComboBox cmbClassNameIndex;
        private System.Windows.Forms.GroupBox grpClassData;
        private System.Windows.Forms.Label lblClassNameStr;
        private System.Windows.Forms.NumericUpDown nudClassPrizeMultiValue;
        private System.Windows.Forms.Label lblClassPrizeMultiValue;
        private System.Windows.Forms.Label lblClassNameIndex;
        private System.Windows.Forms.GroupBox grpExtraData;
        private System.Windows.Forms.NumericUpDown nudBattleMusicIndex;
        private System.Windows.Forms.NumericUpDown nudEncMusicIndex;
        private System.Windows.Forms.Label lblBattleMusicIndex;
        private System.Windows.Forms.Label lblEncMusicIndex;
        private System.Windows.Forms.NumericUpDown nudBaseIvValue;
        private System.Windows.Forms.NumericUpDown nudPokeBallIndex;
        private System.Windows.Forms.Label lblBaseIvValue;
        private System.Windows.Forms.Label lblPokeBallIndex;
        private _Utilities._CustomCtrls.StrTextBox txtClassNameStr;
    }
}