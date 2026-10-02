using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Authentication
{
	// Token: 0x02000076 RID: 118
	public class MPAuthenticationVM : ViewModel
	{
		// Token: 0x06000B9E RID: 2974 RVA: 0x00022E5C File Offset: 0x0002105C
		public MPAuthenticationVM(LobbyState lobbyState)
		{
			this._lobbyState = lobbyState;
			this._hasPrivilege = this._lobbyState.HasMultiplayerPrivilege;
			LobbyState lobbyState2 = this._lobbyState;
			lobbyState2.OnMultiplayerPrivilegeUpdated = (Action<bool>)Delegate.Combine(lobbyState2.OnMultiplayerPrivilegeUpdated, new Action<bool>(this.OnMultiplayerPrivilegeUpdated));
			InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged = (Action<bool>)Delegate.Combine(InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged, new Action<bool>(this.OnInternetConnectionAvailabilityChanged));
			this.AuthenticationDebug = new MPAuthenticationDebugVM();
			this.AuthenticationDebug.IsEnabled = false;
			this.RefreshValues();
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00022F40 File Offset: 0x00021140
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ExitText = new TextObject("{=exitMenuOption}Exit", null).ToString();
			this.LoginText = new TextObject("{=lugGPVOb}Login", null).ToString();
			this.TitleText = this._idleTitle.ToString();
			this.MessageText = this._idleMessage.ToString();
			this.CommunityGamesText = new TextObject("{=SIIgjILk}Community Games", null).ToString();
			this.AuthenticationDebug.RefreshValues();
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00022FC4 File Offset: 0x000211C4
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			LobbyState lobbyState = this._lobbyState;
			lobbyState.OnMultiplayerPrivilegeUpdated = (Action<bool>)Delegate.Remove(lobbyState.OnMultiplayerPrivilegeUpdated, new Action<bool>(this.OnMultiplayerPrivilegeUpdated));
			InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged = (Action<bool>)Delegate.Remove(InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged, new Action<bool>(this.OnInternetConnectionAvailabilityChanged));
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00023040 File Offset: 0x00021240
		public void OnTick(float dt)
		{
			if (!this.IsEnabled || this._lobbyState == null)
			{
				return;
			}
			LobbyClient.State currentState = NetworkMain.GameClient.CurrentState;
			if (currentState != LobbyClient.State.Working && currentState != LobbyClient.State.Connected)
			{
				bool flag = currentState == LobbyClient.State.SessionRequested;
			}
			if (this._hasPrivilege != null && !this._hasPrivilege.Value)
			{
				this.TitleText = this._idleTitle.ToString();
				this.MessageText = this._noAccessMessage.ToString();
				return;
			}
			if (this._lobbyState.IsLoggingIn)
			{
				this.IsLoginRequestActive = true;
				this.TitleText = this._loggingInTitle.ToString();
				this.MessageText = this._loggingInMessage.ToString();
				return;
			}
			this.IsLoginRequestActive = false;
			this.TitleText = this._idleTitle.ToString();
			this.MessageText = this._idleMessage.ToString();
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00023118 File Offset: 0x00021318
		public void ExecuteExit()
		{
			LobbyClient.State currentState = NetworkMain.GameClient.CurrentState;
			if (currentState == LobbyClient.State.Idle)
			{
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_exit", null).ToString(), GameTexts.FindText("str_mp_exit_query", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					this.OnExit();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			TextObject textObject = MPAuthenticationVM.CantLogoutSearchingForMatchTextObject;
			if (currentState == LobbyClient.State.Working || currentState == LobbyClient.State.Connected || currentState == LobbyClient.State.SessionRequested)
			{
				textObject = MPAuthenticationVM.CantLogoutLoggingInTextObject;
			}
			InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_exit", null).ToString(), textObject.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000231F8 File Offset: 0x000213F8
		private void OnExit()
		{
			LobbyState lobbyState = this._lobbyState;
			if (lobbyState == null || !lobbyState.IsLoggingIn)
			{
				if (Module.CurrentModule.StartupInfo.StartupType == GameStartupType.Multiplayer)
				{
					MBInitialScreenBase.DoExitButtonAction();
					return;
				}
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00023244 File Offset: 0x00021444
		public async void ExecuteLogin()
		{
			LobbyState lobbyState = this._lobbyState;
			if (lobbyState == null || !lobbyState.IsLoggingIn)
			{
				try
				{
					await this._lobbyState.TryLogin();
				}
				catch (Exception ex)
				{
					Debug.Print(ex.StackTrace ?? "", 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002327D File Offset: 0x0002147D
		private void OnMultiplayerPrivilegeUpdated(bool hasPrivilege)
		{
			this._hasPrivilege = new bool?(hasPrivilege);
			this.UpdateCanTryLogin();
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00023291 File Offset: 0x00021491
		private void OnInternetConnectionAvailabilityChanged(bool isInternetAvailable)
		{
			this.UpdatePrivilegeInformation();
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00023299 File Offset: 0x00021499
		private void UpdateCanTryLogin()
		{
			this.CanTryLogin = this._hasPrivilege.GetValueOrDefault() && !this._lobbyState.IsLoggingIn;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000232BF File Offset: 0x000214BF
		private void UpdatePrivilegeInformation()
		{
			LobbyState lobbyState = this._lobbyState;
			if (lobbyState == null)
			{
				return;
			}
			lobbyState.UpdateHasMultiplayerPrivilege();
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000232D2 File Offset: 0x000214D2
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x000232DA File Offset: 0x000214DA
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

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000232F8 File Offset: 0x000214F8
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x00023300 File Offset: 0x00021500
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

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0002331E File Offset: 0x0002151E
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x00023326 File Offset: 0x00021526
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
					if (this.IsEnabled)
					{
						this.UpdatePrivilegeInformation();
					}
				}
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00023352 File Offset: 0x00021552
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x0002335A File Offset: 0x0002155A
		[DataSourceProperty]
		public bool IsLoginRequestActive
		{
			get
			{
				return this._isLoginRequestActive;
			}
			set
			{
				if (value != this._isLoginRequestActive)
				{
					this._isLoginRequestActive = value;
					base.OnPropertyChangedWithValue(value, "IsLoginRequestActive");
					this.UpdateCanTryLogin();
				}
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0002337E File Offset: 0x0002157E
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00023386 File Offset: 0x00021586
		[DataSourceProperty]
		public bool CanTryLogin
		{
			get
			{
				return this._canTryLogin;
			}
			set
			{
				if (value != this._canTryLogin)
				{
					this._canTryLogin = value;
					base.OnPropertyChangedWithValue(value, "CanTryLogin");
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x000233A4 File Offset: 0x000215A4
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x000233AC File Offset: 0x000215AC
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
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x000233CF File Offset: 0x000215CF
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x000233D7 File Offset: 0x000215D7
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
					base.OnPropertyChangedWithValue<string>(value, "MessageText");
				}
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x000233FA File Offset: 0x000215FA
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x00023402 File Offset: 0x00021602
		[DataSourceProperty]
		public string ExitText
		{
			get
			{
				return this._exitText;
			}
			set
			{
				if (value != this._exitText)
				{
					this._exitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExitText");
				}
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00023425 File Offset: 0x00021625
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0002342D File Offset: 0x0002162D
		[DataSourceProperty]
		public string LoginText
		{
			get
			{
				return this._loginText;
			}
			set
			{
				if (value != this._loginText)
				{
					this._loginText = value;
					base.OnPropertyChangedWithValue<string>(value, "LoginText");
				}
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00023450 File Offset: 0x00021650
		// (set) Token: 0x06000BBC RID: 3004 RVA: 0x00023458 File Offset: 0x00021658
		[DataSourceProperty]
		public string CommunityGamesText
		{
			get
			{
				return this._communityGamesText;
			}
			set
			{
				if (value != this._communityGamesText)
				{
					this._communityGamesText = value;
					base.OnPropertyChangedWithValue<string>(value, "CommunityGamesText");
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0002347B File Offset: 0x0002167B
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x00023483 File Offset: 0x00021683
		[DataSourceProperty]
		public MPAuthenticationDebugVM AuthenticationDebug
		{
			get
			{
				return this._authenticationDebug;
			}
			set
			{
				if (value != this._authenticationDebug)
				{
					this._authenticationDebug = value;
					base.OnPropertyChangedWithValue<MPAuthenticationDebugVM>(value, "AuthenticationDebug");
				}
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000234A1 File Offset: 0x000216A1
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000234B0 File Offset: 0x000216B0
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x04000542 RID: 1346
		private readonly LobbyState _lobbyState;

		// Token: 0x04000543 RID: 1347
		private bool? _hasPrivilege;

		// Token: 0x04000544 RID: 1348
		private readonly TextObject _idleTitle = new TextObject("{=g1lgiwn1}Not Logged In", null);

		// Token: 0x04000545 RID: 1349
		private readonly TextObject _idleMessage = new TextObject("{=saZ1OvPt}You can press the login button to establish connection", null);

		// Token: 0x04000546 RID: 1350
		private readonly TextObject _noAccessMessage = new TextObject("{=9P0VL49j}You don't have access to multiplayer.", null);

		// Token: 0x04000547 RID: 1351
		private readonly TextObject _loggingInTitle = new TextObject("{=iNqucBor}Logging In", null);

		// Token: 0x04000548 RID: 1352
		private readonly TextObject _loggingInMessage = new TextObject("{=U4dzbzNb}Please wait while you are being connected to the server", null);

		// Token: 0x04000549 RID: 1353
		private static readonly TextObject CantLogoutLoggingInTextObject = new TextObject("{=E0q43haK}Please wait until you are logged in.", null);

		// Token: 0x0400054A RID: 1354
		private static readonly TextObject CantLogoutSearchingForMatchTextObject = new TextObject("{=DyeaObj5}Please cancel game search request before logging out.", null);

		// Token: 0x0400054B RID: 1355
		private bool _isEnabled;

		// Token: 0x0400054C RID: 1356
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400054D RID: 1357
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400054E RID: 1358
		private bool _isLoginRequestActive;

		// Token: 0x0400054F RID: 1359
		private bool _canTryLogin;

		// Token: 0x04000550 RID: 1360
		private string _titleText;

		// Token: 0x04000551 RID: 1361
		private string _messageText;

		// Token: 0x04000552 RID: 1362
		private string _exitText;

		// Token: 0x04000553 RID: 1363
		private string _loginText;

		// Token: 0x04000554 RID: 1364
		private string _communityGamesText;

		// Token: 0x04000555 RID: 1365
		private MPAuthenticationDebugVM _authenticationDebug;
	}
}
