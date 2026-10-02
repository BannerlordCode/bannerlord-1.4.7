using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Popup
{
	// Token: 0x02000041 RID: 65
	public class MPLobbyQueryPopupVM : ViewModel
	{
		// Token: 0x06000634 RID: 1588 RVA: 0x000145F8 File Offset: 0x000127F8
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleObj = this._titleObj;
			this.Title = ((titleObj != null) ? titleObj.ToString() : null) ?? "";
			TextObject messageObj = this._messageObj;
			this.Message = ((messageObj != null) ? messageObj.ToString() : null) ?? "";
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001464D File Offset: 0x0001284D
		public void ShowMessage(TextObject title, TextObject message)
		{
			this.IsEnabled = true;
			this._titleObj = title;
			this._messageObj = message;
			this.RefreshValues();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001466A File Offset: 0x0001286A
		public void ShowInquiry(TextObject title, TextObject message, Action onAccepted, Action onDeclined)
		{
			this.IsEnabled = true;
			this.IsInquiry = true;
			this._titleObj = title;
			this._messageObj = message;
			this._onAccepted = onAccepted;
			this._onDeclined = onDeclined;
			this.RefreshValues();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001469D File Offset: 0x0001289D
		public void ExecuteAccept()
		{
			this.IsEnabled = false;
			this.IsInquiry = false;
			Action onAccepted = this._onAccepted;
			if (onAccepted == null)
			{
				return;
			}
			onAccepted();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000146BD File Offset: 0x000128BD
		public void ExecuteDecline()
		{
			this.IsEnabled = false;
			this.IsInquiry = false;
			Action onDeclined = this._onDeclined;
			if (onDeclined == null)
			{
				return;
			}
			onDeclined();
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000146DD File Offset: 0x000128DD
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x000146E5 File Offset: 0x000128E5
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00014703 File Offset: 0x00012903
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0001470B File Offset: 0x0001290B
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
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00014729 File Offset: 0x00012929
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x00014731 File Offset: 0x00012931
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001474F File Offset: 0x0001294F
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x00014757 File Offset: 0x00012957
		[DataSourceProperty]
		public bool IsInquiry
		{
			get
			{
				return this._isInquiry;
			}
			set
			{
				if (value != this._isInquiry)
				{
					this._isInquiry = value;
					base.OnPropertyChangedWithValue(value, "IsInquiry");
				}
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00014775 File Offset: 0x00012975
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x0001477D File Offset: 0x0001297D
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x000147A0 File Offset: 0x000129A0
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x000147A8 File Offset: 0x000129A8
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

		// Token: 0x06000645 RID: 1605 RVA: 0x000147CB File Offset: 0x000129CB
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000147DA File Offset: 0x000129DA
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x040002EB RID: 747
		private TextObject _titleObj;

		// Token: 0x040002EC RID: 748
		private TextObject _messageObj;

		// Token: 0x040002ED RID: 749
		private Action _onAccepted;

		// Token: 0x040002EE RID: 750
		private Action _onDeclined;

		// Token: 0x040002EF RID: 751
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040002F0 RID: 752
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040002F1 RID: 753
		private bool _isEnabled;

		// Token: 0x040002F2 RID: 754
		private bool _isInquiry;

		// Token: 0x040002F3 RID: 755
		private string _title;

		// Token: 0x040002F4 RID: 756
		private string _message;
	}
}
