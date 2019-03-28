using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThemeControl.Controls
{
	public partial class LabelControl : UserControl
	{
		private Brush B = Brushes.Black;
		private Point Pnt = new Point(0, 0);
		private Point P1 = new Point(0, 0);
		private Point P2 = new Point(0, 0);
		private Graphics G = Graphics.FromImage(new Bitmap(20, 20));

		public LabelControl()
		{
			InitializeComponent();

			SizeF s = G.MeasureString("Mg", Font);
			Pnt = new Point(0, (int)(Height - 2 - s.Height));
			P1 = new Point(0, Height - 1);
			P2 = new Point(Width, Height - 1);
			B = new SolidBrush(ForeColor);
		}

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private string _DisplayName = string.Empty;
		public string DisplayName { internal get { return _DisplayName; }  set { _DisplayName = value; Refresh(); } }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.DrawString(DisplayName, Font, B, Pnt);
			e.Graphics.DrawLine(Pens.Black, P1, P2);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			SizeF s = G.MeasureString("Mg", Font);
			Pnt = new Point(0, (int)(Height-2-s.Height));
		}
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			B = new SolidBrush(ForeColor);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			P1 = new Point(0, Height - 1);
			P2 = new Point(Width, Height - 1);
		}
	}
}
