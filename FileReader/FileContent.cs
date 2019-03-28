using System;
using System.ComponentModel;
using System.Drawing.Design;

using Theme.Converter;
using Theme.Editor;
using Theme;

namespace FileReader
{
	public class File
	{
		public Content Content { get; internal set; } = new Content();
		public string ControlString { get; internal set; } = string.Empty;

		public bool Load(string fileName)
		{
			bool returnValue = false;

			if ( System.IO.File.Exists(fileName) )
			{
				using ( System.IO.FileStream stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite) )
				{
					byte[] dataarray = new byte[stream.Length];
					stream.Read(dataarray, 0, (int)stream.Length);

					ControlString = System.Text.Encoding.ASCII.GetString(dataarray);
				}

				Content = Newtonsoft.Json.JsonConvert.DeserializeObject<Content>(ControlString);

				returnValue = true;
			}

			return returnValue;
		}

		public void Write(string filePath)
		{
			using ( System.IO.FileStream filestream = System.IO.File.Open(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read) )
			{
				ControlString = Newtonsoft.Json.JsonConvert.SerializeObject(this.Content, Newtonsoft.Json.Formatting.Indented);

				using ( System.IO.StreamWriter sw = new System.IO.StreamWriter(filestream, System.Text.Encoding.ASCII) )
				{
					sw.Write(ControlString);
				}
			}
		}

		public void CopyTo(ref Content instance)
		{
			System.Reflection.PropertyInfo[] piList = instance.GetType().GetProperties();

			foreach ( var item in piList )
			{
				object o = (object)instance;
				HandleProperties(item, Content, ref o);
			}
		}

		private void HandleProperties(System.Reflection.PropertyInfo pi, object tobj, ref object instance)
		{
			if ( tobj == null )
			{
				return;
			}

			if (  pi.PropertyType == typeof(System.Windows.Forms.BindingContext)
				|| pi.PropertyType == typeof(System.Windows.Forms.ControlBindingsCollection)
				|| pi.PropertyType == typeof(System.ComponentModel.ISite)
				)
			{
				return;
			}

			if ( pi.PropertyType.Name == "ColorClass" )
			{
				System.Reflection.PropertyInfo prop = tobj.GetType().GetProperty(pi.Name);
				var val = pi.GetValue(tobj, null);
				prop.SetValue(instance, val, null);
			}
			else if ( !pi.PropertyType.IsValueType && pi.PropertyType.Name != "String" )   // class, struct,...
			{
				foreach ( var item in pi.PropertyType.GetProperties() )
				{
					object o = pi.GetValue(tobj, null);
					object i = pi.GetValue(instance, null);

					HandleProperties(item, o, ref i);
				}
			}
			else if ( pi.PropertyType.Name == "String" )
			{
				System.Reflection.PropertyInfo prop = tobj.GetType().GetProperty(pi.Name);
				var val = pi.GetValue(tobj, null);
				prop.SetValue(instance, val, null);
			}
			else
			{
				System.Reflection.PropertyInfo prop = tobj.GetType().GetProperty(pi.Name);

				if ( prop == null )
				{
				}
				else
				{
					var val = pi.GetValue(tobj, null);
					prop.SetValue(instance, val, null);
				}


				//var uidPi = pi.GetType().GetProperties();

				//foreach ( var up in uidPi )
				//{
				//	System.Reflection.PropertyInfo prop = instance.GetType().GetProperty(up.Name);
				//	var val = up.GetValue(tobj, null);

				//	switch ( up.Name )
				//	{
				//		case "ColorClass":
				//		case "ISite":
				//		case "DataBindings":
				//		case "BindingContext":
				//		case "ControlBindingsCollection":
				//		case "IBindableComponent":
				//			break;
				//		default:
				//			{
				//				prop.SetValue(instance, val, null);
				//			}
				//			break;
				//	}
				//}
			}
		}
	}
}
