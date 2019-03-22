namespace Theme.Editor
{
	using FileReader;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Drawing.Design;
	using System.Windows.Forms;
	using System.Windows.Forms.Design;

	public class MyColorEditor : UITypeEditor
	{
		private IWindowsFormsEditorService service;
		private Bitmap b = new Bitmap(10, 10);

		public MyColorEditor()
		{
			using ( Graphics graphics = Graphics.FromImage(b) )
			{
				graphics.FillRectangle(Brushes.LightGray, 0, 0, b.Width, b.Height);
				graphics.FillRectangle(Brushes.DarkGray, 0, 0, 5, 5);
				graphics.FillRectangle(Brushes.DarkGray, 5, 5, 5, 5);
			}
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			// This tells it to show the [...] button which is clickable firing off EditValue below.
			return UITypeEditorEditStyle.Modal;
			//return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if ( provider != null )
				service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			if ( service != null )
			{
				// This is the code you want to run when the [...] is clicked and after it has been verified.

				// Get our currently selected color.
				ColorClass color = (ColorClass)value;

				// Create a new instance of the ColorDialog.
				ColorDialog selectionControl = new ColorDialog();

				// Set the selected color in the dialog.
				selectionControl.Color = Color.FromArgb(color.GetARGB());

				// Show the dialog.
				selectionControl.ShowDialog();

				// Return the newly selected color.
				value = new ColorClass(selectionControl.Color.ToArgb());
			}

			return value;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
			//return base.GetPaintValueSupported(context);
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			ColorClass c = (ColorClass)e.Value;

			if ( c != null )
			{
				TextureBrush tb = new TextureBrush(b);

				if ( c.A < 255 )
				{
					e.Graphics.FillRectangle(tb, e.Bounds);
				}
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(c.A, c.R, c.G, c.B)), e.Bounds);
				//e.Graphics.DrawRectangle(Pens.Black, e.Bounds);
			}

			base.PaintValue(e);
		}
	}
}
