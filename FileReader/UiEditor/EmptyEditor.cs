using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace AGS.Theme.Editor
{
	public class EmptyEditor : UITypeEditor
	{
		private IWindowsFormsEditorService service;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			// This tells it to show the [...] button which is clickable firing off EditValue below.
			return UITypeEditorEditStyle.None;
			//return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if ( provider != null )
				service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			if ( service != null )
			{
			}

			return "sdf";
		}
	}
}
