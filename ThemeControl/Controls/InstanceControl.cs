using System.Drawing;
using System.Windows.Forms;

namespace ThemeControl.Controls
{
	public partial class InstanceControl : UserControl
	{
		private bool Displayed = false;

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
						HandleProperties(item, instance, 0);
					}
				}
				finally
				{
					Panel.ResumeLayout();
					Displayed = true;
				}
			}
		}

		private void HandleProperties(System.Reflection.PropertyInfo pi, object tobj, int level)
		{
			var uidPi = tobj.GetType().GetProperties();

			if ( !pi.PropertyType.IsValueType && pi.PropertyType.Name != "String" )   // class, struct,...
			{
				switch ( pi.PropertyType.Name )
				{
					case "ColorClass":
						DoColorClass(tobj, pi, level);
						break;
					case "ISite":
					case "DataBindings":
					case "BindingContext":
					case "ControlBindingsCollection":
					case "IBindableComponent":
						break;
					default:
						{
							Label n = new Label { Margin = new Padding(level, 0, 0, 0), Width = 300, Height = 30, Text = pi.Name, Font = new Font("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel), Location = new Point(0, 0) };
							Panel.Controls.Add(n);

							foreach ( var item in pi.PropertyType.GetProperties() )
							{
								object o = pi.GetValue(tobj, null);
								HandleProperties(item, o, level + 20); // 20 pixel einrücken
							}
						}
						break;
				}
			}
			else
			{
				System.Type t = tobj.GetType();

				foreach ( var up in uidPi )
				{
					switch ( up.Name )
					{
						case "ISite":
						case "DataBindings":
						case "BindingContext":
						case "ControlBindingsCollection":
						case "IBindableComponent":
							break;
						default:
							System.Reflection.PropertyInfo prop = t.GetProperty(up.Name);

							switch ( prop.PropertyType.Name )
							{
								case "String":
									{
										Panel p = new Panel { Margin = new Padding(level, 0, 0, 0), Width = 300, Height = 30 };

										Label n = new Label { Width = 150, Height = 30, Text = prop.Name, Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel), Location = new Point(0,0) };

										string s = (string)prop.GetValue(tobj, null);
										Label l = new Label { Width = 250, Height = 30, Text = s, Font = new Font("Segoe UI", 12f, FontStyle.Italic, GraphicsUnit.Pixel), Location = new Point(152, 0) };
										Binding bind = new Binding("Text", tobj, prop.Name, true, DataSourceUpdateMode.OnPropertyChanged);
										l.DataBindings.Add(bind);

										p.Controls.Add(n);
										p.Controls.Add(l);
										Panel.Controls.Add(p);
									}
									break;
								case "ColorClass":
									{
										DoColorClass(tobj, prop, level);
									}
									break;
							};

							break;
					};
				}
			}
		}

		private void DoColorClass(object tobj, System.Reflection.PropertyInfo prop, int level)
		{
			var v = prop.GetValue(tobj, null);

			string[] arr = v.ToString().Split(new[] { ',', ';' });
			int[] iarr = new int[arr.Length];

			for ( int i = 0; i < arr.Length; i++ )
			{
				int.TryParse(arr[i], out int var);
				iarr[i] = var;
			}

			Color color = Color.Transparent;

			switch ( arr.Length )
			{
				case 3:
					{
						color = Color.FromArgb(iarr[0], iarr[1], iarr[2]);
					}
					break;
				case 4:
					{
						color = Color.FromArgb(iarr[0], iarr[1], iarr[2], iarr[3]);
					}
					break;
			};

			if ( v != null )
			{
				ColorControl cc = new ColorControl { Margin = new Padding(level, 0, 0, 0), Width = 250, Height = 24, Description = prop.Name, TileColor = color, Padding = new Padding(0) };
				Binding bind = new Binding("TileColor", tobj, prop.Name, true, DataSourceUpdateMode.OnPropertyChanged);
				cc.DataBindings.Add(bind);
				Panel.Controls.Add(cc);
			}
		}
	}
}
