using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Authentication
{
	// Token: 0x02000075 RID: 117
	public class MPAuthenticationDebugVM : ViewModel
	{
		// Token: 0x06000B8B RID: 2955 RVA: 0x00022C5A File Offset: 0x00020E5A
		public MPAuthenticationDebugVM()
		{
			this.RefreshValues();
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00022C68 File Offset: 0x00020E68
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=!}For Debug Purposes", null).ToString();
			this.UsernameText = new TextObject("{=!}Username:", null).ToString();
			this.PasswordText = new TextObject("{=!}Password:", null).ToString();
			this.LoginText = new TextObject("{=!}Login", null).ToString();
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00022CD4 File Offset: 0x00020ED4
		private async void ExecuteLogin()
		{
			LobbyState lobbyState = Game.Current.GameStateManager.ActiveState as LobbyState;
			this.IsLoginRequestActive = true;
			await lobbyState.TryLogin(this.Username, this.Password);
			this.IsLoginRequestActive = false;
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00022D0D File Offset: 0x00020F0D
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00022D15 File Offset: 0x00020F15
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

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00022D33 File Offset: 0x00020F33
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00022D3B File Offset: 0x00020F3B
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
				}
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00022D59 File Offset: 0x00020F59
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00022D61 File Offset: 0x00020F61
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

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00022D84 File Offset: 0x00020F84
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00022D8C File Offset: 0x00020F8C
		[DataSourceProperty]
		public string UsernameText
		{
			get
			{
				return this._usernameText;
			}
			set
			{
				if (value != this._usernameText)
				{
					this._usernameText = value;
					base.OnPropertyChangedWithValue<string>(value, "UsernameText");
				}
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00022DAF File Offset: 0x00020FAF
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00022DB7 File Offset: 0x00020FB7
		[DataSourceProperty]
		public string PasswordText
		{
			get
			{
				return this._passwordText;
			}
			set
			{
				if (value != this._passwordText)
				{
					this._passwordText = value;
					base.OnPropertyChangedWithValue<string>(value, "PasswordText");
				}
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00022DDA File Offset: 0x00020FDA
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00022DE2 File Offset: 0x00020FE2
		[DataSourceProperty]
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				if (value != this._username)
				{
					this._username = value;
					base.OnPropertyChangedWithValue<string>(value, "Username");
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00022E05 File Offset: 0x00021005
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x00022E0D File Offset: 0x0002100D
		[DataSourceProperty]
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				if (value != this._password)
				{
					this._password = value;
					base.OnPropertyChangedWithValue<string>(value, "Password");
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00022E30 File Offset: 0x00021030
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00022E38 File Offset: 0x00021038
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

		// Token: 0x0400053A RID: 1338
		private bool _isEnabled;

		// Token: 0x0400053B RID: 1339
		private bool _isLoginRequestActive;

		// Token: 0x0400053C RID: 1340
		private string _titleText;

		// Token: 0x0400053D RID: 1341
		private string _usernameText;

		// Token: 0x0400053E RID: 1342
		private string _passwordText;

		// Token: 0x0400053F RID: 1343
		private string _username;

		// Token: 0x04000540 RID: 1344
		private string _password;

		// Token: 0x04000541 RID: 1345
		private string _loginText;
	}
}
