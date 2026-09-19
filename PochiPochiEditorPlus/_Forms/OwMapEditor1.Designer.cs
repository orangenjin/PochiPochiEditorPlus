
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
            this.tbpColl = new System.Windows.Forms.TabPage();
            this.tbpEvent = new System.Windows.Forms.TabPage();
            this.tbcMain.SuspendLayout();
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
            this.tbcMain.Size = new System.Drawing.Size(674, 464);
            this.tbcMain.TabIndex = 0;
            // 
            // tbpBlock
            // 
            this.tbpBlock.Location = new System.Drawing.Point(4, 24);
            this.tbpBlock.Margin = new System.Windows.Forms.Padding(0);
            this.tbpBlock.Name = "tbpBlock";
            this.tbpBlock.Size = new System.Drawing.Size(666, 436);
            this.tbpBlock.TabIndex = 0;
            this.tbpBlock.Text = "ブロック";
            this.tbpBlock.UseVisualStyleBackColor = true;
            // 
            // tbpColl
            // 
            this.tbpColl.Location = new System.Drawing.Point(4, 24);
            this.tbpColl.Margin = new System.Windows.Forms.Padding(0);
            this.tbpColl.Name = "tbpColl";
            this.tbpColl.Size = new System.Drawing.Size(666, 436);
            this.tbpColl.TabIndex = 1;
            this.tbpColl.Text = "移動エリア";
            this.tbpColl.UseVisualStyleBackColor = true;
            // 
            // tbpEvent
            // 
            this.tbpEvent.Location = new System.Drawing.Point(4, 24);
            this.tbpEvent.Margin = new System.Windows.Forms.Padding(0);
            this.tbpEvent.Name = "tbpEvent";
            this.tbpEvent.Size = new System.Drawing.Size(666, 436);
            this.tbpEvent.TabIndex = 2;
            this.tbpEvent.Text = "イベント";
            this.tbpEvent.UseVisualStyleBackColor = true;
            // 
            // OwMapEditor1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 562);
            this.Controls.Add(this.tbcMain);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OwMapEditor1";
            this.Text = "マップ";
            this.tbcMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tbpBlock;
        private System.Windows.Forms.TabPage tbpColl;
        private System.Windows.Forms.TabPage tbpEvent;
    }
}