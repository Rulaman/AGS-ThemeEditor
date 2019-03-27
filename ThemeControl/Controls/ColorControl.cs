using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThemeControl
{
	public partial class ColorControl : UserControl, System.ComponentModel.INotifyPropertyChanged
	{
		private static readonly Rectangle DescPosition = new Rectangle(2, 1, 102, 20);
		private static readonly Rectangle ValuePosition = new Rectangle(150, 5, 12, 12); // 12x12
		private static readonly Rectangle BorderPosition = new Rectangle(149, 4, 13, 13); // 14x14
		private static readonly Rectangle ValueTextPosition = new Rectangle(170, 1, 80, 20);
		private static readonly Font DescFont = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
		private static readonly Brush DescBrush = Brushes.Black;
		private Brush AlphaBrush = null;

		private Brush ValueBrush = Brushes.White;
		private string TileText = "#FFFFFFFF";
		private TextBox ValueTextBox = null;

		public ColorControl()
		{
			InitializeComponent();

			using ( Bitmap b = new Bitmap(ValuePosition.Width, ValuePosition.Height) )
			{
				using ( Graphics g = Graphics.FromImage(b) )
				{
					g.FillRectangle(Brushes.White, 0, 0, b.Width, b.Height);
					g.FillRectangle(Brushes.LightGray, 0, 0, 3, 3);
					g.FillRectangle(Brushes.LightGray, 6, 0, 3, 3);

					g.FillRectangle(Brushes.LightGray, 3, 3, 3, 3);
					g.FillRectangle(Brushes.LightGray, 9, 3, 3, 3);

					g.FillRectangle(Brushes.LightGray, 0, 6, 3, 3);
					g.FillRectangle(Brushes.LightGray, 6, 6, 3, 3);

					g.FillRectangle(Brushes.LightGray, 3, 9, 3, 3);
					g.FillRectangle(Brushes.LightGray, 9, 9, 3, 3);
				}
				AlphaBrush = new TextureBrush(b);
			}
		}

		private TextBox GetTextBox(string value)
		{
			if ( ValueTextBox == null )
			{
				ValueTextBox = new TextBox() { Location = ValueTextPosition.Location, Width = ValueTextPosition.Width, Height = ValueTextPosition.Height };
				ValueTextBox.Text = value;
				ValueTextBox.LostFocus += delegate (object sender, EventArgs e)
				{
					this.Hide();
				};

				ValueTextBox.KeyDown += delegate (object sender, KeyEventArgs e)
				{
					switch ( e.KeyCode )
					{
						case Keys.Enter:

							if ( Parse(ValueTextBox.Text, out int num) )
							{
								ValueTextBox.Visible = false;
								TileColor = Color.FromArgb(num);
							}
							break;

						default:
							break;
					};
				};

				this.Controls.Add(ValueTextBox);
			}

			return ValueTextBox;
		}

		private bool Parse(string value, out int number)
		{
			bool val = true;

			number = 0;

			try
			{
				if ( value.ToLower().StartsWith("0x") )
				{
					number = int.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
				}
				else if ( value.ToLower().StartsWith("#") )
				{
					number = int.Parse(value.Substring(1), System.Globalization.NumberStyles.HexNumber);
				}
				else if ( value.ToLower().EndsWith("h") )
				{
					number = int.Parse(value.TrimEnd('h'), System.Globalization.NumberStyles.HexNumber);
				}
				else if ( value.Contains(",") || value.Contains(";") )
				{
					string[] arr = value.Split(new[] { ',', ';' });
					int[] iarr = new int[arr.Length];

					for ( int i = 0; i < arr.Length; i++ )
					{
						int.TryParse(arr[i], out int var);
						iarr[i] = var;
					}

					switch ( arr.Length )
					{
						case 3:
							number = (int)(iarr[0] * 255 * 255 + iarr[1] * 255 + iarr[2]);
							break;

						case 4:
							number = (int)(iarr[0] * 255 * 255 * 255 + iarr[1] * 255 * 255 + iarr[2] * 255 + iarr[3]);
							break;

						default:
							break;
					}
				}
				else
				{
					number = int.Parse(value, System.Globalization.NumberStyles.Integer);
				}
			}
			catch { val = false; }

			uint v = (uint)number;

			return val;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if ( e.ClipRectangle != Rectangle.Empty )
			{
				if ( TileColor.A < 255 )
				{
					e.Graphics.FillRectangle(AlphaBrush, ValuePosition);
				}
				e.Graphics.DrawRectangle(Pens.Black, BorderPosition);
				e.Graphics.FillRectangle(ValueBrush, ValuePosition);
				e.Graphics.DrawString(Description, DescFont, DescBrush, DescPosition);
				e.Graphics.DrawString(TileText, DescFont, DescBrush, ValueTextPosition);
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			MouseEventArgs ea = (MouseEventArgs)e;

			Rectangle r = new Rectangle(ea.X, ea.Y, 1, 1);

			if ( ValueTextPosition.IntersectsWith(r) )
			{
				GetTextBox(TileText).Show();
			}
			else
			{
				GetTextBox(TileText).Hide();
			}
		}

		public string Description { get => Get("Dummy"); set => Notify(value); }

		public Color TileColor
		{
			get => Get(Color.Aquamarine);
			set
			{
				Notify(value);
				ValueBrush = new SolidBrush(value);
				TileText = "#" + value.A.ToString("X2") + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
				Refresh();
			}
		}

		#region INotifyPropertyChanged - New

		private System.Collections.Generic.Dictionary<string, object> _properties = new System.Collections.Generic.Dictionary<string, object>();

		/// <summary>Gets the value of a property</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		protected T Get<T>(T defaultvalue, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
		{
			System.Diagnostics.Debug.Assert(name != null, "name != null");

			if ( _properties.TryGetValue(name, out object value) )
			{
				if ( value == null )
				{
					_properties.Add(name, defaultvalue);
				}

				return value == null ? defaultvalue : (T)value;
			}

			else
			{
				_properties.Add(name, defaultvalue);
			}

			return defaultvalue;
		}

		/// <summary>Sets the value of a property</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <remarks>Use this overload when implicitly naming the property</remarks>
		protected bool Notify<T>(T value, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
		{
			System.Diagnostics.Debug.Assert(name != null, "name != null");

			if ( _properties.TryGetValue(name, out object o) )
			{
				if ( Equals(value, Get<T>(default(T), name)) )
				{
					return false;
				}

				_properties[name] = value;
			}
			else
			{
				_properties.Add(name, value);
			}

			OnPropertyChanged(name);

			return true;
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		private System.Windows.Forms.Form MainForm = null;

		protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
		{
			System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;

			if ( handler != null )
			{
				if ( MainForm == null )
				{
					if ( System.Windows.Forms.Application.OpenForms.Count > 0 )
					{
						MainForm = System.Windows.Forms.Application.OpenForms[0];
					}
				}

				if ( MainForm != null )
				{
					if ( MainForm.InvokeRequired )
					{
						// We are not in UI Thread now
						MainForm.Invoke(handler, new object[] { this, new System.ComponentModel.PropertyChangedEventArgs(propertyName) });
					}
					else
					{
						handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
					}
				}
			}
		}

		#endregion INotifyPropertyChanged - New
	}
}
