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
		[Newtonsoft.Json.JsonProperty(PropertyName = "document-gradient")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public DocumentGradientClass DocumentGradient { get; set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "tool-window")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ToolWindowClass ToolWindow { get; set; }
	}



	public class MainMenuClass
	{
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
			[DisplayName("Gradient")]
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public BeginEndGradientClass Gradient { get; set; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PressedClass Pressed { get; set; }

		public class PressedClass
		{
			[DisplayName("Gradient")]
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public BeginMiddleEndGradientClass Gradient { get; set; }
		}

	}

	public class FileContent
	{
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
