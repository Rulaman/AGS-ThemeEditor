using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Design;

namespace FileReader
{
	public class NoTypeConverterJsonConverter<T> : JsonConverter
	{
		static readonly IContractResolver resolver = new NoTypeConverterContractResolver();

		class NoTypeConverterContractResolver : DefaultContractResolver
		{
			protected override JsonContract CreateContract(Type objectType)
			{
				if ( typeof(T).IsAssignableFrom(objectType) )
				{
					var contract = this.CreateObjectContract(objectType);
					contract.Converter = null; // Also null out the converter to prevent infinite recursion.
					return contract;
				}
				return base.CreateContract(objectType);
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//return JsonSerializer.CreateDefault(new JsonSerializerSettings { ContractResolver = resolver }).Deserialize(reader, objectType);
			return JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = resolver }).Deserialize(reader, objectType);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			//JsonSerializer.CreateDefault(new JsonSerializerSettings { ContractResolver = resolver }).Serialize(writer, value);
			JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = resolver }).Serialize(writer, value);
		}
	}

	public class MyColorConverter : TypeConverter
	{
		// This is used, for example, by DefaultValueAttribute to convert from string to MyColor.
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if ( value.GetType() == typeof(string) )
				return new ColorClass((string)value);
			return base.ConvertFrom(context, culture, value);
		}
		// This is used, for example, by the PropertyGrid to convert MyColor to a string.
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
		{
			if ( (destType == typeof(string)) && (value is ColorClass) )
			{
				ColorClass color = (ColorClass)value;
				return color.ToString();
			}
			else if ( (destType == typeof(string)) && (value is Color) )
			{
			}
			return base.ConvertTo(context, culture, value, destType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return true;
			//return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
			//return base.CanConvertTo(context, destinationType);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return base.GetPropertiesSupported(context);
		}
	}

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

	public class EmptyConverter : TypeConverter
	{
	}

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

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("Color")]
	[TypeConverter(typeof(MyColorConverter))]
	[JsonConverter(typeof(NoTypeConverterJsonConverter<ColorClass>))]
	public class ColorClass
	{
		#region DebuggerDisplay
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Browsable(false)]
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
		public byte R { get { return _R; } set { _R = value;  _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _G;
		[Browsable(false)]
		public byte G { get { return _G; } set { _G = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _B;
		[Browsable(false)]
		public byte B { get { return _B; } set { _B = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private byte _A;
		[Browsable(false)]
		public byte A { get { return _A; } set { _A = value; _Value = System.Drawing.Color.FromArgb(A, R, G, B); } }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private System.Drawing.Color _Value;
		[Browsable(false)]
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
	}


	[Editor(typeof(EmptyEditor), typeof(UITypeEditor))] // specify editor for the property
	public class MainContainerClass
	{
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "dock-background")]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("DockBackground")]
		public ColorClass DockBackground { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DisplayName("")]
		public SkinClass Skin { get; set; }
	}

	public class SkinClass
	{
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Start={Start.DebuggerDisplay}, End={End.DebuggerDisplay}, Text={Text.DebuggerDisplay}"; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Start { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass End { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Text { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class StartEndGradientClass
	{
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Start={Start.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Start")]
		public ColorClass Start { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class BeginEndGradientClass
	{
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Begin={Begin.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Start")]
		public ColorClass Begin { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get; set; }
	}

	[System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
	[DisplayName("")]
	public class BeginMiddleEndGradientClass
	{
		public override string ToString() { return ""; }

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		[Browsable(false)]
		public string DebuggerDisplay
		{
			get { return $"Begin={Begin.DebuggerDisplay}, Middle={Middle.DebuggerDisplay}, End={End.DebuggerDisplay}"; }
		}

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Start")]
		public ColorClass Begin { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Middle")]
		public ColorClass Middle { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("End")]
		public ColorClass End { get; set; }
	}

	[DisplayName("")]
	public class DocumentGradientClass
	{
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "document-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DocumentGradientClass DocumentGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tool-window")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ToolWindowClass ToolWindow { get; set; }
	}

	public class MainMenuClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[Newtonsoft.Json.JsonProperty(PropertyName = "background-dropdown")]
		[DisplayName("Background-Dropdown")]
		public ColorClass BackgroundDropdown { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Separator")]
		public ColorClass Separator { get; set; }


		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SelectedClass Selected { get; set; }

		public class SelectedClass
		{
			public override string ToString() { return ""; }

			[DisplayName("Gradient")]
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public BeginEndGradientClass Gradient { get; set; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SingleGradientClass Pressed { get; set; }

		public class SingleGradientClass
		{
			public override string ToString() { return ""; }

			[DisplayName("Gradient")]
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public BeginMiddleEndGradientClass Gradient { get; set; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ItemClass Item { get; set; }

		public class ItemClass
		{
			public override string ToString() { return ""; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Border")]
			public ColorClass Border { get; set; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Selected")]
			public ColorClass Selected { get; set; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public CheckClass Check { get; set; }

		public class CheckClass
		{
			public override string ToString() { return ""; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Background")]
			public ColorClass Background { get; set; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Foreground")]
			public ColorClass Foreground { get; set; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Border")]
			public ColorClass Border { get; set; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Selected")]
			public ColorClass Selected { get; set; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			[DisplayName("Pressed")]
			public ColorClass Pressed { get; set; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public SingleGradientClass Margin { get; set; }
	}

	public class ToolbarClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		public ColorClass Border { get; set; }

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
			public override string ToString() { return ""; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			public ColorClass Light { get; set; }

			[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
			public ColorClass Dark { get; set; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public GripClass Grip { get; set; }

	}

	public class BackgroundClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }
	}

	public class WelcomeClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Panel1 { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass Panel2 { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass PnlTipOfTheDay { get; set; }

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundClass PnlRight { get; set; }
	}

	public class BackgroundForegroundClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }
	}

	public class BackgroundLineClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get; set; }
	}

	public class BackgroundForegroundLineClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get; set; }
	}

	public class BackgroundForegroundBorderClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Foreground")]
		public ColorClass Foreground { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Border")]
		public ColorClass Border { get; set; }
	}

	public class ProjectPanelClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "project-tree")]
		[DisplayName("ProjectTree")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BackgroundForegroundLineClass ProjectTree { get; set; }
	}

	public class ComboboxClass: BackgroundForegroundClass
	{
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "combobox")]
		[DisplayName("ComboBox")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ComboboxClass Combobox { get; set; }
	}

	public class GridClass
	{
		public override string ToString() { return ""; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Background")]
		public ColorClass Background { get; set; }

		[Editor(typeof(MyColorEditor), typeof(UITypeEditor))] // specify editor for the property
		[DisplayName("Line")]
		public ColorClass Line { get; set; }

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
		public override string ToString() { return ""; }

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

	public class GeneralSettingsClass: BackgroundForegroundClass
	{
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

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
		public override string ToString() { return ""; }

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
			public override string ToString() { return ""; }

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
		public int DrawMode{ get; set; }

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


	public class FileContent
	{
		public override string ToString() { return ""; }

		public string Name { get; set; }
		public string Version { get; set; }

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
		public ProjectPanelClass ProjectPanel {get;set;}

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

	}

	public class File
	{
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

					datastring = System.Text.Encoding.UTF8.GetString(dataarray);
				}

				Content = Newtonsoft.Json.JsonConvert.DeserializeObject<FileContent>(datastring);

				returnValue = true;
			}

			return returnValue;

		}
	}
}
