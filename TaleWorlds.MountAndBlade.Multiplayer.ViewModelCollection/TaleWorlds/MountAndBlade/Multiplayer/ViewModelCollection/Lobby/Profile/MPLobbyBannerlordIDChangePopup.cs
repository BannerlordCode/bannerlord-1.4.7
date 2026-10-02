using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000037 RID: 55
	public class MPLobbyBannerlordIDChangePopup : ViewModel
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x00011EE6 File Offset: 0x000100E6
		public MPLobbyBannerlordIDChangePopup()
		{
			this.BannerlordIDInputText = "";
			this.RefreshValues();
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00011F00 File Offset: 0x00010100
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ChangeBannerlordIDText = new TextObject("{=ozREO8ev}Change Bannerlord ID", null).ToString();
			this.TypeYourNameText = new TextObject("{=clxT9H4T}Type Your Name", null).ToString();
			this.RequestSentText = new TextObject("{=V2lpn6dc}Your Bannerlord ID changing request has been successfully sent.", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.ErrorText = "";
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00011F8C File Offset: 0x0001018C
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
			this.HasRequestSent = false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00011F9C File Offset: 0x0001019C
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
			this.BannerlordIDInputText = "";
			this.ErrorText = "";
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00011FBC File Offset: 0x000101BC
		private async Task<bool> IsInputValid()
		{
			bool flag;
			if (this.BannerlordIDInputText.Length < Parameters.UsernameMinLength)
			{
				GameTexts.SetVariable("STR1", new TextObject("{=k7fJ7TF0}Has to be at least", null));
				GameTexts.SetVariable("STR2", Parameters.UsernameMinLength);
				string text = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", new TextObject("{=nWJGjCgy}characters", null));
				this.ErrorText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				flag = false;
			}
			else if (!Common.IsAllLetters(this.BannerlordIDInputText))
			{
				this.ErrorText = new TextObject("{=Po8jNaXb}Can only contain letters", null).ToString();
				flag = false;
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = PlatformServices.Instance.VerifyString(this.BannerlordIDInputText).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					this.ErrorText = new TextObject("{=bXAIlBHv}Can not contain offensive language", null).ToString();
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00012004 File Offset: 0x00010204
		public async void ExecuteApply()
		{
			if (!this.HasRequestSent)
			{
				TaskAwaiter<bool> taskAwaiter = this.IsInputValid().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					NetworkMain.GameClient.ChangeUsername(this.BannerlordIDInputText);
					this.HasRequestSent = true;
					this.ErrorText = "";
				}
			}
			else
			{
				this.ExecuteClosePopup();
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001203D File Offset: 0x0001023D
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

		// Token: 0x0600052A RID: 1322 RVA: 0x00012066 File Offset: 0x00010266
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00012075 File Offset: 0x00010275
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00012084 File Offset: 0x00010284
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0001208C File Offset: 0x0001028C
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

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x000120A9 File Offset: 0x000102A9
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x000120B1 File Offset: 0x000102B1
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

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000120CE File Offset: 0x000102CE
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x000120D6 File Offset: 0x000102D6
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

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000120F3 File Offset: 0x000102F3
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x000120FB File Offset: 0x000102FB
		[DataSourceProperty]
		public string BannerlordIDInputText
		{
			get
			{
				return this._bannerlordIDInputText;
			}
			set
			{
				if (value != this._bannerlordIDInputText)
				{
					this._bannerlordIDInputText = value;
					this.ErrorText = "";
					base.OnPropertyChanged("BannerlordIDInputText");
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00012128 File Offset: 0x00010328
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00012130 File Offset: 0x00010330
		[DataSourceProperty]
		public string ChangeBannerlordIDText
		{
			get
			{
				return this._changeBannerlordIDText;
			}
			set
			{
				if (value != this._changeBannerlordIDText)
				{
					this._changeBannerlordIDText = value;
					base.OnPropertyChanged("ChangeBannerlordIDText");
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00012152 File Offset: 0x00010352
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x0001215A File Offset: 0x0001035A
		[DataSourceProperty]
		public string TypeYourNameText
		{
			get
			{
				return this._typeYourNameText;
			}
			set
			{
				if (value != this._typeYourNameText)
				{
					this._typeYourNameText = value;
					base.OnPropertyChanged("TypeYourNameText");
				}
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001217C File Offset: 0x0001037C
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00012184 File Offset: 0x00010384
		[DataSourceProperty]
		public string RequestSentText
		{
			get
			{
				return this._requestSentText;
			}
			set
			{
				if (value != this._requestSentText)
				{
					this._requestSentText = value;
					base.OnPropertyChanged("RequestSentText");
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x000121A6 File Offset: 0x000103A6
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x000121AE File Offset: 0x000103AE
		[DataSourceProperty]
		public bool HasRequestSent
		{
			get
			{
				return this._hasRequestSent;
			}
			set
			{
				if (value != this._hasRequestSent)
				{
					this._hasRequestSent = value;
					base.OnPropertyChanged("HasRequestSent");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x000121CB File Offset: 0x000103CB
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x000121D3 File Offset: 0x000103D3
		[DataSourceProperty]
		public string ErrorText
		{
			get
			{
				return this._errorText;
			}
			set
			{
				if (value != this._errorText)
				{
					this._errorText = value;
					base.OnPropertyChanged("ErrorText");
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x000121F5 File Offset: 0x000103F5
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x000121FD File Offset: 0x000103FD
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChanged("CancelText");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001221F File Offset: 0x0001041F
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00012227 File Offset: 0x00010427
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChanged("DoneText");
				}
			}
		}

		// Token: 0x04000277 RID: 631
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000278 RID: 632
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000279 RID: 633
		private bool _isSelected;

		// Token: 0x0400027A RID: 634
		private bool _hasRequestSent;

		// Token: 0x0400027B RID: 635
		private string _bannerlordIDInputText;

		// Token: 0x0400027C RID: 636
		private string _changeBannerlordIDText;

		// Token: 0x0400027D RID: 637
		private string _typeYourNameText;

		// Token: 0x0400027E RID: 638
		private string _requestSentText;

		// Token: 0x0400027F RID: 639
		private string _errorText;

		// Token: 0x04000280 RID: 640
		private string _cancelText;

		// Token: 0x04000281 RID: 641
		private string _doneText;
	}
}
