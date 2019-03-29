using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThemeControl
{
	public partial class ColorControl : UserControl, System.ComponentModel.INotifyPropertyChanged
	{
		private static readonly int DescLen = 200;
		private static readonly Rectangle DescPosition = new Rectangle(2, 1, DescLen - 2, 20);
		private static readonly Rectangle BorderPosition = new Rectangle(DescLen - 1, 4, 13, 13); // 14x14
		private static readonly Rectangle ValuePosition = new Rectangle(DescLen, 5, 12, 12); // 12x12
		private static readonly Font DescFont = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
		private static readonly Brush DescBrush = Brushes.Black;

		private static readonly Font ValueFont = new Font("Consolas", 14f, FontStyle.Regular, GraphicsUnit.Pixel);

		private Rectangle ValueTextPosition = new Rectangle(DescLen + 20, 1, 100, 20);
		private Brush AlphaBrush = null;
		private Brush ValueBrush = Brushes.White;
		private string ValueText = "#FFFFFFFF";
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

			ValueTextPosition = new Rectangle(DescLen + 20, 1, Width - (DescLen + 20), 20);
		}

		private TextBox GetTextBox(string value)
		{
			if ( ValueTextBox == null )
			{
				ValueTextBox = new TextBox() { Location = ValueTextPosition.Location, Width = ValueTextPosition.Width, Height = ValueTextPosition.Height };
				ValueTextBox.Text = value;
				ValueTextBox.LostFocus += delegate (object sender, EventArgs e)
				{
					ValueTextBox.Hide();
				};

				ValueTextBox.KeyDown += delegate (object sender, KeyEventArgs e)
				{
					switch ( e.KeyCode )
					{
						case Keys.Enter:
							if ( ValueTextBox.Text.Parse(out int num) )
							{
								ValueTextBox.Visible = false;
								TileColor = Color.FromArgb(num);
								Refresh();
							}
							break;
						case Keys.Escape:
							{
								ValueTextBox.Visible = false;
								ValueTextBox.Text = ValueText;
							}
							break;
						default:
							break;
					};
				};

				Controls.Add(ValueTextBox);
			}

			return ValueTextBox;
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
				e.Graphics.DrawString(ValueText, ValueFont, DescBrush, ValueTextPosition);
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			MouseEventArgs ea = (MouseEventArgs)e;

			Rectangle r = new Rectangle(ea.X, ea.Y, 1, 1);

			if ( ValueTextPosition.IntersectsWith(r) )
			{
				GetTextBox(ValueText).Show();
			}
			else
			{
				GetTextBox(ValueText).Hide();
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			ValueTextPosition = new Rectangle(DescLen + 20, 1, Width - (DescLen+20), 20);
		}

		public string Description { get => Get("Dummy"); set => Notify(value); }

		public Color TileColor
		{
			get => Get(Color.Aquamarine);
			set
			{
				Notify(value);
				ValueBrush = new SolidBrush(value);
				ValueText = "#" + value.A.ToString("X2") + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
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
				else
				{
					handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
				}
			}
		}

		#endregion INotifyPropertyChanged - New
	}
}
