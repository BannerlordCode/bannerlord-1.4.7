using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000016 RID: 22
	public class MultiplayerReportPlayerVM : ViewModel
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00005BDF File Offset: 0x00003DDF
		public MultiplayerReportPlayerVM(Action<string, PlayerId, string, PlayerReportType, string> onReportDone, Action onCancel)
		{
			this._onReportDone = onReportDone;
			this._onCancel = onCancel;
			this.ReportReasons = new SelectorVM<SelectorItemVM>(0, null);
			this.RefreshValues();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005C20 File Offset: 0x00003E20
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DisabledReasonHint = new HintViewModel(new TextObject("{=klkYFik9}You've already reported this player.", null), null);
			this.ReportReasonText = new TextObject("{=cw5QyeRU}Report Reason", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.MuteDescriptionText = new TextObject("{=gGa3ZhqN}This player will be muted automatically.", null).ToString();
			List<string> list = new List<string> { new TextObject("{=koX9okuG}None", null).ToString() };
			foreach (object obj in Enum.GetValues(typeof(PlayerReportType)))
			{
				list.Add(GameTexts.FindText("str_multiplayer_report_reason", ((int)obj).ToString()).ToString());
			}
			this.ReportReasons.Refresh(list, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnReasonSelectionChange));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005D30 File Offset: 0x00003F30
		private void OnReasonSelectionChange(SelectorVM<SelectorItemVM> obj)
		{
			this.CanSendReport = obj.SelectedItem != null && obj.SelectedIndex != 0;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005D4C File Offset: 0x00003F4C
		public void OpenNewReportWithGamePlayerId(string gameId, PlayerId playerId, string playerName, bool isRequestedFromMission)
		{
			this.ReportReasons.SelectedIndex = 0;
			this.ReportMessage = "";
			this._currentGameId = gameId;
			this._currentPlayerId = playerId;
			this._currentPlayerName = playerName;
			this.IsRequestedFromMission = isRequestedFromMission;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005D84 File Offset: 0x00003F84
		public void ExecuteDone()
		{
			if (this.CanSendReport && this.ReportReasons.SelectedIndex > 0 && this._currentGameId != string.Empty && this._currentPlayerId != PlayerId.Empty)
			{
				if (this.ReportMessage.Length > 500)
				{
					this.ReportMessage = this.ReportMessage.Substring(0, 500);
				}
				Action<string, PlayerId, string, PlayerReportType, string> onReportDone = this._onReportDone;
				if (onReportDone != null)
				{
					onReportDone.DynamicInvokeWithLog(new object[]
					{
						this._currentGameId,
						this._currentPlayerId,
						this._currentPlayerName,
						(PlayerReportType)(this.ReportReasons.SelectedIndex - 1),
						this.ReportMessage
					});
				}
				this._currentGameId = string.Empty;
				this._currentPlayerId = PlayerId.Empty;
				this._currentPlayerName = string.Empty;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005E77 File Offset: 0x00004077
		public void ExecuteCancel()
		{
			Action onCancel = this._onCancel;
			if (onCancel == null)
			{
				return;
			}
			onCancel.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005E8F File Offset: 0x0000408F
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

		// Token: 0x06000130 RID: 304 RVA: 0x00005EB8 File Offset: 0x000040B8
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005EC7 File Offset: 0x000040C7
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005ED6 File Offset: 0x000040D6
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00005EDE File Offset: 0x000040DE
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005EFB File Offset: 0x000040FB
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00005F03 File Offset: 0x00004103
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00005F20 File Offset: 0x00004120
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00005F28 File Offset: 0x00004128
		[DataSourceProperty]
		public string ReportMessage
		{
			get
			{
				return this._reportMessage;
			}
			set
			{
				if (this._reportMessage != value)
				{
					this._reportMessage = value;
					base.OnPropertyChangedWithValue<string>(value, "ReportMessage");
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00005F4B File Offset: 0x0000414B
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00005F53 File Offset: 0x00004153
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (this._doneText != value)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00005F76 File Offset: 0x00004176
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00005F7E File Offset: 0x0000417E
		[DataSourceProperty]
		public string ReportReasonText
		{
			get
			{
				return this._reportReasonText;
			}
			set
			{
				if (this._reportReasonText != value)
				{
					this._reportReasonText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReportReasonText");
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00005FA1 File Offset: 0x000041A1
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00005FA9 File Offset: 0x000041A9
		[DataSourceProperty]
		public bool CanSendReport
		{
			get
			{
				return this._canSendReport;
			}
			set
			{
				if (this._canSendReport != value)
				{
					this._canSendReport = value;
					base.OnPropertyChangedWithValue(value, "CanSendReport");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00005FC7 File Offset: 0x000041C7
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00005FCF File Offset: 0x000041CF
		[DataSourceProperty]
		public bool IsRequestedFromMission
		{
			get
			{
				return this._isRequestedFromMission;
			}
			set
			{
				if (value != this._isRequestedFromMission)
				{
					this._isRequestedFromMission = value;
					base.OnPropertyChangedWithValue(value, "IsRequestedFromMission");
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00005FED File Offset: 0x000041ED
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00005FF5 File Offset: 0x000041F5
		[DataSourceProperty]
		public string MuteDescriptionText
		{
			get
			{
				return this._muteDescriptionText;
			}
			set
			{
				if (value != this._muteDescriptionText)
				{
					this._muteDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "MuteDescriptionText");
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00006018 File Offset: 0x00004218
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00006020 File Offset: 0x00004220
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> ReportReasons
		{
			get
			{
				return this._reportReasons;
			}
			set
			{
				if (this._reportReasons != value)
				{
					this._reportReasons = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "ReportReasons");
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000603E File Offset: 0x0000423E
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00006046 File Offset: 0x00004246
		[DataSourceProperty]
		public HintViewModel DisabledReasonHint
		{
			get
			{
				return this._disabledReasonHint;
			}
			set
			{
				if (this._disabledReasonHint != value)
				{
					this._disabledReasonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledReasonHint");
				}
			}
		}

		// Token: 0x0400009F RID: 159
		private readonly Action<string, PlayerId, string, PlayerReportType, string> _onReportDone;

		// Token: 0x040000A0 RID: 160
		private readonly Action _onCancel;

		// Token: 0x040000A1 RID: 161
		private string _currentGameId = string.Empty;

		// Token: 0x040000A2 RID: 162
		private PlayerId _currentPlayerId;

		// Token: 0x040000A3 RID: 163
		private string _currentPlayerName = string.Empty;

		// Token: 0x040000A4 RID: 164
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040000A5 RID: 165
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040000A6 RID: 166
		private string _reportMessage;

		// Token: 0x040000A7 RID: 167
		private string _reportReasonText;

		// Token: 0x040000A8 RID: 168
		private string _doneText;

		// Token: 0x040000A9 RID: 169
		private string _muteDescriptionText;

		// Token: 0x040000AA RID: 170
		private bool _canSendReport;

		// Token: 0x040000AB RID: 171
		private bool _isRequestedFromMission;

		// Token: 0x040000AC RID: 172
		private HintViewModel _disabledReasonHint;

		// Token: 0x040000AD RID: 173
		private SelectorVM<SelectorItemVM> _reportReasons;
	}
}
