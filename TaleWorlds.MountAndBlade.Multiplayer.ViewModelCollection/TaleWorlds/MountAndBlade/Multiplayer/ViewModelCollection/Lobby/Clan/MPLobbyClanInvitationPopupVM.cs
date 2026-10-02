using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006A RID: 106
	public class MPLobbyClanInvitationPopupVM : ViewModel
	{
		// Token: 0x06000A57 RID: 2647 RVA: 0x0001FFAC File Offset: 0x0001E1AC
		public MPLobbyClanInvitationPopupVM()
		{
			this.PartyMembersList = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001FFC8 File Offset: 0x0001E1C8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=D9zIAw9y}Clan Invite", null).ToString();
			this.InviteReceivedText = new TextObject("{=wNAl9o4A}You received an invite from", null).ToString();
			this.WantToJoinText = new TextObject("{=qa9aOxLm}Do you want to join this clan?", null).ToString();
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00020020 File Offset: 0x0001E220
		public void Open(string clanName, string clanTag, bool isCreation)
		{
			GameTexts.SetVariable("STR", clanTag);
			string text = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
			GameTexts.SetVariable("STR1", clanName);
			GameTexts.SetVariable("STR2", text);
			this.ClanNameAndTag = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.PartyMembersList.Clear();
			this.IsCreation = isCreation;
			if (isCreation)
			{
				this._invitationMode = MPLobbyClanInvitationPopupVM.InvitationMode.Creation;
				using (List<PartyPlayerInLobbyClient>.Enumerator enumerator = NetworkMain.GameClient.PlayersInParty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PartyPlayerInLobbyClient partyPlayerInLobbyClient = enumerator.Current;
						if (partyPlayerInLobbyClient.PlayerId != NetworkMain.GameClient.PlayerID)
						{
							MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = new MPLobbyClanMemberItemVM(partyPlayerInLobbyClient.PlayerId);
							mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=c0ZdKSkn}Waiting", null).ToString();
							this.PartyMembersList.Add(mplobbyClanMemberItemVM);
						}
					}
					goto IL_00E3;
				}
			}
			this._invitationMode = MPLobbyClanInvitationPopupVM.InvitationMode.Invitation;
			IL_00E3:
			this.WithPlayersText = ((this.PartyMembersList.Count > 1) ? new TextObject("{=iCaRFZpG}along with these players", null).ToString() : string.Empty);
			this.IsEnabled = true;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00020154 File Offset: 0x0001E354
		public void Close()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00020160 File Offset: 0x0001E360
		public void UpdateConfirmation(PlayerId playerId, ClanCreationAnswer answer)
		{
			foreach (MPLobbyClanMemberItemVM mplobbyClanMemberItemVM in this.PartyMembersList)
			{
				if (mplobbyClanMemberItemVM.ProvidedID == playerId)
				{
					if (answer == ClanCreationAnswer.Accepted)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=JTMegIk4}Accepted", null).ToString();
					}
					else if (answer == ClanCreationAnswer.Declined)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=FgaORzy5}Declined", null).ToString();
					}
				}
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000201EC File Offset: 0x0001E3EC
		private void ExecuteAcceptInvitation()
		{
			this.IsEnabled = false;
			if (this._invitationMode == MPLobbyClanInvitationPopupVM.InvitationMode.Creation)
			{
				NetworkMain.GameClient.AcceptClanCreationRequest();
				return;
			}
			NetworkMain.GameClient.AcceptClanInvitation();
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00020212 File Offset: 0x0001E412
		private void ExecuteDeclineInvitation()
		{
			this.IsEnabled = false;
			if (this._invitationMode == MPLobbyClanInvitationPopupVM.InvitationMode.Creation)
			{
				NetworkMain.GameClient.DeclineClanCreationRequest();
				return;
			}
			NetworkMain.GameClient.DeclineClanInvitation();
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x00020238 File Offset: 0x0001E438
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x00020240 File Offset: 0x0001E440
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

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0002025D File Offset: 0x0001E45D
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x00020265 File Offset: 0x0001E465
		[DataSourceProperty]
		public bool IsCreation
		{
			get
			{
				return this._isCreation;
			}
			set
			{
				if (value != this._isCreation)
				{
					this._isCreation = value;
					base.OnPropertyChanged("IsCreation");
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00020282 File Offset: 0x0001E482
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x0002028A File Offset: 0x0001E48A
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

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x000202AC File Offset: 0x0001E4AC
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x000202B4 File Offset: 0x0001E4B4
		[DataSourceProperty]
		public string ClanNameAndTag
		{
			get
			{
				return this._clanNameAndTag;
			}
			set
			{
				if (value != this._clanNameAndTag)
				{
					this._clanNameAndTag = value;
					base.OnPropertyChanged("ClanNameAndTag");
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x000202D6 File Offset: 0x0001E4D6
		// (set) Token: 0x06000A67 RID: 2663 RVA: 0x000202DE File Offset: 0x0001E4DE
		[DataSourceProperty]
		public string InviteReceivedText
		{
			get
			{
				return this._inviteReceivedText;
			}
			set
			{
				if (value != this._inviteReceivedText)
				{
					this._inviteReceivedText = value;
					base.OnPropertyChanged("InviteReceivedText");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00020300 File Offset: 0x0001E500
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x00020308 File Offset: 0x0001E508
		[DataSourceProperty]
		public string WithPlayersText
		{
			get
			{
				return this._withPlayersText;
			}
			set
			{
				if (value != this._withPlayersText)
				{
					this._withPlayersText = value;
					base.OnPropertyChanged("WithPlayersText");
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0002032A File Offset: 0x0001E52A
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x00020332 File Offset: 0x0001E532
		[DataSourceProperty]
		public string WantToJoinText
		{
			get
			{
				return this._wantToJoinText;
			}
			set
			{
				if (value != this._wantToJoinText)
				{
					this._wantToJoinText = value;
					base.OnPropertyChanged("WantToJoinText");
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00020354 File Offset: 0x0001E554
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0002035C File Offset: 0x0001E55C
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> PartyMembersList
		{
			get
			{
				return this._partyMembersList;
			}
			set
			{
				if (value != this._partyMembersList)
				{
					this._partyMembersList = value;
					base.OnPropertyChanged("PartyMembersList");
				}
			}
		}

		// Token: 0x040004B7 RID: 1207
		private MPLobbyClanInvitationPopupVM.InvitationMode _invitationMode;

		// Token: 0x040004B8 RID: 1208
		private bool _isEnabled;

		// Token: 0x040004B9 RID: 1209
		private bool _isCreation;

		// Token: 0x040004BA RID: 1210
		private string _titleText;

		// Token: 0x040004BB RID: 1211
		private string _clanNameAndTag;

		// Token: 0x040004BC RID: 1212
		private string _inviteReceivedText;

		// Token: 0x040004BD RID: 1213
		private string _withPlayersText;

		// Token: 0x040004BE RID: 1214
		private string _wantToJoinText;

		// Token: 0x040004BF RID: 1215
		private MBBindingList<MPLobbyClanMemberItemVM> _partyMembersList;

		// Token: 0x02000145 RID: 325
		public enum InvitationMode
		{
			// Token: 0x04000993 RID: 2451
			Creation,
			// Token: 0x04000994 RID: 2452
			Invitation
		}
	}
}
