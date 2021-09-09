using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using AGS.Types;


namespace ThemeEditor
{
	[RequiredAGSVersion("3.5.0.0")]
	public class PluginLoader : IAGSEditorPlugin
	{
		public PluginLoader(IAGSEditor editor)
		{
			editor.AddComponent(new Component(editor));
		}

		public void Dispose()
		{
		}
	}

	public class Component : IEditorComponent
	{
		private const string COMPONENT_ID = "ThemeEditor";
		private const string COMPONENT_MENU_COMMAND = "ThemeEditorMenuCommand";
		private const string CONTROL_ID_ROOT_NODE = "ThemeEditorRoot";
		private IAGSEditor LocalEditor;
		//private ContentDocument						Pane;
		//private Dictionary<int, FontPane> FontPaneList = new Dictionary<int, FontPane>();
		private Dictionary<int, ContentDocument> PaneDictionary = new Dictionary<int, ContentDocument>();
		private MenuCommand MenuCommand;
		private int MenuEntry;
		public Component(IAGSEditor editor)
		{
			LocalEditor = editor;
			LocalEditor.GUIController.RegisterIcon("ThemeEditorIcon", Properties.Resources.AGSTheme);
			LocalEditor.GUIController.ProjectTree.AddTreeRoot(this, CONTROL_ID_ROOT_NODE, "ThemeEditor", "ThemeEditorIcon");
			LocalEditor.GUIController.ProjectTree.BeforeShowContextMenu += new BeforeShowContextMenuHandler(ProjectTree_BeforeShowContextMenu);
			this.MenuCommand = LocalEditor.GUIController.CreateMenuCommand(this, COMPONENT_MENU_COMMAND, "Edit Theme");
		}

		void ProjectTree_BeforeShowContextMenu(BeforeShowContextMenuEventArgs evArgs)
		{
			if ( evArgs.SelectedNodeID.StartsWith("Fnt") )
			{
				int entry = int.Parse(evArgs.SelectedNodeID.Replace("Fnt", ""));

				//if ( FontPaneList.ContainsKey(entry) )
				//{
				//	MenuEntry = entry;
				//	evArgs.MenuCommands.Commands.Add(MenuCommand);
				//}
			}
		}

		string IEditorComponent.ComponentID
		{
			get { return COMPONENT_ID; }
		}
		IList<MenuCommand> IEditorComponent.GetContextMenu(string controlID)
		{
			return null;
		}
		void IEditorComponent.CommandClick(string controlID)
		{
			System.Int32 entry = -1;

			if ( controlID == COMPONENT_MENU_COMMAND )
			{
				entry = MenuEntry;
			}
			if ( controlID.Contains(CONTROL_ID_ROOT_NODE) )
			{
				try
				{
					entry = int.Parse(controlID.Replace(CONTROL_ID_ROOT_NODE, ""));
				}
				catch
				{
				}
			}

			if ( controlID == COMPONENT_MENU_COMMAND || controlID.Contains(CONTROL_ID_ROOT_NODE) && entry > -1 )
			{
				ContentDocument editpane;
				if ( PaneDictionary.TryGetValue(entry, out editpane) )
				{
					if ( null == editpane )
					{
						//FontPane fontpane;
						//FontPaneList.TryGetValue(entry, out fontpane);
						ThemeEditor fep = new ThemeEditor();// LocalEditor.CurrentGame.DirectoryPath, fontpane.Font.WFNFileName, fontpane.Font.Name);
						//fep.OnFontModified += new System.EventHandler(fep_OnFontModified);

						//fontpane.Document = new ContentDocument(fep, "FontEditor: " + fontpane.Font.Name, this);
						//fep.Tag = fontpane.Document;
						//editpane = fontpane.Document;

						//PaneDictionary[entry] = fontpane.Document;
					}

					LocalEditor.GUIController.AddOrShowPane(editpane);
				}
			}
		}

		void IEditorComponent.PropertyChanged(string propertyName, object oldValue) { }
		void IEditorComponent.BeforeSaveGame()
		{
			foreach ( ContentDocument item in PaneDictionary.Values )
			{
				if ( null != item )
				{
					//FontEditorPane pane = (FontEditorPane)item.Control;
					//pane.Save();
				}
			}
		}
		void IEditorComponent.RefreshDataFromGame()
		{
			//LocalEditor.GUIController.RemovePaneIfExists(Pane);
			//var x = LocalEditor.GUIController.

			//AGS.Editor.ColorThemes t = new AGS.Editor.ColorThemes();

			if ( LocalEditor.CurrentGame.Fonts.Count > 0 )
			{
				int fontcounter = 0;

				LocalEditor.GUIController.ProjectTree.RemoveAllChildNodes(this, CONTROL_ID_ROOT_NODE);
				LocalEditor.GUIController.ProjectTree.StartFromNode(this, CONTROL_ID_ROOT_NODE);

				//foreach ( var item in LocalEditor.Components )
				//{
				//	switch ( item.ComponentID )
				//	{
				//		case "Rooms":
				//			{
				//				int i = 0;

				//				FieldInfo[] fields = item.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

				//				FieldInfo info = item.GetType().GetField("_loadedRoom", BindingFlags.NonPublic | BindingFlags.Instance);
				//				object o = info.GetValue(item);

				//				var ii = fields.Where(x => x.Name == "_loadedRoom");
				//				var l = ii.ToList()[0];
				//				var ll = l.GetValue(item);


				//				foreach ( var inner in fields )
				//				{
				//					int j = 0;
				//				}
				//			}
				//			break;
				//		default:
				//			break;
				//	};
				//}

				foreach ( AGS.Types.Font font in LocalEditor.CurrentGame.Fonts )
				{
					if ( System.IO.File.Exists(System.IO.Path.Combine(LocalEditor.CurrentGame.DirectoryPath, font.WFNFileName)) )
					{
						//FontPaneList.Add(fontcounter, new FontPane(null, null, font));
						PaneDictionary.Add(fontcounter, null);
						LocalEditor.GUIController.ProjectTree.AddTreeLeaf(this, CONTROL_ID_ROOT_NODE + fontcounter.ToString(), font.Name, "FontEditorIcon", false);
					}

					fontcounter++;
				}
			}
		}
		void IEditorComponent.GameSettingsChanged() { }
		void IEditorComponent.ToXml(System.Xml.XmlTextWriter writer) { }
		void IEditorComponent.FromXml(System.Xml.XmlNode node) { }
		void IEditorComponent.EditorShutdown() { }

		void fep_OnFontModified(object sender, System.EventArgs e)
		{
			//ContentDocument tp = (ContentDocument)((FontEditorPane)sender).Tag;
			//MyEventArgs me = (MyEventArgs)e;

			//if ( tp.Name.Contains("*") && me.Modified == false )
			//{
			//	tp.Name = tp.Name.Replace("*", "");
			//}
			//else if ( !tp.Name.Contains("*") && me.Modified == true )
			//{
			//	tp.Name += "*";
			//}

			//LocalEditor.GUIController.AddOrShowPane(tp);
		}
	}
}
