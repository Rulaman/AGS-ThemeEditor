using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

using Theme.Converter;
using Theme.Editor;

namespace FileReader
{
	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("Color")]
	[TypeConverter(typeof(MyColorConverter))]
	[Newtonsoft.Json.JsonConverter(typeof(NoTypeConverterJsonConverter<ColorClass>))]
	public class ColorClass
	{
		#region DebuggerDisplay

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public string DebuggerDisplay
		{
			get
			{
				if ( _Value.IsKnownColor )
				{
					return $"RGBA={R},{G},{B},{A}  {_Value.ToKnownColor()}";
				}
				else
				{
					return $"RGBA={R},{G},{B},{A}";
				}
			}
		}

		#endregion DebuggerDisplay

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _R;

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "r")]
		public byte R { get { return _R; } set { _R = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _G;

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "g")]
		public byte G { get { return _G; } set { _G = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _B;

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "b")]
		public byte B { get { return _B; } set { _B = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _A;

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "a")]
		public byte A { get { return _A; } set { _A = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private System.Drawing.Color _Value;

		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public System.Drawing.Color Value { get { return _Value; } set { _Value = value; R = _Value.R; G = _Value.G; B = _Value.B; A = _Value.A; } }

		public override string ToString()
		{
			System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(Value);
			string colorAsString = converter.ConvertToString(Value);

			//return $"Color [A={_A}, R={_R}, G={_G}, B={_B}]";

			return colorAsString;
		}

		public ColorClass(Color color)
		{
		}

		public ColorClass()
		{
		}

		public ColorClass(string rgb)
		{
			string[] parts = null;

			if ( rgb.Contains(";") )
			{
				parts = rgb.Split(';');
			}
			else
			{
				parts = rgb.Split(' ');
			}

			if ( parts.Length == 3 )
			{
				//throw new Exception("Array must have a length of 3.");
				A = 255;
				R = Convert.ToByte(parts[0]);
				G = Convert.ToByte(parts[1]);
				B = Convert.ToByte(parts[2]);
			}
			else if ( parts.Length == 4 )
			{
				A = Convert.ToByte(parts[0]);
				R = Convert.ToByte(parts[1]);
				G = Convert.ToByte(parts[2]);
				B = Convert.ToByte(parts[3]);
			}
			else
			{
				throw new Exception("Array must have a length of 3 or 4.");
			}
		}

		public ColorClass(int argb)
		{
			byte[] bytes = BitConverter.GetBytes(argb);
			A = bytes[3];
			R = bytes[2];
			G = bytes[1];
			B = bytes[0];
		}

		public byte[] GetRGB()
		{
			return new byte[] { _R, _G, _B };
		}

		public int GetARGB()
		{
			byte[] temp = new byte[] { _B, _G, _R, _A };

			return BitConverter.ToInt32(temp, 0);
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
				return value == null ? defaultvalue : (T)value;
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

	[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
	public class MainContainerClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("DockBackground")]
		public ColorClass DockBackground { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "skin")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public SkinClass Skin { get; set; }
	}

	public class SkinClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "auto-hide")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public AutoHideClass AutoHide { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-pane")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public DockPaneClass DockPane { get; set; }
	}

	public class AutoHideClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass TabGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-strip-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public StartEndGradientClass DocStripGradient { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class TabGradientClass
	{
		public override string ToString()
		{
			return "";
		}

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Start={Start.DebuggerDisplay}, End={End.DebuggerDisplay}, Text={Text.DebuggerDisplay}"; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "start")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Start { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass End { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "text")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Text { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class StartEndGradientClass
	{
		public override string ToString()
		{
			return "";
		}

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Start={Start.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "start")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Start")]
		public ColorClass Start { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class BeginEndGradientClass
	{
		public override string ToString()
		{
			return "";
		}

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Begin={Begin.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "begin")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Begin")]
		public ColorClass Begin { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class BeginMiddleEndGradientClass
	{
		public override string ToString()
		{
			return "";
		}

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Begin={Begin.DebuggerDisplay}, Middle={Middle.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "begin")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Begin")]
		public ColorClass Begin { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "middle")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Middle")]
		public ColorClass Middle { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get; set; }
	}

	[DisplayName("")]
	public class DocumentGradientClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-strip-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public StartEndGradientClass DocStripGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "active-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass ActiveTabGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inactive-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass InactiveTabGradient { get; set; }
	}

	[DisplayName("")]
	public class ToolWindowClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "active-caption-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass ActiveCaptionGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inactive-caption-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass InactiveCaptionGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "active-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass ActiveTabGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inactive-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass InactiveTabGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-strip-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public StartEndGradientClass DocStripGradient { get; set; }
	}

	[DisplayName("")]
	public class DockPaneClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "document-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DocumentGradientClass DocumentGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tool-window")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ToolWindowClass ToolWindow { get; set; }
	}

	public class MainMenuClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[Newtonsoft.Json.JsonProperty(PropertyName = "background-dropdown")]
		[DisplayName("Background-Dropdown")]
		public ColorClass BackgroundDropdown { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "separator")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Separator")]
		public ColorClass Separator { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SelectedClass Selected { get; set; }

		public class SelectedClass
		{
			public override string ToString()
			{
				return "";
			}

			[Newtonsoft.Json.JsonProperty(PropertyName = "gradient")]
			[DisplayName("Gradient")]
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public BeginEndGradientClass Gradient { get; set; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "pressed")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SingleGradientClass Pressed { get; set; }

		public class SingleGradientClass
		{
			public override string ToString()
			{
				return "";
			}

			[Newtonsoft.Json.JsonProperty(PropertyName = "gradient")]
			[DisplayName("Gradient")]
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public BeginMiddleEndGradientClass Gradient { get; set; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "item")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ItemClass Item { get; set; }

		public class ItemClass
		{
			public override string ToString()
			{
				return "";
			}

			[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Border")]
			public ColorClass Border { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Selected")]
			public ColorClass Selected { get; set; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "check")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public CheckClass Check { get; set; }

		public class CheckClass
		{
			public override string ToString()
			{
				return "";
			}

			[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Background")]
			public ColorClass Background { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Foreground")]
			public ColorClass Foreground { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Border")]
			public ColorClass Border { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Selected")]
			public ColorClass Selected { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "pressed")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Pressed")]
			public ColorClass Pressed { get; set; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "margin")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SingleGradientClass Margin { get; set; }
	}

	public class ToolbarClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Border { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "separator")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Separator { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginEndGradientClass SelectedGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "overflow-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginMiddleEndGradientClass OverflowGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginMiddleEndGradientClass Gradient { get; set; }

		public class GripClass
		{
			public override string ToString()
			{
				return "";
			}

			[Newtonsoft.Json.JsonProperty(PropertyName = "light")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			public ColorClass Light { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "dark")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			public ColorClass Dark { get; set; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "grip")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GripClass Grip { get; set; }
	}

	public class BackgroundClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }
	}

	public class WelcomeClass : BackgroundForegroundClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "panel1")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Panel1 { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "panel2")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Panel2 { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "pnlTipOfTheDay")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass PnlTipOfTheDay { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "pnlRight")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass PnlRight { get; set; }
	}

	public class BackgroundForegroundClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }
	}

	public class BackgroundLineClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get; set; }
	}

	public class BackgroundForegroundLineClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get; set; }
	}

	public class BackgroundForegroundBorderClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get; set; }
	}

	public class BackgroundForegroundFlatClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "flat")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FlatClass Flat { get; set; }
	}

	public class BackgroundForegroundBoxClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get; set; }
	}

	public class ProjectPanelClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "project-tree")]
		[DisplayName("ProjectTree")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundLineClass ProjectTree { get; set; }
	}

	public class ComboboxClass : BackgroundForegroundClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "drop-down")]
		[DisplayName("DropDown")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass DropDown { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "drop-down-closed")]
		[DisplayName("DropDown Closed")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass DropDownClosed { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "item-selected")]
		[DisplayName("Item selected")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ItemSelected { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "item-not-selected")]
		[DisplayName("Item not selected")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ItemNotSelected { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[DisplayName("Border")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundClass Border { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "button-dropped-down")]
		[DisplayName("Button dropped down")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ButtonDroppedDown { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "button-not-dropped-down")]
		[DisplayName("Button not dropped down")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ButtonNotDroppedDown { get; set; }
	}

	public class PropertiesPanelClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combobox")]
		[DisplayName("ComboBox")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass Combobox { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grid")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GridClass Grid { get; set; }
	}

	public class GridClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "category")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Category")]
		public ColorClass Category { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass View { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "help")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Help { get; set; }
	}

	public class OutputPanelClass : BackgroundForegroundClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "owner-draw")]
		public bool OwnerDraw { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grid-lines")]
		public bool GridLines { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "last-column-width")]
		[DisplayName("Last column width")]
		public int LastColumnWidth { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-item")]
		[DisplayName("Draw item")]
		public bool DrawItem { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-sub-item")]
		[DisplayName("Draw sub item")]
		public bool DrawSubItem { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "column-header")]
		[DisplayName("Column header")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundBorderClass ColumnHeader { get; set; }
	}

	public class PropertyGridClass : BackgroundLineClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "category-fore")]
		[DisplayName("Category fore")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass CategoryFore { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view")]
		[DisplayName("View")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public BackgroundForegroundClass View { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "help")]
		[DisplayName("Help")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public BackgroundForegroundClass Help { get; set; }
	}

	public class GeneralSettingsClass : BackgroundForegroundClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "property-grid")]
		[DisplayName("Property grid")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PropertyGridClass PropertyGrid { get; set; }
	}

	public class ColorNumberBoxClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "border-style")]
		[DisplayName("Border style")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int BorderStyle { get; set; }
	}

	public class BorderClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "size")]
		[DisplayName("Size")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int Size { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "color")]
		[DisplayName("Color")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass Color { get; set; }
	}

	public class FlatClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "style")]
		[DisplayName("Style")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int Style { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[DisplayName("Border")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BorderClass Border { get; set; }
	}

	public class BtnColorDialogClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "flat")]
		[DisplayName("Flat")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FlatClass Flat { get; set; }
	}

	public class ColorFinderClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "group-box")]
		[DisplayName("Group box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GroupBox { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "color-number-box")]
		[DisplayName("Color number box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorNumberBoxClass ColorNumberBox { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-color-dialog")]
		[DisplayName("Button color Dialog")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BtnColorDialogClass BtnColorDialog { get; set; }
	}

	public class PalettePageClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "group-box")]
		[DisplayName("Group box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GroupBox { get; set; }
	}

	public class DrawItemClass
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[DisplayName("Background")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SelectedColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[DisplayName("Foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Foreground { get; set; }

		public class SelectedColorClass
		{
			public override string ToString()
			{
				return "";
			}

			[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
			[DisplayName("Selected")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			public ColorClass Selected { get; set; }

			[Newtonsoft.Json.JsonProperty(PropertyName = "not-selected")]
			[DisplayName("Not selected")]
			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			public ColorClass NotSelected { get; set; }
		}
	}

	public class PaletteClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "color-finder")]
		[DisplayName("Color finder")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorFinderClass ColorFinder { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "palette-page")]
		[DisplayName("Palette")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PalettePageClass PalettePage { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-mode")]
		[DisplayName("Draw mode")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int DrawMode { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-item")]
		[DisplayName("Draw item")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DrawItemClass DrawItem { get; set; }
	}

	public class SpriteSelectorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "list")]
		[DisplayName("List")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass List { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tree")]
		[DisplayName("Tree")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundLineClass Tree { get; set; }
	}

	public class TextParserEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[DisplayName("Box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "list-view")]
		[DisplayName("ListView")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ListViewClass ListView { get; set; }
	}

	public class ListViewClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "owner-draw")]
		[DisplayName("Owner Draw")]
		public bool OwnerDraw { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grid-lines")]
		[DisplayName("Grid lines")]
		public bool GridLines { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "last-column-width")]
		[DisplayName("Last column width")]
		public int LastColumnWidth { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-item")]
		[DisplayName("Draw item")]
		public bool DrawItem { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-sub-item")]
		[DisplayName("Draw sub item")]
		public bool DrawSubItem { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "column-header")]
		[DisplayName("Column Header")]
		public BackgroundForegroundBorderClass ColumnHeader { get; set; }
	}

	public class TextBoxClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "border-style")]
		[DisplayName("Border style")]
		public int BorderStyle { get; set; }
	}

	public class LipSyncEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "text-boxes")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextBoxClass TextBox { get; set; }
	}

	public class InventoryEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "current-item-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CurrentItemBox { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "left-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass LeftBox { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "right-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass RightBox { get; set; }
	}

	public class DialogEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-delete-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonDeleteOption { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-new-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonNewOption { get; set; }
	}

	public class ViewEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-delete-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonDeleteOption { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-new-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonNewOption { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-new-frame")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonNewFrame { get; set; }
	}

	public class CharacterEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-make")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonMake { get; set; }
	}

	public class ViewPreviewClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "numeric-loop")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass NumericLoop { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "numeric-frame")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass NumericFrame { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "numeric-delay")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass NumericDelay { get; set; }
	}

	public class FontEditorClass : BackgroundForegroundBoxClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-import")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonImport { get; set; }
	}

	public class AudioEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "audio-type")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass AudioType { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "audio-clip-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass AudioClipBox { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-play")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonPlay { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-pause")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonPause { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-stop")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonStop { get; set; }
	}

	public class GlobalVariablesEditorClass : BackgroundForegroundBoxClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "list")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ListViewClass List { get; set; }
	}

	public class RoomEditorClass : BackgroundForegroundBoxClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "buffered-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass BufferedPanel { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-change-image")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonChangeImage { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-delete")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonDelete { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-export")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonExport { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combo-view-type")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass ComboViewType { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combo-backgrounds")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass ComboBackgrounds { get; set; }
	}

	public class TextEditorClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "global-default")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GlobalDefault { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "default")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Default { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "word-1")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Word1 { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "word-2")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Word2 { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "identifier")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Identifier { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Comment { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-line")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentLine { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-doc")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentDoc { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-line-doc")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentLineDoc { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-doc-keyword")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentDocKeyword { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-doc-keyword-error")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentDocKeywordError { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "number")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Number { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "regex")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Regex { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "string")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass String { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "string-eol")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass StringEOL { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "operator")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Operator { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "preprocessor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Preprocessor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line-number")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass LineNumber { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "indent-guide")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass IndentGuide { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "fold-margin")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass FoldMargin { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "fold-margin-hi")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass FoldMarginHi { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolder { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-end")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolderEnd { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-open")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolderOpen { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-open-mid")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolderOpenMid { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-mid-tail")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass MarknumFolderMidTail { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-sub")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass MarknumFolderSub { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-tail")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass MarknumFolderTail { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass Selected { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "caret")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass Caret { get; set; }
	}

	public class ScriptEditorClass : BackgroundForegroundClass
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "combo-functions")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass ComboFunctions { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "text-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextEditorClass TextEditor { get; set; }
	}

	public class FileContent
	{
		public override string ToString()
		{
			return "";
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "main-container")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public MainContainerClass MainContainer { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "main-menu")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public MainMenuClass MainMenu { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tool-bar")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ToolbarClass Toolbar { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "status-strip")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundClass StatusStrip { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "welcome")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public WelcomeClass Welcome { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "project-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ProjectPanelClass ProjectPanel { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "properties-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PropertiesPanelClass PropertiesPanel { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "output-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public OutputPanelClass OutputPanel { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "find-results-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public OutputPanelClass FindResultsPanel { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "call-stack-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public OutputPanelClass CallStackPanel { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "general-settings")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GeneralSettingsClass GeneralSettings { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "palette")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PaletteClass Palette { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "sprite-selector")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SpriteSelectorClass SpriteSelector { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "text-parser-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextParserEditorClass TextParserEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "lip-sync-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public LipSyncEditorClass LipSyncEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "gui-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GuiEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inventory-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public InventoryEditorClass InventoryEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dialog-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DialogEditorClass DialogEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ViewEditorClass ViewEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "character-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public CharacterEditorClass CharacterEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view-preview")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ViewPreviewClass ViewPreview { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "cursor-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundBoxClass CursorEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "font-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FontEditorClass FontEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "audio-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public AudioEditorClass AudioEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "global-variables-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GlobalVariablesEditorClass GlobalVariablesEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "room-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public RoomEditorClass RoomEditor { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "script-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ScriptEditorClass ScriptEditor { get; set; }
	}

	public class File
	{
		public static string tempcontent = string.Empty;

		public FileContent Content { get; internal set; }

		public bool Load(string fileName)
		{
			bool returnValue = false;

			if ( !System.IO.File.Exists(fileName) )
			{
			}
			else
			{
				string datastring;

				using ( System.IO.FileStream stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite) )
				{
					byte[] dataarray = new byte[stream.Length];
					stream.Read(dataarray, 0, (int)stream.Length);

					datastring = System.Text.Encoding.ASCII.GetString(dataarray);
				}

				Content = Newtonsoft.Json.JsonConvert.DeserializeObject<FileContent>(datastring);
				tempcontent = datastring;

				returnValue = true;
			}

			return returnValue;
		}

		public void Write(string filePath)
		{
			using ( System.IO.FileStream filestream = System.IO.File.Open(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read) )
			{
				//var settings = new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = BaseFirstContractResolver.Instance };
				//string json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented, settings);
				string json = Newtonsoft.Json.JsonConvert.SerializeObject(this.Content, Newtonsoft.Json.Formatting.Indented);

				using ( System.IO.StreamWriter sw = new System.IO.StreamWriter(filestream, System.Text.Encoding.ASCII) )
				{
					sw.Write(json);
				}
			}
		}
	}

	public class BaseFirstContractResolver : DefaultContractResolver
	{
		// As of 7.0.1, Json.NET suggests using a static instance for "stateless" contract resolvers, for performance reasons.
		// http://www.newtonsoft.com/json/help/html/ContractResolver.htm
		// http://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_Serialization_DefaultContractResolver__ctor_1.htm
		// "Use the parameterless constructor and cache instances of the contract resolver within your application for optimal performance."
		private static BaseFirstContractResolver instance;

		static BaseFirstContractResolver()
		{
			instance = new BaseFirstContractResolver();
		}

		public static BaseFirstContractResolver Instance { get { return instance; } }

		protected override IList<JsonProperty> CreateProperties(JsonObjectContract contract)
		{
			var properties = base.CreateProperties(contract);

			//if ( properties != null )
			//{
			//	return ((List<JsonProperty>)properties).Sort(delegate (Point p1, Point p2) { return p1.X.CompareTo(p2.X); });
			//}

			return properties;
		}

		//protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(Type type, Newtonsoft.Json.MemberSerialization memberSerialization)
		//{
		//	var properties = base.CreateProperties(type, memberSerialization);
		//	if ( properties != null )
		//		return properties.OrderBy(p => p.DeclaringType.BaseTypesAndSelf().Count()).ToList();
		//	return properties;
		//}
	}

	public static class TypeExtensions
	{
		public static System.Collections.Generic.IEnumerable<Type> BaseTypesAndSelf(this Type type)
		{
			while ( type != null )
			{
				yield return type;
				type = type.BaseType;
			}
		}
	}
}

namespace System
{
	public delegate void Action();
}

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class |
		AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute { }
}
