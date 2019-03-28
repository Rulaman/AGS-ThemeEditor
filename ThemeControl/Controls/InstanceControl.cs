using System.Drawing;
using System.Windows.Forms;

namespace ThemeControl.Controls
{
	public partial class InstanceControl : UserControl
	{
		private bool Displayed = false;
		private string SeparateSymbol = " -> ";
		private string LastLabel = string.Empty;

		public InstanceControl()
		{
			InitializeComponent();
		}

		public void DisplayClass(object instance)
		{
			if ( !Displayed )
			{
				Panel.SuspendLayout();

				try
				{
					System.Reflection.PropertyInfo[] piList = instance.GetType().GetProperties();

					foreach ( var item in piList )
					{
						HandleProperties(item, instance);
					}
				}
				finally
				{
					Panel.ResumeLayout();
					Displayed = true;
				}
			}
		}

		private void HandleProperties(System.Reflection.PropertyInfo pi, object tobj, string multilabel = null)
		{
			if ( !pi.PropertyType.IsValueType ) // class, struct, (string), ...
			{
				switch ( pi.PropertyType.Name )
				{
					case "ISite":
					case "DataBindings":
					case "BindingContext":
					case "ControlBindingsCollection":
					case "IBindableComponent":
						break;
					case "ColorClass":
						{
							CheckMultilabel(multilabel);
							DoColor(tobj, pi);
						}
						break;
					case "String":
						{
							CheckMultilabel(multilabel);
							DoString(tobj, pi);
						}
						break;
					default:
						{
							multilabel += SeparateSymbol + pi.Name;

							foreach ( var item in pi.PropertyType.GetProperties() )
							{
								object o = pi.GetValue(tobj, null);
								HandleProperties(item, o, multilabel);
							}
						}
						break;
				}
			}
			else
			{
				switch ( pi.PropertyType.Name )
				{
					case "ISite":
					case "DataBindings":
					case "BindingContext":
					case "ControlBindingsCollection":
					case "IBindableComponent":
						break;
					case "Boolean":
						{
							DoBoolean(tobj, pi);
						}
						break;
					case "Int32":
						{
							DoInteger(tobj, pi);
						}
						break;
					default:
						{
						}
						break;
				};

			}
		}

		private void CheckMultilabel(string label)
		{
			if ( !string.IsNullOrEmpty(label) && label != LastLabel )
			{
				if ( label != LastLabel )
				{
					LastLabel = label;
				}

				if ( label.StartsWith(SeparateSymbol) )
				{
					label = label.Substring(SeparateSymbol.Length, label.Length - SeparateSymbol.Length);
				}
				if ( label.EndsWith(SeparateSymbol) )
				{
					label = label.Substring(0, label.Length - SeparateSymbol.Length);
				}

				LabelControl lbl = new LabelControl { Margin = new Padding(0, 0, 0, 0), Width = 500, DisplayName = label, Font = new Font("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel) };
				Panel.Controls.Add(lbl);
			}
		}

		private void DoString(object tobj, System.Reflection.PropertyInfo pi)
		{
			string s = (string)pi.GetValue(tobj, null);

			StringControl lbl = new StringControl { Margin = new Padding(0, 0, 0, 0), Width = 500, Description = pi.Name, Value = s, Font = new Font("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel) };
			Binding bind = new Binding("Text", tobj, pi.Name, true, DataSourceUpdateMode.OnPropertyChanged);
			lbl.DataBindings.Add(bind);
			Panel.Controls.Add(lbl);
		}

		private void DoBoolean(object tobj, System.Reflection.PropertyInfo pi)
		{
			bool s = (bool)pi.GetValue(tobj, null);

			StringControl lbl = new StringControl { Margin = new Padding(0, 0, 0, 0), Width = 500, Description = pi.Name, Value = s.ToString(), Font = new Font("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel) };
			Binding bind = new Binding("Text", tobj, pi.Name, true, DataSourceUpdateMode.OnPropertyChanged);
			bind.Format += Data.BoolBind.Format;
			bind.Parse += Data.BoolBind.Parse;
			lbl.DataBindings.Add(bind);
			Panel.Controls.Add(lbl);
		}

		private void DoInteger(object tobj, System.Reflection.PropertyInfo pi)
		{
			int s = (int)pi.GetValue(tobj, null);

			IntegerControl lbl = new IntegerControl { Margin = new Padding(0, 0, 0, 0), Width = 500, Description = pi.Name, Value = s, Font = new Font("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel) };
			Binding bind = new Binding("Text", tobj, pi.Name, true, DataSourceUpdateMode.OnPropertyChanged);
			bind.Format += Data.IntBind.Format;
			bind.Parse += Data.IntBind.Parse;
			lbl.DataBindings.Add(bind);
			Panel.Controls.Add(lbl);
		}

		private void DoColor(object tobj, System.Reflection.PropertyInfo prop)
		{
			var v = prop.GetValue(tobj, null);

			if ( v != null )
			{
				string[] arr = v.ToString().Split(new[] { ',', ';' });
				int[] iarr = System.Array.ConvertAll(arr, int.Parse);

				Color color = Color.Transparent;

				switch ( arr.Length )
				{
					case 1: // known name
						color = Color.FromName(v.ToString());
						break;
					case 3:
						color = Color.FromArgb(iarr[0], iarr[1], iarr[2]);
						break;
					case 4:
						color = Color.FromArgb(iarr[0], iarr[1], iarr[2], iarr[3]);
						break;
				};

				ColorControl cc = new ColorControl { Margin = new Padding(0, 0, 0, 0), Width = 250, Height = 24, Description = prop.Name, TileColor = color, Padding = new Padding(0) };
				Binding bind = new Binding("TileColor", tobj, prop.Name, true, DataSourceUpdateMode.OnPropertyChanged);
				bind.Format += Data.ColorBind.Format;
				bind.Parse += Data.ColorBind.Parse;
				cc.DataBindings.Add(bind);
				Panel.Controls.Add(cc);
			}
		}
	}
}
