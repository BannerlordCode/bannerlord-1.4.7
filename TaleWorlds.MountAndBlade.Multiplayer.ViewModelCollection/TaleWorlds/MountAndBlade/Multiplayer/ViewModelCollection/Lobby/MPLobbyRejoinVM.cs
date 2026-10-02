using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002F RID: 47
	public class MPLobbyRejoinVM : ViewModel
	{
		// Token: 0x0600038A RID: 906 RVA: 0x0000CCEF File Offset: 0x0000AEEF
		public MPLobbyRejoinVM(Action<MPLobbyVM.LobbyPage> onChangePageRequest)
		{
			this._onChangePageRequest = onChangePageRequest;
			this.RefreshValues();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000CD04 File Offset: 0x0000AF04
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=6zYeU0VO}Disconnected from a match", null).ToString();
			this.DescriptionText = new TextObject("{=1A1t1naG}You have left a ranked game in progress. Please reconnect to the game.", null).ToString();
			this.RejoinText = new TextObject("{=5gGyaTPL}Reconnect", null).ToString();
			this.FleeText = new TextObject("{=3sRdGQou}Leave", null).ToString();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000CD70 File Offset: 0x0000AF70
		private void ExecuteRejoin()
		{
			NetworkMain.GameClient.RejoinBattle();
			this.TitleText = new TextObject("{=N0DXasar}Reconnecting", null).ToString();
			this.DescriptionText = new TextObject("{=BZcFB1My}Please wait while you are reconnecting to the game", null).ToString();
			this.IsRejoining = true;
			Action onRejoinRequested = this.OnRejoinRequested;
			if (onRejoinRequested == null)
			{
				return;
			}
			onRejoinRequested();
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000CDCA File Offset: 0x0000AFCA
		private void ExecuteFlee()
		{
			NetworkMain.GameClient.FleeBattle();
			Action<MPLobbyVM.LobbyPage> onChangePageRequest = this._onChangePageRequest;
			if (onChangePageRequest == null)
			{
				return;
			}
			onChangePageRequest(MPLobbyVM.LobbyPage.Home);
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000CDE7 File Offset: 0x0000AFE7
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000CDEF File Offset: 0x0000AFEF
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000CE0D File Offset: 0x0000B00D
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000CE15 File Offset: 0x0000B015
		[DataSourceProperty]
		public bool IsRejoining
		{
			get
			{
				return this._isRejoining;
			}
			set
			{
				if (value != this._isRejoining)
				{
					this._isRejoining = value;
					base.OnPropertyChangedWithValue(value, "IsRejoining");
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000CE33 File Offset: 0x0000B033
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000CE3B File Offset: 0x0000B03B
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000CE5E File Offset: 0x0000B05E
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000CE66 File Offset: 0x0000B066
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000CE89 File Offset: 0x0000B089
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000CE91 File Offset: 0x0000B091
		[DataSourceProperty]
		public string RejoinText
		{
			get
			{
				return this._rejoinText;
			}
			set
			{
				if (value != this._rejoinText)
				{
					this._rejoinText = value;
					base.OnPropertyChangedWithValue<string>(value, "RejoinText");
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000CEB4 File Offset: 0x0000B0B4
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		[DataSourceProperty]
		public string FleeText
		{
			get
			{
				return this._fleeText;
			}
			set
			{
				if (value != this._fleeText)
				{
					this._fleeText = value;
					base.OnPropertyChangedWithValue<string>(value, "FleeText");
				}
			}
		}

		// Token: 0x040001CA RID: 458
		private readonly Action<MPLobbyVM.LobbyPage> _onChangePageRequest;

		// Token: 0x040001CB RID: 459
		public Action OnRejoinRequested;

		// Token: 0x040001CC RID: 460
		private bool _isEnabled;

		// Token: 0x040001CD RID: 461
		private bool _isRejoining;

		// Token: 0x040001CE RID: 462
		private string _titleText;

		// Token: 0x040001CF RID: 463
		private string _descriptionText;

		// Token: 0x040001D0 RID: 464
		private string _rejoinText;

		// Token: 0x040001D1 RID: 465
		private string _fleeText;
	}
}
