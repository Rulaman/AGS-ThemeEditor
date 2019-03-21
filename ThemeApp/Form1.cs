using System.Windows.Forms;

namespace ThemeApp
{
	public partial class Form1 : Form
	{
		private static readonly string CurrDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		private FileReader.File AGSTheme;
		private string FilePath = string.Empty;

		public Form1()
		{
			InitializeComponent();

			AGSTheme = new FileReader.File();

			//string filePath = System.IO.Path.Combine(CurrDir, @"VisualStudioDark.json");
			//AGSTheme.Load(filePath);
			//pg.SelectedObject = AGSTheme.Content;
		}

		private void Load_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "AGS theme files|*.json"
			};

			if ( ofd.ShowDialog() == DialogResult.OK )
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(ofd.FileName);

				if ( fi.Exists )
				{
					FilePath = ofd.FileName;
					AGSTheme.Load(FilePath);
					pg.SelectedObject = AGSTheme.Content;
				}
			}
		}

		private void Save_Click(object sender, System.EventArgs e)
		{
			if ( FilePath != null )
			{
				AGSTheme.Write(FilePath);
			}
		}

		private void SaveAs_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog
			{
				Filter = "AGS theme files|*.json"
			};

			if ( sfd.ShowDialog() == DialogResult.OK )
			{
				FilePath = sfd.FileName;
				AGSTheme.Write(FilePath);
			}

		}
	}
}
