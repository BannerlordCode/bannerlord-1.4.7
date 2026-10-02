using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000036 RID: 54
	public class MPLobbyBannerlordIDAddFriendPopupVM : ViewModel
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x00011CC2 File Offset: 0x0000FEC2
		public MPLobbyBannerlordIDAddFriendPopupVM()
		{
			this.BannerlordIDInputText = "";
			this.ErrorText = "";
			this.RefreshValues();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00011CE6 File Offset: 0x0000FEE6
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00011CEF File Offset: 0x0000FEEF
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
			this.BannerlordIDInputText = "";
			this.ErrorText = "";
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00011D0E File Offset: 0x0000FF0E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=L3DJHTdY}Enter Bannerlord ID", null).ToString();
			this.AddText = new TextObject("{=tC9C8TLi}Add Friend", null).ToString();
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00011D44 File Offset: 0x0000FF44
		public async void ExecuteTryAddFriend()
		{
			string[] array = this.BannerlordIDInputText.Split(new char[] { '#' });
			if (array.Length == 2 && !array[1].IsEmpty<char>())
			{
				string username = array[0];
				int id = 0;
				bool flag = Common.IsAllLetters(array[0]) && array[0].Length >= Parameters.UsernameMinLength;
				if (int.TryParse(array[1], out id) && flag)
				{
					TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.DoesPlayerWithUsernameAndIdExist(username, id).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						NetworkMain.GameClient.AddFriendByUsernameAndId(username, id, BannerlordConfig.EnableGenericNames);
						this.ExecuteClosePopup();
					}
					else
					{
						this.ErrorText = new TextObject("{=tTwQsP6j}Player does not exist", null).ToString();
					}
				}
				else
				{
					this.ErrorText = new TextObject("{=rWm5udCd}You must enter a valid Bannerlord ID", null).ToString();
				}
				username = null;
			}
			else
			{
				this.ErrorText = new TextObject("{=rWm5udCd}You must enter a valid Bannerlord ID", null).ToString();
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00011D7D File Offset: 0x0000FF7D
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

		// Token: 0x06000513 RID: 1299 RVA: 0x00011DA6 File Offset: 0x0000FFA6
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00011DB5 File Offset: 0x0000FFB5
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00011DC4 File Offset: 0x0000FFC4
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x00011DCC File Offset: 0x0000FFCC
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

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00011DE9 File Offset: 0x0000FFE9
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x00011DF1 File Offset: 0x0000FFF1
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00011E0E File Offset: 0x0001000E
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x00011E16 File Offset: 0x00010016
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00011E33 File Offset: 0x00010033
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00011E3B File Offset: 0x0001003B
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00011E5D File Offset: 0x0001005D
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00011E65 File Offset: 0x00010065
		[DataSourceProperty]
		public string AddText
		{
			get
			{
				return this._addText;
			}
			set
			{
				if (value != this._addText)
				{
					this._addText = value;
					base.OnPropertyChanged("AddText");
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00011E87 File Offset: 0x00010087
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x00011E8F File Offset: 0x0001008F
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

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00011EB1 File Offset: 0x000100B1
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x00011EB9 File Offset: 0x000100B9
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
					base.OnPropertyChanged("BannerlordIDInputText");
					this.ErrorText = "";
				}
			}
		}

		// Token: 0x04000270 RID: 624
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000271 RID: 625
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000272 RID: 626
		private bool _isSelected;

		// Token: 0x04000273 RID: 627
		private string _titleText;

		// Token: 0x04000274 RID: 628
		private string _addText;

		// Token: 0x04000275 RID: 629
		private string _errorText;

		// Token: 0x04000276 RID: 630
		private string _bannerlordIDInputText;
	}
}
