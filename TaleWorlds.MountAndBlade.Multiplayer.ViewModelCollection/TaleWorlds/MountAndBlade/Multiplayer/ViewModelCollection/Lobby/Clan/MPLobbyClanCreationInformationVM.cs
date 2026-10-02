using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000068 RID: 104
	public class MPLobbyClanCreationInformationVM : ViewModel
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x0001EEB7 File Offset: 0x0001D0B7
		public MPLobbyClanCreationInformationVM(Action openClanCreationPopup)
		{
			this._openClanCreationPopup = openClanCreationPopup;
			this.PartyMembers = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001EED8 File Offset: 0x0001D0D8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.CreateClanText = new TextObject("{=ECb8IPbA}Create Clan", null).ToString();
			this.CreateClanDescriptionText = new TextObject("{=aWzdkfvn}Currently you are not a member of a clan or you don't own a clan. You need to create a party from non-clan member players to form your own clan.", null).ToString();
			this.CreateYourClanText = new TextObject("{=kF3b8cH1}Create Your Clan", null).ToString();
			this.DontHaveEnoughPlayersInPartyText = new TextObject("{=bynNUfSr}Your party does not have enough members to create a clan.", null).ToString();
			this.PlayerText = new TextObject("{=RN6zHak0}Player", null).ToString();
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001EF70 File Offset: 0x0001D170
		public void RefreshWith(ClanHomeInfo info)
		{
			if (info == null)
			{
				this.CanCreateClan = false;
				this.CantCreateHint = new HintViewModel(new TextObject("{=EQAjujjO}Clan creation information can't be retrieved", null), null);
				this.DoesHaveEnoughPlayersToCreateClan = false;
				this.PartyMemberCountText = new TextObject("{=y1AGNqyV}Clan creation is not available", null).ToString();
				this.PartyMembers.Clear();
				this.PartyMembers.Add(new MPLobbyClanMemberItemVM(NetworkMain.GameClient.PlayerID));
				return;
			}
			this.CanCreateClan = info.CanCreateClan && NetworkMain.GameClient.IsPartyLeader;
			this.CantCreateHint = new HintViewModel();
			if (!NetworkMain.GameClient.IsPartyLeader)
			{
				this.CantCreateHint = new HintViewModel(new TextObject("{=OiWquyWY}You have to be the leader of the party to create a clan", null), null);
			}
			if (info.NotEnoughPlayersInfo == null)
			{
				this.DoesHaveEnoughPlayersToCreateClan = true;
			}
			else
			{
				this.CurrentPlayerCount = info.NotEnoughPlayersInfo.CurrentPlayerCount;
				this.RequiredPlayerCount = info.NotEnoughPlayersInfo.RequiredPlayerCount;
				this.DoesHaveEnoughPlayersToCreateClan = this.CurrentPlayerCount == this.RequiredPlayerCount;
				GameTexts.SetVariable("LEFT", this.CurrentPlayerCount);
				GameTexts.SetVariable("RIGHT", this.RequiredPlayerCount);
				GameTexts.SetVariable("STR1", GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString());
				GameTexts.SetVariable("STR2", this.PlayerText);
				this.PartyMemberCountText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			}
			this.PartyMembers.Clear();
			if (NetworkMain.GameClient.IsInParty)
			{
				using (List<PartyPlayerInLobbyClient>.Enumerator enumerator = NetworkMain.GameClient.PlayersInParty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PartyPlayerInLobbyClient partyPlayerInLobbyClient = enumerator.Current;
						MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = new MPLobbyClanMemberItemVM(partyPlayerInLobbyClient.PlayerId);
						if (info.PlayerNotEligibleInfos != null)
						{
							foreach (PlayerNotEligibleInfo playerNotEligibleInfo in info.PlayerNotEligibleInfos)
							{
								if (playerNotEligibleInfo.PlayerId == partyPlayerInLobbyClient.PlayerId)
								{
									foreach (PlayerNotEligibleError playerNotEligibleError in playerNotEligibleInfo.Errors)
									{
										mplobbyClanMemberItemVM.SetNotEligibleInfo(playerNotEligibleError);
									}
								}
							}
						}
						this.PartyMembers.Add(mplobbyClanMemberItemVM);
					}
					return;
				}
			}
			this.PartyMembers.Add(new MPLobbyClanMemberItemVM(NetworkMain.GameClient.PlayerID));
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001F1D0 File Offset: 0x0001D3D0
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			foreach (MPLobbyClanMemberItemVM mplobbyClanMemberItemVM in this.PartyMembers)
			{
				mplobbyClanMemberItemVM.UpdateNameAndAvatar(forceUpdate);
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0001F21C File Offset: 0x0001D41C
		public void OnPlayerNameUpdated()
		{
			for (int i = 0; i < this.PartyMembers.Count; i++)
			{
				MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = this.PartyMembers[i];
				if (mplobbyClanMemberItemVM.Id == NetworkMain.GameClient.PlayerID)
				{
					mplobbyClanMemberItemVM.UpdateNameAndAvatar(true);
				}
			}
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001F26A File Offset: 0x0001D46A
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0001F273 File Offset: 0x0001D473
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0001F27C File Offset: 0x0001D47C
		private void ExecuteOpenClanCreationPopup()
		{
			this.ExecuteClosePopup();
			this._openClanCreationPopup();
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0001F28F File Offset: 0x0001D48F
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0001F297 File Offset: 0x0001D497
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

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x0001F2BC File Offset: 0x0001D4BC
		[DataSourceProperty]
		public bool CanCreateClan
		{
			get
			{
				return this._canCreateClan;
			}
			set
			{
				if (value != this._canCreateClan)
				{
					this._canCreateClan = value;
					base.OnPropertyChanged("CanCreateClan");
				}
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0001F2D9 File Offset: 0x0001D4D9
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x0001F2E1 File Offset: 0x0001D4E1
		[DataSourceProperty]
		public bool DoesHaveEnoughPlayersToCreateClan
		{
			get
			{
				return this._doesHaveEnoughPlayersToCreateClan;
			}
			set
			{
				if (value != this._doesHaveEnoughPlayersToCreateClan)
				{
					this._doesHaveEnoughPlayersToCreateClan = value;
					base.OnPropertyChanged("DoesHaveEnoughPlayersToCreateClan");
				}
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0001F2FE File Offset: 0x0001D4FE
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x0001F306 File Offset: 0x0001D506
		[DataSourceProperty]
		public int CurrentPlayerCount
		{
			get
			{
				return this._currentPlayerCount;
			}
			set
			{
				if (value != this._currentPlayerCount)
				{
					this._currentPlayerCount = value;
					base.OnPropertyChanged("CurrentPlayerCount");
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0001F323 File Offset: 0x0001D523
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x0001F32B File Offset: 0x0001D52B
		[DataSourceProperty]
		public int RequiredPlayerCount
		{
			get
			{
				return this._requiredPlayerCount;
			}
			set
			{
				if (value != this._requiredPlayerCount)
				{
					this._requiredPlayerCount = value;
					base.OnPropertyChanged("RequiredPlayerCount");
				}
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0001F348 File Offset: 0x0001D548
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0001F350 File Offset: 0x0001D550
		[DataSourceProperty]
		public string CreateClanText
		{
			get
			{
				return this._createClanText;
			}
			set
			{
				if (value != this._createClanText)
				{
					this._createClanText = value;
					base.OnPropertyChanged("CreateClanText");
				}
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0001F372 File Offset: 0x0001D572
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0001F37A File Offset: 0x0001D57A
		[DataSourceProperty]
		public string CreateClanDescriptionText
		{
			get
			{
				return this._createClanDescriptionText;
			}
			set
			{
				if (value != this._createClanDescriptionText)
				{
					this._createClanDescriptionText = value;
					base.OnPropertyChanged("CreateClanDescriptionText");
				}
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0001F39C File Offset: 0x0001D59C
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0001F3A4 File Offset: 0x0001D5A4
		[DataSourceProperty]
		public string DontHaveEnoughPlayersInPartyText
		{
			get
			{
				return this._dontHaveEnoughPlayersInPartyText;
			}
			set
			{
				if (value != this._dontHaveEnoughPlayersInPartyText)
				{
					this._dontHaveEnoughPlayersInPartyText = value;
					base.OnPropertyChanged("DontHaveEnoughPlayersInPartyText");
				}
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0001F3C6 File Offset: 0x0001D5C6
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x0001F3CE File Offset: 0x0001D5CE
		[DataSourceProperty]
		public string PartyMemberCountText
		{
			get
			{
				return this._partyMemberCountText;
			}
			set
			{
				if (value != this._partyMemberCountText)
				{
					this._partyMemberCountText = value;
					base.OnPropertyChanged("PartyMemberCountText");
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0001F3F0 File Offset: 0x0001D5F0
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
		[DataSourceProperty]
		public string PlayerText
		{
			get
			{
				return this._playerText;
			}
			set
			{
				if (value != this._playerText)
				{
					this._playerText = value;
					base.OnPropertyChanged("PlayerText");
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0001F41A File Offset: 0x0001D61A
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0001F422 File Offset: 0x0001D622
		[DataSourceProperty]
		public string CreateYourClanText
		{
			get
			{
				return this._createYourClanText;
			}
			set
			{
				if (value != this._createYourClanText)
				{
					this._createYourClanText = value;
					base.OnPropertyChanged("CreateYourClanText");
				}
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001F444 File Offset: 0x0001D644
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0001F44C File Offset: 0x0001D64C
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0001F46F File Offset: 0x0001D66F
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0001F477 File Offset: 0x0001D677
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> PartyMembers
		{
			get
			{
				return this._partyMembers;
			}
			set
			{
				if (value != this._partyMembers)
				{
					this._partyMembers = value;
					base.OnPropertyChanged("PartyMembers");
				}
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0001F494 File Offset: 0x0001D694
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0001F49C File Offset: 0x0001D69C
		[DataSourceProperty]
		public HintViewModel CantCreateHint
		{
			get
			{
				return this._cantCreateHint;
			}
			set
			{
				if (value != this._cantCreateHint)
				{
					this._cantCreateHint = value;
					base.OnPropertyChanged("CantCreateHint");
				}
			}
		}

		// Token: 0x04000491 RID: 1169
		private Action _openClanCreationPopup;

		// Token: 0x04000492 RID: 1170
		private bool _isEnabled;

		// Token: 0x04000493 RID: 1171
		private bool _canCreateClan;

		// Token: 0x04000494 RID: 1172
		private bool _doesHaveEnoughPlayersToCreateClan;

		// Token: 0x04000495 RID: 1173
		private int _currentPlayerCount;

		// Token: 0x04000496 RID: 1174
		private int _requiredPlayerCount;

		// Token: 0x04000497 RID: 1175
		private string _createClanText;

		// Token: 0x04000498 RID: 1176
		private string _createClanDescriptionText;

		// Token: 0x04000499 RID: 1177
		private string _dontHaveEnoughPlayersInPartyText;

		// Token: 0x0400049A RID: 1178
		private string _partyMemberCountText;

		// Token: 0x0400049B RID: 1179
		private string _playerText;

		// Token: 0x0400049C RID: 1180
		private string _createYourClanText;

		// Token: 0x0400049D RID: 1181
		private string _closeText;

		// Token: 0x0400049E RID: 1182
		private MBBindingList<MPLobbyClanMemberItemVM> _partyMembers;

		// Token: 0x0400049F RID: 1183
		private HintViewModel _cantCreateHint;
	}
}
