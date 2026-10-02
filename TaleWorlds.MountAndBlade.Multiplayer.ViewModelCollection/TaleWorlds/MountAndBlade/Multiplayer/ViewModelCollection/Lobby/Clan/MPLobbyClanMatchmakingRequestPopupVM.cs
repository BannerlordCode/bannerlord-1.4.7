using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006F RID: 111
	public class MPLobbyClanMatchmakingRequestPopupVM : ViewModel
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x00020F5E File Offset: 0x0001F15E
		public MPLobbyClanMatchmakingRequestPopupVM()
		{
			this.ChallengerPartyPlayers = new MBBindingList<MPLobbyPlayerBaseVM>();
			this.RefreshValues();
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00020F78 File Offset: 0x0001F178
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=1pwQgr04}Matchmaking Request", null).ToString();
			this.WantsToJoinText = new TextObject("{=WHKG5Rbq}This team wants to join the match you created.", null).ToString();
			this.DoYouAcceptText = new TextObject("{=xkV9g4le}Do you accept them as your opponent?", null).ToString();
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00020FD0 File Offset: 0x0001F1D0
		public void OpenWith(string clanName, string clanSigilCode, Guid partyId, PlayerId[] challengerPlayerIDs, PlayerId challengerPartyLeaderID, PremadeGameType premadeGameType)
		{
			this.ChallengerPartyPlayers.Clear();
			this.IsClanMatch = false;
			this.IsPracticeMatch = false;
			this._partyId = partyId;
			if (premadeGameType == PremadeGameType.Clan)
			{
				this.IsClanMatch = true;
				this.ClanName = clanName;
				this.ClanSigil = new BannerImageIdentifierVM(new Banner(clanSigilCode), true);
			}
			else if (premadeGameType == PremadeGameType.Practice)
			{
				this.IsPracticeMatch = true;
				this.ChallengerPartyLeader = new MPLobbyPlayerBaseVM(challengerPartyLeaderID, "", null, null);
				foreach (PlayerId playerId in challengerPlayerIDs)
				{
					this.ChallengerPartyPlayers.Add(new MPLobbyPlayerBaseVM(playerId, "", null, null));
				}
			}
			this.IsEnabled = true;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002107B File Offset: 0x0001F27B
		public void Close()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00021084 File Offset: 0x0001F284
		public void ExecuteAcceptMatchmaking()
		{
			NetworkMain.GameClient.AcceptJoinPremadeGameRequest(this._partyId);
			this.Close();
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002109C File Offset: 0x0001F29C
		public void ExecuteDeclineMatchmaking()
		{
			NetworkMain.GameClient.DeclineJoinPremadeGameRequest(this._partyId);
			this.Close();
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x000210B4 File Offset: 0x0001F2B4
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x000210BC File Offset: 0x0001F2BC
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChanged("IsEnabled");
				}
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x000210D9 File Offset: 0x0001F2D9
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x000210E1 File Offset: 0x0001F2E1
		[DataSourceProperty]
		public bool IsClanMatch
		{
			get
			{
				return this._isClanMatch;
			}
			set
			{
				if (value != this._isClanMatch)
				{
					this._isClanMatch = value;
					base.OnPropertyChanged("IsClanMatch");
				}
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x000210FE File Offset: 0x0001F2FE
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x00021106 File Offset: 0x0001F306
		[DataSourceProperty]
		public bool IsPracticeMatch
		{
			get
			{
				return this._isPracticeMatch;
			}
			set
			{
				if (value != this._isPracticeMatch)
				{
					this._isPracticeMatch = value;
					base.OnPropertyChanged("IsPracticeMatch");
				}
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00021123 File Offset: 0x0001F323
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x0002112B File Offset: 0x0001F32B
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0002114D File Offset: 0x0001F34D
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x00021155 File Offset: 0x0001F355
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChanged("ClanName");
				}
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00021177 File Offset: 0x0001F377
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x0002117F File Offset: 0x0001F37F
		[DataSourceProperty]
		public string WantsToJoinText
		{
			get
			{
				return this._wantsToJoinText;
			}
			set
			{
				if (value != this._wantsToJoinText)
				{
					this._wantsToJoinText = value;
					base.OnPropertyChanged("WantsToJoinText");
				}
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x000211A1 File Offset: 0x0001F3A1
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x000211A9 File Offset: 0x0001F3A9
		[DataSourceProperty]
		public string DoYouAcceptText
		{
			get
			{
				return this._doYouAcceptText;
			}
			set
			{
				if (value != this._doYouAcceptText)
				{
					this._doYouAcceptText = value;
					base.OnPropertyChanged("DoYouAcceptText");
				}
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x000211CB File Offset: 0x0001F3CB
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x000211D3 File Offset: 0x0001F3D3
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanSigil
		{
			get
			{
				return this._clanSigil;
			}
			set
			{
				if (value != this._clanSigil)
				{
					this._clanSigil = value;
					base.OnPropertyChanged("ClanSigil");
				}
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000211F0 File Offset: 0x0001F3F0
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x000211F8 File Offset: 0x0001F3F8
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM ChallengerPartyLeader
		{
			get
			{
				return this._challengerPartyLeader;
			}
			set
			{
				if (value != this._challengerPartyLeader)
				{
					this._challengerPartyLeader = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "ChallengerPartyLeader");
				}
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00021216 File Offset: 0x0001F416
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x0002121E File Offset: 0x0001F41E
		[DataSourceProperty]
		public MBBindingList<MPLobbyPlayerBaseVM> ChallengerPartyPlayers
		{
			get
			{
				return this._challengerPartyPlayers;
			}
			set
			{
				if (value != this._challengerPartyPlayers)
				{
					this._challengerPartyPlayers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyPlayerBaseVM>>(value, "ChallengerPartyPlayers");
				}
			}
		}

		// Token: 0x040004EB RID: 1259
		private Guid _partyId;

		// Token: 0x040004EC RID: 1260
		private bool _isEnabled;

		// Token: 0x040004ED RID: 1261
		private bool _isClanMatch;

		// Token: 0x040004EE RID: 1262
		private bool _isPracticeMatch;

		// Token: 0x040004EF RID: 1263
		private string _titleText;

		// Token: 0x040004F0 RID: 1264
		private string _clanName;

		// Token: 0x040004F1 RID: 1265
		private string _wantsToJoinText;

		// Token: 0x040004F2 RID: 1266
		private string _doYouAcceptText;

		// Token: 0x040004F3 RID: 1267
		private BannerImageIdentifierVM _clanSigil;

		// Token: 0x040004F4 RID: 1268
		private MPLobbyPlayerBaseVM _challengerPartyLeader;

		// Token: 0x040004F5 RID: 1269
		private MBBindingList<MPLobbyPlayerBaseVM> _challengerPartyPlayers;
	}
}
