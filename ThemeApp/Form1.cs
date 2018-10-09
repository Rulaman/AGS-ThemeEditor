using System.Windows.Forms;

namespace ThemeApp
{
	public partial class Form1 : Form
	{
		private static readonly string CurrDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

		public Form1()
		{
			InitializeComponent();

			FileReader.File fc = new FileReader.File();

			string filePath = System.IO.Path.Combine(CurrDir, @"VisualStudioDark.json");
			fc.Load(filePath);
			pg.SelectedObject = fc.Content;
		}
	}
}
