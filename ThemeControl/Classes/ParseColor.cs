public static class ColorExtension
{
	public static bool Parse(this string value, out int number)
	{
		bool val = true;

		number = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb(); // schwarz

		try
		{
			if ( value.ToLower().StartsWith("0x") )
			{
				number = int.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
			}
			else if ( value.ToLower().StartsWith("#") )
			{
				number = int.Parse(value.Substring(1), System.Globalization.NumberStyles.HexNumber);
			}
			else if ( value.ToLower().EndsWith("h") )
			{
				number = int.Parse(value.TrimEnd('h'), System.Globalization.NumberStyles.HexNumber);
			}
			else if ( value.Contains(",") || value.Contains(";") )
			{
				string[] arr = value.Split(new[] { ',', ';' });
				int[] iarr = System.Array.ConvertAll(arr, int.Parse);

				switch ( arr.Length )
				{
					case 1: // known name
						number = System.Drawing.Color.FromName(value).ToArgb();
						break;
					case 3:
						number = (iarr[0] * 255 * 255 + iarr[1] * 255 + iarr[2]);
						break;
					case 4:
						number = (iarr[0] * 255 * 255 * 255 + iarr[1] * 255 * 255 + iarr[2] * 255 + iarr[3]);
						break;
					default:
						break;
				}
			}
			else
			{
				number = int.Parse(value, System.Globalization.NumberStyles.Integer);
			}
		}
		catch { val = false; }

		uint v = (uint)number;

		return val;
	}
}
