using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002D RID: 45
	public class MPLobbyPartyInvitationPopupVM : ViewModel
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000C78C File Offset: 0x0000A98C
		public MPLobbyPartyInvitationPopupVM()
		{
			this.RefreshValues();
			this.MaxAnswerDuration = 60f;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C7A5 File Offset: 0x0000A9A5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = new TextObject("{=QDNcl3DH}Party Invitation", null).ToString();
			this.Message = new TextObject("{=AaAcmalE}You've been invited to join a party by", null).ToString();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C7D9 File Offset: 0x0000A9D9
		public void OpenWith(PlayerId invitingPlayerID)
		{
			this.RemainingAnswerDuration = this.MaxAnswerDuration;
			this.InvitingPlayer = new MPLobbyPlayerBaseVM(invitingPlayerID, "", null, null);
			this.IsEnabled = true;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C801 File Offset: 0x0000AA01
		public void Close()
		{
			if (this.IsEnabled)
			{
				this.ExecuteDecline();
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C811 File Offset: 0x0000AA11
		public void OnTick(float dt)
		{
			if (this.IsEnabled)
			{
				this.RemainingAnswerDuration -= dt;
				if (this.RemainingAnswerDuration <= 0f)
				{
					this.ExecuteDecline();
				}
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000C83C File Offset: 0x0000AA3C
		private void ExecuteAccept()
		{
			this.IsEnabled = false;
			NetworkMain.GameClient.AcceptPartyInvitation();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C84F File Offset: 0x0000AA4F
		private void ExecuteDecline()
		{
			this.IsEnabled = false;
			NetworkMain.GameClient.DeclinePartyInvitation();
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000C862 File Offset: 0x0000AA62
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000C86A File Offset: 0x0000AA6A
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
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000C888 File Offset: 0x0000AA88
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000C890 File Offset: 0x0000AA90
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000C8B3 File Offset: 0x0000AAB3
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000C8BB File Offset: 0x0000AABB
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000C8DE File Offset: 0x0000AADE
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000C8E6 File Offset: 0x0000AAE6
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM InvitingPlayer
		{
			get
			{
				return this._invitingPlayer;
			}
			set
			{
				if (value != this._invitingPlayer)
				{
					this._invitingPlayer = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "InvitingPlayer");
				}
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000C904 File Offset: 0x0000AB04
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000C90C File Offset: 0x0000AB0C
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000C92A File Offset: 0x0000AB2A
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000C932 File Offset: 0x0000AB32
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

		// Token: 0x040001BA RID: 442
		private bool _isEnabled;

		// Token: 0x040001BB RID: 443
		private float _remainingAnswerDuration;

		// Token: 0x040001BC RID: 444
		private float _maxAnswerDuration;

		// Token: 0x040001BD RID: 445
		private string _title;

		// Token: 0x040001BE RID: 446
		private string _message;

		// Token: 0x040001BF RID: 447
		private MPLobbyPlayerBaseVM _invitingPlayer;
	}
}
