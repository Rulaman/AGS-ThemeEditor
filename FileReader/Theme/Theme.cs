namespace Theme
{
	using System;
	using System.ComponentModel;
	using System.Drawing.Design;

	using Theme.Converter;
	using Theme.Editor;
	using Notify;

	public static class DEFINE { public static ColorClass COLORNORMAL = new ColorClass(-16777216); }

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("Color")]
	[TypeConverter(typeof(MyColorConverter))]
	[Newtonsoft.Json.JsonConverter(typeof(ColorClassJsonConverter<ColorClass>))]
	public class ColorClass : NotifyBase
	{
		[Newtonsoft.Json.JsonIgnore, Browsable(false), System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		internal string DebuggerDisplay { get => Value.ToString(); }

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "r")]
		public byte R { get => Get<byte>(0); set { Notify(value); Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "g")]
		public byte G { get => Get<byte>(0); set { Notify(value); Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "b")]
		public byte B { get => Get<byte>(0); set { Notify(value); Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[Browsable(false)]
		[Newtonsoft.Json.JsonProperty(PropertyName = "a")]
		public byte A { get => Get<byte>(0); set { Notify(value); Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		public System.Drawing.Color Value { get => Get(System.Drawing.Color.FromArgb(255, 0, 0, 0)); set { Notify(value); } }

		public override string ToString()
		{
			System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(Value);
			string colorAsString = converter.ConvertToString(Value);

			return colorAsString;
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

		public System.Drawing.Color GetColor()
		{
			return System.Drawing.Color.FromArgb(A, R, G, B);
		}

		public ColorClass(int argb)
		{
			byte[] bytes = BitConverter.GetBytes(argb);
			A = bytes[3];
			R = bytes[2];
			G = bytes[1];
			B = bytes[0];
		}

		public ColorClass(byte a, byte r, byte g, byte b)
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}

		public ColorClass(System.Drawing.Color color)
		{
			A = color.A;
			R = color.R;
			G = color.G;
			B = color.B;
		}

		public ColorClass() { }

		public byte[] GetRGB()
		{
			return new byte[] { R, G, B };
		}

		public int GetARGB()
		{
			byte[] temp = new byte[] { B, G, R, A };

			return BitConverter.ToInt32(temp, 0);
		}
	}

	[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
	public class MainContainerClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-background"), DisplayName("Dock Background")]
		public ColorClass DockBackground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "background"), DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground"), DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "skin"), DisplayName("Skin")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SkinClass Skin { get => Get(new SkinClass()); set { Notify(value); } }
	}

	public class SkinClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "auto-hide"), DisplayName("")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public AutoHideClass AutoHide { get => Get(new AutoHideClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-pane"), DisplayName("")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DockPaneClass DockPane { get => Get(new DockPaneClass()); set { Notify(value); } }
	}

	public class AutoHideClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass TabGradient { get => Get(new TabGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-strip-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public StartEndGradientClass DocStripGradient { get => Get(new StartEndGradientClass()); set { Notify(value); } }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class TabGradientClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonIgnore, Browsable(false), System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		internal string DebuggerDisplay { get => $"Start={Start.DebuggerDisplay}, End={End.DebuggerDisplay}, Text={Text.DebuggerDisplay}"; }

		[TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "start")]
		public ColorClass Start { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		public ColorClass End { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
		[Newtonsoft.Json.JsonProperty(PropertyName = "text")]
		public ColorClass Text { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class StartEndGradientClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		internal string DebuggerDisplay
		{
			get { return $"Start={Start.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "start")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Start")]
		public ColorClass Start { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class BeginEndGradientClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		internal string DebuggerDisplay
		{
			get { return $"Begin={Begin.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Newtonsoft.Json.JsonProperty(PropertyName = "begin")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Begin")]
		public ColorClass Begin { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class BeginMiddleEndGradientClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Newtonsoft.Json.JsonIgnore, Browsable(false)]
		internal string DebuggerDisplay { get { return $"Begin={Begin.DebuggerDisplay}, Middle={Middle.DebuggerDisplay}, End={End.DebuggerDisplay}"; } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "begin")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Begin")]
		public ColorClass Begin { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "middle")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Middle")]
		public ColorClass Middle { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "end")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class DocumentGradientClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-strip-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public StartEndGradientClass DocStripGradient { get => Get(new StartEndGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "active-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass ActiveTabGradient { get => Get(new TabGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inactive-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass InactiveTabGradient { get => Get(new TabGradientClass()); set { Notify(value); } }
	}

	public class ToolWindowClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "active-caption-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass ActiveCaptionGradient { get => Get(new TabGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inactive-caption-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass InactiveCaptionGradient { get => Get(new TabGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "active-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass ActiveTabGradient { get => Get(new TabGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inactive-tab-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public TabGradientClass InactiveTabGradient { get => Get(new TabGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-strip-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public StartEndGradientClass DocStripGradient { get => Get(new StartEndGradientClass()); set { Notify(value); } }
	}

	public class DockPaneClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "document-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DocumentGradientClass DocumentGradient { get => Get(new DocumentGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tool-window")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ToolWindowClass ToolWindow { get => Get(new ToolWindowClass()); set { Notify(value); } }
	}

	public class SelectedClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "gradient")]
		[DisplayName("Gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginEndGradientClass Gradient { get => Get(new BeginEndGradientClass()); set { Notify(value); } }
	}

	public class SingleGradientClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "gradient")]
		[DisplayName("Gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginMiddleEndGradientClass Gradient { get => Get(new BeginMiddleEndGradientClass()); set { Notify(value); } }
	}

	public class ItemClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Selected")]
		public ColorClass Selected { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class CheckClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Selected")]
		public ColorClass Selected { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "pressed")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Pressed")]
		public ColorClass Pressed { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class MainMenuClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[Newtonsoft.Json.JsonProperty(PropertyName = "background-dropdown")]
		[DisplayName("Background-Dropdown")]
		public ColorClass BackgroundDropdown { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "separator")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Separator")]
		public ColorClass Separator { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SelectedClass Selected { get => Get(new SelectedClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "pressed")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SingleGradientClass Pressed { get => Get(new SingleGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "item")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ItemClass Item { get => Get(new ItemClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "check")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public CheckClass Check { get => Get(new CheckClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "margin")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SingleGradientClass Margin { get => Get(new SingleGradientClass()); set { Notify(value); } }
	}

	public class GripClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "light")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Light { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dark")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Dark { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class ToolbarClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Border { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "separator")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Separator { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginEndGradientClass SelectedGradient { get => Get(new BeginEndGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "overflow-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginMiddleEndGradientClass OverflowGradient { get => Get(new BeginMiddleEndGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BeginMiddleEndGradientClass Gradient { get => Get(new BeginMiddleEndGradientClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grip")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GripClass Grip { get => Get(new GripClass()); set { Notify(value); } }
	}

	public class BackgroundClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class WelcomeClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "panel1")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Panel1 { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "panel2")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Panel2 { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "pnlTipOfTheDay")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass PnlTipOfTheDay { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "pnlRight")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass PnlRight { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class BackgroundForegroundClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }
	}

	public class BackgroundLineClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class BackgroundForegroundLineClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class BackgroundForegroundBorderClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class BackgroundForegroundFlatClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "flat")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FlatClass Flat { get => Get(new FlatClass()); set { Notify(value); } }
	}

	public class BackgroundForegroundBoxClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class ProjectPanelClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "project-tree")]
		[DisplayName("ProjectTree")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundLineClass ProjectTree { get => Get(new BackgroundForegroundLineClass()); set { Notify(value); } }
	}

	public class ComboboxClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "drop-down")]
		[DisplayName("DropDown")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass DropDown { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "drop-down-closed")]
		[DisplayName("DropDown Closed")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass DropDownClosed { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "item-selected")]
		[DisplayName("Item selected")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ItemSelected { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "item-not-selected")]
		[DisplayName("Item not selected")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ItemNotSelected { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[DisplayName("Border")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundClass Border { get => Get(new BackgroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "button-dropped-down")]
		[DisplayName("Button dropped down")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ButtonDroppedDown { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "button-not-dropped-down")]
		[DisplayName("Button not dropped down")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass ButtonNotDroppedDown { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class PropertiesPanelClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combobox")]
		[DisplayName("ComboBox")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass Combobox { get => Get(new ComboboxClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grid")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GridClass Grid { get => Get(new GridClass()); set { Notify(value); } }
	}

	public class GridClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "category")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Category")]
		public ColorClass Category { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass View { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "help")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Help { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class OutputPanelClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "owner-draw")]
		public bool OwnerDraw { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grid-lines")]
		public bool GridLines { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "last-column-width")]
		[DisplayName("Last column width")]
		public int LastColumnWidth { get => Get(1); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-item")]
		[DisplayName("Draw item")]
		public bool DrawItem { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-sub-item")]
		[DisplayName("Draw sub item")]
		public bool DrawSubItem { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "column-header")]
		[DisplayName("Column header")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundBorderClass ColumnHeader { get => Get(new BackgroundForegroundBorderClass()); set { Notify(value); } }
	}

	public class PropertyGridClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "category-fore")]
		[DisplayName("Category fore")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass CategoryFore { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view")]
		[DisplayName("View")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public BackgroundForegroundClass View { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "help")]
		[DisplayName("Help")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public BackgroundForegroundClass Help { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class GeneralSettingsClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "property-grid")]
		[DisplayName("Property grid")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PropertyGridClass PropertyGrid { get => Get(new PropertyGridClass()); set { Notify(value); } }
	}

	public class ColorNumberBoxClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border-style")]
		[DisplayName("Border style")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int BorderStyle { get => Get(1); set { Notify(value); } }
	}

	public class BorderClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "size")]
		[DisplayName("Size")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int Size { get => Get(1); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "color")]
		[DisplayName("Color")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass Color { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class FlatClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "style")]
		[DisplayName("Style")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int Style { get => Get(1); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border")]
		[DisplayName("Border")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BorderClass Border { get => Get(new BorderClass()); set { Notify(value); } }
	}

	public class BtnColorDialogClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "flat")]
		[DisplayName("Flat")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FlatClass Flat { get => Get(new FlatClass()); set { Notify(value); } }
	}

	public class ColorFinderClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "group-box")]
		[DisplayName("Group box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GroupBox { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "color-number-box")]
		[DisplayName("Color number box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorNumberBoxClass ColorNumberBox { get => Get(new ColorNumberBoxClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-color-dialog")]
		[DisplayName("Button color Dialog")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BtnColorDialogClass BtnColorDialog { get => Get(new BtnColorDialogClass()); set { Notify(value); } }
	}

	public class PalettePageClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "group-box")]
		[DisplayName("Group box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GroupBox { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class SelectedColorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[DisplayName("Selected")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Selected { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "not-selected")]
		[DisplayName("Not selected")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass NotSelected { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class DrawItemClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[DisplayName("Background")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SelectedColorClass Background { get => Get(new SelectedColorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[DisplayName("Foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class PaletteClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "color-finder")]
		[DisplayName("Color finder")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorFinderClass ColorFinder { get => Get(new ColorFinderClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "palette-page")]
		[DisplayName("Palette")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PalettePageClass PalettePage { get => Get(new PalettePageClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-mode")]
		[DisplayName("Draw mode")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public int DrawMode { get => Get(1); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-item")]
		[DisplayName("Draw item")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DrawItemClass DrawItem { get => Get(new DrawItemClass()); set { Notify(value); } }
	}

	public class SpriteSelectorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "list")]
		[DisplayName("List")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass List { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tree")]
		[DisplayName("Tree")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundLineClass Tree { get => Get(new BackgroundForegroundLineClass()); set { Notify(value); } }
	}

	public class TextParserEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[DisplayName("Box")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "list-view")]
		[DisplayName("ListView")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ListViewClass ListView { get => Get(new ListViewClass()); set { Notify(value); } }
	}

	public class ListViewClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "owner-draw")]
		[DisplayName("Owner Draw")]
		public bool OwnerDraw { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "grid-lines")]
		[DisplayName("Grid lines")]
		public bool GridLines { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "last-column-width")]
		[DisplayName("Last column width")]
		public int LastColumnWidth { get => Get(1); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-item")]
		[DisplayName("Draw item")]
		public bool DrawItem { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "draw-sub-item")]
		[DisplayName("Draw sub item")]
		public bool DrawSubItem { get => Get(false); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "column-header")]
		[DisplayName("Column Header")]
		public BackgroundForegroundBorderClass ColumnHeader { get => Get(new BackgroundForegroundBorderClass()); set { Notify(value); } }
	}

	public class TextBoxClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "border-style")]
		[DisplayName("Border style")]
		public int BorderStyle { get => Get(1); set { Notify(value); } }
	}

	public class LipSyncEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "text-boxes")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextBoxClass TextBox { get => Get(new TextBoxClass()); set { Notify(value); } }
	}

	public class InventoryEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "current-item-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CurrentItemBox { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "left-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass LeftBox { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "right-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass RightBox { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class DialogEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-delete-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonDeleteOption { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-new-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonNewOption { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }
	}

	public class ViewEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-delete-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonDeleteOption { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-new-option")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonNewOption { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-new-frame")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonNewFrame { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }
	}

	public class CharacterEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-make")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonMake { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }
	}

	public class ViewPreviewClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "numeric-loop")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass NumericLoop { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "numeric-frame")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass NumericFrame { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "numeric-delay")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass NumericDelay { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }
	}

	public class FontEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-import")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonImport { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }
	}

	public class AudioEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "audio-type")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass AudioType { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "audio-clip-box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass AudioClipBox { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-play")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonPlay { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-pause")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonPause { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-stop")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonStop { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }
	}

	public class GlobalVariablesEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "list")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ListViewClass List { get => Get(new ListViewClass()); set { Notify(value); } }
	}

	public class RoomEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "box")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Box { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "buffered-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass BufferedPanel { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-change-image")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonChangeImage { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-delete")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonDelete { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "btn-export")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundFlatClass ButtonExport { get => Get(new BackgroundForegroundFlatClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combo-view-type")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass ComboViewType { get => Get(new ComboboxClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combo-backgrounds")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass ComboBackgrounds { get => Get(new ComboboxClass()); set { Notify(value); } }
	}

	public class TextEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "global-default")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GlobalDefault { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "default")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Default { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "word-1")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Word1 { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "word-2")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Word2 { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "identifier")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Identifier { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Comment { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-line")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentLine { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-doc")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentDoc { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-line-doc")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentLineDoc { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-doc-keyword")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentDocKeyword { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "comment-doc-keyword-error")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass CommentDocKeywordError { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "number")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Number { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "regex")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Regex { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "string")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass String { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "string-eol")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass StringEOL { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "operator")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Operator { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "preprocessor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Preprocessor { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "line-number")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass LineNumber { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "indent-guide")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass IndentGuide { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "fold-margin")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass FoldMargin { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "fold-margin-hi")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass FoldMarginHi { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolder { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-end")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolderEnd { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-open")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolderOpen { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-open-mid")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass MarknumFolderOpenMid { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-mid-tail")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass MarknumFolderMidTail { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-sub")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass MarknumFolderSub { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "marknum-folder-tail")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass MarknumFolderTail { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "selected")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass Selected { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "caret")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorClass Caret { get => Get(DEFINE.COLORNORMAL); set { Notify(value); } }
	}

	public class ScriptEditorClass : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "foreground")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combo-functions")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass ComboFunctions { get => Get(new ComboboxClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "text-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextEditorClass TextEditor { get => Get(new TextEditorClass()); set { Notify(value); } }
	}

	public class Content : NotifyBase
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "name")]
		public string Name { get => Get(""); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "version")]
		public string Version { get => Get(""); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get => Get(DEFINE.COLORNORMAL); set => Notify(value); }

		[Newtonsoft.Json.JsonProperty(PropertyName = "main-container")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public MainContainerClass MainContainer { get => Get(new MainContainerClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "main-menu")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public MainMenuClass MainMenu { get => Get(new MainMenuClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tool-bar")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ToolbarClass Toolbar { get => Get(new ToolbarClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "status-strip")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundClass StatusStrip { get => Get(new BackgroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "welcome")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public WelcomeClass Welcome { get => Get(new WelcomeClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "project-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ProjectPanelClass ProjectPanel { get => Get(new ProjectPanelClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "properties-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PropertiesPanelClass PropertiesPanel { get => Get(new PropertiesPanelClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "output-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public OutputPanelClass OutputPanel { get => Get(new OutputPanelClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "find-results-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public OutputPanelClass FindResultsPanel { get => Get(new OutputPanelClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "call-stack-panel")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public OutputPanelClass CallStackPanel { get => Get(new OutputPanelClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "general-settings")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GeneralSettingsClass GeneralSettings { get => Get(new GeneralSettingsClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "palette")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PaletteClass Palette { get => Get(new PaletteClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "sprite-selector")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SpriteSelectorClass SpriteSelector { get => Get(new SpriteSelectorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "text-parser-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public TextParserEditorClass TextParserEditor { get => Get(new TextParserEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "lip-sync-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public LipSyncEditorClass LipSyncEditor { get => Get(new LipSyncEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "gui-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass GuiEditor { get => Get(new BackgroundForegroundClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "inventory-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public InventoryEditorClass InventoryEditor { get => Get(new InventoryEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dialog-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DialogEditorClass DialogEditor { get => Get(new DialogEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ViewEditorClass ViewEditor { get => Get(new ViewEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "character-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public CharacterEditorClass CharacterEditor { get => Get(new CharacterEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "view-preview")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ViewPreviewClass ViewPreview { get => Get(new ViewPreviewClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "cursor-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundBoxClass CursorEditor { get => Get(new BackgroundForegroundBoxClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "font-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FontEditorClass FontEditor { get => Get(new FontEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "audio-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public AudioEditorClass AudioEditor { get => Get(new AudioEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "global-variables-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GlobalVariablesEditorClass GlobalVariablesEditor { get => Get(new GlobalVariablesEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "room-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public RoomEditorClass RoomEditor { get => Get(new RoomEditorClass()); set { Notify(value); } }

		[Newtonsoft.Json.JsonProperty(PropertyName = "script-editor")]
		[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ScriptEditorClass ScriptEditor { get => Get(new ScriptEditorClass()); set { Notify(value); } }
	}
}
