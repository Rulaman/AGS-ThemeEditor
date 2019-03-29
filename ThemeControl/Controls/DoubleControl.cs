using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThemeControl.Controls
{
	public partial class DoubleControl : UserControl, System.ComponentModel.INotifyPropertyChanged
	{
		private static readonly Rectangle DescPosition = new Rectangle(2, 1, 102, 20);
		private static readonly Font DescFont = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
		private static readonly Brush DescBrush = Brushes.Black;

		private Rectangle ValuePosition = new Rectangle(200, 1, 500, 20);
		private string ValueText = "Dummy";
		private TextBox ValueTextBox = null;

		public DoubleControl()
		{
			InitializeComponent();
		}

		private TextBox GetTextBox(string value)
		{
			if ( ValueTextBox == null )
			{
				ValueTextBox = new TextBox() { Location = ValuePosition.Location, Width = ValuePosition.Width, Height = ValuePosition.Height };
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
							{
								ValueTextBox.Visible = false;
								double.TryParse(ValueTextBox.Text, out double d);
								Value = d;
							}
							break;
						case Keys.Escape:
							{
								ValueTextBox.Visible = false;
								ValueTextBox.Text = Value.ToString();
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
				e.Graphics.DrawString(Description, DescFont, DescBrush, DescPosition);
				e.Graphics.DrawString(ValueText, DescFont, DescBrush, ValuePosition);
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			MouseEventArgs ea = (MouseEventArgs)e;

			Rectangle r = new Rectangle(ea.X, ea.Y, 1, 1);

			if ( ValuePosition.IntersectsWith(r) )
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

			ValuePosition = new Rectangle(170, 1, Width - 170, 20);
		}
		public string Description { get => Get("Dummy"); set => Notify(value); }
		public double Value
		{
			get => Get(0);
			set
			{
				Notify(value);
				ValueText = Value.ToString();
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
