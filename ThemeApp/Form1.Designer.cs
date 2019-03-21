using System.Windows.Forms;

namespace ThemeApp
{
	partial class Form1
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
			if ( disposing && (components != null) )
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
			this.label1 = new System.Windows.Forms.Label();
			this.pg = new System.Windows.Forms.PropertyGrid();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.BtnLoad = new System.Windows.Forms.Button();
			this.BtnSave = new System.Windows.Forms.Button();
			this.BtnSaveAs = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(701, 70);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.Visible = false;
			// 
			// pg
			// 
			this.pg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pg.Location = new System.Drawing.Point(12, 49);
			this.pg.Name = "pg";
			this.pg.Size = new System.Drawing.Size(573, 663);
			this.pg.TabIndex = 1;
			// 
			// menuStrip1
			// 
			this.menuStrip1.AutoSize = false;
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(692, 30);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(91, 26);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			this.menuStrip1.Visible = false;
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// BtnLoad
			// 
			this.BtnLoad.Location = new System.Drawing.Point(12, 12);
			this.BtnLoad.Name = "BtnLoad";
			this.BtnLoad.Size = new System.Drawing.Size(75, 23);
			this.BtnLoad.TabIndex = 3;
			this.BtnLoad.Text = "Load";
			this.BtnLoad.UseVisualStyleBackColor = true;
			this.BtnLoad.Click += new System.EventHandler(this.Load_Click);
			// 
			// BtnSave
			// 
			this.BtnSave.Location = new System.Drawing.Point(102, 12);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(75, 23);
			this.BtnSave.TabIndex = 3;
			this.BtnSave.Text = "Save";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.Save_Click);
			// 
			// BtnSaveAs
			// 
			this.BtnSaveAs.Location = new System.Drawing.Point(192, 12);
			this.BtnSaveAs.Name = "BtnSaveAs";
			this.BtnSaveAs.Size = new System.Drawing.Size(75, 23);
			this.BtnSaveAs.TabIndex = 3;
			this.BtnSaveAs.Text = "Save as …";
			this.BtnSaveAs.UseVisualStyleBackColor = true;
			this.BtnSaveAs.Click += new System.EventHandler(this.SaveAs_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(597, 724);
			this.Controls.Add(this.BtnSaveAs);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.BtnLoad);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pg);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private PropertyGrid pg;

		#endregion

		private Label label1;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private Button BtnLoad;
		private Button BtnSave;
		private Button BtnSaveAs;
	}
}

