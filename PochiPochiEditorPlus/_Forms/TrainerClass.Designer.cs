
namespace PochiPochiEditorPlus._Forms
{
    partial class TrainerClass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainerClass));
            this.nudClassNameIndex = new System.Windows.Forms.NumericUpDown();
            this.cmbClassNameIndex = new System.Windows.Forms.ComboBox();
            this.grpClassData = new System.Windows.Forms.GroupBox();
            this.nudClassPrizeMulti = new System.Windows.Forms.NumericUpDown();
            this.lblClassPrizeMulti = new System.Windows.Forms.Label();
            this.lblClassName = new System.Windows.Forms.Label();
            this.lblClassNameIndex = new System.Windows.Forms.Label();
            this.grpExtraData = new System.Windows.Forms.GroupBox();
            this.nudBaseIv = new System.Windows.Forms.NumericUpDown();
            this.nudPokeBall = new System.Windows.Forms.NumericUpDown();
            this.lblBaseIv = new System.Windows.Forms.Label();
            this.lblPokeBall = new System.Windows.Forms.Label();
            this.nudBattleMusic = new System.Windows.Forms.NumericUpDown();
            this.nudEncMusic = new System.Windows.Forms.NumericUpDown();
            this.lblBaltteMusic = new System.Windows.Forms.Label();
            this.lblEncMusic = new System.Windows.Forms.Label();
            this.txtClassName = new PochiPochiEditorPlus._Utilities._CustomCtrls.StrTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassNameIndex)).BeginInit();
            this.grpClassData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPrizeMulti)).BeginInit();
            this.grpExtraData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseIv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPokeBall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBattleMusic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEncMusic)).BeginInit();
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
            this.grpClassData.Controls.Add(this.txtClassName);
            this.grpClassData.Controls.Add(this.nudClassPrizeMulti);
            this.grpClassData.Controls.Add(this.lblClassPrizeMulti);
            this.grpClassData.Controls.Add(this.lblClassName);
            this.grpClassData.Location = new System.Drawing.Point(20, 56);
            this.grpClassData.Margin = new System.Windows.Forms.Padding(0);
            this.grpClassData.Name = "grpClassData";
            this.grpClassData.Padding = new System.Windows.Forms.Padding(0);
            this.grpClassData.Size = new System.Drawing.Size(270, 100);
            this.grpClassData.TabIndex = 2;
            this.grpClassData.TabStop = false;
            this.grpClassData.Text = "肩書きデータ";
            // 
            // nudClassPrizeMulti
            // 
            this.nudClassPrizeMulti.Location = new System.Drawing.Point(104, 58);
            this.nudClassPrizeMulti.Margin = new System.Windows.Forms.Padding(0);
            this.nudClassPrizeMulti.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassPrizeMulti.Name = "nudClassPrizeMulti";
            this.nudClassPrizeMulti.Size = new System.Drawing.Size(56, 23);
            this.nudClassPrizeMulti.TabIndex = 1;
            // 
            // lblClassPrizeMulti
            // 
            this.lblClassPrizeMulti.AutoSize = true;
            this.lblClassPrizeMulti.Location = new System.Drawing.Point(20, 62);
            this.lblClassPrizeMulti.Margin = new System.Windows.Forms.Padding(0);
            this.lblClassPrizeMulti.Name = "lblClassPrizeMulti";
            this.lblClassPrizeMulti.Size = new System.Drawing.Size(61, 15);
            this.lblClassPrizeMulti.TabIndex = 0;
            this.lblClassPrizeMulti.Text = "賞金倍率 :";
            // 
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Location = new System.Drawing.Point(20, 32);
            this.lblClassName.Margin = new System.Windows.Forms.Padding(0);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(58, 15);
            this.lblClassName.TabIndex = 0;
            this.lblClassName.Text = "肩書き名 :";
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
            this.grpExtraData.Controls.Add(this.nudBaseIv);
            this.grpExtraData.Controls.Add(this.nudPokeBall);
            this.grpExtraData.Controls.Add(this.lblBaseIv);
            this.grpExtraData.Controls.Add(this.lblPokeBall);
            this.grpExtraData.Controls.Add(this.nudBattleMusic);
            this.grpExtraData.Controls.Add(this.nudEncMusic);
            this.grpExtraData.Controls.Add(this.lblBaltteMusic);
            this.grpExtraData.Controls.Add(this.lblEncMusic);
            this.grpExtraData.Location = new System.Drawing.Point(20, 168);
            this.grpExtraData.Margin = new System.Windows.Forms.Padding(0);
            this.grpExtraData.Name = "grpExtraData";
            this.grpExtraData.Padding = new System.Windows.Forms.Padding(0);
            this.grpExtraData.Size = new System.Drawing.Size(200, 160);
            this.grpExtraData.TabIndex = 4;
            this.grpExtraData.TabStop = false;
            this.grpExtraData.Text = "追加データ";
            // 
            // nudBaseIv
            // 
            this.nudBaseIv.Location = new System.Drawing.Point(104, 118);
            this.nudBaseIv.Margin = new System.Windows.Forms.Padding(0);
            this.nudBaseIv.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudBaseIv.Name = "nudBaseIv";
            this.nudBaseIv.Size = new System.Drawing.Size(56, 23);
            this.nudBaseIv.TabIndex = 6;
            // 
            // nudPokeBall
            // 
            this.nudPokeBall.Location = new System.Drawing.Point(104, 88);
            this.nudPokeBall.Margin = new System.Windows.Forms.Padding(0);
            this.nudPokeBall.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudPokeBall.Name = "nudPokeBall";
            this.nudPokeBall.Size = new System.Drawing.Size(56, 23);
            this.nudPokeBall.TabIndex = 7;
            // 
            // lblBaseIv
            // 
            this.lblBaseIv.AutoSize = true;
            this.lblBaseIv.Location = new System.Drawing.Point(20, 122);
            this.lblBaseIv.Margin = new System.Windows.Forms.Padding(0);
            this.lblBaseIv.Name = "lblBaseIv";
            this.lblBaseIv.Size = new System.Drawing.Size(73, 15);
            this.lblBaseIv.TabIndex = 4;
            this.lblBaseIv.Text = "基礎個体値 :";
            // 
            // lblPokeBall
            // 
            this.lblPokeBall.AutoSize = true;
            this.lblPokeBall.Location = new System.Drawing.Point(20, 92);
            this.lblPokeBall.Margin = new System.Windows.Forms.Padding(0);
            this.lblPokeBall.Name = "lblPokeBall";
            this.lblPokeBall.Size = new System.Drawing.Size(76, 15);
            this.lblPokeBall.TabIndex = 5;
            this.lblPokeBall.Text = "使用ボールID :";
            // 
            // nudBattleMusic
            // 
            this.nudBattleMusic.Location = new System.Drawing.Point(104, 58);
            this.nudBattleMusic.Margin = new System.Windows.Forms.Padding(0);
            this.nudBattleMusic.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBattleMusic.Name = "nudBattleMusic";
            this.nudBattleMusic.Size = new System.Drawing.Size(72, 23);
            this.nudBattleMusic.TabIndex = 3;
            // 
            // nudEncMusic
            // 
            this.nudEncMusic.Location = new System.Drawing.Point(104, 28);
            this.nudEncMusic.Margin = new System.Windows.Forms.Padding(0);
            this.nudEncMusic.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudEncMusic.Name = "nudEncMusic";
            this.nudEncMusic.Size = new System.Drawing.Size(72, 23);
            this.nudEncMusic.TabIndex = 3;
            // 
            // lblBaltteMusic
            // 
            this.lblBaltteMusic.AutoSize = true;
            this.lblBaltteMusic.Location = new System.Drawing.Point(20, 62);
            this.lblBaltteMusic.Margin = new System.Windows.Forms.Padding(0);
            this.lblBaltteMusic.Name = "lblBaltteMusic";
            this.lblBaltteMusic.Size = new System.Drawing.Size(75, 15);
            this.lblBaltteMusic.TabIndex = 2;
            this.lblBaltteMusic.Text = "戦闘中BGM :";
            // 
            // lblEncMusic
            // 
            this.lblEncMusic.AutoSize = true;
            this.lblEncMusic.Location = new System.Drawing.Point(20, 32);
            this.lblEncMusic.Margin = new System.Windows.Forms.Padding(0);
            this.lblEncMusic.Name = "lblEncMusic";
            this.lblEncMusic.Size = new System.Drawing.Size(75, 15);
            this.lblEncMusic.TabIndex = 2;
            this.lblEncMusic.Text = "戦闘前BGM :";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(104, 28);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(0);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(144, 23);
            this.txtClassName.TabIndex = 2;
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
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPrizeMulti)).EndInit();
            this.grpExtraData.ResumeLayout(false);
            this.grpExtraData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseIv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPokeBall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBattleMusic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEncMusic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudClassNameIndex;
        private System.Windows.Forms.ComboBox cmbClassNameIndex;
        private System.Windows.Forms.GroupBox grpClassData;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.NumericUpDown nudClassPrizeMulti;
        private System.Windows.Forms.Label lblClassPrizeMulti;
        private System.Windows.Forms.Label lblClassNameIndex;
        private System.Windows.Forms.GroupBox grpExtraData;
        private System.Windows.Forms.NumericUpDown nudBattleMusic;
        private System.Windows.Forms.NumericUpDown nudEncMusic;
        private System.Windows.Forms.Label lblBaltteMusic;
        private System.Windows.Forms.Label lblEncMusic;
        private System.Windows.Forms.NumericUpDown nudBaseIv;
        private System.Windows.Forms.NumericUpDown nudPokeBall;
        private System.Windows.Forms.Label lblBaseIv;
        private System.Windows.Forms.Label lblPokeBall;
        private _Utilities._CustomCtrls.StrTextBox txtClassName;
    }
}