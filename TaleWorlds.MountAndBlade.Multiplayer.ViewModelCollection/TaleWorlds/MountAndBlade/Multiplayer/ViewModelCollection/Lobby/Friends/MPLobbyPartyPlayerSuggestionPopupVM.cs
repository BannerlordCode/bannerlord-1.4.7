using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000056 RID: 86
	public class MPLobbyPartyPlayerSuggestionPopupVM : ViewModel
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x000191FF File Offset: 0x000173FF
		public MPLobbyPartyPlayerSuggestionPopupVM()
		{
			this.RefreshValues();
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001920D File Offset: 0x0001740D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=q2Y7aHSF}Invite Suggestion", null).ToString();
			this.DoYouWantToInviteText = new TextObject("{=VFqoa6vD}Do you want to invite this player to your party?", null).ToString();
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00019244 File Offset: 0x00017444
		public void OpenWith(MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData data)
		{
			this._suggestedPlayerId = data.PlayerId;
			this.SuggestedPlayer = new MPLobbyPlayerBaseVM(data.PlayerId, "", null, null);
			TextObject textObject = new TextObject("{=C7OHivNl}Your friend <a style=\"Strong\"><b>{PLAYER_NAME}</b></a> wants you to invite the player below to your party.", null);
			GameTexts.SetVariable("PLAYER_NAME", data.SuggestingPlayerName);
			this.PlayerSuggestedText = textObject.ToString();
			this.IsEnabled = true;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000192A4 File Offset: 0x000174A4
		public void Close()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000192AD File Offset: 0x000174AD
		private void ExecuteAcceptSuggestion()
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					PlatformServices.Instance.CheckPermissionWithUser(Permission.PlayMultiplayer, this._suggestedPlayerId, async delegate(bool permissionResult)
					{
						if (permissionResult)
						{
							if (PlatformServices.Instance.UsePlatformInvitationService(this._suggestedPlayerId))
							{
								await NetworkMain.GameClient.InviteToPlatformSession(this._suggestedPlayerId);
							}
							else
							{
								bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this._suggestedPlayerId);
								NetworkMain.GameClient.InviteToParty(this._suggestedPlayerId, flag);
							}
						}
						this.Close();
					});
					return;
				}
				this.Close();
			});
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000192C7 File Offset: 0x000174C7
		private void ExecuteDeclineSuggestion()
		{
			this.Close();
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x000192CF File Offset: 0x000174CF
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x000192D7 File Offset: 0x000174D7
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

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x000192F4 File Offset: 0x000174F4
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x000192FC File Offset: 0x000174FC
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

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001931E File Offset: 0x0001751E
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x00019326 File Offset: 0x00017526
		[DataSourceProperty]
		public string DoYouWantToInviteText
		{
			get
			{
				return this._doYouWantToInviteText;
			}
			set
			{
				if (value != this._doYouWantToInviteText)
				{
					this._doYouWantToInviteText = value;
					base.OnPropertyChanged("DoYouWantToInviteText");
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00019348 File Offset: 0x00017548
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00019350 File Offset: 0x00017550
		[DataSourceProperty]
		public string PlayerSuggestedText
		{
			get
			{
				return this._playerSuggestedText;
			}
			set
			{
				if (value != this._playerSuggestedText)
				{
					this._playerSuggestedText = value;
					base.OnPropertyChanged("PlayerSuggestedText");
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00019372 File Offset: 0x00017572
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x0001937A File Offset: 0x0001757A
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM SuggestedPlayer
		{
			get
			{
				return this._suggestedPlayer;
			}
			set
			{
				if (value != this._suggestedPlayer)
				{
					this._suggestedPlayer = value;
					base.OnPropertyChanged("SuggestedPlayer");
				}
			}
		}

		// Token: 0x0400038B RID: 907
		private PlayerId _suggestedPlayerId;

		// Token: 0x0400038C RID: 908
		private bool _isEnabled;

		// Token: 0x0400038D RID: 909
		private string _titleText;

		// Token: 0x0400038E RID: 910
		private string _doYouWantToInviteText;

		// Token: 0x0400038F RID: 911
		private string _playerSuggestedText;

		// Token: 0x04000390 RID: 912
		private MPLobbyPlayerBaseVM _suggestedPlayer;

		// Token: 0x0200011C RID: 284
		public class PlayerPartySuggestionData
		{
			// Token: 0x170005A4 RID: 1444
			// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00037B93 File Offset: 0x00035D93
			// (set) Token: 0x060011E5 RID: 4581 RVA: 0x00037B9B File Offset: 0x00035D9B
			public PlayerId PlayerId { get; private set; }

			// Token: 0x170005A5 RID: 1445
			// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00037BA4 File Offset: 0x00035DA4
			// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00037BAC File Offset: 0x00035DAC
			public string PlayerName { get; private set; }

			// Token: 0x170005A6 RID: 1446
			// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00037BB5 File Offset: 0x00035DB5
			// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00037BBD File Offset: 0x00035DBD
			public PlayerId SuggestingPlayerId { get; private set; }

			// Token: 0x170005A7 RID: 1447
			// (get) Token: 0x060011EA RID: 4586 RVA: 0x00037BC6 File Offset: 0x00035DC6
			// (set) Token: 0x060011EB RID: 4587 RVA: 0x00037BCE File Offset: 0x00035DCE
			public string SuggestingPlayerName { get; private set; }

			// Token: 0x060011EC RID: 4588 RVA: 0x00037BD7 File Offset: 0x00035DD7
			public PlayerPartySuggestionData(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
			{
				this.PlayerId = playerId;
				this.PlayerName = playerName;
				this.SuggestingPlayerId = suggestingPlayerId;
				this.SuggestingPlayerName = suggestingPlayerName;
			}
		}
	}
}
