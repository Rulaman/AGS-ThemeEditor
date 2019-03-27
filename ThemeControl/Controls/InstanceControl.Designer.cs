namespace ThemeControl.Controls
{
	partial class InstanceControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Panel = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// Panel
			// 
			this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Panel.AutoScroll = true;
			this.Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.Panel.Location = new System.Drawing.Point(3, 3);
			this.Panel.Name = "Panel";
			this.Panel.Size = new System.Drawing.Size(277, 564);
			this.Panel.TabIndex = 0;
			this.Panel.WrapContents = false;
			// 
			// InstanceControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.Panel);
			this.MinimumSize = new System.Drawing.Size(260, 100);
			this.Name = "InstanceControl";
			this.Size = new System.Drawing.Size(283, 570);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel Panel;
	}
}
