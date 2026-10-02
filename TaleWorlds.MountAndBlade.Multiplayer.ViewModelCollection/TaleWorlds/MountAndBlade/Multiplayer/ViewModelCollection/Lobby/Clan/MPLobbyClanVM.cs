using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000074 RID: 116
	public class MPLobbyClanVM : ViewModel
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x000229DF File Offset: 0x00020BDF
		public MPLobbyClanVM(Action openInviteClanMemberPopup)
		{
			this.ClanOverview = new MPLobbyClanOverviewVM(openInviteClanMemberPopup);
			this.ClanRoster = new MPLobbyClanRosterVM();
			this._activeNotifications = new List<LobbyNotification>();
			this.TrySetClanSubPage(MPLobbyClanVM.ClanSubPages.Overview);
			this.RefreshValues();
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00022A16 File Offset: 0x00020C16
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.ClanOverview.RefreshValues();
			this.ClanRoster.RefreshValues();
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00022A4C File Offset: 0x00020C4C
		private void OnIsEnabledChanged()
		{
			if (this.IsEnabled)
			{
				this.TrySetClanSubPage(MPLobbyClanVM.ClanSubPages.Overview);
				foreach (LobbyNotification lobbyNotification in this._activeNotifications)
				{
					NetworkMain.GameClient.MarkNotificationAsRead(lobbyNotification.Id);
				}
				this._activeNotifications.Clear();
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00022AC4 File Offset: 0x00020CC4
		public async void OnClanInfoChanged()
		{
			ClanHomeInfo clanHomeInfo = NetworkMain.GameClient.ClanHomeInfo;
			if (clanHomeInfo == null)
			{
				Debug.FailedAssert("Retrieved clan home info is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Clan\\MPLobbyClanVM.cs", "OnClanInfoChanged", 65);
			}
			else
			{
				await this.ClanOverview.RefreshClanInformation(clanHomeInfo);
				this.ClanRoster.RefreshClanInformation(clanHomeInfo);
				if (!clanHomeInfo.IsInClan)
				{
					this.ExecuteClosePopup();
				}
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00022AFD File Offset: 0x00020CFD
		private void ExecuteChangeEnabledSubPage(int subpageIndex)
		{
			this.TrySetClanSubPage((MPLobbyClanVM.ClanSubPages)subpageIndex);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00022B08 File Offset: 0x00020D08
		public async void TrySetClanSubPage(MPLobbyClanVM.ClanSubPages newPage)
		{
			this.ClanOverview.IsSelected = false;
			this.ClanRoster.IsSelected = false;
			this._currentSubPage = newPage;
			this.SelectedSubPageIndex = (int)newPage;
			if (newPage == MPLobbyClanVM.ClanSubPages.Overview)
			{
				this.ClanOverview.IsSelected = true;
				await this.ClanOverview.RefreshClanInformation(NetworkMain.GameClient.ClanHomeInfo);
			}
			else if (newPage == MPLobbyClanVM.ClanSubPages.Roster)
			{
				this.ClanRoster.IsSelected = true;
				this.ClanRoster.RefreshClanInformation(NetworkMain.GameClient.ClanHomeInfo);
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00022B49 File Offset: 0x00020D49
		public void OnNotificationReceived(LobbyNotification notification)
		{
			if (this.IsEnabled)
			{
				NetworkMain.GameClient.MarkNotificationAsRead(notification.Id);
				return;
			}
			this._activeNotifications.Add(notification);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00022B70 File Offset: 0x00020D70
		public void OnPlayerNameUpdated(string playerName)
		{
			MPLobbyClanRosterVM clanRoster = this.ClanRoster;
			if (clanRoster == null)
			{
				return;
			}
			clanRoster.OnPlayerNameUpdated(playerName);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00022B83 File Offset: 0x00020D83
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00022B8C File Offset: 0x00020D8C
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00022B95 File Offset: 0x00020D95
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00022B9D File Offset: 0x00020D9D
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
					this.OnIsEnabledChanged();
				}
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00022BC0 File Offset: 0x00020DC0
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00022BC8 File Offset: 0x00020DC8
		[DataSourceProperty]
		public int SelectedSubPageIndex
		{
			get
			{
				return this._selectedSubPageIndex;
			}
			set
			{
				if (value != this._selectedSubPageIndex)
				{
					this._selectedSubPageIndex = value;
					base.OnPropertyChanged("SelectedSubPageIndex");
				}
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00022BE5 File Offset: 0x00020DE5
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x00022BED File Offset: 0x00020DED
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

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00022C10 File Offset: 0x00020E10
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x00022C18 File Offset: 0x00020E18
		[DataSourceProperty]
		public MPLobbyClanOverviewVM ClanOverview
		{
			get
			{
				return this._clanOverview;
			}
			set
			{
				if (value != this._clanOverview)
				{
					this._clanOverview = value;
					base.OnPropertyChanged("ClanOverview");
				}
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00022C35 File Offset: 0x00020E35
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00022C3D File Offset: 0x00020E3D
		[DataSourceProperty]
		public MPLobbyClanRosterVM ClanRoster
		{
			get
			{
				return this._clanRoster;
			}
			set
			{
				if (value != this._clanRoster)
				{
					this._clanRoster = value;
					base.OnPropertyChanged("ClanRoster");
				}
			}
		}

		// Token: 0x04000533 RID: 1331
		private MPLobbyClanVM.ClanSubPages _currentSubPage;

		// Token: 0x04000534 RID: 1332
		private List<LobbyNotification> _activeNotifications;

		// Token: 0x04000535 RID: 1333
		private bool _isEnabled;

		// Token: 0x04000536 RID: 1334
		private int _selectedSubPageIndex;

		// Token: 0x04000537 RID: 1335
		private string _closeText;

		// Token: 0x04000538 RID: 1336
		private MPLobbyClanOverviewVM _clanOverview;

		// Token: 0x04000539 RID: 1337
		private MPLobbyClanRosterVM _clanRoster;

		// Token: 0x02000158 RID: 344
		public enum ClanSubPages
		{
			// Token: 0x040009B8 RID: 2488
			Overview,
			// Token: 0x040009B9 RID: 2489
			Roster
		}
	}
}
