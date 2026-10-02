using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Popup
{
	// Token: 0x02000040 RID: 64
	public class MPLobbyInformationPopup : ViewModel
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x0001445C File Offset: 0x0001265C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._titleTextObj != null)
			{
				this.Title = this._titleTextObj.ToString();
			}
			if (this._messageTextObj != null)
			{
				this.Message = this._messageTextObj.ToString();
			}
			this.CloseText = new TextObject("{=yQtzabbe}Close", null).ToString();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000144C3 File Offset: 0x000126C3
		public void ShowInformation(TextObject title, TextObject message)
		{
			this.IsEnabled = true;
			this._titleTextObj = title;
			this._messageTextObj = message;
			this.RefreshValues();
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000144E0 File Offset: 0x000126E0
		public void ShowInformation(string title, string message)
		{
			this.IsEnabled = true;
			this._titleTextObj = null;
			this._messageTextObj = null;
			this.Title = title;
			this.Message = message;
			this.RefreshValues();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001450B File Offset: 0x0001270B
		public void ExecuteClose()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00014514 File Offset: 0x00012714
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x0001451C File Offset: 0x0001271C
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
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001453A File Offset: 0x0001273A
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x00014542 File Offset: 0x00012742
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

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00014560 File Offset: 0x00012760
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x00014568 File Offset: 0x00012768
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001458B File Offset: 0x0001278B
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00014593 File Offset: 0x00012793
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000145B6 File Offset: 0x000127B6
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x000145BE File Offset: 0x000127BE
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

		// Token: 0x06000632 RID: 1586 RVA: 0x000145E1 File Offset: 0x000127E1
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x040002E4 RID: 740
		private TextObject _titleTextObj;

		// Token: 0x040002E5 RID: 741
		private TextObject _messageTextObj;

		// Token: 0x040002E6 RID: 742
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040002E7 RID: 743
		private bool _isEnabled;

		// Token: 0x040002E8 RID: 744
		private string _title;

		// Token: 0x040002E9 RID: 745
		private string _message;

		// Token: 0x040002EA RID: 746
		private string _closeText;
	}
}
