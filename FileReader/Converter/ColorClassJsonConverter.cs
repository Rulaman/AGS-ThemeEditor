namespace AGS.Theme.Converter
{
	using Newtonsoft.Json.Serialization;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Globalization;

	public class ColorClassJsonConverter<T> : Newtonsoft.Json.JsonConverter
	{
		private static readonly IContractResolver resolver = new NoTypeConverterContractResolver();

		private class NoTypeConverterContractResolver : DefaultContractResolver
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

		public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			return Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = resolver }).Deserialize(reader, objectType);
		}

		public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = resolver }).Serialize(writer, value);
		}
	}

	public class MyColorConverter : TypeConverter
	{
		// This is used, for example, by DefaultValueAttribute to convert from string to MyColor.
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if ( value.GetType() == typeof(string) )
			{
				return new ColorClass((string)value);
			}
			else if ( value.GetType() == typeof(Color) )
			{
				return new ColorClass((Color)value);
			}


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
			else if ( (destType == typeof(Color)) && (value is ColorClass) )
			{
				return ((ColorClass)value).GetColor();
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
}
