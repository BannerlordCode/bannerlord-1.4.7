using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000073 RID: 115
	public class MPLobbyClanSendPostPopupVM : ViewModel
	{
		// Token: 0x06000B63 RID: 2915 RVA: 0x000227D4 File Offset: 0x000209D4
		public MPLobbyClanSendPostPopupVM(MPLobbyClanSendPostPopupVM.PostPopupMode popupMode)
		{
			this._popupMode = popupMode;
			this.RefreshValues();
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000227EC File Offset: 0x000209EC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SendText = new TextObject("{=qTYsYJ9V}Send", null).ToString();
			if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Information)
			{
				this.TitleText = new TextObject("{=zravuI1b}Type Clan Information", null).ToString();
				return;
			}
			if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Announcement)
			{
				this.TitleText = new TextObject("{=g5W32uf4}Type Your Announcement", null).ToString();
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00022853 File Offset: 0x00020A53
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
			this.PostData = "";
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00022867 File Offset: 0x00020A67
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00022870 File Offset: 0x00020A70
		public void ExecuteSend()
		{
			if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Information)
			{
				NetworkMain.GameClient.SetClanInformationText(this.PostData);
			}
			else if (this._popupMode == MPLobbyClanSendPostPopupVM.PostPopupMode.Announcement)
			{
				NetworkMain.GameClient.AddClanAnnouncement(this.PostData);
			}
			this.ExecuteClosePopup();
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000228AB File Offset: 0x00020AAB
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000228D4 File Offset: 0x00020AD4
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x000228E3 File Offset: 0x00020AE3
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x000228F2 File Offset: 0x00020AF2
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x000228FA File Offset: 0x00020AFA
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00022917 File Offset: 0x00020B17
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0002291F File Offset: 0x00020B1F
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0002293C File Offset: 0x00020B3C
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x00022944 File Offset: 0x00020B44
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00022961 File Offset: 0x00020B61
		// (set) Token: 0x06000B72 RID: 2930 RVA: 0x00022969 File Offset: 0x00020B69
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

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0002298B File Offset: 0x00020B8B
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x00022993 File Offset: 0x00020B93
		[DataSourceProperty]
		public string PostData
		{
			get
			{
				return this._postData;
			}
			set
			{
				if (value != this._postData)
				{
					this._postData = value;
					base.OnPropertyChanged("PostData");
				}
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x000229B5 File Offset: 0x00020BB5
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x000229BD File Offset: 0x00020BBD
		[DataSourceProperty]
		public string SendText
		{
			get
			{
				return this._sendText;
			}
			set
			{
				if (value != this._sendText)
				{
					this._sendText = value;
					base.OnPropertyChanged("SendText");
				}
			}
		}

		// Token: 0x0400052C RID: 1324
		private MPLobbyClanSendPostPopupVM.PostPopupMode _popupMode;

		// Token: 0x0400052D RID: 1325
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400052E RID: 1326
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400052F RID: 1327
		private bool _isSelected;

		// Token: 0x04000530 RID: 1328
		private string _titleText;

		// Token: 0x04000531 RID: 1329
		private string _postData;

		// Token: 0x04000532 RID: 1330
		private string _sendText;

		// Token: 0x02000157 RID: 343
		public enum PostPopupMode
		{
			// Token: 0x040009B5 RID: 2485
			Information,
			// Token: 0x040009B6 RID: 2486
			Announcement
		}
	}
}
