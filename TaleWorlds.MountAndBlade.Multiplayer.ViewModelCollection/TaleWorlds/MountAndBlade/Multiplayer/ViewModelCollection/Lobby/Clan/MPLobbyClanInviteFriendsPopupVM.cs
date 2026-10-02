using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006B RID: 107
	public class MPLobbyClanInviteFriendsPopupVM : ViewModel
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x00020379 File Offset: 0x0001E579
		public MPLobbyClanInviteFriendsPopupVM(Func<MBBindingList<MPLobbyPlayerBaseVM>> getAllFriends)
		{
			this._getAllFriends = getAllFriends;
			this.OnlineFriends = new MBBindingList<MPLobbyPlayerBaseVM>();
			this.RefreshValues();
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0002039C File Offset: 0x0001E59C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=v4hVLpap}Invite Players to Clan", null).ToString();
			this.InviteText = new TextObject("{=aZnS9ECC}Invite", null).ToString();
			this.CloseText = new TextObject("{=yQtzabbe}Close", null).ToString();
			this.SelectPlayersText = new TextObject("{=ZAejS7WF}Select players to invite to your clan", null).ToString();
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00020408 File Offset: 0x0001E608
		public void Open()
		{
			if (NetworkMain.GameClient.ClanID == Guid.Empty || NetworkMain.GameClient.ClanInfo == null)
			{
				return;
			}
			IEnumerable<PlayerId> enumerable = NetworkMain.GameClient.ClanInfo.Players.Select<ClanPlayer, PlayerId>((ClanPlayer c) => c.PlayerId);
			this.OnlineFriends.Clear();
			using (IEnumerator<MPLobbyPlayerBaseVM> enumerator = this._getAllFriends().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MPLobbyPlayerBaseVM onlineFriend = enumerator.Current;
					if (!enumerable.Contains(onlineFriend.ProvidedID) && !this.OnlineFriends.Any<MPLobbyPlayerBaseVM>((MPLobbyPlayerBaseVM f) => f.ProvidedID == onlineFriend.ProvidedID))
					{
						this.OnlineFriends.Add(onlineFriend);
					}
				}
			}
			this.IsEnabled = true;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00020504 File Offset: 0x0001E704
		private void ExecuteSendInvitation()
		{
			foreach (MPLobbyPlayerBaseVM mplobbyPlayerBaseVM in this.OnlineFriends)
			{
				if (mplobbyPlayerBaseVM.IsSelected)
				{
					mplobbyPlayerBaseVM.ExecuteInviteToClan();
				}
			}
			this.ExecuteClosePopup();
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00020560 File Offset: 0x0001E760
		private void ResetSelection()
		{
			foreach (MPLobbyPlayerBaseVM mplobbyPlayerBaseVM in this.OnlineFriends)
			{
				mplobbyPlayerBaseVM.IsSelected = false;
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000205AC File Offset: 0x0001E7AC
		public void ExecuteClosePopup()
		{
			if (this.IsEnabled)
			{
				this.ResetSelection();
				this.IsEnabled = false;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x000205C3 File Offset: 0x0001E7C3
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x000205CB File Offset: 0x0001E7CB
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

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x000205E8 File Offset: 0x0001E7E8
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x000205F0 File Offset: 0x0001E7F0
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

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00020612 File Offset: 0x0001E812
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x0002061A File Offset: 0x0001E81A
		[DataSourceProperty]
		public string InviteText
		{
			get
			{
				return this._inviteText;
			}
			set
			{
				if (value != this._inviteText)
				{
					this._inviteText = value;
					base.OnPropertyChanged("InviteText");
				}
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0002063C File Offset: 0x0001E83C
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x00020644 File Offset: 0x0001E844
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
					base.OnPropertyChanged("CloseText");
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00020666 File Offset: 0x0001E866
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x0002066E File Offset: 0x0001E86E
		[DataSourceProperty]
		public string SelectPlayersText
		{
			get
			{
				return this._selectPlayersText;
			}
			set
			{
				if (value != this._selectPlayersText)
				{
					this._selectPlayersText = value;
					base.OnPropertyChanged("SelectPlayersText");
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00020690 File Offset: 0x0001E890
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x00020698 File Offset: 0x0001E898
		[DataSourceProperty]
		public MBBindingList<MPLobbyPlayerBaseVM> OnlineFriends
		{
			get
			{
				return this._onlineFriends;
			}
			set
			{
				if (value != this._onlineFriends)
				{
					this._onlineFriends = value;
					base.OnPropertyChanged("OnlineFriends");
				}
			}
		}

		// Token: 0x040004C0 RID: 1216
		private Func<MBBindingList<MPLobbyPlayerBaseVM>> _getAllFriends;

		// Token: 0x040004C1 RID: 1217
		private bool _isEnabled;

		// Token: 0x040004C2 RID: 1218
		private string _titleText;

		// Token: 0x040004C3 RID: 1219
		private string _inviteText;

		// Token: 0x040004C4 RID: 1220
		private string _closeText;

		// Token: 0x040004C5 RID: 1221
		private string _selectPlayersText;

		// Token: 0x040004C6 RID: 1222
		private MBBindingList<MPLobbyPlayerBaseVM> _onlineFriends;
	}
}
