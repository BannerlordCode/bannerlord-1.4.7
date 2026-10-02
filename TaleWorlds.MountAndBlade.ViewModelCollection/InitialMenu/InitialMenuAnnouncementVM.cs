using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu
{
	// Token: 0x0200004A RID: 74
	public class InitialMenuAnnouncementVM : ViewModel
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x00017464 File Offset: 0x00015664
		public InitialMenuAnnouncementVM()
		{
			this.Refresh();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00017474 File Offset: 0x00015674
		public void Tick()
		{
			if (this._needsRefresh && !this._isFetchingData)
			{
				try
				{
					this.SetDataFromAnnouncementInfo();
				}
				catch (Exception)
				{
					this._announcementInfo = null;
					this.ImageSourcePath = null;
					this._clickLink = null;
				}
				this.IsVisible = !string.IsNullOrEmpty(this.ImageSourcePath);
				this.IsLinkAvailable = !string.IsNullOrEmpty(this._clickLink);
				this._needsRefresh = false;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000174F0 File Offset: 0x000156F0
		private void SetDataFromAnnouncementInfo()
		{
			Dictionary<string, InitialMenuAnnouncementVM.AnnouncementInformation> dictionary;
			if (this._announcementInfo == null || !this._announcementInfo.TryGetValue(this.GetPlatformString(), out dictionary) || dictionary == null || dictionary.Count <= 0)
			{
				this.ImageSourcePath = null;
				this._clickLink = null;
				return;
			}
			InitialMenuAnnouncementVM.AnnouncementInformation announcementInformation;
			if (!dictionary.TryGetValue(this.GetLanguageString(), out announcementInformation) && !dictionary.TryGetValue("en", out announcementInformation))
			{
				announcementInformation = dictionary.Values.FirstOrDefault<InitialMenuAnnouncementVM.AnnouncementInformation>();
			}
			bool flag = true;
			List<string> excludedModules = announcementInformation.ExcludedModules;
			if (excludedModules != null && excludedModules.Count > 0)
			{
				if (excludedModules.TrueForAll((string module) => ModuleHelper.IsModuleActive(module)))
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.ImageSourcePath = announcementInformation.ImageUrl;
				this._clickLink = announcementInformation.LinkUrl;
				return;
			}
			this.ImageSourcePath = null;
			this._clickLink = null;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000175D7 File Offset: 0x000157D7
		public void Refresh()
		{
			if (this._isFetchingData)
			{
				return;
			}
			if (this._announcementInfo != null)
			{
				this._needsRefresh = true;
				return;
			}
			this._isFetchingData = true;
			this.RefreshAux();
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00017600 File Offset: 0x00015800
		private async void RefreshAux()
		{
			try
			{
				string text = await HttpHelper.DownloadStringTaskAsync("https://taleworldswebsiteassets.blob.core.windows.net/upload/upsell/data.json");
				this._announcementInfo = Common.DeserializeObjectFromJson<Dictionary<string, Dictionary<string, InitialMenuAnnouncementVM.AnnouncementInformation>>>(text);
			}
			catch (Exception)
			{
			}
			this._isFetchingData = false;
			this._needsRefresh = true;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001763C File Offset: 0x0001583C
		public void ExecuteNavigateToLink()
		{
			if (!this.IsLinkAvailable || string.IsNullOrEmpty(this._clickLink))
			{
				return;
			}
			if (ApplicationPlatform.CurrentPlatform == Platform.Durango || ApplicationPlatform.CurrentPlatform == Platform.Orbis)
			{
				Utilities.OpenConsoleStorePage(this._clickLink);
				return;
			}
			if (!PlatformServices.Instance.ShowOverlayForWebPage(this._clickLink).Result)
			{
				Process.Start(new ProcessStartInfo(this._clickLink)
				{
					UseShellExecute = true
				});
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x000176AC File Offset: 0x000158AC
		private string GetPlatformString()
		{
			switch (ApplicationPlatform.CurrentPlatform)
			{
			case Platform.WindowsSteam:
				return "steam";
			case Platform.WindowsEpic:
				return "epic";
			case Platform.Orbis:
				return "ps4";
			case Platform.Durango:
				return "xbone";
			case Platform.WindowsGOG:
				return "gog";
			case Platform.GDKDesktop:
				return "gdk";
			}
			return string.Empty;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00017714 File Offset: 0x00015914
		private string GetLanguageString()
		{
			string language = BannerlordConfig.Language;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(language);
			if (num <= 1760062771U)
			{
				if (num <= 463134907U)
				{
					if (num != 134208041U)
					{
						if (num != 425085109U)
						{
							if (num == 463134907U)
							{
								if (language == "English")
								{
									return "en";
								}
							}
						}
						else if (language == "Português (BR)")
						{
							return "br";
						}
					}
					else if (language == "Polski")
					{
						return "pl";
					}
				}
				else if (num != 1161419880U)
				{
					if (num != 1409693518U)
					{
						if (num == 1760062771U)
						{
							if (language == "Русский")
							{
								return "ru";
							}
						}
					}
					else if (language == "日本語")
					{
						return "jp";
					}
				}
				else if (language == "繁體中文")
				{
					return "cnt";
				}
			}
			else if (num <= 2613828866U)
			{
				if (num != 1833040324U)
				{
					if (num != 1856371212U)
					{
						if (num == 2613828866U)
						{
							if (language == "한국어")
							{
								return "kr";
							}
						}
					}
					else if (language == "Italiano")
					{
						return "it";
					}
				}
				else if (language == "简体中文")
				{
					return "cns";
				}
			}
			else if (num <= 3399016062U)
			{
				if (num != 2616412764U)
				{
					if (num == 3399016062U)
					{
						if (language == "Türkçe")
						{
							return "tr";
						}
					}
				}
				else if (language == "Español (LA)")
				{
					return "sp";
				}
			}
			else if (num != 4095678947U)
			{
				if (num == 4176589014U)
				{
					if (language == "Français")
					{
						return "fr";
					}
				}
			}
			else if (language == "Deutsch")
			{
				return "de";
			}
			return "en";
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00017933 File Offset: 0x00015B33
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0001793B File Offset: 0x00015B3B
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00017959 File Offset: 0x00015B59
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x00017961 File Offset: 0x00015B61
		[DataSourceProperty]
		public bool IsLinkAvailable
		{
			get
			{
				return this._isLinkAvailable;
			}
			set
			{
				if (value != this._isLinkAvailable)
				{
					this._isLinkAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsLinkAvailable");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001797F File Offset: 0x00015B7F
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x00017987 File Offset: 0x00015B87
		[DataSourceProperty]
		public string ImageSourcePath
		{
			get
			{
				return this._imageSourcePath;
			}
			set
			{
				if (value != this._imageSourcePath)
				{
					this._imageSourcePath = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageSourcePath");
				}
			}
		}

		// Token: 0x040002CB RID: 715
		private bool _isFetchingData;

		// Token: 0x040002CC RID: 716
		private bool _needsRefresh;

		// Token: 0x040002CD RID: 717
		private Dictionary<string, Dictionary<string, InitialMenuAnnouncementVM.AnnouncementInformation>> _announcementInfo;

		// Token: 0x040002CE RID: 718
		private string _clickLink;

		// Token: 0x040002CF RID: 719
		private bool _isVisible;

		// Token: 0x040002D0 RID: 720
		private bool _isLinkAvailable;

		// Token: 0x040002D1 RID: 721
		private string _imageSourcePath;

		// Token: 0x020000E6 RID: 230
		private struct AnnouncementInformation
		{
			// Token: 0x17000387 RID: 903
			// (get) Token: 0x06000CED RID: 3309 RVA: 0x0002A0EA File Offset: 0x000282EA
			// (set) Token: 0x06000CEE RID: 3310 RVA: 0x0002A0F2 File Offset: 0x000282F2
			public string ImageUrl { get; set; }

			// Token: 0x17000388 RID: 904
			// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0002A0FB File Offset: 0x000282FB
			// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x0002A103 File Offset: 0x00028303
			public string LinkUrl { get; set; }

			// Token: 0x17000389 RID: 905
			// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0002A10C File Offset: 0x0002830C
			// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0002A114 File Offset: 0x00028314
			public List<string> ExcludedModules { get; set; }
		}
	}
}
