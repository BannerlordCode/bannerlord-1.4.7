using System;
using System.IO;
using System.Xml;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Credits
{
	// Token: 0x02000083 RID: 131
	public class CreditsVM : ViewModel
	{
		// Token: 0x06000ACC RID: 2764 RVA: 0x00026D0D File Offset: 0x00024F0D
		public CreditsVM()
		{
			this.ExitKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"), false);
			this.ExitText = new TextObject("{=exitMenuOption}Exit", null).ToString();
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00026D4C File Offset: 0x00024F4C
		private static CreditsItemVM CreateFromFile(string path)
		{
			CreditsItemVM creditsItemVM = null;
			try
			{
				if (File.Exists(path))
				{
					XmlDocument xmlDocument = new XmlDocument();
					XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
					xmlReaderSettings.IgnoreComments = true;
					using (XmlReader xmlReader = XmlReader.Create(new StreamReader(path), xmlReaderSettings))
					{
						xmlDocument.Load(xmlReader);
					}
					XmlNode xmlNode = null;
					for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
					{
						XmlNode xmlNode2 = xmlDocument.ChildNodes.Item(i);
						if (xmlNode2.NodeType == XmlNodeType.Element && xmlNode2.Name == "Credits")
						{
							xmlNode = xmlNode2;
							break;
						}
					}
					if (xmlNode != null)
					{
						creditsItemVM = CreditsVM.CreateItem(xmlNode);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Print("Could not load Credits xml from " + path + ". Exception: " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				creditsItemVM = null;
			}
			return creditsItemVM;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00026E44 File Offset: 0x00025044
		public void FillFromFile(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					XmlDocument xmlDocument = new XmlDocument();
					XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
					xmlReaderSettings.IgnoreComments = true;
					using (XmlReader xmlReader = XmlReader.Create(new StreamReader(path), xmlReaderSettings))
					{
						xmlDocument.Load(xmlReader);
					}
					XmlNode xmlNode = null;
					for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
					{
						XmlNode xmlNode2 = xmlDocument.ChildNodes.Item(i);
						if (xmlNode2.NodeType == XmlNodeType.Element && xmlNode2.Name == "Credits")
						{
							xmlNode = xmlNode2;
							break;
						}
					}
					if (xmlNode != null)
					{
						CreditsItemVM creditsItemVM = CreditsVM.CreateItem(xmlNode);
						this._rootItem = creditsItemVM;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Print("Could not load Credits xml. Exception: " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00026F34 File Offset: 0x00025134
		private static CreditsItemVM CreateItem(XmlNode node)
		{
			CreditsItemVM creditsItemVM = null;
			if (node.Name.ToLower() == "LoadFromFile".ToLower())
			{
				string value = node.Attributes["Name"].Value;
				string text = "";
				if (node.Attributes["PlatformSpecific"] != null && node.Attributes["PlatformSpecific"].Value.ToLower() == "true")
				{
					if (ApplicationPlatform.IsPlatformConsole())
					{
						text = "Console";
					}
					else
					{
						text = "PC";
					}
				}
				if (node.Attributes["ConsoleSpecific"] != null && node.Attributes["ConsoleSpecific"].Value.ToLower() == "true")
				{
					if (ApplicationPlatform.CurrentPlatform == Platform.Durango)
					{
						text = "XBox";
					}
					else if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
					{
						text = "PlayStation";
					}
					else
					{
						text = "PC";
					}
				}
				creditsItemVM = CreditsVM.CreateFromFile(ModuleHelper.GetModuleFullPath("Native") + "ModuleData/" + value + text + ".xml");
			}
			else
			{
				creditsItemVM = new CreditsItemVM();
				creditsItemVM.Type = node.Name;
				if (node.Attributes["Text"] != null)
				{
					creditsItemVM.Text = new TextObject(node.Attributes["Text"].Value, null).ToString();
				}
				else
				{
					creditsItemVM.Text = "";
				}
				foreach (object obj in node.ChildNodes)
				{
					CreditsItemVM creditsItemVM2 = CreditsVM.CreateItem((XmlNode)obj);
					creditsItemVM.Items.Add(creditsItemVM2);
				}
			}
			return creditsItemVM;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00027108 File Offset: 0x00025308
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ExitKey.OnFinalize();
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0002711B File Offset: 0x0002531B
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x00027123 File Offset: 0x00025323
		[DataSourceProperty]
		public CreditsItemVM RootItem
		{
			get
			{
				return this._rootItem;
			}
			set
			{
				if (value != this._rootItem)
				{
					this._rootItem = value;
					base.OnPropertyChangedWithValue<CreditsItemVM>(value, "RootItem");
				}
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00027141 File Offset: 0x00025341
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x00027149 File Offset: 0x00025349
		[DataSourceProperty]
		public InputKeyItemVM ExitKey
		{
			get
			{
				return this._exitKey;
			}
			set
			{
				if (value != this._exitKey)
				{
					this._exitKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ExitKey");
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00027167 File Offset: 0x00025367
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0002716F File Offset: 0x0002536F
		[DataSourceProperty]
		public string ExitText
		{
			get
			{
				return this._exitText;
			}
			set
			{
				if (value != this._exitText)
				{
					this._exitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExitText");
				}
			}
		}

		// Token: 0x040004ED RID: 1261
		public CreditsItemVM _rootItem;

		// Token: 0x040004EE RID: 1262
		private InputKeyItemVM _exitKey;

		// Token: 0x040004EF RID: 1263
		private string _exitText;
	}
}
