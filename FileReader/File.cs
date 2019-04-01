using System.Text.RegularExpressions;

namespace AGS.Theme
{
	public class File
	{
		public ThemeContainer Content { get; set; } = new ThemeContainer();
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

				//ControlString = Regex.Replace(ControlString, ".*//.*$", "$1/*$2*/", RegexOptions.Multiline);
				//ControlString = Regex.Replace(ControlString, @"^\s*//.*$", "", RegexOptions.Multiline);  // removes comments like this
				//ControlString = Regex.Replace(ControlString, @"^\s*/\*(\s|\S)*?\*/\s*$", "", RegexOptions.Multiline); /* comments like this */

				Content = Newtonsoft.Json.JsonConvert.DeserializeObject<ThemeContainer>(ControlString);

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

		public void CopyTo(ref ThemeContainer instance)
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
			else
			{
				System.Reflection.PropertyInfo prop = tobj.GetType().GetProperty(pi.Name);

				if ( prop != null )
				{
					var val = pi.GetValue(tobj, null);
					prop.SetValue(instance, val, null);
				}
			}
		}
	}
}
