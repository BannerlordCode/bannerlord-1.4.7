using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000072 RID: 114
	public class MPLobbyClanRosterVM : ViewModel
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x00021D6C File Offset: 0x0001FF6C
		public MPLobbyClanRosterVM()
		{
			this.MembersList = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.MemberActionsList = new MBBindingList<StringPairItemWithActionVM>();
			this._memberComparer = new MPLobbyClanRosterVM.MemberComparer();
			this.RefreshValues();
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00021D9C File Offset: 0x0001FF9C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RosterText = new TextObject("{=oyVeCtlg}Roster", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.BadgeText = new TextObject("{=4PrfimcK}Badge", null).ToString();
			this.StatusText = new TextObject("{=DXczLzml}Status", null).ToString();
			this.PromoteToClanOfficerHint = new HintViewModel(new TextObject("{=oeSrXaKt}You need to demote one of the officers", null), null);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00021E20 File Offset: 0x00020020
		public void RefreshClanInformation(ClanHomeInfo info)
		{
			if (info == null || info.ClanInfo == null)
			{
				return;
			}
			this._isClanLeader = NetworkMain.GameClient.IsClanLeader;
			this._isClanOfficer = NetworkMain.GameClient.IsClanOfficer;
			this.MembersList.Clear();
			ClanPlayer[] players = info.ClanInfo.Players;
			for (int j = 0; j < players.Length; j++)
			{
				ClanPlayer member = players[j];
				if (!MultiplayerPlayerHelper.IsBlocked(member.PlayerId))
				{
					ClanPlayerInfo clanPlayerInfo = info.ClanPlayerInfos.First<ClanPlayerInfo>((ClanPlayerInfo i) => i.PlayerId.Equals(member.PlayerId));
					if (clanPlayerInfo != null)
					{
						bool flag = clanPlayerInfo.State == AnotherPlayerState.AtLobby || clanPlayerInfo.State == AnotherPlayerState.InMultiplayerGame || clanPlayerInfo.State == AnotherPlayerState.InParty;
						this.MembersList.Add(new MPLobbyClanMemberItemVM(member, flag, clanPlayerInfo.ActiveBadgeId, clanPlayerInfo.State, new Action<MPLobbyClanMemberItemVM>(this.ExecutePopulateActionsList)));
					}
				}
			}
			this.MembersList.Sort(this._memberComparer);
			this.IsPrivilegedMember = NetworkMain.GameClient.IsClanLeader || NetworkMain.GameClient.IsClanOfficer;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00021F40 File Offset: 0x00020140
		public void OnPlayerNameUpdated(string playerName)
		{
			for (int i = 0; i < this.MembersList.Count; i++)
			{
				MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = this.MembersList[i];
				if (mplobbyClanMemberItemVM.Id == NetworkMain.GameClient.PlayerID)
				{
					mplobbyClanMemberItemVM.UpdateNameAndAvatar(true);
				}
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00021F90 File Offset: 0x00020190
		private void ExecutePopulateActionsList(MPLobbyClanMemberItemVM member)
		{
			this.MemberActionsList.Clear();
			if (NetworkMain.GameClient.PlayerID != member.Id)
			{
				if (this._isClanLeader)
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecutePromoteToClanLeader), new TextObject("{=GRpGNYHW}Promote To Clan Leader", null).ToString(), "PromoteToClanLeader", member));
					if (NetworkMain.GameClient.IsPlayerClanOfficer(member.Id))
					{
						this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteDemoteFromClanOfficer), new TextObject("{=gowlLS2b}Demote From Clan Officer", null).ToString(), "DemoteFromClanOfficer", member));
					}
					else
					{
						StringPairItemWithActionVM stringPairItemWithActionVM = new StringPairItemWithActionVM(new Action<object>(this.ExecutePromoteToClanOfficer), new TextObject("{=BXI1ObU8}Promote To Clan Officer", null).ToString(), "PromoteToClanOfficer", member);
						if (NetworkMain.GameClient.PlayersInClan.Count<ClanPlayer>((ClanPlayer m) => m.Role == ClanPlayerRole.Officer) == Parameters.ClanOfficerCount)
						{
							stringPairItemWithActionVM.IsEnabled = false;
							stringPairItemWithActionVM.Hint = this.PromoteToClanOfficerHint;
						}
						this.MemberActionsList.Add(stringPairItemWithActionVM);
					}
				}
				if ((this._isClanOfficer || this._isClanLeader) && !NetworkMain.GameClient.IsPlayerClanLeader(member.Id) && (!this._isClanOfficer || !NetworkMain.GameClient.IsPlayerClanOfficer(member.Id)))
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteKickFromClan), new TextObject("{=S8pZEPni}Kick From Clan", null).ToString(), "KickFromClan", member));
				}
				if (NetworkMain.GameClient.FriendInfos.All<FriendInfo>((FriendInfo f) => f.Id != member.Id))
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteRequestFriendship), GameTexts.FindText("str_mp_scoreboard_context_request_friendship", null).ToString(), "RequestFriendship", member));
				}
				else
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteTerminateFriendship), new TextObject("{=2YIVRuRa}Remove From Friends", null).ToString(), "TerminateFriendship", member));
				}
				if (NetworkMain.GameClient.SupportedFeatures.SupportsFeatures(Features.Party))
				{
					this.MemberActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteInviteToParty), new TextObject("{=RzROgBkv}Invite To Party", null).ToString(), "InviteToParty", member));
				}
				MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(member, this.MemberActionsList);
			}
			if (this.MemberActionsList.Count > 0)
			{
				this.IsMemberActionsActive = false;
				this.IsMemberActionsActive = true;
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0002226C File Offset: 0x0002046C
		private void ExecuteRequestFriendship(object memberObj)
		{
			MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = memberObj as MPLobbyClanMemberItemVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyClanMemberItemVM.Id);
			NetworkMain.GameClient.AddFriend(mplobbyClanMemberItemVM.Id, flag);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x000222B0 File Offset: 0x000204B0
		private void ExecuteTerminateFriendship(object memberObj)
		{
			MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = memberObj as MPLobbyClanMemberItemVM;
			NetworkMain.GameClient.RemoveFriend(mplobbyClanMemberItemVM.Id);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x000222D4 File Offset: 0x000204D4
		private void ExecutePromoteToClanLeader(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=GRpGNYHW}Promote To Clan Leader", null).ToString();
			string text2 = new TextObject("{=Z0TW2cub}Are you sure want to promote {MEMBER_NAME} as clan leader? You will lose your leadership.", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.PromoteToClanLeader(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002237C File Offset: 0x0002057C
		private void ExecutePromoteToClanOfficer(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=BXI1ObU8}Promote To Clan Officer", null).ToString();
			string text2 = new TextObject("{=MS4Ng2iw}Are you sure want to promote {MEMBER_NAME} as clan officer?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.PromoteToClanOfficer(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00022424 File Offset: 0x00020624
		private void ExecuteDemoteFromClanOfficer(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=gowlLS2b}Demote From Clan Officer", null).ToString();
			string text2 = new TextObject("{=pSb1P6ZA}Are you sure want to demote {MEMBER_NAME} from clan officers?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.DemoteFromClanOfficer(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x000224CC File Offset: 0x000206CC
		private void ExecuteKickFromClan(object memberObj)
		{
			MPLobbyClanMemberItemVM member = memberObj as MPLobbyClanMemberItemVM;
			GameTexts.SetVariable("MEMBER_NAME", member.Name);
			string text = new TextObject("{=S8pZEPni}Kick From Clan", null).ToString();
			string text2 = new TextObject("{=L6eaNe2q}Are you sure want to kick {MEMBER_NAME} from clan?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.KickFromClan(member.Id);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00022574 File Offset: 0x00020774
		private void ExecuteInviteToParty(object memberObj)
		{
			MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = memberObj as MPLobbyClanMemberItemVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyClanMemberItemVM.Id);
			NetworkMain.GameClient.InviteToParty(mplobbyClanMemberItemVM.Id, flag);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x000225B7 File Offset: 0x000207B7
		private void ExecuteViewProfile(object memberObj)
		{
			(memberObj as MPLobbyClanMemberItemVM).ExecuteShowProfile();
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000225C4 File Offset: 0x000207C4
		private void PromoteToClanLeader(PlayerId playerId)
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(playerId);
			NetworkMain.GameClient.PromoteToClanLeader(playerId, flag);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x000225F8 File Offset: 0x000207F8
		private void PromoteToClanOfficer(PlayerId playerId)
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(playerId);
			NetworkMain.GameClient.AssignAsClanOfficer(playerId, flag);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0002262A File Offset: 0x0002082A
		private void DemoteFromClanOfficer(PlayerId playerId)
		{
			NetworkMain.GameClient.RemoveClanOfficerRoleForPlayer(playerId);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00022637 File Offset: 0x00020837
		private void KickFromClan(PlayerId playerId)
		{
			NetworkMain.GameClient.KickFromClan(playerId);
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00022644 File Offset: 0x00020844
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0002264C File Offset: 0x0002084C
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
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0002266A File Offset: 0x0002086A
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x00022672 File Offset: 0x00020872
		[DataSourceProperty]
		public bool IsMemberActionsActive
		{
			get
			{
				return this._isMemberActionsActive;
			}
			set
			{
				if (value != this._isMemberActionsActive)
				{
					this._isMemberActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsMemberActionsActive");
				}
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00022690 File Offset: 0x00020890
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x00022698 File Offset: 0x00020898
		[DataSourceProperty]
		public bool IsPrivilegedMember
		{
			get
			{
				return this._isPrivilegedMember;
			}
			set
			{
				if (value != this._isPrivilegedMember)
				{
					this._isPrivilegedMember = value;
					base.OnPropertyChangedWithValue(value, "IsPrivilegedMember");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x000226B6 File Offset: 0x000208B6
		// (set) Token: 0x06000B56 RID: 2902 RVA: 0x000226BE File Offset: 0x000208BE
		[DataSourceProperty]
		public string RosterText
		{
			get
			{
				return this._rosterText;
			}
			set
			{
				if (value != this._rosterText)
				{
					this._rosterText = value;
					base.OnPropertyChangedWithValue<string>(value, "RosterText");
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x000226E1 File Offset: 0x000208E1
		// (set) Token: 0x06000B58 RID: 2904 RVA: 0x000226E9 File Offset: 0x000208E9
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
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0002270C File Offset: 0x0002090C
		// (set) Token: 0x06000B5A RID: 2906 RVA: 0x00022714 File Offset: 0x00020914
		[DataSourceProperty]
		public string BadgeText
		{
			get
			{
				return this._badgeText;
			}
			set
			{
				if (value != this._badgeText)
				{
					this._badgeText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeText");
				}
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x00022737 File Offset: 0x00020937
		// (set) Token: 0x06000B5C RID: 2908 RVA: 0x0002273F File Offset: 0x0002093F
		[DataSourceProperty]
		public string StatusText
		{
			get
			{
				return this._statusText;
			}
			set
			{
				if (value != this._statusText)
				{
					this._statusText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatusText");
				}
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00022762 File Offset: 0x00020962
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0002276A File Offset: 0x0002096A
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> MembersList
		{
			get
			{
				return this._membersList;
			}
			set
			{
				if (value != this._membersList)
				{
					this._membersList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClanMemberItemVM>>(value, "MembersList");
				}
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00022788 File Offset: 0x00020988
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x00022790 File Offset: 0x00020990
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> MemberActionsList
		{
			get
			{
				return this._memberActionsList;
			}
			set
			{
				if (value != this._memberActionsList)
				{
					this._memberActionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "MemberActionsList");
				}
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x000227AE File Offset: 0x000209AE
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x000227B6 File Offset: 0x000209B6
		[DataSourceProperty]
		public HintViewModel PromoteToClanOfficerHint
		{
			get
			{
				return this._promoteToClanOfficerHint;
			}
			set
			{
				if (value != this._promoteToClanOfficerHint)
				{
					this._promoteToClanOfficerHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PromoteToClanOfficerHint");
				}
			}
		}

		// Token: 0x0400051F RID: 1311
		private bool _isClanLeader;

		// Token: 0x04000520 RID: 1312
		private bool _isClanOfficer;

		// Token: 0x04000521 RID: 1313
		private MPLobbyClanRosterVM.MemberComparer _memberComparer;

		// Token: 0x04000522 RID: 1314
		private bool _isSelected;

		// Token: 0x04000523 RID: 1315
		private bool _isMemberActionsActive;

		// Token: 0x04000524 RID: 1316
		private bool _isPrivilegedMember;

		// Token: 0x04000525 RID: 1317
		private string _rosterText;

		// Token: 0x04000526 RID: 1318
		private string _nameText;

		// Token: 0x04000527 RID: 1319
		private string _badgeText;

		// Token: 0x04000528 RID: 1320
		private string _statusText;

		// Token: 0x04000529 RID: 1321
		private MBBindingList<MPLobbyClanMemberItemVM> _membersList;

		// Token: 0x0400052A RID: 1322
		private MBBindingList<StringPairItemWithActionVM> _memberActionsList;

		// Token: 0x0400052B RID: 1323
		private HintViewModel _promoteToClanOfficerHint;

		// Token: 0x0200014F RID: 335
		private class MemberComparer : IComparer<MPLobbyClanMemberItemVM>
		{
			// Token: 0x06001259 RID: 4697 RVA: 0x00039B44 File Offset: 0x00037D44
			public int Compare(MPLobbyClanMemberItemVM x, MPLobbyClanMemberItemVM y)
			{
				if (y.Rank != x.Rank)
				{
					return y.Rank.CompareTo(x.Rank);
				}
				return y.IsOnline.CompareTo(x.IsOnline);
			}
		}
	}
}
