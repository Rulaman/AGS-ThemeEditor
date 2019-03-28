namespace Data
{
	public static class ColorBind
	{
		public static void Format(object sender, System.Windows.Forms.ConvertEventArgs e)
		{
			if ( e.Value is string )
			{
				e.Value = GetColorFromString(((string)e.Value));
			}
			else if ( e.Value is System.Drawing.Color color )
			{
				e.Value = color.R + "; " + color.G + "; " + color.B;
			}
			else if ( e.Value is Theme.ColorClass color2 )
			{
				e.Value = System.Drawing.Color.FromArgb(color2.A, color2.R, color2.G, color2.B);
			}
			else
			{
			}
		}

		public static void Parse(object sender, System.Windows.Forms.ConvertEventArgs e)
		{
			if ( e.Value is string )
			{
				e.Value = GetColorFromString(((string)e.Value));
			}
			else if ( e.Value is System.Drawing.Color color )
			{
				if ( e.DesiredType.Name == "ColorClass" )
				{
					e.Value = new Theme.ColorClass(color.A, color.R, color.G, color.B);
				}
			}
			else
			{
			}
		}

		private static System.Drawing.Color GetColorFromString(string color)
		{
			string[] sa = color.Split(new[] { ',', ';' });
			System.Drawing.Color col = System.Drawing.Color.White;

			try
			{
				switch ( sa.Length )
				{
					case 1: // known name
						col = System.Drawing.Color.FromName(color);
						break;
					case 3: // RGB value
						col = System.Drawing.Color.FromArgb(int.Parse(sa[0]), int.Parse(sa[1]), int.Parse(sa[2]));
						break;
					case 4: // ARGB value
						col = System.Drawing.Color.FromArgb(int.Parse(sa[0]), int.Parse(sa[1]), int.Parse(sa[2]), int.Parse(sa[3]));
						break;
					default:
						break;
				};
			}
			catch ( System.Exception ) { }

			return col;
		}
	}
}
