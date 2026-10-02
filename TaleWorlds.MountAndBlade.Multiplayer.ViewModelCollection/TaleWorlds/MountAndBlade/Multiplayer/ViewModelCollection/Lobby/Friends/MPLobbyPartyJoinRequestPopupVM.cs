using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005C RID: 92
	public class MPLobbyPartyJoinRequestPopupVM : ViewModel
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x0001B8B0 File Offset: 0x00019AB0
		public MPLobbyPartyJoinRequestPopupVM()
		{
			this.RefreshValues();
			this.MaxAnswerDuration = 30f;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001B8C9 File Offset: 0x00019AC9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=re37GzKI}Party Join Request", null).ToString();
			this.AcceptJoinRequestText = new TextObject("{=Ogr2N5bx}Accept request to join to your party?", null).ToString();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001B900 File Offset: 0x00019B00
		public void OpenWith(PlayerId joiningPlayer, PlayerId viaPlayerId, string viaPlayerName)
		{
			this._viaPlayerId = viaPlayerId;
			this.JoiningPlayer = new MPLobbyPlayerBaseVM(joiningPlayer, "", null, null);
			this.RemainingAnswerDuration = this.MaxAnswerDuration;
			if (viaPlayerId == NetworkMain.GameClient.PlayerID)
			{
				TextObject textObject = new TextObject("{=BcEN71ts}Player wants to join your party.", null);
				this.JoiningPlayerText = textObject.ToString();
			}
			else
			{
				TextObject textObject = new TextObject("{=q3uBjUyB}Player wants to join your party through your party member <a style=\"Strong\"><b>{PLAYER_NAME}</b></a>.", null);
				GameTexts.SetVariable("PLAYER_NAME", viaPlayerName);
				this.JoiningPlayerText = textObject.ToString();
			}
			this.IsEnabled = true;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001B989 File Offset: 0x00019B89
		public void OpenWithNewParty(PlayerId joiningPlayer)
		{
			this.JoiningPlayer = new MPLobbyPlayerBaseVM(joiningPlayer, "", null, null);
			this.JoiningPlayerText = "";
			this.RemainingAnswerDuration = this.MaxAnswerDuration;
			this.IsEnabled = true;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001B9BC File Offset: 0x00019BBC
		public void Close()
		{
			if (this.IsEnabled)
			{
				this.ExecuteDeclineJoinRequest();
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001B9CC File Offset: 0x00019BCC
		public void OnTick(float dt)
		{
			if (this.IsEnabled)
			{
				this.RemainingAnswerDuration -= dt;
				if (this.RemainingAnswerDuration <= 0f)
				{
					this.ExecuteDeclineJoinRequest();
				}
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001B9F7 File Offset: 0x00019BF7
		private void ExecuteAcceptJoinRequest()
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					PlatformServices.Instance.CheckPermissionWithUser(Permission.PlayMultiplayer, this.JoiningPlayer.ProvidedID, delegate(bool permissionResult)
					{
						if (permissionResult)
						{
							NetworkMain.GameClient.AcceptPartyJoinRequest(this.JoiningPlayer.ProvidedID);
						}
						else
						{
							NetworkMain.GameClient.DeclinePartyJoinRequest(this.JoiningPlayer.ProvidedID, PartyJoinDeclineReason.NoPlatformPermission);
						}
						this.IsEnabled = false;
					});
					return;
				}
				NetworkMain.GameClient.DeclinePartyJoinRequest(this.JoiningPlayer.ProvidedID, PartyJoinDeclineReason.NoPlatformPermission);
				this.IsEnabled = false;
			});
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001BA11 File Offset: 0x00019C11
		private void ExecuteDeclineJoinRequest()
		{
			NetworkMain.GameClient.DeclinePartyJoinRequest(this.JoiningPlayer.ProvidedID, PartyJoinDeclineReason.DeclinedByLeader);
			this.IsEnabled = false;
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001BA30 File Offset: 0x00019C30
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0001BA38 File Offset: 0x00019C38
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

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0001BA55 File Offset: 0x00019C55
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x0001BA5D File Offset: 0x00019C5D
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

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0001BA7F File Offset: 0x00019C7F
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x0001BA87 File Offset: 0x00019C87
		[DataSourceProperty]
		public string AcceptJoinRequestText
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
					base.OnPropertyChanged("AcceptJoinRequestText");
				}
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0001BAA9 File Offset: 0x00019CA9
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x0001BAB1 File Offset: 0x00019CB1
		[DataSourceProperty]
		public string JoiningPlayerText
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
					base.OnPropertyChanged("JoiningPlayerText");
				}
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0001BAD3 File Offset: 0x00019CD3
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x0001BADB File Offset: 0x00019CDB
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM JoiningPlayer
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
					base.OnPropertyChanged("JoiningPlayer");
				}
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0001BAF8 File Offset: 0x00019CF8
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x0001BB00 File Offset: 0x00019D00
		[DataSourceProperty]
		public float RemainingAnswerDuration
		{
			get
			{
				return this._remainingAnswerDuration;
			}
			set
			{
				if (value != this._remainingAnswerDuration)
				{
					this._remainingAnswerDuration = value;
					base.OnPropertyChangedWithValue(value, "RemainingAnswerDuration");
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0001BB1E File Offset: 0x00019D1E
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x0001BB26 File Offset: 0x00019D26
		[DataSourceProperty]
		public float MaxAnswerDuration
		{
			get
			{
				return this._maxAnswerDuration;
			}
			set
			{
				if (value != this._maxAnswerDuration)
				{
					this._maxAnswerDuration = value;
					base.OnPropertyChangedWithValue(value, "MaxAnswerDuration");
				}
			}
		}

		// Token: 0x040003FC RID: 1020
		private PlayerId _viaPlayerId;

		// Token: 0x040003FD RID: 1021
		private bool _isEnabled;

		// Token: 0x040003FE RID: 1022
		private float _remainingAnswerDuration;

		// Token: 0x040003FF RID: 1023
		private float _maxAnswerDuration;

		// Token: 0x04000400 RID: 1024
		private string _titleText;

		// Token: 0x04000401 RID: 1025
		private string _doYouWantToInviteText;

		// Token: 0x04000402 RID: 1026
		private string _playerSuggestedText;

		// Token: 0x04000403 RID: 1027
		private MPLobbyPlayerBaseVM _suggestedPlayer;
	}
}
