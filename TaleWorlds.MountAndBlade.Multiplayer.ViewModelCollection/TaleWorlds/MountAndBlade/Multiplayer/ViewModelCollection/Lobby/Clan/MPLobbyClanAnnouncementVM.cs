using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000065 RID: 101
	public class MPLobbyClanAnnouncementVM : ViewModel
	{
		// Token: 0x060009B6 RID: 2486 RVA: 0x0001E5A8 File Offset: 0x0001C7A8
		public MPLobbyClanAnnouncementVM(PlayerId senderId, string message, DateTime date, int id, bool canBeDeleted)
		{
			this._id = id;
			this._senderId = senderId;
			this._announcedDate = date;
			this.SenderPlayer = new MPLobbyPlayerBaseVM(senderId, "", null, null);
			this.MessageText = message;
			this.CanBeDeleted = canBeDeleted;
			this.RefreshValues();
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001E5FC File Offset: 0x0001C7FC
		public override void RefreshValues()
		{
			base.RefreshValues();
			string text = new TextObject("{=oMiNaY1E}Posted By", null).ToString();
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("STR2", this.SenderPlayer.Name);
			string text2 = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			string dateFormattedByLanguage = LocalizedTextManager.GetDateFormattedByLanguage(BannerlordConfig.Language, this._announcedDate);
			GameTexts.SetVariable("STR1", text2);
			GameTexts.SetVariable("STR2", dateFormattedByLanguage);
			this.Details = new TextObject("{=QvDxB57o}{STR1} | {STR2}", null).ToString();
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001E690 File Offset: 0x0001C890
		private void ExecuteDeleteAnnouncement()
		{
			string text = new TextObject("{=P1MybNr7}Delete Announcement", null).ToString();
			string text2 = new TextObject("{=CW2JkWzC}Are you sure want to delete this announcement?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.DeleteAnnouncement), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001E707 File Offset: 0x0001C907
		private void DeleteAnnouncement()
		{
			NetworkMain.GameClient.RemoveClanAnnouncement(this._id);
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0001E719 File Offset: 0x0001C919
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0001E721 File Offset: 0x0001C921
		[DataSourceProperty]
		public bool CanBeDeleted
		{
			get
			{
				return this._canBeDeleted;
			}
			set
			{
				if (value != this._canBeDeleted)
				{
					this._canBeDeleted = value;
					base.OnPropertyChanged("CanBeDeleted");
				}
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001E73E File Offset: 0x0001C93E
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0001E746 File Offset: 0x0001C946
		[DataSourceProperty]
		public string MessageText
		{
			get
			{
				return this._messageText;
			}
			set
			{
				if (value != this._messageText)
				{
					this._messageText = value;
					base.OnPropertyChanged("MessageText");
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0001E768 File Offset: 0x0001C968
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0001E770 File Offset: 0x0001C970
		[DataSourceProperty]
		public string Details
		{
			get
			{
				return this._details;
			}
			set
			{
				if (value != this._details)
				{
					this._details = value;
					base.OnPropertyChanged("Details");
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0001E792 File Offset: 0x0001C992
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0001E79A File Offset: 0x0001C99A
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM SenderPlayer
		{
			get
			{
				return this._senderPlayer;
			}
			set
			{
				if (value != this._senderPlayer)
				{
					this._senderPlayer = value;
					base.OnPropertyChanged("SenderPlayer");
				}
			}
		}

		// Token: 0x0400047A RID: 1146
		private DateTime _announcedDate;

		// Token: 0x0400047B RID: 1147
		private PlayerId _senderId;

		// Token: 0x0400047C RID: 1148
		private int _id;

		// Token: 0x0400047D RID: 1149
		private bool _canBeDeleted;

		// Token: 0x0400047E RID: 1150
		private string _messageText;

		// Token: 0x0400047F RID: 1151
		private string _details;

		// Token: 0x04000480 RID: 1152
		private MPLobbyPlayerBaseVM _senderPlayer;
	}
}
