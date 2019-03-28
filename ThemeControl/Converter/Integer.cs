namespace Data
{
	public static class IntBind
	{
		public static void Format(object sender, System.Windows.Forms.ConvertEventArgs e)
		{
			if ( e.DesiredType == typeof(string) )
			{
				e.Value = ((int)(e.Value)).ToString();
			}
		}

		public static void Parse(object sender, System.Windows.Forms.ConvertEventArgs e)
		{
			if ( e.DesiredType == typeof(int) )
			{
				int.TryParse((string)e.Value, out int result);
				e.Value = result;
			}
		}
	}
}
