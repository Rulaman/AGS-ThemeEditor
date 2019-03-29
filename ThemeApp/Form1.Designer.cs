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
			this.pg = new System.Windows.Forms.PropertyGrid();
			this.BtnLoad = new System.Windows.Forms.Button();
			this.BtnSave = new System.Windows.Forms.Button();
			this.BtnSaveAs = new System.Windows.Forms.Button();
			this.instanceControl1 = new ThemeControl.Controls.InstanceControl();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pg
			// 
			this.pg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.pg.Location = new System.Drawing.Point(12, 49);
			this.pg.Name = "pg";
			this.pg.Size = new System.Drawing.Size(391, 613);
			this.pg.TabIndex = 1;
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
			// instanceControl1
			// 
			this.instanceControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.instanceControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.instanceControl1.BackColor = System.Drawing.SystemColors.ButtonShadow;
			this.instanceControl1.Location = new System.Drawing.Point(409, 49);
			this.instanceControl1.MinimumSize = new System.Drawing.Size(260, 100);
			this.instanceControl1.Name = "instanceControl1";
			this.instanceControl1.Size = new System.Drawing.Size(691, 613);
			this.instanceControl1.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(336, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "   ";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1112, 674);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.instanceControl1);
			this.Controls.Add(this.BtnSaveAs);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.BtnLoad);
			this.Controls.Add(this.pg);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private PropertyGrid pg;

		#endregion
		private Button BtnLoad;
		private Button BtnSave;
		private Button BtnSaveAs;
		private ThemeControl.Controls.InstanceControl instanceControl1;
		private Label label1;
	}
}

