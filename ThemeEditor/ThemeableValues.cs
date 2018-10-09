using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemeEditor
{
	class ThemeableValues
	{
		dynamic account;

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
					byte[] dataarray = null; ;
					stream.Read(dataarray, 0, (int)stream.Length);

					datastring = System.Text.Encoding.UTF8.GetString(dataarray);
				}
				
				account = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(datastring);

				returnValue = true;
			}

			return returnValue;

		}
	}
}
