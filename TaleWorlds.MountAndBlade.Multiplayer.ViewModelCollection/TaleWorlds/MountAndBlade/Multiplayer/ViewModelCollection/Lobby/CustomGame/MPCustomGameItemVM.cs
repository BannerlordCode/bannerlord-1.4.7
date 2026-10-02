using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x0200005F RID: 95
	public class MPCustomGameItemVM : ViewModel
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0001C0FA File Offset: 0x0001A2FA
		public GameServerEntry GameServerInfo { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0001C102 File Offset: 0x0001A302
		public PremadeGameEntry PremadeGameInfo { get; }

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001C10C File Offset: 0x0001A30C
		public MPCustomGameItemVM(GameServerEntry gameServerInfo, Action<MPCustomGameItemVM> onSelect, Action<MPCustomGameItemVM> onJoin, Action<MPCustomGameItemVM> onRequestActions, Action<MPCustomGameItemVM> onToggleFavorite)
		{
			this._onSelect = onSelect;
			this._onJoin = onJoin;
			this._onRequestActions = onRequestActions;
			this._onToggleFavorite = onToggleFavorite;
			this.GameServerInfo = gameServerInfo;
			string text = new TextObject("{=vBkrw5VV}Random", null).ToString();
			this._randomString = "-- " + text + " --";
			this.LoadedModulesHint = new BasicTooltipViewModel(() => this.GetLoadedModulesTooltipProperties());
			this.UpdateGameServerInfo();
			this.UpdateIsFavorite();
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001C190 File Offset: 0x0001A390
		public MPCustomGameItemVM(PremadeGameEntry premadeGameInfo, Action<MPCustomGameItemVM> onJoin)
		{
			this._onJoin = onJoin;
			this.PremadeGameInfo = premadeGameInfo;
			this.IsClanMatchItem = true;
			this.IsPingInfoAvailable = false;
			string text = new TextObject("{=vBkrw5VV}Random", null).ToString();
			this._randomString = "-- " + text + " --";
			this.UpdatePremadeGameInfo();
			this.UpdateIsFavorite();
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
		private async void UpdateGameServerInfo()
		{
			this.IsPasswordProtected = this.GameServerInfo.PasswordProtected;
			this.PlayerCount = this.GameServerInfo.PlayerCount;
			this.MaxPlayerCount = this.GameServerInfo.MaxPlayerCount;
			this.NameText = this.GameServerInfo.ServerName;
			TextObject textObject = GameTexts.FindText("str_multiplayer_official_game_type_name", this.GameServerInfo.GameType);
			this.GameTypeText = (textObject.ToString().StartsWith("ERROR: ") ? new TextObject("{=MT4b8H9h}Unknown", null).ToString() : textObject.ToString());
			GameTexts.SetVariable("LEFT", this.PlayerCount);
			GameTexts.SetVariable("RIGHT", this.MaxPlayerCount);
			this.PlayerCountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			this.IsOfficialServer = this.GameServerInfo.IsOfficial;
			this.IsByOfficialServerProvider = this.GameServerInfo.ByOfficialProvider;
			this.IsCommunityServer = !this.IsOfficialServer && !this.IsByOfficialServerProvider;
			this.HostText = this.GameServerInfo.HostName;
			this.IsPingInfoAvailable = MPCustomGameVM.IsPingInfoAvailable;
			await this.UpdatePingText();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001C230 File Offset: 0x0001A430
		private async Task UpdatePingText()
		{
			if (this.IsPingInfoAvailable)
			{
				long num = await NetworkMain.GameClient.GetPingToServer(this.GameServerInfo.Address);
				this.PingText = ((num < 0L) ? "-" : num.ToString());
			}
			else
			{
				this.PingText = "-";
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001C278 File Offset: 0x0001A478
		private void UpdatePremadeGameInfo()
		{
			this.IsPasswordProtected = this.PremadeGameInfo.IsPasswordProtected;
			this.NameText = this.PremadeGameInfo.Name;
			this.GameTypeText = (this.GameTypeText = GameTexts.FindText("str_multiplayer_official_game_type_name", this.PremadeGameInfo.GameType).ToString());
			this.RegionName = this.PremadeGameInfo.Region;
			this.FirstFactionName = ((this.PremadeGameInfo.FactionA == Parameters.RandomSelectionString) ? this._randomString : this.PremadeGameInfo.FactionA);
			this.SecondFactionName = ((this.PremadeGameInfo.FactionB == Parameters.RandomSelectionString) ? this._randomString : this.PremadeGameInfo.FactionB);
			this.HostText = MPCustomGameItemVM.OfficialServerHostName;
			this.IsOfficialServer = true;
			if (this.PremadeGameInfo.PremadeGameType == PremadeGameType.Clan)
			{
				this.PremadeMatchTypeText = new TextObject("{=YNkPy4ta}Clan Match", null).ToString();
				return;
			}
			if (this.PremadeGameInfo.PremadeGameType == PremadeGameType.Practice)
			{
				this.PremadeMatchTypeText = new TextObject("{=H5tiRTya}Practice", null).ToString();
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001C39C File Offset: 0x0001A59C
		private List<TooltipProperty> GetLoadedModulesTooltipProperties()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			if (this.GameServerInfo != null)
			{
				if (this.GameServerInfo.LoadedModules.Count > 0)
				{
					list.Add(new TooltipProperty(string.Empty, new TextObject("{=JXyxj1J5}Modules", null).ToString(), 1, false, TooltipProperty.TooltipPropertyFlags.Title));
					string text = " " + new TextObject("{=oYS9sabI}(optional)", null).ToString();
					foreach (ModuleInfoModel moduleInfoModel in this.GameServerInfo.LoadedModules)
					{
						string text2 = moduleInfoModel.Version;
						if (moduleInfoModel.IsOptional)
						{
							text2 += text;
						}
						list.Add(new TooltipProperty(moduleInfoModel.Name, text2, 0, false, TooltipProperty.TooltipPropertyFlags.None));
					}
				}
				TextObject textObject = (this.GameServerInfo.AllowsOptionalModules ? new TextObject("{=BBmEESTT}This server allows optional modules.", null) : new TextObject("{=sEbeLmZP}This server does not allow optional modules.", null));
				list.Add(new TooltipProperty("", textObject.ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.None));
				if (this.IsCommunityServer)
				{
					list.Add(new TooltipProperty("", string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
					TextObject textObject2 = new TextObject("{=W51HSyXy}Press {VIEW_OPTIONS_KEY} to view options", null);
					string text3 = HotKeyManager.GetCategory("MultiplayerHotkeyCategory").GetHotKey("PreviewCosmeticItem").ToString();
					textObject2.SetTextVariable("VIEW_OPTIONS_KEY", Module.CurrentModule.GlobalTextManager.GetHotKeyGameTextFromKeyID(text3.ToLower()));
					list.Add(new TooltipProperty(string.Empty, textObject2.ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.None)
					{
						OnlyShowWhenNotExtended = true
					});
				}
			}
			return list;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001C560 File Offset: 0x0001A760
		public void UpdateIsFavorite()
		{
			bool flag = false;
			if (this.GameServerInfo != null)
			{
				FavoriteServerData favoriteServerData;
				flag = MultiplayerLocalDataManager.Instance.FavoriteServers.TryGetServerData(this.GameServerInfo, out favoriteServerData);
			}
			this.IsFavorite = flag;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001C596 File Offset: 0x0001A796
		public void ExecuteSelect()
		{
			Action<MPCustomGameItemVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001C5A9 File Offset: 0x0001A7A9
		public void ExecuteFavorite()
		{
			Action<MPCustomGameItemVM> onToggleFavorite = this._onToggleFavorite;
			if (onToggleFavorite == null)
			{
				return;
			}
			onToggleFavorite(this);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001C5BC File Offset: 0x0001A7BC
		public void ExecuteJoin()
		{
			Action<MPCustomGameItemVM> onJoin = this._onJoin;
			if (onJoin == null)
			{
				return;
			}
			onJoin(this);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001C5CF File Offset: 0x0001A7CF
		public void ExecuteViewHostOptions()
		{
			if (this._onRequestActions != null)
			{
				this._onRequestActions(this);
				MBInformationManager.HideInformations();
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001C5EA File Offset: 0x0001A7EA
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0001C5F2 File Offset: 0x0001A7F2
		[DataSourceProperty]
		public bool IsPasswordProtected
		{
			get
			{
				return this._isPasswordProtected;
			}
			set
			{
				if (value != this._isPasswordProtected)
				{
					this._isPasswordProtected = value;
					base.OnPropertyChanged("IsPasswordProtected");
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001C60F File Offset: 0x0001A80F
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0001C617 File Offset: 0x0001A817
		[DataSourceProperty]
		public bool IsFavorite
		{
			get
			{
				return this._isFavorite;
			}
			set
			{
				if (value != this._isFavorite)
				{
					this._isFavorite = value;
					base.OnPropertyChanged("IsFavorite");
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001C634 File Offset: 0x0001A834
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0001C63C File Offset: 0x0001A83C
		[DataSourceProperty]
		public bool IsClanMatchItem
		{
			get
			{
				return this._isClanMatchItem;
			}
			set
			{
				if (value != this._isClanMatchItem)
				{
					this._isClanMatchItem = value;
					base.OnPropertyChanged("IsClanMatchItem");
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001C659 File Offset: 0x0001A859
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0001C661 File Offset: 0x0001A861
		[DataSourceProperty]
		public bool IsOfficialServer
		{
			get
			{
				return this._isOfficialServer;
			}
			set
			{
				if (value != this._isOfficialServer)
				{
					this._isOfficialServer = value;
					base.OnPropertyChanged("IsOfficialServer");
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001C67E File Offset: 0x0001A87E
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0001C686 File Offset: 0x0001A886
		[DataSourceProperty]
		public bool IsByOfficialServerProvider
		{
			get
			{
				return this._isByOfficialServerProvider;
			}
			set
			{
				if (value != this._isByOfficialServerProvider)
				{
					this._isByOfficialServerProvider = value;
					base.OnPropertyChanged("IsByOfficialServerProvider");
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001C6A3 File Offset: 0x0001A8A3
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0001C6AB File Offset: 0x0001A8AB
		[DataSourceProperty]
		public bool IsCommunityServer
		{
			get
			{
				return this._isCommunityServer;
			}
			set
			{
				if (value != this._isCommunityServer)
				{
					this._isCommunityServer = value;
					base.OnPropertyChanged("IsCommunityServer");
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
		[DataSourceProperty]
		public bool IsPingInfoAvailable
		{
			get
			{
				return this._isPingInfoAvailable;
			}
			set
			{
				if (value != this._isPingInfoAvailable)
				{
					this._isPingInfoAvailable = value;
					base.OnPropertyChanged("IsPingInfoAvailable");
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0001C6ED File Offset: 0x0001A8ED
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0001C6F5 File Offset: 0x0001A8F5
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001C712 File Offset: 0x0001A912
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x0001C71A File Offset: 0x0001A91A
		[DataSourceProperty]
		public int PlayerCount
		{
			get
			{
				return this._playerCount;
			}
			set
			{
				if (value != this._playerCount)
				{
					this._playerCount = value;
					base.OnPropertyChanged("PlayerCount");
				}
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001C737 File Offset: 0x0001A937
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x0001C73F File Offset: 0x0001A93F
		[DataSourceProperty]
		public int MaxPlayerCount
		{
			get
			{
				return this._maxPlayerCount;
			}
			set
			{
				if (value != this._maxPlayerCount)
				{
					this._maxPlayerCount = value;
					base.OnPropertyChanged("MaxPlayerCount");
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001C75C File Offset: 0x0001A95C
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0001C764 File Offset: 0x0001A964
		[DataSourceProperty]
		public string HostText
		{
			get
			{
				return this._hostText;
			}
			set
			{
				if (value != this._hostText)
				{
					this._hostText = value;
					base.OnPropertyChanged("HostText");
				}
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0001C786 File Offset: 0x0001A986
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x0001C78E File Offset: 0x0001A98E
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0001C7B0 File Offset: 0x0001A9B0
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
		[DataSourceProperty]
		public string GameTypeText
		{
			get
			{
				return this._gameTypeText;
			}
			set
			{
				if (value != this._gameTypeText)
				{
					this._gameTypeText = value;
					base.OnPropertyChanged("GameTypeText");
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0001C7DA File Offset: 0x0001A9DA
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x0001C7E2 File Offset: 0x0001A9E2
		[DataSourceProperty]
		public string PlayerCountText
		{
			get
			{
				return this._playerCountText;
			}
			set
			{
				if (value != this._playerCountText)
				{
					this._playerCountText = value;
					base.OnPropertyChanged("PlayerCountText");
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0001C804 File Offset: 0x0001AA04
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x0001C80C File Offset: 0x0001AA0C
		[DataSourceProperty]
		public string PingText
		{
			get
			{
				return this._pingText;
			}
			set
			{
				if (value != this._pingText)
				{
					this._pingText = value;
					base.OnPropertyChanged("PingText");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0001C82E File Offset: 0x0001AA2E
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x0001C836 File Offset: 0x0001AA36
		[DataSourceProperty]
		public string FirstFactionName
		{
			get
			{
				return this._firstFactionName;
			}
			set
			{
				if (value != this._firstFactionName)
				{
					this._firstFactionName = value;
					base.OnPropertyChanged("FirstFactionName");
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0001C858 File Offset: 0x0001AA58
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x0001C860 File Offset: 0x0001AA60
		[DataSourceProperty]
		public string SecondFactionName
		{
			get
			{
				return this._secondFactionName;
			}
			set
			{
				if (value != this._secondFactionName)
				{
					this._secondFactionName = value;
					base.OnPropertyChanged("SecondFactionName");
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0001C882 File Offset: 0x0001AA82
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x0001C88A File Offset: 0x0001AA8A
		[DataSourceProperty]
		public string RegionName
		{
			get
			{
				return this._regionName;
			}
			set
			{
				if (value != this._regionName)
				{
					this._regionName = value;
					base.OnPropertyChanged("RegionName");
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0001C8AC File Offset: 0x0001AAAC
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x0001C8B4 File Offset: 0x0001AAB4
		[DataSourceProperty]
		public string PremadeMatchTypeText
		{
			get
			{
				return this._premadeMatchTypeText;
			}
			set
			{
				if (value != this._premadeMatchTypeText)
				{
					this._premadeMatchTypeText = value;
					base.OnPropertyChanged("PremadeMatchTypeText");
				}
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0001C8D6 File Offset: 0x0001AAD6
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x0001C8DE File Offset: 0x0001AADE
		[DataSourceProperty]
		public BasicTooltipViewModel LoadedModulesHint
		{
			get
			{
				return this._loadedModulesHint;
			}
			set
			{
				if (value != this._loadedModulesHint)
				{
					this._loadedModulesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LoadedModulesHint");
				}
			}
		}

		// Token: 0x0400040F RID: 1039
		public const string PingTimeoutText = "-";

		// Token: 0x04000410 RID: 1040
		private readonly Action<MPCustomGameItemVM> _onSelect;

		// Token: 0x04000411 RID: 1041
		private readonly Action<MPCustomGameItemVM> _onJoin;

		// Token: 0x04000412 RID: 1042
		private readonly Action<MPCustomGameItemVM> _onRequestActions;

		// Token: 0x04000413 RID: 1043
		private readonly Action<MPCustomGameItemVM> _onToggleFavorite;

		// Token: 0x04000416 RID: 1046
		private string _randomString;

		// Token: 0x04000417 RID: 1047
		public static readonly string OfficialServerHostName = "TaleWorlds";

		// Token: 0x04000418 RID: 1048
		private bool _isPasswordProtected;

		// Token: 0x04000419 RID: 1049
		private bool _isFavorite;

		// Token: 0x0400041A RID: 1050
		private bool _isClanMatchItem;

		// Token: 0x0400041B RID: 1051
		private bool _isOfficialServer;

		// Token: 0x0400041C RID: 1052
		private bool _isByOfficialServerProvider;

		// Token: 0x0400041D RID: 1053
		private bool _isCommunityServer;

		// Token: 0x0400041E RID: 1054
		private bool _isPingInfoAvailable;

		// Token: 0x0400041F RID: 1055
		private bool _isSelected;

		// Token: 0x04000420 RID: 1056
		private int _playerCount;

		// Token: 0x04000421 RID: 1057
		private int _maxPlayerCount;

		// Token: 0x04000422 RID: 1058
		private string _hostText;

		// Token: 0x04000423 RID: 1059
		private string _nameText;

		// Token: 0x04000424 RID: 1060
		private string _gameTypeText;

		// Token: 0x04000425 RID: 1061
		private string _playerCountText;

		// Token: 0x04000426 RID: 1062
		private string _pingText;

		// Token: 0x04000427 RID: 1063
		private string _firstFactionName;

		// Token: 0x04000428 RID: 1064
		private string _secondFactionName;

		// Token: 0x04000429 RID: 1065
		private string _regionName;

		// Token: 0x0400042A RID: 1066
		private string _premadeMatchTypeText;

		// Token: 0x0400042B RID: 1067
		private BasicTooltipViewModel _loadedModulesHint;
	}
}
