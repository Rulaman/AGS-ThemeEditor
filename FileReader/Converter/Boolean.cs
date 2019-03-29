namespace AGS.Theme.Converter
{
	public static class BoolBind
	{
		public static void Format(object sender, System.Windows.Forms.ConvertEventArgs e)
		{
			if ( e.DesiredType == typeof(string) )
			{
				e.Value = ((bool)(e.Value)).ToString();
			}
		}

		public static void Parse(object sender, System.Windows.Forms.ConvertEventArgs e)
		{
			if ( e.DesiredType == typeof(bool) )
			{
				bool.TryParse((string)e.Value, out bool result);
				e.Value = result;
			}
		}
	}
}
